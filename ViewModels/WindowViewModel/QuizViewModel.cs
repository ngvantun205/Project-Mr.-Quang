using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using TDEduEnglish.DomainModels;

namespace TDEduEnglish.ViewModels.WindowViewModel {
    public class QuizViewModel : Bindable, INotifyPropertyChanged {
        private readonly AppNavigationService _navigationService;
        private readonly ISessonService _sessonService;
        private readonly IQuizQuestionService _quizQuestionService;
        private readonly IQuizService _quizService;
        private readonly ILeaderBoardService _leaderBoardService;

        private bool showresults;
        public bool ShowResults {
            get => showresults; set {
                Set(ref showresults, value);
                OnPropertyChanged(nameof(ShowResults));
            }
        }
        private int score;
        public int Score {
            get => score; set {
                Set(ref score, value);
                OnPropertyChanged(nameof(Score));
            }
        }

        private bool showSubmitConfirmation;
        public bool ShowSubmitConfirmation {
            get => showSubmitConfirmation; set {
                Set(ref showSubmitConfirmation, value);
                OnPropertyChanged(nameof(ShowSubmitConfirmation));
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
                OnPropertyChanged(nameof(NextButtonText));
                UpdateCurrentQuestionState();
            }
        }
        public string NextButtonText => currentQuestionIndex == TotalQuestions ? "Submit" : "Next";
        public string FlagButtonText => CurrentQuestion?.IsFlagged == true ? "🚩 Flagged" : "🚩 Flag";
        public Quiz CurrentQuiz { get; set; }

        private List<QuizQuestion> questions;
        public List<QuizQuestion> Questions {
            get => questions; set {
                Set(ref questions, value);
                OnPropertyChanged(nameof(Questions));
                UpdateAnsweredCount();
            }
        }

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
        private string explaination;
        public string Explaination {
            get => explaination; set {
                Set(ref explaination, value);
                OnPropertyChanged(nameof(Explaination));
            }
        }
        private int answeredCount;
        public int AnsweredCount {
            get => answeredCount;
            set {
                Set(ref answeredCount, value);
                OnPropertyChanged(nameof(AnsweredCount));
            }
        }
        public int UnansweredCount { get; set; }

        private bool showflagged;
        public bool ShowFlagged {
            get => showflagged; set {
                Set(ref showflagged, value);
                OnPropertyChanged(nameof(ShowFlagged));
            }
        }
        public int QuizQuestionNumber { get; set; } = 1;


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
        public ICommand FlagCommand { get; set; }
        public ICommand ReviewFlaggedCommand { get; set; }
        public ICommand NavigateToQuestionCommand { get; set; }
        public ICommand ConfirmSubmitCommand { get; set; }
        public ICommand CancelSubmitCommand { get; set; }

