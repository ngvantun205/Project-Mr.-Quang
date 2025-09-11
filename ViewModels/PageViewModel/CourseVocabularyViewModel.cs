using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TDEduEnglish.Views.CoursesPageView;

namespace TDEduEnglish.ViewModels.PageViewModel {
    class CourseVocabularyViewModel {
        private readonly AppNavigationService _appNavigationService;
        public CourseVocabularyViewModel(AppNavigationService appNavigationService) {
            _appNavigationService = appNavigationService;
            DailyLifeCommand = new RelayCommand(o => {
                _appNavigationService.NavigateTo(new CoursesVocabularyListPage(_appNavigationService));
            });
        }
        public ICommand DailyLifeCommand { get; set; }
    }
}
