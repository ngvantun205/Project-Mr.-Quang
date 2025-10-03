using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using TDEduEnglish.Views.Pages;

namespace TDEduEnglish.ViewModels.WindowViewModel {
    class SuperAdminViewModel {
        private readonly AppNavigationService appNavigationService;
        private readonly IUserService userService;
        private readonly IVocabularyService vocabularyService;
        private readonly ISessonService sessonService;
        private readonly IReadingService readingService;

        public ICommand LogoutCommand { get; set; }
        public ICommand AddVocabularyCommand { get; set; }
        public ICommand ManageReadingCommand { get; set; }
        public ICommand ManageListeningCommand { get; set; }
        public ICommand ManageUserCommand { get; set; }
        public ICommand ManageVocabularyCommand { get; set; }
        public ICommand ManageQuizzesCommand { get; set; }

        public SuperAdminViewModel(AppNavigationService appNavigationService, ISessonService sessonService, IUserService userService, IVocabularyService vocabularyService, IReadingService readingService) {
            this.appNavigationService = appNavigationService;
            this.sessonService = sessonService;
            this.userService = userService;
            this.vocabularyService = vocabularyService;
            this.readingService = readingService;

            LogoutCommand = new RelayCommand(o => Logout());
            ManageReadingCommand = new RelayCommand(o => appNavigationService.NavigateToManageReadingWindow());
            ManageListeningCommand = new RelayCommand(o => appNavigationService.NavigateToManageListeningWindow());
            ManageUserCommand = new RelayCommand(o => appNavigationService.NavigateToManageUserWindow());
            ManageVocabularyCommand = new RelayCommand(o => appNavigationService.NavigateToManageVocabularyWindow());
            ManageQuizzesCommand = new RelayCommand(o => appNavigationService.NavigateToManageQuizWindow());
        }
        private void Logout() {
            sessonService.Logout();
            appNavigationService.NavigateToLogWindow();
        }
    }
}
