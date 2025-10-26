using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;
using TDEduEnglish.Views.SuperAdminWindow;
using TDEduEnglish.Views.Windows;

namespace TDEduEnglish.Services {
    public class AppNavigationService : IAppNavigationService {
        private Frame _mainFrame;

        public AppNavigationService(Frame frame) {
            _mainFrame = frame;
        }

        public void SetFrame(Frame frame) {
            _mainFrame = frame;
        }

        public void NavigateTo(Page page) {
            _mainFrame?.Navigate(page);
        }

        public void NavigateTo<TPage>() where TPage : Page {
            var page = App.Provider?.GetRequiredService<TPage>();
            _mainFrame?.Navigate(page);
        }

        public void NavigateToEnglishApp() {

        }

        public void CloseCurrentWindow() {
            Window currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive)!;
            currentWindow.Close();
        }
        public void HideCurrentWindow() {
            Window currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive)!;
            currentWindow.Hide();
        }

        public void NavigateToUserWindow() {
            var newWindow = App.Provider?.GetRequiredService<MainWindow>();
            Application.Current.MainWindow = newWindow;
            HideCurrentWindow();
            newWindow?.Show();
        }

        public void NavigateToLogWindow() {
            var newWindow = App.Provider?.GetRequiredService<LogWindow>();
            Application.Current.MainWindow = newWindow;
            HideCurrentWindow();
            newWindow?.Show();
        }
        public void NavigateToSuperAdminWindow() {
            using (var scope = App.Provider!.CreateScope()) {
                var newWindow = scope.ServiceProvider.GetRequiredService<SuperAdminWindow>();
                Application.Current.MainWindow = newWindow;
                HideCurrentWindow();
                newWindow.Show();
            }
        }
        public void NavigateToReadingWindow() {
            var newWindow = App.Provider?.GetRequiredService<ReadingWindow>();
            Application.Current.MainWindow = newWindow;
            newWindow?.Show();
        }
        public void NavigateToManageReadingWindow() {
            var newwindow = App.Provider?.GetRequiredService<ManageReadingWindow>();
            Application.Current.MainWindow = newwindow;
            newwindow?.Show();
        }
        public void NavigateToUserReadingResultWindow() {
            var newwindow = App.Provider?.GetRequiredService<UserReadingResultWindow>();
            newwindow?.Show();
        }
        public void NavigateToListeningWindow() {
            var newwindow = App.Provider?.GetRequiredService<ListeningWindow>();
            Application.Current.MainWindow = newwindow;
            newwindow?.Show();
        }
        public void NavigateToManageListeningWindow() {
            var newwindow = App.Provider?.GetRequiredService<ManageListeningWindow>();
            Application.Current.MainWindow = newwindow;
            newwindow?.Show();
        }
        public void NavigateToManageUserWindow() {
            var newwindow = App.Provider?.GetRequiredService<ManageUserWindow>();
            Application.Current.MainWindow = newwindow;
            newwindow?.Show();
        }
        public void NavigateToManageVocabularyWindow() {
            var newwindow = App.Provider?.GetRequiredService<ManageVocabularyWindow>();
            Application.Current.MainWindow = newwindow;
            newwindow?.Show();
        }
        public void NavigateToQuizWindow() {
            var newwindow = App.Provider?.GetRequiredService<QuizWindow>();
            Application.Current.MainWindow = newwindow;
            newwindow?.Show();
        }
        public void NavigateToManageQuizWindow() {
            var newwindow = App.Provider?.GetRequiredService<ManageQuizWindow>();
            Application.Current.MainWindow = newwindow;
            newwindow?.Show();
        }
        public void NaviagteToManageSpeakingWindow() {
            var newwindow = App.Provider?.GetRequiredService<ManageSpeakingWindow>();
            Application.Current.MainWindow = newwindow;
            newwindow?.Show();
        }
    }

}
