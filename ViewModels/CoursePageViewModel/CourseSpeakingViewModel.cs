using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TDEduEnglish.AppServices;
using TDEduEnglish.DomainModels;
using TDEduEnglish.Views.CoursesPageView;

namespace TDEduEnglish.ViewModels.CoursePageViewModel {
    // Giả định rằng 'Bindable' là một base class đã triển khai INotifyPropertyChanged
    // và phương thức Set()
    internal class CourseSpeakingViewModel : Bindable, INotifyPropertyChanged {
        private readonly ISpeechService _speechService;
        private readonly ISessonService _sessonService;
        private readonly ISpeakingSentenceService _sentenceService;
        private readonly ILeaderBoardService _leaderBoardService;
        private readonly AppNavigationService _appNavigationService;

        // CỜ ĐỂ NGĂN GHI ÂM CHỒNG CHÉO
        private bool _isRecording = false;

        public CourseSpeakingViewModel(ISpeechService speechService, ISessonService sessonService, ISpeakingSentenceService sentenceService, ILeaderBoardService leaderBoardService, AppNavigationService appNavigationService) {
            _speechService = speechService;
            _sessonService = sessonService;
            _sentenceService = sentenceService;
            _leaderBoardService = leaderBoardService;
            _appNavigationService = appNavigationService;

            // SỬA LỖI: Thêm điều kiện CanExecute ( _ => !_isRecording )
            // Lệnh này chỉ có thể chạy khi _isRecording = false
            StartSpeakingCommand = new RelayCommand(async _ => await StartSpeakingAsync(), _ => !_isRecording);
            NextSentenceCommand = new RelayCommand(o => NextSentence());
            SubmitCommand = new RelayCommand(async _ => await Submit());

            _ = LoadData();
        }

        private double score;
        public double Score {
            get => score;
            set => Set(ref score, value);
        }

        private ObservableCollection<SpeakingSentence> sentences = new();
        public ObservableCollection<SpeakingSentence> Sentences {
            get => sentences;
            set => Set(ref sentences, value);
        }

        private SpeakingSentence currentSentence;
        public SpeakingSentence CurrentSentence {
            get => currentSentence;
            set {
                Set(ref currentSentence, value);
                OnPropertyChanged(nameof(CurrentSentence));
            }
        }

        private int sentenceNumber;
        public int SentenceNumber {
            get => sentenceNumber;
            set => Set(ref sentenceNumber, value);
        }
        private double currentScore;
        public double CurrentScore {
            get => currentScore; set {
                Set(ref currentScore, value);
                OnPropertyChanged(nameof(CurrentScore));
            }
        }

        private string _feedback;
        public string Feedback {
            get => _feedback; set {
                Set(ref _feedback, value);
                OnPropertyChanged(nameof(Feedback));
            }
        }

        public ICommand StartSpeakingCommand { get; }
        public ICommand NextSentenceCommand { get; }
        public ICommand SubmitCommand { get; }

        private async Task LoadData() {
            try {
                var exSentences = await _sentenceService.GetByTopicId(_sessonService.CurrentSpeakingTopic.TopicId);
                Sentences = exSentences != null
                    ? new ObservableCollection<SpeakingSentence>(exSentences)
                    : new ObservableCollection<SpeakingSentence>();

                if (Sentences.Count > 0) {
                    SentenceNumber = 1;
                    CurrentSentence = Sentences[0];
                    Score = 0;
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }

        private void NextSentence() {
            if (SentenceNumber < Sentences.Count) {
                Score = Score + CurrentScore;
                SentenceNumber++;
                CurrentSentence = Sentences[SentenceNumber - 1];
                Feedback = string.Empty;
                CurrentScore = 0; // Reset điểm của câu hiện tại
            }
        }

        private async Task Submit() {
            Score = Score + CurrentScore;
            await _leaderBoardService.SubmitAttemptAsync(
                _sessonService.CurrentUser.UserId,
                _sessonService.CurrentSpeakingTopic.TopicId,
                "Speaking",
                (int)Math.Round(Score, 0),
                Sentences.Count
            );
            MessageBox.Show($"Your final score: {Score:F1} 🎯");
            _appNavigationService.NavigateTo<CourseTopicSpeakingPage>();
        }

        private async Task StartSpeakingAsync() {
            // Kiểm tra cờ, mặc dù CanExecute đã xử lý nhưng đây là lớp bảo vệ thứ 2
            if (_isRecording)
                return;

            try {
                _isRecording = true;
                // Vô hiệu hóa nút bấm ngay lập tức
                (StartSpeakingCommand as RelayCommand)?.RaiseCanExecuteChanged();

                Feedback = "🎧 Listening... Please start speaking clearly!";

                // SỬA LỖI: Truyền cả ID của câu
                var record = await _speechService.AssessAndSaveAsync(
                    _sessonService.CurrentUser.UserId,
                    CurrentSentence.SpeakingSentenceId, // <-- ĐÃ THÊM
                    CurrentSentence.SentenceText
                );

                if (record == null) {
                    Feedback = "❌ No speech detected or error occurred. Try again!";
                    CurrentScore = 0; // Đặt điểm về 0 nếu không phát hiện
                    return;
                }

                double avg = (record.Accuracy + record.Fluency + record.Completeness) / 3;
                Feedback = $"✅ Accuracy: {record.Accuracy:F1}%\n" +
                           $"💨 Fluency: {record.Fluency:F1}%\n" +
                           $"📖 Completeness: {record.Completeness:F1}%\n\n" +
                           $"🧠 Pronunciation: {record.PronScore:F1}\n" +
                           $"⭐ Score gained: {avg:F1}";
                CurrentScore = avg;
            }
            catch (Exception ex) {
                // Xử lý các lỗi không mong muốn (ví dụ: service bị lỗi)
                Feedback = $"❌ An unexpected error occurred: {ex.Message}";
                MessageBox.Show($"Error during assessment: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                CurrentScore = 0;
            }
            finally {
                _isRecording = false;
                // Kích hoạt lại nút bấm
                (StartSpeakingCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    // Ghi chú: Code này giả định rằng bạn có một class RelayCommand
    // có phương thức RaiseCanExecuteChanged()
    // Nếu không, bạn cần thêm một thuộc tính bool IsNotRecording và
    // binding IsEnabled của Button vào đó.
}