using System.ComponentModel;
using System.Windows.Input;
using TDEduEnglish.Commands;
using TDEduEnglish.Services;
using TDEduEnglish.Views.Pages;

namespace TDEduEnglish.ViewModels {
    public class HomeViewModel {
        private readonly AppNavigationService _navigationService;
        private readonly ISessonService _sessonService;

        private string _loginButtonText = "Log in";
        public string LoginButtonText {
            get => _loginButtonText;
            set {
                if (_loginButtonText != value) {
                    _loginButtonText = value;
                    OnPropertyChanged(nameof(LoginButtonText));
                }
            }
        }

        public HomeViewModel(AppNavigationService navigationService, ISessonService sessonService) {
            _navigationService = navigationService;
            _sessonService = sessonService;

            StartLearningCommand = new RelayCommand(o => {
                _navigationService.NavigateTo<CoursesPage>();
            });
            ProfileCommand = new RelayCommand(o => {
                _navigationService.NavigateTo<UserProfilePage>();
            });
            

            NavigateCommand = new RelayCommand(o => {
                string pageName = o?.ToString();
                if (string.IsNullOrEmpty(pageName)) return;

                switch (pageName) {
                    case "CoursesPage":
                        _navigationService.NavigateTo<CoursesPage>();
                        break;
                    case "HomePage":
                        _navigationService.NavigateTo<HomePage>();
                        break;
                    case "QuizzesPage":
                        _navigationService.NavigateTo<QuizzesPage>();
                        break;
                    case "CommunityPage":
                        _navigationService.NavigateTo<CommunityPage>();
                        break;
                    case "LeaderboardPage":
                        _navigationService.NavigateTo<LeaderboardPage>();
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("Unknown page: " + pageName);
                        break;
                }
            });
        }
        public ICommand StartLearningCommand { get; set; }
        public ICommand NavigateCommand { get; set; }
        public ICommand ProfileCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
