using GenerativeAI.Types;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
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
        private readonly IUserService _userService;
        private readonly ISessonService _sessonService;

        private string content;
        public string Content { get => content;
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
        public ICommand GradeWritingCommand { get; set; }

        public CourseWritingViewModel(IWritingService writingService, IUserService userService, ISessonService sessonService) { 
            _writingService = writingService;
            _userService = userService;
            _sessonService = sessonService;

            GradeWritingCommand = new RelayCommand(async o => await GradeWriting());
        }
        private async Task GradeWriting() {
            Content = "Correcting ...";
            var result = await _writingService.GenerateTextAsync(WritingText, _sessonService.GetCurrentUser().Level, "Writing Task 2");
            Content = result;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
