using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.Services;

namespace TDEduEnglish.ViewModels {
    internal class RegisterViewModel {
        private readonly AppNavigationService _navigationService;
        public RegisterViewModel(AppNavigationService navigationService) => _navigationService = navigationService;
    }
}
