using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TDEduEnglish.ViewModels.CoursePageViewModel;
using TDEduEnglish.Views.CoursesPageView;
using TDEduEnglish.Views.Pages;

namespace TDEduEnglish.ViewModels {
    public class CoursesViewModel {
        private readonly AppNavigationService _navigationService;
        private readonly IUserVocabularyService _userVocabularyService;
        public CoursesViewModel(AppNavigationService navigationService, IUserVocabularyService userVocabularyService) {
            _navigationService = navigationService;
            _userVocabularyService = userVocabularyService;

            VocabularyCommand = new RelayCommand(o => _navigationService.NavigateTo<CourseVocabularyPage>());
            GrammarCommand = new RelayCommand(o => _navigationService.NavigateTo<CourseGrammarPage>());
            ReadingCommand = new RelayCommand(o => _navigationService.NavigateTo<CourseReadingListPage>());
            ListeningCommand = new RelayCommand(o => _navigationService.NavigateTo<CourseListeningListPage>());
            WritingCommand = new RelayCommand(o => _navigationService.NavigateTo<CourseWritingPage>());
            MyVocabularyCommand = new RelayCommand(o => _navigationService.NavigateTo<CourseMyVocabularyPage>());
        }
        

        public ICommand ListeningCommand { get; set; }
        public ICommand SpeakingCommand { get; set; }
        public ICommand ReadingCommand { get; set; }
        public ICommand WritingCommand { get; set; }
        public ICommand VocabularyCommand { get; set; }
        public ICommand GrammarCommand { get; set; }
        public ICommand MyVocabularyCommand { get; set; }
    }
}
