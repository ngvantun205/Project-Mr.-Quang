using System;
using System.Collections.Generic;
using System.Collections.ObjectModel; 
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TDEduEnglish.DomainModels; 
using TDEduEnglish.Services;

namespace TDEduEnglish.ViewModels {
    public class LeaderboardEntry {
        public int Rank { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = new User(); 
        public int ThisWeekScore { get; set; }
        public int ThisMonthScore { get; set; }
        public int AllTimeScore { get; set; }
    }

    internal class LeaderboardViewModel : Bindable, INotifyPropertyChanged {
        private readonly AppNavigationService _navigationService;
        private readonly IUserScoreService _userScoreService;
        private readonly ISessonService _sessionService;

        private LeaderboardEntry currentUserScore = new LeaderboardEntry();
        public LeaderboardEntry CurrentUserScore {
            get => currentUserScore;
            set {
                Set(ref currentUserScore, value);
                OnPropertyChanged(nameof(CurrentUserScore));
            }
        }

        private int rankWeek;
        public int RankWeek {
            get => rankWeek;
            set {
                Set(ref rankWeek, value);
                OnPropertyChanged(nameof(RankWeek));
            }
        }

        private int rankMonth;
        public int RankMonth {
            get => rankMonth;
            set {
                Set(ref rankMonth, value);
                OnPropertyChanged(nameof(RankMonth));
            }
        }

        private int rankAllTime;
        public int RankAllTime {
            get => rankAllTime;
            set {
                Set(ref rankAllTime, value);
                OnPropertyChanged(nameof(RankAllTime));
            }
        }

        public User CurrentUser { get; set; }
        public string FullName { get; set; }

        private ObservableCollection<LeaderboardEntry> thisWeek;
        public ObservableCollection<LeaderboardEntry> ThisWeek {
            get => thisWeek;
            set {
                Set(ref thisWeek, value);
                OnPropertyChanged(nameof(ThisWeek));
            }
        }

        private ObservableCollection<LeaderboardEntry> thisMonth;
        public ObservableCollection<LeaderboardEntry> ThisMonth {
            get => thisMonth;
            set {
                Set(ref thisMonth, value);
                OnPropertyChanged(nameof(ThisMonth));
            }
        }

        private ObservableCollection<LeaderboardEntry> allTime;
        public ObservableCollection<LeaderboardEntry> AllTime {
            get => allTime;
            set {
                Set(ref allTime, value);
                OnPropertyChanged(nameof(AllTime));
            }
        }

        public LeaderboardViewModel(AppNavigationService navigationService, IUserScoreService userScoreService, ISessonService sessionService) {
            _navigationService = navigationService;
            _userScoreService = userScoreService;
            _sessionService = sessionService;

            _ = LoadData();
        }

        private async Task LoadData() {
            CurrentUser = _sessionService.GetCurrentUser();
            FullName = CurrentUser.FullName;
            var allScores = (await _userScoreService.GetAll()).ToList();

            var currentUserScoreDb = allScores.FirstOrDefault(s => s.UserId == CurrentUser.UserId);
            if (currentUserScoreDb == null) {
                currentUserScoreDb = new UserScore {
                    UserId = CurrentUser.UserId,
                    User = CurrentUser,
                    AllTimeScore = 0,
                    ThisMonthScore = 0,
                    ThisWeekScore = 0
                };
                allScores.Add(currentUserScoreDb);
            }


            var rankedWeek = allScores.OrderByDescending(s => s.ThisWeekScore)
                                      .Select((s, index) => new LeaderboardEntry {
                                          Rank = index + 1,
                                          UserId = s.UserId,
                                          User = s.User,
                                          ThisWeekScore = s.ThisWeekScore,
                                          ThisMonthScore = s.ThisMonthScore,
                                          AllTimeScore = s.AllTimeScore
                                      }).ToList();

            var rankedMonth = allScores.OrderByDescending(s => s.ThisMonthScore)
                                       .Select((s, index) => new LeaderboardEntry {
                                           Rank = index + 1,
                                           UserId = s.UserId,
                                           User = s.User,
                                           ThisWeekScore = s.ThisWeekScore,
                                           ThisMonthScore = s.ThisMonthScore,
                                           AllTimeScore = s.AllTimeScore
                                       }).ToList();

            var rankedAllTime = allScores.OrderByDescending(s => s.AllTimeScore)
                                         .Select((s, index) => new LeaderboardEntry {
                                             Rank = index + 1,
                                             UserId = s.UserId,
                                             User = s.User,
                                             ThisWeekScore = s.ThisWeekScore,
                                             ThisMonthScore = s.ThisMonthScore,
                                             AllTimeScore = s.AllTimeScore
                                         }).ToList();

            ThisWeek = new ObservableCollection<LeaderboardEntry>(rankedWeek.Take(10));
            while (ThisWeek.Count < 3)
                ThisWeek.Add(new LeaderboardEntry { Rank = 0, User = new User { FullName = "—" } });

            ThisMonth = new ObservableCollection<LeaderboardEntry>(rankedMonth.Take(10));
            while (ThisMonth.Count < 3)
                ThisMonth.Add(new LeaderboardEntry { Rank = 0, User = new User { FullName = "—" } });

            AllTime = new ObservableCollection<LeaderboardEntry>(rankedAllTime.Take(10));
            while (AllTime.Count < 3)
                AllTime.Add(new LeaderboardEntry { Rank = 0, User = new User { FullName = "—" } });

            var currentUserWeekEntry = rankedWeek.First(u => u.UserId == CurrentUser.UserId);
            var currentUserMonthEntry = rankedMonth.First(u => u.UserId == CurrentUser.UserId);
            var currentUserAllTimeEntry = rankedAllTime.First(u => u.UserId == CurrentUser.UserId);


            CurrentUserScore = currentUserWeekEntry;

            RankWeek = currentUserWeekEntry.Rank;
            RankMonth = currentUserMonthEntry.Rank;
            RankAllTime = currentUserAllTimeEntry.Rank;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}