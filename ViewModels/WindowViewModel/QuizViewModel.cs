using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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
        private bool showExplanation;
        public bool ShowExplanation {
            get => showExplanation; set {
                Set(ref showExplanation, value);
                OnPropertyChanged(nameof(ShowExplanation));
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
        public string Title { get; set; }
        public string Level { get; set; }
        public string Topic { get; set; }
        public ICommand NextQuestionCommand { get; set; }
        public ICommand PreviousQuestionCommand { get; set; }

        public QuizViewModel(AppNavigationService appNavigationService, ISessonService sessonService, IQuizQuestionService quizQuestionService, IQuizService quizService) {
            _navigationService = appNavigationService;
            _sessonService = sessonService;
            _quizQuestionService = quizQuestionService;
            _quizService = quizService;

            NextQuestionCommand = new RelayCommand(o => NextQuestion());
            PreviousQuestionCommand = new RelayCommand(o =>  PreviousQuestion());

            _ = LoadData();
        }
        private async Task LoadData() {
            //var currentquiz = _sessonService.CurrentQuiz;
            //Title = currentquiz.Title;
            //Topic = currentquiz.Topic;
            //Level = currentquiz.Level;
            //var questions = await _quizQuestionService.GetByQuizId(currentquiz.QuizId);
            List<QuizQuestion> questions = new List<QuizQuestion>() {
                     new QuizQuestion
                    {
                        QuestionText = "Từ nào là động từ?",
                        Option1 = "Blue",
                        Option2 = "Run",
                        Option3 = "Happy",
                        Option4 = "Quickly",
                        CorrectAnswer = "Run",
                        Explananation = "Run là động từ, các từ còn lại là tính từ/trạng từ."
                    },
                    new QuizQuestion
                    {
                        QuestionText = "Từ nào là tính từ?",
                        Option1 = "Apple",
                        Option2 = "Play",
                        Option3 = "Beautiful",
                        Option4 = "Swim",
                        CorrectAnswer = "Beautiful",
                        Explananation = "Beautiful là tính từ, nghĩa là 'đẹp'."
                    }};
            Questions = questions != null ? new List<QuizQuestion>(questions) : new List<QuizQuestion>();
            ShowResults = false;
            ShowExplanation = false;
            CurrentQuestion = Questions[0];
            CurrentQuestionIndex = 1;
        }
        private void NextQuestion() {
            if (CurrentQuestionIndex < TotalQuestions) {
                CurrentQuestionIndex++;
                CurrentQuestion = Questions[CurrentQuestionIndex - 1];
                ShowExplanation = false;
            }
        }
        private void PreviousQuestion() {
            if (CurrentQuestionIndex > 1) {
                CurrentQuestionIndex--;
                CurrentQuestion = Questions[CurrentQuestionIndex - 1];
                ShowExplanation = false;
            }
            else
                MessageBox.Show("No question before to previous"); 
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
