using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.ViewModels.CoursePageViewModel {
    class CourseListViewModel {
        private readonly AppNavigationService _navigationService;   
        public CourseListViewModel(AppNavigationService navigationService) {
            _navigationService = navigationService;
        }
    }
}
