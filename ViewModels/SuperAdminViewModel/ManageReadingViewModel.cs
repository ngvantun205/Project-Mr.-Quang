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
        public ICommand CancelAddByAICommand { get; set; }
        public ICommand GenerateReadingByAICommand { get; set; }
        public ICommand AddReadingLessonByAICommand { get; set; }
        public ICommand AddReadingQuestionByAICommand { get; set; }
        public ICommand CancelAddQuestionByAICommand { get; set; }
        public ICommand GenerateReadingQuestionByAICommand { get; set; }


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
                OnPropertyChanged(nameof(SelectedReadingLesson));
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
        private bool isAddByAIPopupVisible;
        public bool IsAddByAIPopupVisible {
            get => isAddByAIPopupVisible;
            set { Set(ref isAddByAIPopupVisible, value); OnPropertyChanged(); }
        }

        private string aiTopic;
        public string AITopic {
            get => aiTopic;
            set { Set(ref aiTopic, value); OnPropertyChanged(); }
        }

        private string aiLevel;
        public string AILevel {
            get => aiLevel;
            set { Set(ref aiLevel, value); OnPropertyChanged(); }
        }

        private string aiSuggestedTime;
        public string AISuggestedTime {
            get => aiSuggestedTime;
            set { Set(ref aiSuggestedTime, value); OnPropertyChanged(); }
        }
        private bool isAddQuestionByAIPopupVisible;
        public bool IsAddQuestionByAIPopupVisible {
            get => isAddQuestionByAIPopupVisible;
            set { Set(ref isAddQuestionByAIPopupVisible, value); OnPropertyChanged(); }
        }

        private string aiQuestionCount;
        public string AIQuestionCount {
            get => aiQuestionCount;
            set { Set(ref aiQuestionCount, value); OnPropertyChanged(); }
        }
        public List<string> Levels { get; set; } = new() { "Beginner", "Intermediate", "Advanced" };
        public ReadingQuestion SelectedReadingQuestion { get; set; }

        public ManageReadingViewModel(AppNavigationService appNavigationService, IReadingService readingService, ISessonService sessonService, IReadingQuestionService readingQuestionService) {
            this._navigationService = appNavigationService;
            this.readingService = readingService;
            this.sessonService = sessonService;
            this.readingQuestionService = readingQuestionService;

            ImportReadingLessonCommand = new RelayCommand(async o => await ImportReadingLessonFromJsonFile());
            DeleteReadingLessonCommand = new RelayCommand(async o => await DeleteReadingLesson(SelectedReadingLesson));
            AddReadingLessonCommand = new RelayCommand(async o => await AddReadingLesson());
            UpdateReadingLessonCommand = new RelayCommand(async o => await UpdateReadingLesson(SelectedReadingLesson));

            ImportReadingQuestionCommand = new RelayCommand(async o => await ImportReadingQuestionFromJsonFile());
            DeleteReadingQuestionCommand = new RelayCommand(async o => await DeleteReadingQuestion(SelectedReadingQuestion));
            AddReadingQuestionCommand = new RelayCommand(async o => await AddReadingQuestion());
            UpdateReadingQuestionCommand = new RelayCommand(async o => await UpdateReadingQuestion(SelectedReadingQuestion));

            AddReadingLessonByAICommand = new RelayCommand(o => IsAddByAIPopupVisible = true);
            CancelAddByAICommand = new RelayCommand(o => IsAddByAIPopupVisible = false);
            GenerateReadingByAICommand = new RelayCommand(async o => await GenerateReadingByAI());

            AddReadingQuestionByAICommand = new RelayCommand(o => IsAddQuestionByAIPopupVisible = true);
            CancelAddQuestionByAICommand = new RelayCommand(o => IsAddQuestionByAIPopupVisible = false);
            GenerateReadingQuestionByAICommand = new RelayCommand(async o => await GenerateReadingQuestionByAI());

            _ = LoadData();
        }
        private async Task GenerateReadingQuestionByAI() {
            try {
                if (SelectedReadingLesson == null) {
                    MessageBox.Show("⚠️ Please select a reading lesson before generating questions by AI.",
                                    "Missing Lesson", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(AIQuestionCount) || !int.TryParse(AIQuestionCount, out int numQuestions) || numQuestions <= 0) {
                    MessageBox.Show("⚠️ Please enter a valid number of questions.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                IsAddQuestionByAIPopupVisible = false;

                MessageBox.Show("🧠 AI is generating questions... Please wait.", "Generating", MessageBoxButton.OK, MessageBoxImage.Information);

                await readingQuestionService.GenerateQuestionsAsync(SelectedReadingLesson, numQuestions);

                await LoadData();

                MessageBox.Show("✅ Questions generated and saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex) {
                MessageBox.Show($"❌ Error while generating questions: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task GenerateReadingByAI() {
            try {
                if (string.IsNullOrWhiteSpace(AITopic) || string.IsNullOrWhiteSpace(AILevel) || string.IsNullOrWhiteSpace(AISuggestedTime)) {
                    MessageBox.Show("⚠️ Please fill all fields before generating.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!double.TryParse(AISuggestedTime, out double minutes)) {
                    MessageBox.Show("⚠️ Suggested time must be a number.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                IsAddByAIPopupVisible = false;

                MessageBox.Show("🧠 AI is generating lessons... Please wait.", "Generating", MessageBoxButton.OK, MessageBoxImage.Information);

                await readingService.GenerateReadingAsync(AITopic, AILevel, TimeSpan.FromMinutes(minutes));

                await LoadData();

                MessageBox.Show("✅ Lessons generated and saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex) {
                MessageBox.Show($"❌ Error while generating lessons: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadData() {
            var lessons = await readingService.GetAll();
            ReadingLessons = lessons != null ? new ObservableCollection<ReadingLesson>(lessons) : new ObservableCollection<ReadingLesson>();
            var questions = await readingQuestionService.GetAll();
            ReadingQuestions = questions != null ? new ObservableCollection<ReadingQuestion>(questions) : new ObservableCollection<ReadingQuestion>();
        }
        private async Task AddReadingLesson() {
            var lesson = new ReadingLesson() {
                Title = "Reading",
                Content = "",
                Level = "Beginner",
            };
            await readingService.Add(lesson);
            await LoadData();
        }
        private async Task AddReadingQuestion() {
            if (SelectedReadingLesson == null) {
                MessageBox.Show("Please select a reading lesson before adding a question.");
                return;
            }

            var question = new ReadingQuestion {
                ReadingLessonId = SelectedReadingLesson.ReadingLessonId,
                QuestionNumber = ReadingQuestions.Count + 1
            };

            ReadingQuestions.Add(question);
            await readingQuestionService.Add(question);
        }

        private async Task UpdateReadingLesson(object? o) {
            if (o is ReadingLesson readingLesson) {
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
