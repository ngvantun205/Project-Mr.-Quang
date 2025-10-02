using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using TDEduEnglish.Views.Pages;

namespace TDEduEnglish.ViewModels.SuperAdminViewModel {
    internal class ManageReadingViewModel : Bindable, INotifyPropertyChanged {
        private readonly AppNavigationService _navigationService;
        private readonly IReadingService readingService;
        private readonly ISessonService sessonService;
        private readonly IReadingQuestionService readingQuestionService;

        public ICommand ImportReadingLessonCommand { get; set; }
        public ICommand UpdateReadingLessonCommand { get; set; }
        public ICommand DeleteReadingLessonCommand { get; set; }
        public ICommand AddReadingLessonCommand { get; set; }
        public ICommand ImportReadingQuestionCommand { get; set; }
        public ICommand UpdateReadingQuestionCommand { get; set; }
        public ICommand DeleteReadingQuestionCommand { get; set; }
        public ICommand AddReadingQuestionCommand { get; set; }

        private ObservableCollection<ReadingLesson> readinglessons;

        public ObservableCollection<ReadingLesson> ReadingLessons {
            get => readinglessons; set {
                Set(ref readinglessons, value);
                OnPropertyChanged(nameof(ReadingLessons));
            }
        }
        private ObservableCollection<ReadingQuestion> readingQuestions;
        public ObservableCollection<ReadingQuestion> ReadingQuestions {
            get => readingQuestions; set {
                Set(ref readingQuestions, value);
                OnPropertyChanged(nameof(ReadingQuestions));
            }
        }

        private ReadingLesson selectedReadingLesson;
        public ReadingLesson SelectedReadingLesson {
            get => selectedReadingLesson; set {
                Set(ref selectedReadingLesson, value);
                if (selectedReadingLesson != null) {
                    ReadingQuestions = new ObservableCollection<ReadingQuestion>(readingQuestionService.GetByLessonId(selectedReadingLesson.ReadingLessonId).Result);
                    Title = SelectedReadingLesson.Title;
                    Content = SelectedReadingLesson.Content;
                    Level = SelectedReadingLesson.Level;
                    SuggestedTime = SelectedReadingLesson.SuggestedTime;
                }
            }
        }
        private string title;
        public string Title {
            get => title; set {
                Set(ref title, value);
                OnPropertyChanged(nameof(Title));
            }
        }
        private string level;
        public string Level {
            get => level; set {
                Set(ref level, value);
                OnPropertyChanged(nameof(Level));
            }
        }
        private string content;
        public string Content {
            get => content; set {
                Set(ref content, value);
                OnPropertyChanged(nameof(Content));
            }
        }
        private TimeSpan? suggestedtime;
        public TimeSpan? SuggestedTime {
            get => suggestedtime; set {
                Set(ref suggestedtime, value);
                OnPropertyChanged(nameof(SuggestedTime));
            }
        }


        public ReadingQuestion SelectedReadingQuestion { get; set; }

        public ManageReadingViewModel(AppNavigationService appNavigationService, IReadingService readingService, ISessonService sessonService, IReadingQuestionService readingQuestionService) {
            this._navigationService = appNavigationService;
            this.readingService = readingService;
            this.sessonService = sessonService;
            this.readingQuestionService = readingQuestionService;

            ReadingLessons = new ObservableCollection<ReadingLesson>(readingService.GetAll().Result);
            ReadingQuestions = new ObservableCollection<ReadingQuestion>(readingQuestionService.GetAll().Result);

            ImportReadingLessonCommand = new RelayCommand(async o => await ImportReadingLessonFromJsonFile());
            DeleteReadingLessonCommand = new RelayCommand(async o => await DeleteReadingLesson(SelectedReadingLesson));
            AddReadingLessonCommand = new RelayCommand(async o => await AddReadingLesson());
            UpdateReadingLessonCommand = new RelayCommand(async o => await UpdateReadingLesson(SelectedReadingLesson));

            ImportReadingQuestionCommand = new RelayCommand(async o => await ImportReadingQuestionFromJsonFile());
            DeleteReadingQuestionCommand = new RelayCommand(async o => await DeleteReadingQuestion(SelectedReadingQuestion));
            AddReadingQuestionCommand = new RelayCommand(async o => await AddReadingQuestion());
            UpdateReadingQuestionCommand = new RelayCommand(async o => await UpdateReadingQuestion(SelectedReadingQuestion));

            _ = LoadData();
        }
        private async Task LoadData() {
            var lessons = await readingService.GetAll();
            ReadingLessons = lessons != null ?  new ObservableCollection<ReadingLesson>(lessons) :  new ObservableCollection<ReadingLesson>();
            var questions = await readingQuestionService.GetAll();
            ReadingQuestions = questions != null ? new ObservableCollection<ReadingQuestion>(questions) : new ObservableCollection<ReadingQuestion>();
        }
        private async Task AddReadingLesson() {
            var lesson = new ReadingLesson();
            ReadingLessons.Add(lesson);
            await readingService.Add(lesson);
        }
        private async Task AddReadingQuestion() {
            var question = new ReadingQuestion();
            ReadingQuestions.Add(question);
            await readingQuestionService.Add(question);
        }
        private async Task UpdateReadingLesson(object? o) {
            if (o is ReadingLesson readingLesson) {
                SelectedReadingLesson.Title = Title;
                SelectedReadingLesson.Level = Level;
                SelectedReadingLesson.Content = Content;
                SelectedReadingLesson.SuggestedTime = SuggestedTime;
                await readingService.Update(readingLesson);
                await LoadData();
                MessageBox.Show("Reading lesson is updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private async Task UpdateReadingQuestion(object? o) {
            if (o is ReadingQuestion question) {
                await readingQuestionService.Update(question);
                MessageBox.Show("Reading question is updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
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
                    await readingQuestionService.Delete(question.ReadingQuestionId);
                    ReadingQuestions.Remove(question);
                    MessageBox.Show("Reading question deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private async Task ImportReadingLessonFromJsonFile() {

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
        private async Task ImportReadingQuestionFromJsonFile() {
            if (selectedReadingLesson != null) {
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
                            foreach (var readingQuestion in readingQuestions) {
                                readingQuestion.ReadingLessonId = selectedReadingLesson.ReadingLessonId;
                            }
                            await readingQuestionService.AddListAsync(readingQuestions);

                            MessageBox.Show($"✅ Đã thêm {readingQuestions.Count} câu hỏi đọc vào cơ sở d�� liệu.",
                                            "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            ReadingQuestions = new ObservableCollection<ReadingQuestion>(readingQuestionService.GetAll().Result);
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
            else {
                MessageBox.Show("Please select a lesson to add questions");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
