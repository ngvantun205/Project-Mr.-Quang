using Microsoft.Win32;
using Mscc.GenerativeAI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Security.RightsManagement;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TDEduEnglish.ViewModels.SuperAdminViewModel {
    public class ManageQuizViewModel : Bindable, INotifyPropertyChanged {
        private readonly AppNavigationService _navigationService;
        private readonly IQuizService _quizService;
        private readonly IQuizQuestionService _quizQuestionService;

        private ObservableCollection<Quiz> quizzes;
        public ObservableCollection<Quiz> Quizzes {
            get => quizzes; set {
                Set(ref quizzes, value);
                OnPropertyChanged(nameof(Quizzes));
            }
        }
        private ObservableCollection<QuizQuestion> quizQuestions;
        public ObservableCollection<QuizQuestion> QuizQuestions {
            get => quizQuestions; set {
                Set(ref quizQuestions, value);
                OnPropertyChanged(nameof(QuizQuestions));
            }
        }
        private Quiz? selectedQuiz;
        public Quiz? SelectedQuiz {
            get => selectedQuiz; set {
                Set(ref selectedQuiz, value);
                OnPropertyChanged(nameof(SelectedQuiz));
                if(SelectedQuiz != null) {
                    QuizQuestions = new ObservableCollection<QuizQuestion>(_quizQuestionService.GetByQuizId(SelectedQuiz.QuizId).Result);
                    Title = SelectedQuiz.Title;
                    Topic = SelectedQuiz.Topic;
                    Level = SelectedQuiz.Level;
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
        private string topic;
        public string Topic {
            get => topic; set {
                Set(ref topic, value);
                OnPropertyChanged(nameof(Topic));
            }
        }
        public QuizQuestion? SelectedQuizQuestion { get; set; }
        public ICommand ImportQuizCommand { get; set; }
        public ICommand AddQuizCommand { get; set; }
        public ICommand DeleteQuizCommand { get; set; }
        public ICommand UpdateQuizCommand { get; set; }
        public ICommand ImportQuizQuestionCommand { get; set; }
        public ICommand AddQuizQuestionCommand { get; set; }
        public ICommand DeleteQuizQuestionCommand { get; set; }
        public ICommand UpdateQuizQuestionCommand { get; set; }

        public ManageQuizViewModel(AppNavigationService navigationService, IQuizService quizService, IQuizQuestionService quizQuestionService) {
            _navigationService = navigationService;
            _quizService = quizService;
            _quizQuestionService = quizQuestionService;

            ImportQuizCommand = new RelayCommand(async o => await ImportQuizFromJsonFile());
            AddQuizCommand = new RelayCommand(async o => await AddQuiz());
            DeleteQuizCommand = new RelayCommand(async o => await DeleteQuiz(SelectedQuiz));
            UpdateQuizCommand = new RelayCommand(async o => await UpdateQuiz(SelectedQuiz));
            ImportQuizQuestionCommand = new RelayCommand(async o => await ImportQuizQuestionFromJsonFile());
            AddQuizQuestionCommand = new RelayCommand(async o => await AddQuizQuestion());
            DeleteQuizQuestionCommand = new RelayCommand(async o => await DeleteQuizQuestion(SelectedQuizQuestion));
            UpdateQuizQuestionCommand = new RelayCommand(async o => await UpdateQuizQuestion(SelectedQuizQuestion));

            _ = LoadData();
        }
        private async Task LoadData() {
            var quizzes = await _quizService.GetAll();
            Quizzes = quizzes != null ? new ObservableCollection<Quiz>(quizzes) : new ObservableCollection<Quiz>();
            var questions = await _quizQuestionService.GetAll();
            QuizQuestions = questions != null ? new ObservableCollection<QuizQuestion>(questions) : new ObservableCollection<QuizQuestion>();
        }
        private async Task AddQuiz() {
            var newQuiz = new Quiz();
            Quizzes.Add(newQuiz);
            await _quizService.Add(newQuiz);
        }
        private async Task AddQuizQuestion() {
            if (SelectedQuiz != null) {
                var newQuestion = new QuizQuestion() {QuizId = SelectedQuiz.QuizId };
                QuizQuestions.Add(newQuestion);
                await _quizQuestionService.Add(newQuestion);
            }
            else
                MessageBox.Show("Please select a quiz to add quiz questions", "Infomation", MessageBoxButton.OK, MessageBoxImage.Information);

        }
        private async Task DeleteQuiz(object o) {
            if (o is Quiz quiz) {
                var result = MessageBox.Show($"Are you sure you want to delete quiz '{quiz.Title}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes) {
                    await _quizService.Delete(quiz.QuizId);
                    MessageBox.Show("Delete successfully", "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
                MessageBox.Show("Please select a quiz to delete");
        }
        private async Task DeleteQuizQuestion(object o) {
            if (o is QuizQuestion question) {
                var result = MessageBox.Show($"Are you sure you want to delete quiz question '{question.QuestionText}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes) {
                    await _quizQuestionService.Delete(question.QuizQuestionId);
                    MessageBox.Show("Delete successfully", "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
                MessageBox.Show("Please select a quiz to delete");
        }
        private async Task UpdateQuiz(object o) {
            if (o is Quiz quiz) {
                await _quizService.Update(quiz);
                MessageBox.Show("Update successfully", "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("Please select a quiz to update", "Updating...", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private async Task UpdateQuizQuestion(object o) {
            if (o is QuizQuestion quiz) {
                await _quizQuestionService.Update(quiz);
                MessageBox.Show("Update successfully", "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("Please select a quiz question to update", "Updating...", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private async Task ImportQuizFromJsonFile() {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a quiz json file";
            openFileDialog.Filter = "JSON files (*.json)|*json|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true) {
                string filepath = openFileDialog.FileName;
                try {
                    string jsonContent = await File.ReadAllTextAsync(filepath);
                    var quizzes = JsonSerializer.Deserialize<ObservableCollection<Quiz>>(jsonContent);
                    if (quizzes != null && quizzes.Count > 0) {
                        await _quizService.AddListAsync(quizzes);
                        MessageBox.Show($"Added {quizzes.Count} quizzes into database");
                        await LoadData();
                    }
                    else
                        MessageBox.Show("⚠️ File JSON rỗng hoặc không đúng định dạng.",
                                        "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (Exception ex) {
                    MessageBox.Show($"❌ Lỗi khi đọc file JSON: {ex.Message}",
                                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private async Task ImportQuizQuestionFromJsonFile() {
            if (SelectedQuiz != null) {
                OpenFileDialog openFileDialog = new OpenFileDialog() {
                    Title = "Select a quiz question json file",
                    Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*"
                };
                if (openFileDialog.ShowDialog() == true) {
                    string filepath = openFileDialog.FileName;
                    try {
                        string jsonContent = await File.ReadAllTextAsync(filepath);
                        var questions = JsonSerializer.Deserialize<ObservableCollection<QuizQuestion>>(jsonContent);
                        if (questions != null && questions.Count > 0) {
                            foreach (var q in questions)
                                q.QuizId = SelectedQuiz.QuizId;
                            await _quizQuestionService.AddListAsync(questions);
                            MessageBox.Show($"Added {questions.Count} quiz questions into database");
                            await LoadData();
                        }
                        else
                            MessageBox.Show("⚠️ File JSON rỗng hoặc không đúng định dạng.",
                                            "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    catch (Exception ex) {
                        MessageBox.Show($"❌ Lỗi khi đọc file JSON: {ex.Message}",
                                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
                MessageBox.Show("Please select a quiz to add quiz questions", "Infomation", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyname = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
    }
}
