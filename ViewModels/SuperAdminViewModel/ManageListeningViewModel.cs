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
            Set(ref  listeningLessons, value);
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
            }
        }
        public ICommand ImportLessonFromJsonCommand { get; set; }
        public ICommand ImportQuestionFromJsonCommand { get; set; }

        

        public ManageListeningViewModel(AppNavigationService appNavigationService, ISessonService sessonService, IListeningService listeningService, IListeningQuestionService listeningQuestionService, IUserListeningResultService userListeningResultService) {
            _navigationService = appNavigationService;
            _sessonService = sessonService;
            _listeningService = listeningService;
            _listeningQuestionService = listeningQuestionService;
            _userlisteningResultService = userListeningResultService;

            ImportLessonFromJsonCommand = new RelayCommand(async o => await ImportListeningLessonFormJsonFile());
            ImportQuestionFromJsonCommand = new RelayCommand(async o => await ImportListeningQuestionFromJsonFile());

            ListeningLessons = new ObservableCollection<ListeningLesson>(_listeningService.GetAll().Result);

            ListeningQuestions = new ObservableCollection<ListeningQuestion>(_listeningQuestionService.GetAll().Result);    
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
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select your json file";
            openFileDialog.Filter = "Json files (*.json)|*.json|All files  (*.*)|*.*";
            if(openFileDialog.ShowDialog() == true) {
                string filepath = openFileDialog.FileName;
                try {
                    string jsoncontent = await File.ReadAllTextAsync(filepath);
                    var listeningQuestion = JsonSerializer.Deserialize<List<ListeningQuestion>>(jsoncontent);
                    if(listeningQuestion != null && listeningQuestion.Count > 0) {
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

