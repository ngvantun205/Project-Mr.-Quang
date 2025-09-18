using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TDEduEnglish.ViewModels.CoursePageViewModel {
    internal class CourseListeningListViewModel {
        private readonly AppNavigationService _appNavigationService;
        private readonly IListeningService _listeningService;
        private readonly ISessonService _sessonService;

        public ICommand StartListeningCommand { get; set; }

        public List<ListeningLesson> BeginnerListenings { get; set; }
        public List<ListeningLesson> IntermediateListenings { get; set; }
        public List<ListeningLesson> AdvancedListenings { get; set; }

        public CourseListeningListViewModel(AppNavigationService appNavigationService, IListeningService listeningService, ISessonService sessonService) {
            _appNavigationService = appNavigationService;
            _listeningService = listeningService;
            _sessonService = sessonService;

            StartListeningCommand = new RelayCommand(StartListening);
            LoadData();
            //create sample data for BeginnerListenings of ListeningLesson
            BeginnerListenings = new List<ListeningLesson> {
                new ListeningLesson { ListeningLessonId = 1, Title = "Beginner Lesson 1", Level = "Beginner"},
                new ListeningLesson { ListeningLessonId = 2, Title = "Beginner Lesson 2", Level = "Beginner" },
                new ListeningLesson { ListeningLessonId = 3, Title = "Beginner Lesson 3", Level = "Beginner"}
            };
        }
        private void StartListening(object? o) {
            if(o is ListeningLesson lesson) {
                _sessonService.SetCurrentListening(lesson);

            }
        }
        private void LoadData() {
            BeginnerListenings = _listeningService.GetByLevel("Beginner").Result.ToList();
            IntermediateListenings = _listeningService.GetByLevel("Intermediate").Result.ToList();
            AdvancedListenings = _listeningService.GetByLevel("Advanced").Result.ToList();
        }
    }
}
