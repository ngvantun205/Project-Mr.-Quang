using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TDEduEnglish.AppServices;
using TDEduEnglish.Views.CoursesPageView;

namespace TDEduEnglish.ViewModels.CoursePageViewModel {
    public class CourseTopicSpeakingViewModel : Bindable, INotifyPropertyChanged {
        private readonly ISessonService _sessonService;
        private readonly AppNavigationService _appNavigationService;
        private readonly ITopicService _topicService;

        private IEnumerable<Topic> topics;
        public IEnumerable<Topic> Topics {
            get => topics; set {
                topics = value;
                OnPropertyChanged(nameof(Topics));
            }
        }

        private IEnumerable<Topic> beginnerTopics;
        public IEnumerable<Topic> BeginnerTopics {
            get => beginnerTopics; set {
                beginnerTopics = value;
                OnPropertyChanged(nameof(BeginnerTopics));
            }
        }
        private IEnumerable<Topic> intermediateTopic;
        public IEnumerable<Topic> IntermediateTopic {
            get => intermediateTopic; set {
                intermediateTopic = value;
                OnPropertyChanged(nameof(IntermediateTopic));
            }
        }
        private IEnumerable<Topic> advancedTopic;
        public IEnumerable<Topic> AdvancedTopic {
            get => advancedTopic; set {
                advancedTopic = value;
                OnPropertyChanged(nameof(AdvancedTopic));
            }
        }

        public ICommand StartSpeakingCommand { get; set; }
        public CourseTopicSpeakingViewModel(ISessonService sessonService, AppNavigationService appNavigationService, ITopicService topicService) {
            _sessonService = sessonService;
            _appNavigationService = appNavigationService;
            _topicService = topicService;

            LoadData();

            StartSpeakingCommand = new RelayCommand(o => StartSpeaking(o));
        }
        private async Task LoadData() {
            BeginnerTopics = await _topicService.GetByLevel("Beginner");
            IntermediateTopic = await _topicService.GetByLevel("Intermediate");
            AdvancedTopic = await _topicService.GetByLevel("Advanced");
        }

        private void StartSpeaking(object topic) {
            if (topic is Topic selectedTopic) {
                _sessonService.SetCurrentSpeakingTopic(selectedTopic);
                _appNavigationService.NavigateTo<CourseSpeakingPage>();
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
