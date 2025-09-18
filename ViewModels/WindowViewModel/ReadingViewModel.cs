using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TDEduEnglish.ViewModels.WindowViewModel {
    internal class ReadingViewModel {
        private readonly AppNavigationService _navigationService;
        private readonly IReadingService _readingService;
        private readonly ISessonService _sessonService;
        private readonly IUserService _userService;

        public ReadingLesson readingLesson { get; set; }
        public ObservableCollection<ReadingQuestion> ReadingQuestions { get; set; } = new ObservableCollection<ReadingQuestion>();

        public string Title { get; set; }
        public string Content { get; set; }

        public ICommand SubmitCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ReadingViewModel(AppNavigationService navigationService, IReadingService readingService, ISessonService sessonService, IUserService userService) {
            _navigationService = navigationService;
            _readingService = readingService;
            _sessonService = sessonService;
            _userService = userService;

            var currentReading = _sessonService.GetCurrentReading();
            if (currentReading != null) {
                readingLesson = currentReading;
                Title = readingLesson.Title;
                Content = readingLesson.Content;
                ReadingQuestions = new ObservableCollection<ReadingQuestion>(readingLesson.Questions);
            }

            //SubmitCommand = new RelayCommand(o => SubmitAnswers());
            ExitCommand = new RelayCommand(o => Exit());
        }
        //private void SubmitAnswers() {
        //    int correct = 0;
        //    foreach (var q in ReadingQuestions) {
        //        var selected = q.Options.FirstOrDefault(o => o.IsSelected);
        //        if (selected != null && selected.OptionText == q.CorrectAnswer) {
        //            correct++;
        //        }
        //    }

        //    System.Windows.MessageBox.Show($"Ngài trả lời đúng {correct}/{ReadingQuestions.Count} câu 👑");
        //}

        private void Exit() {
            _navigationService.NavigateToUserWindow();
        }
    }
}
