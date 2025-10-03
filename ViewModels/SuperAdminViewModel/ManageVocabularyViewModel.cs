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
    internal class ManageVocabularyViewModel : Bindable, INotifyPropertyChanged {
        private readonly AppNavigationService _navigationService;
        private readonly IVocabularyService _vocabularyService;

        private ObservableCollection<Vocabulary> vocabularylist;

        public ObservableCollection<Vocabulary> VocabularyList {
            get => vocabularylist; set {
                Set(ref vocabularylist, value);
                OnPropertyChanged(nameof(VocabularyList));
            }
        }
        public Vocabulary SelectedVocabulary { get; set; }

        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand ImportFromJsonCommand { get; set; }
        public ManageVocabularyViewModel(AppNavigationService navigationService, IVocabularyService vocabularyService) {
            _navigationService = navigationService;
            _vocabularyService = vocabularyService;

            AddCommand = new RelayCommand(async o => await Add());
            DeleteCommand = new RelayCommand(async o => await Delete(SelectedVocabulary));
            UpdateCommand = new RelayCommand(async o => await Update());
            ImportFromJsonCommand = new RelayCommand(async o => await ImportVocabularyFromJsonFile());

            _ = LoadData();
        }
        private async Task LoadData() {
            var vocab = await _vocabularyService.GetAll();
           VocabularyList = vocab != null ? new ObservableCollection<Vocabulary>(vocab) : new ObservableCollection<Vocabulary>();   
        }
        private async Task Add() {
            var vocab = new Vocabulary();
            VocabularyList.Add(vocab);
            await _vocabularyService.Add(vocab);
        }
        private async Task Delete(object o) {
            if (o is Vocabulary vocab) {
                var result = MessageBox.Show($"Are you sure you want to delete the vocabulary '{vocab.Word}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes) {
                    await _vocabularyService.Delete(vocab.VocabularyId);
                    VocabularyList.Remove(vocab);
                    MessageBox.Show("vocabulary deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
                MessageBox.Show("Please select a vocabulary to delete");
        }
        private async Task Update() {
            foreach (var vocab in VocabularyList) {
                await _vocabularyService.Update(vocab);
            }
            MessageBox.Show("All changes have been saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            await LoadData();
        }
        private async Task ImportVocabularyFromJsonFile() {
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
                        await _vocabularyService.AddListAsync(vocabularies);
                        foreach(var item in vocabularies) VocabularyList.Add(item);

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
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyname = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
