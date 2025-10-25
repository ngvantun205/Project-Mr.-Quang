using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TDEduEnglish.AppServices;

namespace TDEduEnglish.ViewModels.CoursePageViewModel {
    internal class CourseSpeakingViewModel : Bindable, INotifyPropertyChanged {
        private readonly ISpeechService _speechService; 
        private readonly int _userId = 1;

        public CourseSpeakingViewModel(ISpeechService speakingRecordRepository) {
            _speechService = speakingRecordRepository;

            StartSpeakingCommand = new RelayCommand(async _ => await StartSpeakingAsync());
        }

        private string _referenceText = "I am the most hand-some man in the world";
        public string ReferenceText {
            get => _referenceText;
            set => Set(ref _referenceText, value);
        }

        private string _feedback;
        public string Feedback {
            get => _feedback;
            set => Set(ref _feedback, value);
        }

        public ICommand StartSpeakingCommand { get; }

        private async Task StartSpeakingAsync() {
            Feedback = "Listening... 🎙️";

            var record = await _speechService.AssessAndSaveAsync(_userId, ReferenceText);


            if (record == null) {
                MessageBox.Show("Please speak louder");
                return;
            }

            MessageBox.Show($"Accuracy: {record.Accuracy}");

            Feedback = $"Accuracy: {record.Accuracy:F1} | Fluency: {record.Fluency:F1}\n" +
                       $"Completeness: {record.Completeness:F1} | Pron: {record.PronScore:F1}";
        }
    }
}
