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
using TDEduEnglish.AppServices;
using TDEduEnglish.DomainModels;
using TDEduEnglish.IAppServices;

namespace TDEduEnglish.ViewModels.SuperAdminViewModel {
    internal class ManageListeningViewModel : Bindable, INotifyPropertyChanged {
        private readonly AppNavigationService _navigationService;
        private readonly ISessonService _sessonService;
        private readonly IListeningService _listeningService;
        private readonly IListeningQuestionService _listeningQuestionService;
        private readonly IUserListeningResultService _userlisteningResultService;

        private ObservableCollection<ListeningLesson> listeningLessons;

        public ObservableCollection<ListeningLesson> ListeningLessons {
            get => listeningLessons;
            set {
                Set(ref listeningLessons, value);
                OnPropertyChanged(nameof(ListeningLessons));
            }
        }

        private ObservableCollection<ListeningQuestion> listeningQuestions;
        public ObservableCollection<ListeningQuestion> ListeningQuestions {
            get => listeningQuestions;
            set {
                Set(ref listeningQuestions, value);
                OnPropertyChanged(nameof(ListeningQuestions));
            }
        }
        private ListeningLesson _selectedLesson;
        public ListeningLesson SelectedLesson {
            get => _selectedLesson;
            set {
                _selectedLesson = value;
                if (_selectedLesson != null) {
                    ListeningQuestions = new ObservableCollection<ListeningQuestion>(
                        _listeningQuestionService.GetByListeningId(_selectedLesson.ListeningLessonId).Result
                    );
                }
                else {
                    ListeningQuestions = new ObservableCollection<ListeningQuestion>();
                }
                OnPropertyChanged(nameof(SelectedLesson));
            }
        }
        public ListeningQuestion SelectedQuestion { get; set; }
        public ICommand ImportLessonFromJsonCommand { get; set; }
        public ICommand ImportQuestionFromJsonCommand { get; set; }
        public ICommand AddListeningLessonCommand { get; set; }
        public ICommand AddListeningQuestionCommand { get; set; }
        public ICommand DeleteListeningLessonCommand { get; set; }
        public ICommand DeleteListeningQuestionCommand { get; set; }
        public ICommand UpdateListeningLessonCommand { get; set; }
        public ICommand UpdateListeningQuestionCommand { get; set; }    

        public ManageListeningViewModel(AppNavigationService appNavigationService, ISessonService sessonService, IListeningService listeningService, IListeningQuestionService listeningQuestionService, IUserListeningResultService userListeningResultService) {
            _navigationService = appNavigationService;
            _sessonService = sessonService;
            _listeningService = listeningService;
            _listeningQuestionService = listeningQuestionService;
            _userlisteningResultService = userListeningResultService;

            ImportLessonFromJsonCommand = new RelayCommand(async o => await ImportListeningLessonFormJsonFile());
            ImportQuestionFromJsonCommand = new RelayCommand(async o => await ImportListeningQuestionFromJsonFile());
            AddListeningLessonCommand = new RelayCommand(async o => await AddListeningLesson());
            AddListeningQuestionCommand = new RelayCommand(async o => await AddListeningQuestion());
            UpdateListeningLessonCommand = new RelayCommand(async o => await UpdateListeningLesson());
            UpdateListeningQuestionCommand = new RelayCommand(async o => await UpdateListeningQuestion());
            DeleteListeningLessonCommand = new RelayCommand(async o => await DeleteListeningLesson(SelectedLesson));
            DeleteListeningQuestionCommand = new RelayCommand(async o => await DeleteListeningQuestion(SelectedQuestion));

            _ = LoadData();

        }
        private async Task LoadData() {
            var lessons = await _listeningService.GetAll();
            ListeningLessons = lessons != null ? new ObservableCollection<ListeningLesson>(lessons) : new ObservableCollection<ListeningLesson>();  
            var questions = await _listeningQuestionService.GetAll();   
            ListeningQuestions = questions != null ? new ObservableCollection<ListeningQuestion>(questions) : new ObservableCollection<ListeningQuestion>();
        }
        private async Task AddListeningLesson() {
            var lesson = new ListeningLesson {
                Title = "New lesson",
                Level = "Beginner"
            };
            await _listeningService.Add(lesson);
            await LoadData(); 
        }

        private async Task AddListeningQuestion() {
            var question = new ListeningQuestion();
            ListeningQuestions.Add(question);
            await _listeningQuestionService.Add(question);
        }
        private async Task UpdateListeningLesson() {
            foreach(var lesson in ListeningLessons) {
                await _listeningService.Update(lesson);
            }
            await LoadData();
            MessageBox.Show("Update listening lesson successfully");
        }
        private async Task UpdateListeningQuestion() {
            foreach(var question in ListeningQuestions) {
                await _listeningQuestionService.Update(question);
            }
            await LoadData();
            MessageBox.Show("Update listening question successfully");
        }
        private async Task DeleteListeningLesson(object obj) {
            if (obj is ListeningLesson lesson) {
                var result = MessageBox.Show($"Are you sure you want to delete the listening lesson '{lesson.Title}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes) {
                    await _listeningService.Delete(lesson.ListeningLessonId);
                    ListeningLessons.Remove(lesson);
                    MessageBox.Show("Listening lesson deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        private async Task DeleteListeningQuestion(object o) {
            if(o is ListeningQuestion question) {
                var result = MessageBox.Show($"Are you sure you want to delete the listening question '{question.QuestionText}' ?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes) {
                    await _listeningQuestionService.Delete(question.ListeningQuestionId);
                    ListeningQuestions.Remove(question);
                    MessageBox.Show("Listening question deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        private async Task ImportListeningLessonFormJsonFile() {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select your Json file";
            openFileDialog.Filter = "Json files (*.json)|*.json|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true) {
                string filepath = openFileDialog.FileName;
                try {
                    string jsonContent = await File.ReadAllTextAsync(filepath);
                    var listeninglesson = JsonSerializer.Deserialize<List<ListeningLesson>>(jsonContent);
                    if (listeninglesson != null && listeninglesson.Count > 0) {
                        await _listeningService.AddListAsync(listeninglesson);

                        MessageBox.Show($"✅ Đã thêm {listeninglesson.Count} bài đọc vào cơ sở dữ liệu.",
                                           "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        ListeningLessons = new ObservableCollection<ListeningLesson>(_listeningService.GetAll().Result);
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
        private async Task ImportListeningQuestionFromJsonFile() {
            if (SelectedLesson != null) {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Select your json file";
                openFileDialog.Filter = "Json files (*.json)|*.json|All files  (*.*)|*.*";
                if (openFileDialog.ShowDialog() == true) {
                    string filepath = openFileDialog.FileName;
                    try {
                        string jsoncontent = await File.ReadAllTextAsync(filepath);
                        var listeningQuestion = JsonSerializer.Deserialize<List<ListeningQuestion>>(jsoncontent);
                        if (listeningQuestion != null && listeningQuestion.Count > 0) {
                            foreach(var question in listeningQuestion) {
                                question.ListeningLessonId = SelectedLesson.ListeningLessonId;
                            }
                            await _listeningQuestionService.AddListAsync(listeningQuestion);
                            MessageBox.Show($"✅ Đã thêm {listeningQuestion.Count} bài đọc vào cơ sở dữ liệu.",
                                              "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            ListeningQuestions = new ObservableCollection<ListeningQuestion>(_listeningQuestionService.GetAll().Result);
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
                MessageBox.Show("Plese select a lesson to add questions", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

