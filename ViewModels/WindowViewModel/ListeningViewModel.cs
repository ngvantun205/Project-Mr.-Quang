using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.ViewModels.WindowViewModel {
    internal class ListeningViewModel : Bindable, INotifyPropertyChanged {
        private readonly AppNavigationService _appNavigationService;
        private readonly IListeningService _listeningService;
        private readonly ISessonService _sessonService;
        private readonly IListeningQuestionService _listeningQuestionService;
        private readonly IUserListeningResultService _userListeningResultService;

        public string Title { get; set; }
        public string Level { get; set; }
        private ObservableCollection<ListeningQuestion> questions;

        public ObservableCollection<ListeningQuestion> Questions {
            get => questions;
            set {
                if(value != null)
                Set(ref questions, value);
               else 
                    questions = new ObservableCollection<ListeningQuestion>();
            }
        }


        public ListeningViewModel(AppNavigationService navigationService, IListeningService listening, ISessonService sesson, IListeningQuestionService listeningQuestionService, IUserListeningResultService userListeningResultService) {
            _appNavigationService = navigationService;
            _listeningService = listening;
            _sessonService = sesson;
            _listeningQuestionService = listeningQuestionService;
            _userListeningResultService = userListeningResultService;

            ListeningLesson? listeningLessonn = _sessonService.GetCurrentListening();
            if (listeningLessonn != null) {
                Questions = new ObservableCollection<ListeningQuestion>(_listeningQuestionService.GetByListeningId(listeningLessonn.ListeningLessonId).Result);
                Title = listeningLessonn.Title;
                Level = listeningLessonn.Level;
            }
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
