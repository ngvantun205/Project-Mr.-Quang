using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace TDEduEnglish.ViewModels.WindowViewModel {
    public class QuizViewModel : Bindable, INotifyPropertyChanged {
        private readonly AppNavigationService _navigationService;
        private readonly ISessonService _sessonService;
        private readonly IQuizQuestionService _quizQuestionService;
        private readonly IQuizService _quizService;

        private bool showresults;
        public bool ShowResults {
            get => showresults; set {
                Set(ref showresults, value);
                OnPropertyChanged(nameof(ShowResults));
            }
        }
        private bool showExplaination;
        public bool ShowExplaination {
            get => showExplaination; set {
                Set(ref showExplaination, value);
                OnPropertyChanged(nameof(ShowExplaination));
            }
        }

        private QuizQuestion currentQuestion;
        public QuizQuestion CurrentQuestion {
            get => currentQuestion; set {
                Set(ref currentQuestion, value);
                OnPropertyChanged(nameof(CurrentQuestion));
            }
        }
        private int currentQuestionIndex = 1;
        public int CurrentQuestionIndex {
            get => currentQuestionIndex; set {
                Set(ref currentQuestionIndex, value);
                OnPropertyChanged(nameof(CurrentQuestionIndex));
            }
        }

        public string NextButtonText => currentQuestionIndex == TotalQuestions ? "Submit" : "Next";
        public List<QuizQuestion> Questions { get; set; }
        private int totalQuestions;
        public int TotalQuestions {
            get => Questions?.Count ?? totalQuestions;
            set {
                totalQuestions = value;
                OnPropertyChanged();
            }
        }
        private int correctAnswers;
        public int CorrectAnswers {
            get => correctAnswers; set {
                Set(ref correctAnswers, value);
                OnPropertyChanged(nameof(CorrectAnswers));
            }
        }
        private TimeSpan timeRemaining;
        public TimeSpan TimeRemaining {
            get => timeRemaining; set {
                Set(ref timeRemaining, value);
                OnPropertyChanged(nameof(TimeRemaining));
            }
        }

        private DispatcherTimer _timer;
        public string Title { get; set; }
        public string Level { get; set; }
        public string Topic { get; set; }
        public ICommand NextQuestionCommand { get; set; }
        public ICommand PreviousQuestionCommand { get; set; }
        public ICommand RetakeQuizCommand { get; set; }
        public ICommand BackToHomeCommand { get; set; }
        public ICommand CloseQuizCommand { get; set; }
        public ICommand SkipQuestionCommand { get; set; }

        public QuizViewModel(AppNavigationService appNavigationService, ISessonService sessonService, IQuizQuestionService quizQuestionService, IQuizService quizService) {
            _navigationService = appNavigationService;
            _sessonService = sessonService;
            _quizQuestionService = quizQuestionService;
            _quizService = quizService;

            NextQuestionCommand = new RelayCommand(o => NextQuestion());
            PreviousQuestionCommand = new RelayCommand(o => PreviousQuestion());
            RetakeQuizCommand = new RelayCommand(o => RetakeQuiz());
            BackToHomeCommand = new RelayCommand(o => appNavigationService.NavigateToUserWindow());
            CloseQuizCommand = new RelayCommand(o => appNavigationService.NavigateToUserWindow());
            SkipQuestionCommand = new RelayCommand(o => NextQuestion());

            _ = LoadData();
        }
        private async Task LoadData() {
            var currentquiz = _sessonService.CurrentQuiz;
            Title = currentquiz.Title;
            Topic = currentquiz.Topic;
            Level = currentquiz.Level;

            var questions = await _quizQuestionService.GetByQuizId(currentquiz.QuizId);
            Questions = questions != null ? new List<QuizQuestion>(questions) : new List<QuizQuestion>();

            ShowResults = false;
            ShowExplaination = false;
            CurrentQuestion = Questions[0];
            CurrentQuestionIndex = 1;
            TimeRemaining = CurrentQuestion.AnswerTime;
            StartTimer();
        }
        private void StartTimer() {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e) {
            if (TimeRemaining.TotalSeconds > 0) {
                TimeRemaining = TimeRemaining.Subtract(TimeSpan.FromSeconds(1));
            }
            else {
                _timer.Stop();
                ShowExplaination = true;
                if (CurrentQuestionIndex < TotalQuestions)
                    NextQuestion();
                else
                    FinishQuiz();
            }

        }
        private void NextQuestion() {
            if (CurrentQuestionIndex < TotalQuestions) {
                CurrentQuestionIndex++;
                CurrentQuestion = Questions[CurrentQuestionIndex - 1];
                TimeRemaining = CurrentQuestion.AnswerTime;
                ShowExplaination = false;
                _timer.Stop();
                StartTimer();
            }
            else
                FinishQuiz();
        }
        private void PreviousQuestion() {
            if (CurrentQuestionIndex > 1) {
                CurrentQuestionIndex--;
                CurrentQuestion = Questions[CurrentQuestionIndex - 1];
                ShowExplaination = false;
            }
            else
                MessageBox.Show("No question before to previous");
        }
        private void FinishQuiz() {
            _timer?.Stop();
            int corrects = 0;
            foreach (var q in Questions) {
                if (q.IsOption1Selected && q.Option1 == q.CorrectAnswer)
                    corrects++;
                if (q.IsOption2Selected && q.Option2 == q.CorrectAnswer)
                    corrects++;
                if (q.IsOption3Selected && q.Option3 == q.CorrectAnswer)
                    corrects++;
                if (q.IsOption4Selected && q.Option4 == q.CorrectAnswer)
                    corrects++;
            }
            CorrectAnswers = corrects;
            ShowResults = true;
        }
        private void RetakeQuiz() {
            CurrentQuestionIndex = 1;
            CurrentQuestion = Questions[0];
            ShowResults = false;
            CorrectAnswers = 0;
            ShowExplaination = false;
            TimeRemaining = CurrentQuestion.AnswerTime;
            _timer?.Stop();
            StartTimer();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
