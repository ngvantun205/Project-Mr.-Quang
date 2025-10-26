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
using TDEduEnglish.DomainModels; // <-- Đảm bảo bạn đã using DomainModels

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

            try {
                var config = SpeechConfig.FromSubscription(_key, _region);
                config.SpeechRecognitionLanguage = "en-US";

                using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
                using var recognizer = new SpeechRecognizer(config, audioConfig);

                var pronConfig = new PronunciationAssessmentConfig(referenceText,
                    GradingSystem.HundredMark, Granularity.Phoneme, enableMiscue: true);

                // Áp dụng cấu hình đánh giá vào recognizer
                pronConfig.ApplyTo(recognizer);

                // Bắt đầu nhận dạng một lần
                var result = await recognizer.RecognizeOnceAsync();

                // 1. XỬ LÝ KẾT QUẢ THÀNH CÔNG
                if (result.Reason == ResultReason.RecognizedSpeech) {
                    var pronResult = PronunciationAssessmentResult.FromResult(result);

                    var record = new UserSpeakingRecord {
                        UserId = userId,
                        SpeakingSentenceId = speakingSentenceId, // <-- SỬA LỖI: Lưu ID câu
                        ReferenceText = referenceText,
                        RecognizedText = result.Text, // <-- SỬA LỖI: Lưu văn bản người dùng nói
                        Accuracy = pronResult.AccuracyScore,
                        Fluency = pronResult.FluencyScore,
                        Completeness = pronResult.CompletenessScore,
                        PronScore = pronResult.PronunciationScore,
                        // SỬA LỖI: Lưu JSON chi tiết để phân tích lỗi sau này
                        DetailsJson = result.Properties.GetProperty(PropertyId.SpeechServiceResponse_JsonResult)
                    };

                    await _recordService.Add(record);
                    return record;
                }
                // 2. XỬ LÝ TRƯỜNG HỢP KHÔNG PHÁT HIỆN GIỌNG NÓI
                else if (result.Reason == ResultReason.NoMatch) {
                    MessageBox.Show("❌ Không phát hiện thấy giọng nói. Vui lòng thử lại.", "Không có âm thanh", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return null;
                }
                // 3. XỬ LÝ LỖI (Mic, Mạng, Key)
                else if (result.Reason == ResultReason.Canceled) {
                    var cancellation = CancellationDetails.FromResult(result);
                    string errorDetails = cancellation.ErrorDetails;

                    // Cung cấp thông báo lỗi rõ ràng hơn
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
            }
            catch (Exception ex) {
                // Bắt các lỗi không mong muốn khác
                MessageBox.Show($"Đã xảy ra lỗi không mong muốn: {ex.Message}", "Lỗi nghiêm trọng", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return null; // Trả về null nếu có bất kỳ lỗi nào
        }
    }
}