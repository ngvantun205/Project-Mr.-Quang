using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.Services;

namespace TDEduEnglish.ViewModels {
    internal class Leaderboard {
       private readonly AppNavigationService _navigationService;
        public Leaderboard(AppNavigationService navigationService) {
            _navigationService = navigationService;
        }
    }
}
