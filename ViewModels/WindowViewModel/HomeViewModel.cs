using System.ComponentModel;
using System.Windows.Input;
using TDEduEnglish.Commands;
using TDEduEnglish.Services;
using TDEduEnglish.Views.Pages;

namespace TDEduEnglish.ViewModels {
    public class HomeViewModel {
        private readonly AppNavigationService _navigationService;
        private readonly ISessonService _sessonService;


        public HomeViewModel(AppNavigationService navigationService, ISessonService sessonService) {
            _navigationService = navigationService;
            _sessonService = sessonService;

            StartLearningCommand = new RelayCommand(o => {
                _navigationService.NavigateTo<CoursesPage>();
            });
            ProfileCommand = new RelayCommand(o => {
                _navigationService.NavigateTo<UserProfilePage>();
            });
            LogoutCommand = new RelayCommand(o =>  Logout());


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
        public ICommand LogoutCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        private void Logout() {
            _sessonService.Logout();
            _navigationService.NavigateToLogWindow();
        }
    }
}
