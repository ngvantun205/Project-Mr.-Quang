using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TDEduEnglish.ViewModels.SuperAdminViewModel {
    public class ManageSpeakingViewModel : Bindable, INotifyPropertyChanged {
        private readonly ISpeakingSentenceService _speakingSentenceService;
        private readonly ITopicService _topicService;

        private ObservableCollection<SpeakingSentence> sentences;
        public ObservableCollection<SpeakingSentence> Sentences {
            get => sentences; set {
                sentences = value;
                OnPropertyChanged(nameof(Sentences));
            }
        }
        private ObservableCollection<Topic> topics;
        public ObservableCollection<Topic> Topics {
            get => topics; set {
                Set(ref topics, value);
                OnPropertyChanged(nameof(Topics));
                
            }
        }
        private SpeakingSentence selectedSentence;
        public SpeakingSentence SelectedSentence {
            get => selectedSentence; set {
                selectedSentence = value;
                OnPropertyChanged(nameof(SelectedSentence));
            }
        }
        private Topic selectedTopic;
        public Topic SelectedTopic {
            get => selectedTopic; set {
                Set(ref selectedTopic, value);
                if (selectedTopic != null) {
                    var getSentences = _speakingSentenceService.GetByTopicId(selectedTopic.TopicId).Result;
                    Sentences = getSentences != null ? new ObservableCollection<SpeakingSentence>(getSentences) : new ObservableCollection<SpeakingSentence>() { };
                }
                OnPropertyChanged(nameof(SelectedTopic));
            }
        }

        public ICommand AddSpeakingSentenceCommand { get; set; }
        public ICommand DeleteSpeakingSentenceCommand { get; set; }
        public ICommand UpdateSpeakingSentenceCommand { get; set; }
        public ICommand ImportSpeakingSentenceFromJsonFileCommand { get; set; }
        public ICommand AddTopicCommand { get; set; }
        public ICommand DeleteTopicCommand { get; set; }
        public ICommand UpdateTopicCommand { get; set; }
        public ICommand ImportTopicFromJsonFileCommand { get; set; }
        public ManageSpeakingViewModel(ISpeakingSentenceService speakingSentenceService, ITopicService topicService) {
            _speakingSentenceService = speakingSentenceService;
            _topicService = topicService;

            AddSpeakingSentenceCommand = new RelayCommand(async o => await Add());
            DeleteSpeakingSentenceCommand = new RelayCommand(async o => await Delete(o));
            UpdateSpeakingSentenceCommand = new RelayCommand(async o => await Update());
            ImportSpeakingSentenceFromJsonFileCommand = new RelayCommand(async o => await ImportJson());
            
            AddTopicCommand = new RelayCommand(async o => await AddTopic());
            DeleteTopicCommand = new RelayCommand(async o => await DeleteTopic(o));
            UpdateTopicCommand = new RelayCommand(async o => await UpdateTopic());
            ImportTopicFromJsonFileCommand = new RelayCommand(async o => await ImportTopicJson());

            _ = LoadData();
        }   
        private async Task LoadData() {
            var sentenceList = await _speakingSentenceService.GetAll();
            Sentences = sentenceList != null ? new ObservableCollection<SpeakingSentence>(sentenceList) : new ObservableCollection<SpeakingSentence>();
            var topicList = await _topicService.GetAll();
            Topics = topicList != null ? new ObservableCollection<Topic>(topicList) : new ObservableCollection<Topic>();
        }
        private async Task Delete(object o) {
            if (o is SpeakingSentence sentence) {
                await _speakingSentenceService.Delete(sentence.SpeakingSentenceId);
                Sentences.Remove(sentence);
            }   
        }
        private async Task Add() {
            var newSentence = new SpeakingSentence {
            };
            await _speakingSentenceService.Add(newSentence);
            Sentences.Add(newSentence);
        }
        private async Task Update() {
            foreach (var sentence in Sentences) {
                await _speakingSentenceService.Update(sentence);
            }
        }
        private async Task ImportJson() {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a json file";
            openFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            if( openFileDialog.ShowDialog() == true) {
                string filepath = openFileDialog.FileName;
                try {
                    string jsonContent = await File.ReadAllTextAsync(filepath);
                    var sentences = JsonSerializer.Deserialize<ObservableCollection<SpeakingSentence>>(jsonContent);
                    if (sentences != null && sentences.Count > 0) {
                        foreach(var sentence in sentences) {
                            sentence.TopicId = SelectedTopic.TopicId;
                            sentence.Level = SelectedTopic.Level;
                        }
                        await _speakingSentenceService.AddListAsync(sentences);
                        MessageBox.Show($"Added {sentences.Count} speaking sentences into database");
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
        private async Task AddTopic() {
            var topic = new Topic();
            Topics.Add(topic);
            await _topicService.Add(topic);
        }
        private async Task DeleteTopic(object o) {
            if (o is Topic topic) {
                await _topicService.Delete(topic.TopicId);
                Topics.Remove(topic);
            }
        }
        private async Task UpdateTopic() {
            foreach (var topic in Topics) {
                await _topicService.Update(topic);
            }
        }
        private async Task ImportTopicJson() {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select your topic json file";
            openFileDialog.Filter = "JSON files (*.json)|*.json| All files (*.*)|*.*";
            if(openFileDialog.ShowDialog() == true) {
                string filepath = openFileDialog.FileName;
                try {
                    var jsoncontent = await File.ReadAllTextAsync(filepath);
                    var newtopics = JsonSerializer.Deserialize<ObservableCollection<Topic>>(jsoncontent);
                    if (newtopics != null && newtopics.Count > 0) {
                        await _topicService.AddListAsync(newtopics);
                        foreach (var topic in newtopics)
                            Topics.Add(topic);
                    }
                    MessageBox.Show("⚠️ File JSON rỗng hoặc không đúng định dạng.",
                                        "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                } 
                catch (Exception ex) {
                    MessageBox.Show($"❌ Lỗi khi đọc file JSON: {ex.Message}",
                                   "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }   
    }
}
