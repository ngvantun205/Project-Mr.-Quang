using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;
using TDEduEnglish.DomainModels;

namespace TDEduEnglish.ViewModels.WindowViewModel {
    internal class ListeningViewModel : Bindable, INotifyPropertyChanged {
        private readonly AppNavigationService _appNavigationService;
        private readonly IListeningService _listeningService;
        private readonly ISessonService _sessonService;
        private readonly IListeningQuestionService _listeningQuestionService;
        private readonly IUserListeningResultService _userListeningResultService;

        public MediaPlayer? mediaPlayer { get; set; } = new MediaPlayer();

        public string Title { get; set; }
        public string Level { get; set; }
        public bool _isSubmitted { get; set; }   
        public ListeningLesson listeningLesson { get; set; }
        private string isPlaying;
        public string IsPlaying {get => isPlaying;set {Set(ref isPlaying, value);OnPropertyChanged(nameof(IsPlaying));}}
        private double currentposition;
        public double CurrentPosition { get => currentposition; set { Set(ref currentposition, value); OnPropertyChanged(nameof(CurrentPosition)); } }
        private double totalduration;
        public double TotalDuration {get => totalduration;set { Set(ref totalduration, value); OnPropertyChanged(nameof(TotalDuration)); }  }
        private double volume;
        public double Volume {get => volume; set {volume = value; mediaPlayer.Volume = volume; OnPropertyChanged(); } }


        public DispatcherTimer _timer { get; set; }
        private int _elapsedSeconds;

        private ObservableCollection<ListeningQuestion> questions;
        public ObservableCollection<ListeningQuestion> Questions {get => questions;set { if (value != null) Set(ref questions, value); else questions = new ObservableCollection<ListeningQuestion>(); } }

        public ICommand PlayPauseCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand Previous5sCommand { get; set; }
        public ICommand Next5sCommand { get; set; }
        public ICommand SubmitCommand { get; set; }

        public ListeningViewModel(AppNavigationService navigationService, IListeningService listening, ISessonService sesson, IListeningQuestionService listeningQuestionService, IUserListeningResultService userListeningResultService) {
            _appNavigationService = navigationService;
            _listeningService = listening;
            _sessonService = sesson;
            _listeningQuestionService = listeningQuestionService;
            _userListeningResultService = userListeningResultService;

            ListeningLesson? listeninglesson = _sessonService.GetCurrentListening();
            if (listeninglesson != null) {
                listeningLesson = listeninglesson;
                Questions = new ObservableCollection<ListeningQuestion>(_listeningQuestionService.GetByListeningId(listeninglesson.ListeningLessonId).Result);
                Title = listeninglesson.Title;
                Level = listeninglesson.Level;
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, listeninglesson.ListeningAudioPath);
                mediaPlayer.Open(new Uri(fullPath, UriKind.Absolute));
                mediaPlayer.Play();
                IsPlaying = "⏸️";

                StartTimer();
            }

            PlayPauseCommand = new RelayCommand(o => PlayPause());
            ExitCommand = new RelayCommand(o => Exit());
            Previous5sCommand = new RelayCommand(o => Previous5s());
            Next5sCommand = new RelayCommand( o => Next5s());
            SubmitCommand = new RelayCommand(async o => await SubmitAnswers());
        }
        public string TimeRemaining {
            get {
                var totalSeconds = (int)(listeningLesson.SuggestedTime?.TotalSeconds ?? 0);
                int remain = Math.Max(totalSeconds - _elapsedSeconds, 0);
                return $"{remain / 60:D2}:{remain % 60:D2}";
            }
        }

        private void StartTimer() {
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (s, e) => {
                if (mediaPlayer.NaturalDuration.HasTimeSpan) {
                    TotalDuration = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                    CurrentPosition = mediaPlayer.Position.TotalSeconds;
                }
            };
            _timer.Start();
        }
        private void PlayPause() {
            if (IsPlaying == "⏸️") {
                mediaPlayer.Pause();
                IsPlaying = "Play";
            }
            else { mediaPlayer.Play(); IsPlaying = "⏸️"; }
        }
        
        private void Exit() {
            mediaPlayer.Stop();
            _appNavigationService.NavigateToUserWindow();
        }
        private void Previous5s() {
            mediaPlayer.Position = mediaPlayer.Position - TimeSpan.FromSeconds(5);
            CurrentPosition = mediaPlayer.Position.TotalSeconds;
        }
        private void Next5s() {
            mediaPlayer.Position = mediaPlayer.Position + TimeSpan.FromSeconds(5);
            CurrentPosition = mediaPlayer.Position.TotalSeconds;
        }
        private async Task SubmitAnswers() {
            if (_isSubmitted) return;
            _isSubmitted = true;
            int correct = 0;
            foreach (var q in Questions) {
                string? selected = null;
                if (q.IsOption1Selected) selected = q.Option1;
                else if (q.IsOption2Selected) selected = q.Option2;
                else if (q.IsOption3Selected) selected = q.Option3;
                else if (q.IsOption4Selected) selected = q.Option4;

                if (selected != null && selected == q.CorrectAnswer) {
                    correct++;
                }
            }

            int score = (int)Math.Round((double)correct / Questions.Count * 100);

            var user = _sessonService.GetCurrentUser();
            var result = new UserListeningResult {
                UserId = user.UserId,
                ListeningLessonId = listeningLesson.ListeningLessonId,
                Score = score,
                CompletedAt = DateTime.Now
            };

            await _listeningService.SaveResult(result);

            System.Windows.MessageBox.Show(
                $"You answered {correct}/{Questions.Count} questions 🏆.\nScore: {score}%",
                "Listening Result"
            );
            mediaPlayer.Stop();
            _appNavigationService.HideCurrentWindow();
            
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
