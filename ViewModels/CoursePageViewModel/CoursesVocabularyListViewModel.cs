﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.ViewModels.CoursePageViewModel {
    internal class CoursesVocabularyListViewModel {
        private readonly AppNavigationService _navigationService;
        private readonly IVocabularyService _vocabularyService;  
        private readonly IVocabularyRepository _vocabularyRepository;
        private readonly ISessonService _sessonService;

        public ObservableCollection<Vocabulary> BeginnerVocabulary { get; set; } = new();
        public ObservableCollection<Vocabulary> IntermediateVocabulary { get; set; } = new();
        public ObservableCollection<Vocabulary> AdvancedVocabulary { get; set; } = new();
        public CoursesVocabularyListViewModel(AppNavigationService navigationService, IVocabularyService vocabularyService, IVocabularyRepository vocabularyRepository, ISessonService sessonService) {
            _navigationService = navigationService;
            _vocabularyService = vocabularyService;
            _vocabularyRepository = vocabularyRepository;
            _sessonService = sessonService;

            LoadData();
        }
        private async void LoadData() {

            var beginner = await _vocabularyService.GetByLevelTopic("Beginner", _sessonService.GetCurrentTopic());
            var intermediate = await _vocabularyService.GetByLevelTopic("Intermediate", _sessonService.GetCurrentTopic());
            var advanced = await _vocabularyService.GetByLevelTopic("Advanced", _sessonService.GetCurrentTopic());

            BeginnerVocabulary.Clear();
            foreach (var v in beginner) BeginnerVocabulary.Add(v);

            IntermediateVocabulary.Clear();
            foreach (var v in intermediate) IntermediateVocabulary.Add(v);

            AdvancedVocabulary.Clear();
            foreach (var v in advanced) AdvancedVocabulary.Add(v);

            OnPropertyChanged(nameof(BeginnerVocabulary));
            OnPropertyChanged(nameof(IntermediateVocabulary));
            OnPropertyChanged(nameof(AdvancedVocabulary));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
