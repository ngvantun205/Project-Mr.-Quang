using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using TDEduEnglish.Views.Pages;

namespace TDEduEnglish.ViewModels.WindowViewModel {
    class SuperAdminViewModel {
        private readonly AppNavigationService appNavigationService;
        private readonly IUserService userService;
        private readonly IVocabularyService vocabularyService;  
        private readonly ISessonService sessonService;
        private readonly IReadingService readingService;

        public ICommand LogoutCommand { get; set; }
        public ICommand AddVocabularyCommand { get; set; }
        public ICommand ManageReadingCommand { get; set; }
        public ICommand ManageListeningCommand { get; set; }
        public ICommand ManageUserCommand { get; set; }

        public SuperAdminViewModel(AppNavigationService appNavigationService, ISessonService sessonService, IUserService userService, IVocabularyService vocabularyService, IReadingService readingService) {
            this.appNavigationService = appNavigationService;
            this.sessonService = sessonService;
            this.userService = userService;
            this.vocabularyService = vocabularyService;
            this.readingService = readingService;

            LogoutCommand = new RelayCommand( o => Logout());
            AddVocabularyCommand = new RelayCommand(async o =>await AddVocabularyFromJsonFile());
            ManageReadingCommand = new RelayCommand(o => appNavigationService.NavigateToManageReadingWindow());
            ManageListeningCommand = new RelayCommand( o => appNavigationService.NavigateToManageListeningWindow());
            ManageUserCommand = new RelayCommand(o => appNavigationService.NavigateToManageUserWindow());   
        }
        private void Logout() {
            sessonService.Logout();
            appNavigationService.NavigateToLogWindow();
        }

        private async Task AddVocabularyFromJsonFile() {
            var openFileDialog = new OpenFileDialog {
                Title = "Select a JSON file",
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true) {
                string filePath = openFileDialog.FileName;
                try {
                    // Đọc nội dung JSON từ file
                    string jsonContent = await File.ReadAllTextAsync(filePath);

                    // Parse JSON thành List<Vocabulary>
                    var vocabularies = JsonSerializer.Deserialize<List<Vocabulary>>(jsonContent);

                    if (vocabularies != null && vocabularies.Count > 0) {
                        // Gọi AddListAsync vào service/repository
                        await vocabularyService.AddListAsync(vocabularies);

                        MessageBox.Show($"✅ Đã thêm {vocabularies.Count} từ vựng vào cơ sở dữ liệu.",
                                        "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
