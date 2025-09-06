using System.Collections.ObjectModel;
using System.Windows.Input;
using TDEduEnglish.Commands;
using TDEduEnglish.Services;

namespace TDEduEnglish.ViewModels {
    public class QuizzesViewModel {
        private readonly AppNavigationService _navigationService;

        public QuizzesViewModel(AppNavigationService navigationService) {
            _navigationService = navigationService;
        }
    }
}
