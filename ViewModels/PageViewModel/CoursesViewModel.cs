using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TDEduEnglish.Views.CoursesPageView;
using TDEduEnglish.Views.Pages;

namespace TDEduEnglish.ViewModels {
    public class CoursesViewModel {
        private readonly AppNavigationService _navigationService;
        public CoursesViewModel(AppNavigationService navigationService) {
            _navigationService = navigationService;
            ListeningCommand = new RelayCommand(o => {
                _navigationService.NavigateTo(new CourseListPage(_navigationService));
            });
            SpeakingCommand = new RelayCommand(o => {
                _navigationService.NavigateTo(new CourseListPage(_navigationService));
            });
            ReadingCommand = new RelayCommand(o => {
                _navigationService.NavigateTo(new CourseListPage(_navigationService));
            });
            WritingCommand = new RelayCommand(o => {
                _navigationService.NavigateTo(new CourseListPage(_navigationService));
            });
            VocabularyCommand = new RelayCommand(o => {
                _navigationService.NavigateTo(new CourseVocabularyPage(_navigationService));
            });
            GrammarCommand = new RelayCommand(o => {
                _navigationService.NavigateTo(new CourseGrammarPage(_navigationService));
            });
        }
       
        public ICommand ListeningCommand { get; set; }
        public ICommand SpeakingCommand { get; set; }
        public ICommand ReadingCommand { get; set; }
        public ICommand WritingCommand { get; set; }
        public ICommand VocabularyCommand { get; set; }
        public ICommand GrammarCommand { get; set; }

    }
}
