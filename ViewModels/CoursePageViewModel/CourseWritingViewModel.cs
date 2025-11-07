using GenerativeAI.Types;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Input;

namespace TDEduEnglish.ViewModels.CoursePageViewModel {
    public class CourseWritingViewModel : Bindable, INotifyPropertyChanged {
        private readonly IWritingService _writingService;
        private readonly ISessonService _sessonService;

        private string content;
        public string Content {
            get => content;
            set {
                Set(ref content, value);
                OnPropertyChanged(nameof(Content));
            }
        }
        private string writingtext;
        public string WritingText {
            get => writingtext;
            set {
                Set(ref writingtext, value);
                OnPropertyChanged(nameof(WritingText));
            }
        }
        private string writingTopic;
        public string WritingTopic {
            get => writingTopic; set {
                Set(ref writingTopic, value);
                OnPropertyChanged(nameof(WritingTopic));
            }
        }

        private ObservableCollection<Writing> writinghistorylist;
        public ObservableCollection<Writing> WritingHistoryList {get => writinghistorylist; set {
                Set(ref writinghistorylist, value);
                OnPropertyChanged(nameof(WritingHistoryList));
            }
        }
        private bool showWritingDetail;
        public bool ShowWritingDetail {
            get => showWritingDetail; set {
                Set(ref showWritingDetail, value);
                OnPropertyChanged(nameof(ShowWritingDetail));
            }
        }

        private string currentText;
        public string CurrentText {
            get => currentText; set {
                Set(ref currentText, value);
                OnPropertyChanged(nameof(CurrentText));
            }
        }
        private string currentFeedback;
        public string CurrentFeedback {
            get => currentFeedback; set {
                Set(ref currentFeedback, value);
                OnPropertyChanged(nameof(CurrentFeedback));
            }
        }



        public ICommand GradeWritingCommand { get; set; }
        public ICommand ViewDetailCommand { get; set; }
        public ICommand NewWritingCommand { get; set; }
        public CourseWritingViewModel(IWritingService writingService, ISessonService sessonService) {
            _writingService = writingService;
            _sessonService = sessonService;

            GradeWritingCommand = new RelayCommand(async o => await GradeWriting());
            ViewDetailCommand = new RelayCommand(o => ShowDetailWriting(o));
            NewWritingCommand = new RelayCommand( o => NewWriting());

            _ = LoadHistoryData();
        }

        private async Task GradeWriting() {
            Content = "Correcting ...";
            var result = await _writingService.GenerateTextAsync(WritingText, _sessonService.GetCurrentUser().Level,WritingTopic, "Writing Task 2");
            Content = result;
            Writing writing = new Writing() {
                Text = WritingText,
                Feedback = Content,
                UserId = _sessonService.GetCurrentUser().UserId
            };
            await _writingService.Add(writing);
        }
        private async Task LoadHistoryData() {
            var writingHistory = await _writingService.GetAll();
            WritingHistoryList = new ObservableCollection<Writing>(writingHistory);
        }
        private void ShowDetailWriting(object o) {
            if(o is Writing writing) {
                ShowWritingDetail = true;
                CurrentText = writing.Text;
                CurrentFeedback = writing.Feedback;
            }
        }
        private void NewWriting() {
            WritingText = "";
            WritingTopic = "";
            Content = "";
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
