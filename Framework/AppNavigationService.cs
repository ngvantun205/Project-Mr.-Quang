using System;
using System.Windows.Controls;

namespace TDEduEnglish.Services {
    public class AppNavigationService {
        private Frame _mainFrame;

        public AppNavigationService(Frame mainFrame) {
            _mainFrame = mainFrame;
        }

        public void NavigateTo(Page page) {
            _mainFrame.Navigate(page);
        }

        public void NavigateTo(Type pageType) {
            Page page = (Page)Activator.CreateInstance(pageType);
            _mainFrame.Navigate(page);
        }
    }
}
