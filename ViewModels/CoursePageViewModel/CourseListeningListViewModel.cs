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
        private readonly IListeningQuestionService _listeningQuestionService;
        private readonly ISessonService _sessonService;

        public ICommand StartListeningCommand { get; set; }

        public List<ListeningLesson> BeginnerListenings { get; set; }
        public List<ListeningLesson> IntermediateListenings { get; set; }
        public List<ListeningLesson> AdvancedListenings { get; set; }

        public CourseListeningListViewModel(AppNavigationService appNavigationService, IListeningService listeningService, IListeningQuestionService listeningQuestionService, ISessonService sessonService) {
            _appNavigationService = appNavigationService;
            _listeningService = listeningService;
            _listeningQuestionService = listeningQuestionService;
            _sessonService = sessonService;

            StartListeningCommand = new RelayCommand( o =>  StartListening(o)); 
            LoadData();           
        }
        private async Task StartListening(object? o) {
            if(o is ListeningLesson lesson) {
                _sessonService.SetCurrentListening(lesson);
                var listeningquestions =  await _listeningQuestionService.GetByListeningId(lesson.ListeningLessonId);
                foreach(var item in listeningquestions) {
                    item.IsOption1Selected = false;
                    item.IsOption2Selected = false;
                    item.IsOption3Selected = false;
                    item.IsOption4Selected = false;
                }
                _appNavigationService.NavigateToListeningWindow();
            }
        }
        private void LoadData() {
            BeginnerListenings = _listeningService.GetByLevel("Beginner").Result.ToList();
            IntermediateListenings = _listeningService.GetByLevel("Intermediate").Result.ToList();
            AdvancedListenings = _listeningService.GetByLevel("Advanced").Result.ToList();
        }
    }
}
