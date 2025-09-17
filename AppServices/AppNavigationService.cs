using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;
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

        public void NavigateToUserWindow() {
            var newWindow = App.Provider?.GetRequiredService<MainWindow>(); 
            Application.Current.MainWindow = newWindow;
            CloseCurrentWindow();
            newWindow?.Show();
        }

        public void NavigateToLogWindow() {
            var newWindow = App.Provider?.GetRequiredService<LogWindow>();
            Application.Current.MainWindow = newWindow;
            CloseCurrentWindow();
            newWindow?.Show();
        }
    }

}
