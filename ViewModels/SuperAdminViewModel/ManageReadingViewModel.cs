using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using TDEduEnglish.Views.Pages;

namespace TDEduEnglish.ViewModels.SuperAdminViewModel {
    internal class ManageReadingViewModel {
        private readonly AppNavigationService _navigationService;
        private readonly IReadingService readingService;
        private readonly ISessonService sessonService;  
        private readonly IReadingQuestionRepository readingQuestionRepository;

        public ICommand ImportReadingLessonCommand { get; set; }
        public ICommand EditReadingLessonCommand { get; set; }
        public ICommand DeleteReadingLessonCommand { get; set; }
        public ICommand AddReadingLessonCommand { get; set; }
        public ObservableCollection<ReadingLesson> ReadingLessons { get; set; }

        public ICommand ImportReadingQuestionCommand { get; set; }
        public ICommand EditReadingQuestionCommand { get; set; }
        public ICommand DeleteReadingQuestionCommand { get; set; }
        public ICommand AddReadingQuestionCommand { get; set; }
        public ObservableCollection<ReadingQuestion> ReadingQuestions { get; set; }

        public ReadingLesson SelectedReadingLesson { get; set; }
        public ReadingQuestion SelectedReadingQuestion { get; set; }


        public ManageReadingViewModel(AppNavigationService appNavigationService, IReadingService readingService, ISessonService sessonService, IReadingQuestionRepository readingQuestionRepository) {
            this._navigationService = appNavigationService;
            this.readingService = readingService;
            this.sessonService = sessonService;
            this.readingQuestionRepository = readingQuestionRepository;

            ReadingLessons = new ObservableCollection<ReadingLesson>(readingService.GetAll().Result);
            ReadingQuestions = new ObservableCollection<ReadingQuestion>(readingQuestionRepository.GetAll().Result);

            ImportReadingLessonCommand = new RelayCommand(async o => await AddReadingLessonFromJsonFile());
            DeleteReadingLessonCommand = new RelayCommand(async o => await DeleteReadingLesson(SelectedReadingLesson));
            
            ImportReadingQuestionCommand = new RelayCommand(async o => await AddReadingQuestionFromJsonFile());
            DeleteReadingQuestionCommand = new RelayCommand(async o => await DeleteReadingQuestion(SelectedReadingQuestion));

        }
        private async Task DeleteReadingLesson(object obj) {
            if (obj is ReadingLesson lesson) {
                var result = MessageBox.Show($"Are you sure you want to delete the reading lesson '{lesson.Title}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes) {
                    await readingService.Delete(lesson.ReadingLessonId);
                    ReadingLessons.Remove(lesson);
                    MessageBox.Show("Reading lesson deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        private async Task DeleteReadingQuestion(object obj) {
            if (obj is ReadingQuestion question) {
                var result = MessageBox.Show($"Are you sure you want to delete the reading question '{question.QuestionText}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes) {
                    await readingQuestionRepository.Delete(question.ReadingQuestionId);
                    ReadingQuestions.Remove(question);
                    MessageBox.Show("Reading question deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private async Task AddReadingLessonFromJsonFile() {
            var openFileDialog = new OpenFileDialog {
                Title = "Select a JSON file",
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true) {
                string filePath = openFileDialog.FileName;
                try {
                    string jsonContent = await File.ReadAllTextAsync(filePath);
                    var readinglessons = JsonSerializer.Deserialize<List<ReadingLesson>>(jsonContent);
                    if (readinglessons != null && readinglessons.Count > 0) {
                        await readingService.AddListAsync(readinglessons);

                        MessageBox.Show($"✅ Đã thêm {readinglessons.Count} bài đọc vào cơ sở dữ liệu.",
                                        "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        ReadingLessons = new ObservableCollection<ReadingLesson>(readingService.GetAll().Result);
                    }
                    else {
                        MessageBox.Show("⚠️ File JSON rỗng hoặc không đúng định dạng.",
                                        "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show($"❌ Lỗi khi đọc file JSON: {ex.Message}",
                                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
        private async Task AddReadingQuestionFromJsonFile() {
            var openFileDialog = new OpenFileDialog {
                Title = "Select a JSON file",
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true) {
                string filePath = openFileDialog.FileName;
                try {
                    string jsonContent = await File.ReadAllTextAsync(filePath);
                    var readingQuestions = JsonSerializer.Deserialize<List<ReadingQuestion>>(jsonContent);
                    if (readingQuestions != null && readingQuestions.Count > 0) {
                        await readingQuestionRepository.AddListAsync(readingQuestions);

                        MessageBox.Show($"✅ Đã thêm {readingQuestions.Count} câu hỏi đọc vào cơ sở d�� liệu.",
                                        "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        ReadingQuestions = new ObservableCollection<ReadingQuestion>(readingQuestionRepository.GetAll().Result);
                    }
                    else {
                        MessageBox.Show("⚠️ File JSON rỗng hoặc không đúng định dạng.",
                                        "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show($"❌ Lỗi khi đọc file JSON: {ex.Message}",
                                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

    }
}
