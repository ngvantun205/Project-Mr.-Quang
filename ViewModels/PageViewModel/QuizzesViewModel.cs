using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TDEduEnglish.Commands;
using TDEduEnglish.Services;

namespace TDEduEnglish.ViewModels {
    public class QuizzesViewModel : Bindable, INotifyPropertyChanged {
        private readonly AppNavigationService _navigationService;
        private readonly ISessonService _sessonService;
        private readonly IQuizService _quizService;
        private readonly IQuizQuestionService _quizQuestionService;

        private ObservableCollection<Quiz> quizzes;

        public ObservableCollection<Quiz> Quizzes {
            get => quizzes; set {
                Set(ref quizzes, value);
                OnPropertyChanged(nameof(Quizzes));
            }
        }

        public ICommand StartQuizCommand { get; set; }
        public QuizzesViewModel(AppNavigationService navigationService, ISessonService sessonService, IQuizService quizService, IQuizQuestionService quizQuestionService) {
            _navigationService = navigationService;
            _sessonService = sessonService;
            _quizService = quizService;
            _quizQuestionService = quizQuestionService;

            StartQuizCommand = new RelayCommand(async o => await StartQuiz(o));
            
            _ = LoadData();
        }
        private async Task LoadData() {
            var quiz = await _quizService.GetAll();
            Quizzes = quiz != null ? new ObservableCollection<Quiz>(quiz) : new ObservableCollection<Quiz>();
            foreach(var item in Quizzes)  item.Options = await _quizQuestionService.GetByQuizId(item.QuizId);
        }
        private async Task StartQuiz(object o) {
            if(o is Quiz quiz) {
                _sessonService.SetCurrentQuiz(quiz);
                var questions = await _quizQuestionService.GetByQuizId(quiz.QuizId);
                foreach(var question in questions) {
                    question.IsOption1Selected = false;
                    question.IsOption2Selected = false;
                    question.IsOption3Selected = false;
                    question.IsOption4Selected = false;
                }
                _navigationService.NavigateToQuizWindow();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyname = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
    }
}
