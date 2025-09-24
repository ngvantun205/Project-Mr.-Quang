using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TDEduEnglish.ViewModels.CoursePageViewModel {
    internal class CourseReadingListViewModel {
        private readonly AppNavigationService _navigationService;
        private readonly IReadingService _readingService;
        private readonly IUserService _userService;
        private readonly ISessonService _sessonService;
        private readonly IReadingQuestionService _readingQuestionService;

        public ICommand StartReadingCommand { get; set; }
        public ICommand ShowResultCommand { get; set; }

        public List<ReadingLesson> BeginnerReadings { get; set; } = new List<ReadingLesson>();
        public List<ReadingLesson> IntermediateReadings { get; set; } = new List<ReadingLesson>();
        public List<ReadingLesson> AdvancedReadings { get; set; } = new List<ReadingLesson>();
        public User? CurrentUser { get; set; }
        public CourseReadingListViewModel(AppNavigationService navigationService, IReadingService readingService, IUserService userService, ISessonService sessonService, IReadingQuestionService readingQuestionService) {
            _navigationService = navigationService;
            _readingService = readingService;
            _userService = userService;
            _sessonService = sessonService;
            CurrentUser = _sessonService.GetCurrentUser();
            StartReadingCommand = new RelayCommand(StartReading);
            _readingQuestionService = readingQuestionService;

            ShowResultCommand = new RelayCommand(o => ShowResult(o));

            LoadData();
            
        }

        private void StartReading(object? obj) {
            if (obj is ReadingLesson lesson) {
                _sessonService.SetCurrentReading(lesson);
                var readingQuestions = _readingQuestionService.GetByLessonId(lesson.ReadingLessonId).Result;
                foreach(var q in readingQuestions) {
                    q.IsOption1Selected = false;
                    q.IsOption2Selected = false;
                    q.IsOption3Selected = false;
                    q.IsOption4Selected = false;
                }
                _navigationService.NavigateToReadingWindow();
            }

        }
        private void ShowResult(object? obj) {
            if (obj is ReadingLesson lesson) {
                _sessonService.SetCurrentReading(lesson);
                _navigationService.NavigateToUserReadingResultWindow();
            }
        }

        private void LoadData() {
            BeginnerReadings = _readingService.GetByLevel("Beginner").Result.ToList();
            IntermediateReadings = _readingService.GetByLevel("Intermediate").Result.ToList();
            AdvancedReadings = _readingService.GetByLevel("Advanced").Result.ToList();
        }
    }
}
