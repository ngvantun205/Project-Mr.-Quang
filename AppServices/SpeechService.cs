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

namespace TDEduEnglish.AppServices {
    public class SpeechService : ISpeechService {
        private readonly string _key;
        private readonly string _region;
        private readonly IUserSpeakingRecordRepository _recordService;

        public SpeechService(IUserSpeakingRecordRepository repo) {
            _recordService = repo;

            try {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                _key = config["AzureSpeech:Key"];
                _region = config["AzureSpeech:Region"];

                if (string.IsNullOrEmpty(_key) || string.IsNullOrEmpty(_region)) {
                    MessageBox.Show("Azure Speech Key hoặc Region không được cấu hình trong appsettings.json.", "Lỗi Cấu hình", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"Không thể tải appsettings.json: {ex.Message}", "Lỗi Cấu hình", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public async Task<UserSpeakingRecord> AssessAndSaveAsync(int userId, int speakingSentenceId, string referenceText) {
            if (string.IsNullOrEmpty(_key) || string.IsNullOrEmpty(_region)) {
                MessageBox.Show("Dịch vụ Speech chưa được cấu hình.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

                var config = SpeechConfig.FromSubscription(_key, _region);
                config.SpeechRecognitionLanguage = "en-US";

                using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
                using var recognizer = new SpeechRecognizer(config, audioConfig);

                var pronConfig = new PronunciationAssessmentConfig(referenceText,
                    GradingSystem.HundredMark, Granularity.Phoneme, enableMiscue: true);

                pronConfig.ApplyTo(recognizer);

                var result = await recognizer.RecognizeOnceAsync();

                if (result.Reason == ResultReason.RecognizedSpeech) {
                    var pronResult = PronunciationAssessmentResult.FromResult(result);

                    var record = new UserSpeakingRecord {
                        UserId = userId,
                        SpeakingSentenceId = speakingSentenceId, 
                        ReferenceText = referenceText,
                        RecognizedText = result.Text,
                        Accuracy = pronResult.AccuracyScore,
                        Fluency = pronResult.FluencyScore,
                        Completeness = pronResult.CompletenessScore,
                        PronScore = pronResult.PronunciationScore,
                        DetailsJson = result.Properties.GetProperty(PropertyId.SpeechServiceResponse_JsonResult)
                    };

                    await _recordService.Add(record);
                    return record;
                }
                else if (result.Reason == ResultReason.NoMatch) {
                    MessageBox.Show("❌ Không phát hiện thấy giọng nói. Vui lòng thử lại.", "Không có âm thanh", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return null;
                }
                else if (result.Reason == ResultReason.Canceled) {
                    var cancellation = CancellationDetails.FromResult(result);
                    string errorDetails = cancellation.ErrorDetails;

                    if (cancellation.Reason == CancellationReason.Error) {
                        if (errorDetails.Contains("SPXERR_MIC_NOT_AVAILABLE") || errorDetails.Contains("SPXERR_AUDIO_SYS_ERROR")) {
                            errorDetails = "Không tìm thấy micro hoặc micro đang được sử dụng. Vui lòng kiểm tra micro của bạn.";
                        }
                        else if (errorDetails.Contains("SPXERR_CONNECTION_FAILURE")) {
                            errorDetails = "Lỗi kết nối mạng. Vui lòng kiểm tra internet.";
                        }
                        else if (errorDetails.Contains("SPXERR_AUTH_FAILED")) {
                            errorDetails = "Xác thực thất bại. Vui lòng kiểm tra Azure Speech Key và Region.";
                        }
                    }

                    MessageBox.Show($"Đánh giá bị hủy.\nLý do: {cancellation.Reason}\nChi tiết: {errorDetails}", "Lỗi Dịch vụ Speech", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

            return null; 
        }

        public async Task SpeakAsync(string text, string voice = "en-US-JennyNeural") {
            var config = SpeechConfig.FromSubscription(_key, _region);
            config.SpeechSynthesisVoiceName = voice;

            using var synthesizer = new SpeechSynthesizer(config);
            var result = await synthesizer.SpeakTextAsync(text);

            if (result.Reason == ResultReason.SynthesizingAudioCompleted) {
                Console.WriteLine($"✅ Spoke: {text}");
            }
            else if (result.Reason == ResultReason.Canceled) {
                var details = SpeechSynthesisCancellationDetails.FromResult(result);
                Console.WriteLine($"❌ Canceled: {details.Reason}, {details.ErrorDetails}");
            }
        }
    }
}