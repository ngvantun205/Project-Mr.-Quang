using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace TDEduEnglish.ViewModels.WindowViewModel {
    public class ReadingViewModel : Bindable, INotifyPropertyChanged {
        private readonly AppNavigationService _navigationService;
        private readonly IReadingService _readingService;
        private readonly ISessonService _sessonService;
        private readonly IUserService _userService;
        private readonly IReadingQuestionService _readingQuestionService;
        private readonly IUserVocabularyService _userVocabularyService;
        private readonly IVocabularyService _vocabularyService;
        private readonly ILeaderBoardService _leaderBoardService;

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

        public string Title { get; set; }
        public string Content { get; set; }
        private bool _isPopupOpen;
        public bool IsPopupOpen {
            get => _isPopupOpen;
            set {
                _isPopupOpen = value;
                OnPropertyChanged();
            }
        }
        private string _selectedWord;
        public string SelectedWord {
            get => _selectedWord;
            set {
                _selectedWord = value;
                OnPropertyChanged();
            }
        }
        private bool isResultPopupVisible;
        public bool IsResultPopupVisible {
            get => isResultPopupVisible; set {
                isResultPopupVisible = value;
                OnPropertyChanged(nameof(isResultPopupVisible));
            }
        }
        private int totalscore;
        public int TotalScore {
            get => totalscore; set {
                Set(ref totalscore, value);
                OnPropertyChanged(nameof(TotalScore));
            }
        }
        private int maxscore;
        public int MaxScore {
            get => maxscore; set {
                Set(ref maxscore, value);
                OnPropertyChanged(nameof(MaxScore));
            }
        }
        private int correctanswers;
        public int CorrectAnswers {
            get => correctanswers; set {
                Set(ref correctanswers, value);
                OnPropertyChanged(nameof(CorrectAnswers));
            }
        }



        public ReadingLesson readingLesson { get; set; }
        public ObservableCollection<ReadingQuestion> ReadingQuestions { get; set; }
        public string ResultTitle { get; set; } = "Result";
        public string ResultSubtitle { get; set; } = "Here your reading result";

        public ICommand WordSelectedCommand { get; }
        public ICommand AddToVocabularyCommand { get; }
        public ICommand SubmitCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand ContinueCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ReadingViewModel(AppNavigationService navigationService, IReadingService readingService, ISessonService sessonService, IUserService userService, IReadingQuestionService readingQuestionService, IUserVocabularyService userVocabularyService, IVocabularyService vocabularyService, ILeaderBoardService leaderBoardService) {
            _navigationService = navigationService;
            _readingService = readingService;
            _sessonService = sessonService;
            _userService = userService;
            _readingQuestionService = readingQuestionService;
            _vocabularyService = vocabularyService;
            _userVocabularyService = userVocabularyService;
            _leaderBoardService = leaderBoardService;

            var currentReading = _sessonService.GetCurrentReading();
            if (currentReading != null) {
                readingLesson = currentReading;
                Title = readingLesson.Title;
                Content = readingLesson.Content;
                var questions = _readingQuestionService.GetByLessonId(currentReading.ReadingLessonId).Result;
                ReadingQuestions = new ObservableCollection<ReadingQuestion>(questions);
                CloseCommand = new RelayCommand(o => {
                    IsResultPopupVisible = false;
                    _navigationService.NavigateToUserWindow();
                });
                ContinueCommand = new RelayCommand(o => {
                    IsResultPopupVisible = false;
                    _navigationService.NavigateToUserWindow();
                });
                SubmitCommand = new RelayCommand(async o => await SubmitAnswers());
                ExitCommand = new RelayCommand(o => Exit());

                WordSelectedCommand = new RelayCommand(o => OnWordSelected(o as string));
                AddToVocabularyCommand = new RelayCommand(async o => await OnAddToVocabulary());

                _ = StartTimer();

            }
        }
        private void OnWordSelected(string word) {
            if (string.IsNullOrWhiteSpace(word))
                return;
            SelectedWord = word;
            IsPopupOpen = true;

        }

        private async Task OnAddToVocabulary() {
            if (string.IsNullOrEmpty(SelectedWord))
                return;
            Vocabulary existing = await _vocabularyService.GetByWord(SelectedWord.ToLower());
            if (existing != null) {
                var vocab = new UserVocabulary() {
                    UserId = _sessonService.GetCurrentUser().UserId,
                    Word = SelectedWord,
                    Meaning = existing.Meaning,
                    IPATranscription = existing.IPATranscription,
                    WordType = existing.WordType,
                };
                await _userVocabularyService.Add(vocab);
            }
            else
                await _userVocabularyService.Add(new UserVocabulary() {
                    UserId = _sessonService.GetCurrentUser().UserId,
                    Word = SelectedWord,
                });
            MessageBox.Show($"Đã thêm từ '{SelectedWord}' vào danh sách từ vựng của bạn 📚", "Vocabulary");
            IsPopupOpen = false;
        }
        public string TimeRemaining {
            get {
                var totalSeconds = (int)(readingLesson.SuggestedTime?.TotalSeconds ?? 0);
                int remain = Math.Max(totalSeconds - _elapsedSeconds, 0);
                return $"{remain / 60:D2}:{remain % 60:D2}";
            }
        }

        private async Task StartTimer() {
            _elapsedSeconds = 0;
            _isSubmitted = false;
            _timer = new DispatcherTimer {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += async (s, e) => {
                _elapsedSeconds++;

                int totalSeconds = (int)(readingLesson.SuggestedTime?.TotalSeconds ?? 0);
                if (totalSeconds > 0) {
                    TimerProgress = ((double)_elapsedSeconds / totalSeconds) * 100;
                }
                OnPropertyChanged(nameof(TimeRemaining));

                if (_elapsedSeconds >= totalSeconds && !_isSubmitted) {
                    _timer.Stop();
                    await SubmitAnswers();
                }
            };
            _timer.Start();
        }
        private async Task SubmitAnswers() {
            if (_isSubmitted)
                return;
            _isSubmitted = true;
            int correct = 0;
            foreach (var q in ReadingQuestions) {
                string? selected = null;
                if (q.IsOption1Selected)
                    selected = q.Option1;
                else if (q.IsOption2Selected)
                    selected = q.Option2;
                else if (q.IsOption3Selected)
                    selected = q.Option3;
                else if (q.IsOption4Selected)
                    selected = q.Option4;

                if (selected != null && selected == q.CorrectAnswer) {
                    correct++;
                }
            }

            int score = (int)Math.Round((double)correct / ReadingQuestions.Count * 1000);

            var user = _sessonService.GetCurrentUser();
            var result = new UserReadingResult {
                UserId = user.UserId,
                ReadingLessonId = readingLesson.ReadingLessonId,
                Score = score,
                CompletedAt = DateTime.Now
            };
            bool addedToLeaderboard = await _leaderBoardService.SubmitAttemptAsync(
                userId: user.UserId,
                exerciseId: readingLesson.ReadingLessonId,
                exerciseType: "Reading",
                newScore: score,
                maxScore: 1000
                );
            await _readingService.SaveResult(result);

            CorrectAnswers = correct;
            MaxScore = 1000;
            TotalScore = score;

            IsResultPopupVisible = true;
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
