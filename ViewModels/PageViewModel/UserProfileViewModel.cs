using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.Services;

namespace TDEduEnglish.ViewModels {
    internal class UserProfileViewModel {
        private readonly AppNavigationService _appNavigationService;
        public UserProfileViewModel(AppNavigationService appNavigationService) => _appNavigationService = appNavigationService;
    }
}
