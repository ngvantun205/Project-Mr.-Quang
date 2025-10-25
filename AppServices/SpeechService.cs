using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.PronunciationAssessment;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TDEduEnglish.AppServices
{
    public class SpeechService : ISpeechService
    {
        private readonly string _key;
        private readonly string _region;
        private readonly IUserSpeakingRecordRepository _recordService;

        public SpeechService( IUserSpeakingRecordRepository repo) {
            _recordService = repo;

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

             _key = config["AzureSpeech:Key"];
             _region = config["AzureSpeech:Region"];
        }

        public async Task<UserSpeakingRecord> AssessAndSaveAsync(int userId, string referenceText) {
            var config = SpeechConfig.FromSubscription(_key, _region);
            config.SpeechRecognitionLanguage = "en-US";

            using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            using var recognizer = new SpeechRecognizer(config, audioConfig);

            var pronConfig = new PronunciationAssessmentConfig(referenceText,
                GradingSystem.HundredMark, Granularity.Phoneme, enableMiscue: true);
            pronConfig.ApplyTo(recognizer);

            Console.WriteLine("🎙️ Please start speaking...");
            var result = await recognizer.RecognizeOnceAsync();

            if (result.Reason == ResultReason.NoMatch) {
                Console.WriteLine("❌ No speech detected. Please try again.");
                return null;
            }

            var pronResult = PronunciationAssessmentResult.FromResult(result);

            var record = new UserSpeakingRecord {
                UserId = userId,
                Accuracy = pronResult.AccuracyScore,
                Fluency = pronResult.FluencyScore,
                Completeness = pronResult.CompletenessScore,
                PronScore = pronResult.PronunciationScore
            };

            await _recordService.Add(record);
            return record;
        }

    }
}

