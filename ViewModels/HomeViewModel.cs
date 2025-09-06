using System.Windows.Input;
using TDEduEnglish.Commands;
using TDEduEnglish.Services;
using TDEduEnglish.Views.Pages;

namespace TDEduEnglish.ViewModels {
    public class HomeViewModel {
        private readonly AppNavigationService _navigationService;

        public HomeViewModel(AppNavigationService navigationService) {
            _navigationService = navigationService;

            StartLearningCommand = new RelayCommand(o => {
                _navigationService.NavigateTo(typeof(CoursesPage));
            });
            ProfileCommand = new RelayCommand(o => {
                _navigationService.NavigateTo(new UserProfilePage(_navigationService));
            });
            LoginCommand = new RelayCommand(o => {
                _navigationService.NavigateTo(new LoginPage(_navigationService));
            });
            RegisterCommand = new RelayCommand(o => {
                _navigationService.NavigateTo(new RegisterPage(_navigationService));
            });

            NavigateCommand = new RelayCommand(o => {
                string pageName = o?.ToString();
                if (string.IsNullOrEmpty(pageName)) return;

                switch (pageName) {
                    case "CoursesPage":
                        _navigationService.NavigateTo(typeof(CoursesPage));
                        break;
                    case "HomePage":
                        _navigationService.NavigateTo(typeof(HomePage));
                        break;
                    case "QuizzesPage":
                        _navigationService.NavigateTo(new QuizzesPage(_navigationService));
                        break;
                    case "CommunityPage":
                        _navigationService.NavigateTo(new CommunityPage(_navigationService));
                        break;
                    case "LeaderboardPage":
                        _navigationService.NavigateTo(new LeaderboardPage(_navigationService));
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
        public ICommand LoginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }


    }
}
