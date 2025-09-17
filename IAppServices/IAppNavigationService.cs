using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IAppServices {
    internal interface ICloseCurrentWindow {
         void CloseCurrentWindow();
    }
    internal interface INavigateToUserWindow {
        void NavigateToUserWindow();
    }
    internal interface INavigateToLogWindow {
        void NavigateToLogWindow();
    }
    internal interface IAppNavigationService : ICloseCurrentWindow, INavigateToUserWindow, INavigateToLogWindow {
        void NavigateToEnglishApp();
    }
}
