using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace TDEduEnglish.ViewModels.CoursePageViewModel {
    internal class CourseMyVocabularyViewModel : Bindable, INotifyPropertyChanged {
        private readonly IUserVocabularyService _userVocabularyService;
        private readonly ISessonService _sessonService;

        private bool isempty;
        public bool IsEmpty {
            get => isempty; set {
                Set(ref isempty, value);
                OnPropertyChanged(nameof(IsEmpty));
            }
        }

        private ObservableCollection<UserVocabulary> userVocabularies;
        public ObservableCollection<UserVocabulary> UserVocabularies {
            get => userVocabularies; set {
                Set(ref userVocabularies, value);
                OnPropertyChanged(nameof(UserVocabularies));
            }
        }
        public ICommand DeleteWordCommand { get; set; }
        public CourseMyVocabularyViewModel(IUserVocabularyService userVocabularyService, ISessonService sessonService) {
            _userVocabularyService = userVocabularyService;
            _sessonService = sessonService;

            DeleteWordCommand = new RelayCommand(async o => DeleteWord(o));

            _ = LoadData();
        }
        private async Task LoadData() {
            var uservocab = await _userVocabularyService.GetByUserId(_sessonService.GetCurrentUser().UserId);
            if (uservocab == null) {
                IsEmpty = true;
                return;
            }
            else {
                UserVocabularies = new ObservableCollection<UserVocabulary>(uservocab);
                IsEmpty = false; 
            }
        }
        private async Task DeleteWord(object o) {
            if(o is UserVocabulary vocabulary) {
                var result = MessageBox.Show($"Are you sure you want to remove word '{vocabulary.Word}'? ", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes) {
                    await _userVocabularyService.Delete(vocabulary.UserVocabularyId);
                    _ = LoadData();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