        public QuizViewModel(AppNavigationService appNavigationService, ISessonService sessonService, IQuizQuestionService quizQuestionService, IQuizService quizService, ILeaderBoardService leaderBoardService) {
            _navigationService = appNavigationService;
            _sessonService = sessonService;
            _quizQuestionService = quizQuestionService;
            _quizService = quizService;
            _leaderBoardService = leaderBoardService;

            CurrentQuiz = _sessonService.CurrentQuiz;
            RetakeQuizCommand = new RelayCommand(async o => await RetakeQuiz());
            BackToHomeCommand = new RelayCommand(o => appNavigationService.NavigateToUserWindow());
            CloseQuizCommand = new RelayCommand(o => appNavigationService.NavigateToUserWindow());
            NextQuestionCommand = new RelayCommand(o => NextQuestion());
            SkipQuestionCommand = new RelayCommand(o => SkipQuestion());
            PreviousQuestionCommand = new RelayCommand(o => PreviousQuestion());
            FlagCommand = new RelayCommand(o => FlagQuestion());
            NavigateToQuestionCommand = new RelayCommand(o => NavigateToQuestion(o));
            ReviewFlaggedCommand = new RelayCommand(o => ReviewFlagged());
            ConfirmSubmitCommand = new RelayCommand(o => ConfirmSubmit());
            CancelSubmitCommand = new RelayCommand(o => ShowSubmitConfirmation = false);

            LoadData().ConfigureAwait(false);
        }
        private async Task LoadData() {
            var currentquiz = _sessonService.CurrentQuiz;
            Title = currentquiz.Title;
            Topic = currentquiz.Topic;
            Level = currentquiz.Level;

            var questions = await _quizQuestionService.GetByQuizId(currentquiz.QuizId);
            Questions = questions != null ? new List<QuizQuestion>(questions) : new List<QuizQuestion>();

            for (int i = 0; i < Questions.Count; i++) {
                Questions[i].QuizQuestionNumber = i + 1;
                Questions[i].PropertyChanged += QuestionPropertyChanged;
            }

            OnPropertyChanged(nameof(Questions));
            ShowResults = false;
            CurrentQuestion = Questions[0];
            CurrentQuestionIndex = 1;
            UpdateCurrentQuestionState();
            TimeRemaining = _sessonService.CurrentQuiz.SuggestedTime;
            StartTimer();
        }
        private void QuestionPropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == nameof(QuizQuestion.IsOption1Selected) ||
                e.PropertyName == nameof(QuizQuestion.IsOption2Selected) ||
                e.PropertyName == nameof(QuizQuestion.IsOption3Selected) ||
                e.PropertyName == nameof(QuizQuestion.IsOption4Selected)) {
                UpdateAnsweredCount();
                UpdateIsAnswered(sender as QuizQuestion);
            }
        }

        private void UpdateIsAnswered(QuizQuestion question) {
            if (question != null) {
                question.IsAnswered = question.IsOption1Selected ||
                                     question.IsOption2Selected ||
                                     question.IsOption3Selected ||
                                     question.IsOption4Selected;
            }
        }

        private void UpdateCurrentQuestionState() {
            if (Questions == null || Questions.Count == 0)
                return;

            foreach (var q in Questions) {
                q.IsCurrentQuestion = false;
            }

            if (CurrentQuestionIndex >= 1 && CurrentQuestionIndex <= Questions.Count) {
                Questions[CurrentQuestionIndex - 1].IsCurrentQuestion = true;
            }
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
                ConfirmSubmit();
            }
        }
        private void NextQuestion() {
            if (CurrentQuestionIndex < TotalQuestions) {
                CurrentQuestionIndex++;
                CurrentQuestion = Questions[CurrentQuestionIndex - 1];
                OnPropertyChanged(nameof(FlagButtonText));
            }
            else {
                UnansweredCount = TotalQuestions - AnsweredCount;
                OnPropertyChanged(nameof(UnansweredCount));
                ShowSubmitConfirmation = true;
            }
        }
        private void PreviousQuestion() {
            if (CurrentQuestionIndex > 1) {
                CurrentQuestionIndex--;
                CurrentQuestion = Questions[CurrentQuestionIndex - 1];
                OnPropertyChanged(nameof(FlagButtonText));
            }
        }
        private void SkipQuestion() {
            if (CurrentQuestionIndex < TotalQuestions) {
                CurrentQuestionIndex++;
                CurrentQuestion = Questions[CurrentQuestionIndex - 1];
                OnPropertyChanged(nameof(FlagButtonText));
            }
        }

        private void ConfirmSubmit() {
            ShowSubmitConfirmation = false;
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
            Score = (int)Math.Round((double)corrects / TotalQuestions * 1000);
            OnPropertyChanged(nameof(Score));
            _leaderBoardService.SubmitAttemptAsync(_sessonService.GetCurrentUser().UserId, CurrentQuiz.QuizId, "Quiz", Score, 1000);
            ShowResults = true;

        }
        private async Task RetakeQuiz() {
            foreach (var q in Questions) {
                q.IsOption1Selected = false;
                q.IsOption2Selected = false;
                q.IsOption3Selected = false;
                q.IsOption4Selected = false;
                q.IsFlagged = false;
                q.IsAnswered = false;
            }
            _timer?.Stop();
            CorrectAnswers = 0;
            await LoadData();
            OnPropertyChanged(nameof(FlagButtonText));
        }
        private void FlagQuestion() {
            if (CurrentQuestion != null) {
                CurrentQuestion.IsFlagged = !CurrentQuestion.IsFlagged;
                OnPropertyChanged(nameof(FlagButtonText));
            }
        }
        private void NavigateToQuestion(object parameter) {
            if (parameter is int questionNumber) {
                if (questionNumber >= 1 && questionNumber <= TotalQuestions) {
                    CurrentQuestionIndex = questionNumber;
                    CurrentQuestion = Questions[questionNumber - 1];
                    OnPropertyChanged(nameof(FlagButtonText));
                }
            }
        }
        private void ReviewFlagged() {
            ShowFlagged = !ShowFlagged;
        }
        private void UpdateAnsweredCount() {
            if (Questions == null)
                return;
            AnsweredCount = Questions.Count(q =>
                q.IsOption1Selected || q.IsOption2Selected ||
                q.IsOption3Selected || q.IsOption4Selected);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}