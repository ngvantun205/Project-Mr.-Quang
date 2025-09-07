using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.ViewModels.CoursePageViewModel {
    internal class CourseVocabularyViewModel {
        private readonly AppNavigationService _navigationService;
        public CourseVocabularyViewModel(AppNavigationService navigationService) {
            _navigationService = navigationService;
        }
    }
}
