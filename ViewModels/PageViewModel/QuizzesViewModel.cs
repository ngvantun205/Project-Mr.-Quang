using System.Collections.ObjectModel;
using System.Windows.Input;
using TDEduEnglish.Commands;
using TDEduEnglish.Services;

namespace TDEduEnglish.ViewModels {
    public class QuizzesViewModel {
        private readonly AppNavigationService _navigationService;
        private readonly IVocabularyService vocabularyService;

        public ICommand BasicQuizCommand { get; set; }

        public QuizzesViewModel(AppNavigationService navigationService) {
            _navigationService = navigationService;

            BasicQuizCommand = new RelayCommand(o => _navigationService.NavigateToQuizWindow());
        }
    }
}
