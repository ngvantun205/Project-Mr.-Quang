using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Controls;

namespace TDEduEnglish.Services {
    public class AppNavigationService {
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
            var page = App.Provider.GetRequiredService<TPage>();
            System.Diagnostics.Debug.WriteLine($"Đang điều hướng đến: {typeof(TPage).Name}");
            _mainFrame?.Navigate(page);
        }
    }

}
