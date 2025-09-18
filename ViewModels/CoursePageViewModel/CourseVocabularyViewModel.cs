using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TDEduEnglish.Views.CoursesPageView;

namespace TDEduEnglish.ViewModels.CoursePageViewModel {
    internal class CourseVocabularyViewModel {
        private readonly AppNavigationService _navigationService;
        private readonly ISessonService _sessonService;

        public ICommand DailyLifeCommand { get; set; }
        public ICommand FoodAndDrinksCommand { get; set; }
        public ICommand TransportationCommand { get; set; }
        public ICommand HealthAndBodyCommand { get; set; }
        public CourseVocabularyViewModel(AppNavigationService navigationService, ISessonService sessonService) {
            _navigationService = navigationService;
            _sessonService = sessonService;

            DailyLifeCommand = new RelayCommand(o => {
                _sessonService.SetCurrentTopic("Daily Life");
                _navigationService.NavigateTo<CoursesVocabularyListPage>();
            });
            FoodAndDrinksCommand = new RelayCommand(o => {
                _sessonService.SetCurrentTopic("Food and Drinks");
                _navigationService.NavigateTo<CoursesVocabularyListPage>();
            });
            TransportationCommand = new RelayCommand(o => {
                _sessonService.SetCurrentTopic("Transportation");
                _navigationService.NavigateTo<CoursesVocabularyListPage>();
            });
            HealthAndBodyCommand = new RelayCommand(o => {
                _sessonService.SetCurrentTopic("Health and Body");
                _navigationService.NavigateTo<CoursesVocabularyListPage>();
            });
        }
        
    }
}
