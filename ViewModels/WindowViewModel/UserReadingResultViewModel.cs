using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.ViewModels.WindowViewModel {
    internal class UserReadingResultViewModel {
        private readonly AppNavigationService _navigationService;
        private readonly ISessonService _sessonService;
        private readonly IUserReadingResultRepository _userReadingResultRepository;

        public IEnumerable<UserReadingResult> Results { get; set; }

        public int Score { get; set; }
        public DateTime CompletedAt { get; set; }
        public UserReadingResultViewModel(AppNavigationService navigationService, ISessonService sessonService, IUserReadingResultRepository userReadingResultRepository) {
            _navigationService = navigationService;
            _sessonService = sessonService;
            _userReadingResultRepository = userReadingResultRepository;
            
            Results = _userReadingResultRepository.GetByUserId(_sessonService.GetCurrentUser()!.UserId).Result;
        }

    }
}
