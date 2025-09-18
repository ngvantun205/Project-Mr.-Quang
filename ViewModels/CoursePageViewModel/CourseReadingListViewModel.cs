using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TDEduEnglish.ViewModels.CoursePageViewModel {
    internal class CourseReadingListViewModel {
        private readonly AppNavigationService _navigationService;
        private readonly IReadingService _readingService;
        private readonly IUserService _userService;
        private readonly ISessonService _sessonService;

        public ICommand StartReadingCommand { get; set; }

        public List<ReadingLesson> BeginnerReadings { get; set; } = new List<ReadingLesson>();
        public List<ReadingLesson> IntermediateReadings { get; set; } = new List<ReadingLesson>();
        public List<ReadingLesson> AdvancedReadings { get; set; } = new List<ReadingLesson>();
        public User? CurrentUser { get; set; }
        public CourseReadingListViewModel(AppNavigationService navigationService, IReadingService readingService, IUserService userService, ISessonService sessonService) {
            _navigationService = navigationService;
            _readingService = readingService;
            _userService = userService;
            _sessonService = sessonService;
            CurrentUser = _sessonService.GetCurrentUser();
            StartReadingCommand = new RelayCommand(StartReading);

            LoadData();
        }

        private void StartReading(object? obj) {
            if (obj is ReadingLesson lesson) {
                _sessonService.SetCurrentReading(lesson);
                _navigationService.NavigateToReadingWindow();
            }

        }
        private void LoadData() {
            BeginnerReadings = _readingService.GetByLevel("Beginner").Result.ToList();
            IntermediateReadings = _readingService.GetByLevel("Intermediate").Result.ToList();
            AdvancedReadings = _readingService.GetByLevel("Advanced").Result.ToList();
        }
    }
}
