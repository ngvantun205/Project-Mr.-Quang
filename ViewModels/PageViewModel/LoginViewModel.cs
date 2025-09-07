using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.Services;

namespace TDEduEnglish.ViewModels {
    internal class LoginViewModel {
        private readonly AppNavigationService _navigationService;
        public LoginViewModel(AppNavigationService navigationService) => _navigationService = navigationService;
    }
}
