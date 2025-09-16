using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TDEduEnglish.Services;

namespace TDEduEnglish.ViewModels {
    internal class LoginViewModel {
        private readonly AppNavigationService _navigationService;
        private readonly IRepository<User> _userRepository;
        private readonly IUserService _userService;
        private readonly ISessonService _sessonService;
        private readonly IAuthService _authService;

        public string Email { get; set; }
        public string Password { get; set; }

        public ICommand LoginCommand { get; set; }
        public LoginViewModel(AppNavigationService navigationService, IRepository<User> userRepository, IUserService userService, ISessonService sessonService, IAuthService authService) {
            _navigationService = navigationService;
            _userRepository = userRepository;
            _userService = userService;
            _sessonService = sessonService;
            _authService = authService;

            LoginCommand = new RelayCommand(async (parameter) => await Login());
        }

        private async Task Login() {
            var user = await _authService.Login(Email, Password);
            if (user != null) {
                _sessonService.SetCurrentUser(user);
               MessageBox.Show($"Login successful!, Hello {user.FullName}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else {
                MessageBox.Show("Invalid email or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
