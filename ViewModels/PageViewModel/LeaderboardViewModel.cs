using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.Services;

namespace TDEduEnglish.ViewModels {
    internal class LeaderboardViewModel {
       private readonly AppNavigationService _navigationService;
        public LeaderboardViewModel(AppNavigationService navigationService) {
            _navigationService = navigationService;
        }
    }
}
