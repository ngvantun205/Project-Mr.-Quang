using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace TDEduEnglish.ViewModels.WindowViewModel {
    internal class ReadingViewModel : INotifyPropertyChanged {
        private readonly AppNavigationService _navigationService;
        private readonly IReadingService _readingService;
        private readonly ISessonService _sessonService;
        private readonly IUserService _userService;
        private readonly IReadingQuestionService _readingQuestionService;

        private DispatcherTimer _timer;
        private int _elapsedSeconds;
        private bool _isSubmitted;

        private double _timerProgress;
        public double TimerProgress {
            get => _timerProgress;
            set {
                _timerProgress = value;
                OnPropertyChanged();
            }
        }

        public ReadingLesson readingLesson { get; set; }
        public ObservableCollection<ReadingQuestion> ReadingQuestions { get; set; }


        public string Title { get; set; }
        public string Content { get; set; }

        public ICommand SubmitCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ReadingViewModel(AppNavigationService navigationService, IReadingService readingService, ISessonService sessonService, IUserService userService, IReadingQuestionService readingQuestionService) {
            _navigationService = navigationService;
            _readingService = readingService;
            _sessonService = sessonService;
            _userService = userService;
            _readingQuestionService = readingQuestionService;

            var currentReading = _sessonService.GetCurrentReading();
            if (currentReading != null) {
                readingLesson = currentReading;
                Title = readingLesson.Title;
                Content = readingLesson.Content;
                var questions = _readingQuestionService.GetByLessonId(currentReading.ReadingLessonId).Result;
                ReadingQuestions = new ObservableCollection<ReadingQuestion>(questions);

                SubmitCommand = new RelayCommand(o => SubmitAnswers());
                ExitCommand = new RelayCommand(o => Exit());

                StartTimer();
            }

        }
        public string TimeRemaining {
            get {
                var totalSeconds = (int)(readingLesson.SuggestedTime?.TotalSeconds ?? 0);
                int remain = Math.Max(totalSeconds - _elapsedSeconds, 0);
                return $"{remain / 60:D2}:{remain % 60:D2}";
            }
        }

        private void StartTimer() {
            _elapsedSeconds = 0;
            _isSubmitted = false;
            _timer = new DispatcherTimer {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += (s, e) => {
                _elapsedSeconds++;

                int totalSeconds = (int)(readingLesson.SuggestedTime?.TotalSeconds ?? 0);
                if (totalSeconds > 0) {
                    TimerProgress = ((double)_elapsedSeconds / totalSeconds) * 100;
                }
                OnPropertyChanged(nameof(TimeRemaining));

                if (_elapsedSeconds >= totalSeconds && !_isSubmitted) {
                    _timer.Stop();
                    SubmitAnswers(); // ⏰ auto submit
                }
            };
            _timer.Start();
        }
        private async void SubmitAnswers() {
            if (_isSubmitted) return; 
            _isSubmitted = true;
            int correct = 0;
            foreach (var q in ReadingQuestions) {
                string? selected = null;
                if (q.IsOption1Selected) selected = q.Option1;
                else if (q.IsOption2Selected) selected = q.Option2;
                else if (q.IsOption3Selected) selected = q.Option3;
                else if (q.IsOption4Selected) selected = q.Option4;

                if (selected != null && selected == q.CorrectAnswer) {
                    correct++;
                }
            }

            int score = (int)Math.Round((double)correct / ReadingQuestions.Count * 100);

            var user = _sessonService.GetCurrentUser();
            var result = new UserReadingResult {
                UserId = user.UserId,
                ReadingLessonId = readingLesson.ReadingLessonId,
                Score = score,
                CompletedAt = DateTime.Now
            };

            await _readingService.SaveResult(result);

            System.Windows.MessageBox.Show(
                $"Bạn trả lời đúng {correct}/{ReadingQuestions.Count} câu 🏆.\nĐiểm số: {score}%",
                "Kết quả bài đọc"
            );
            _navigationService.HideCurrentWindow();
            _navigationService.NavigateToUserReadingResultWindow();
        }


        private void Exit() {
            _navigationService.NavigateToUserWindow();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
