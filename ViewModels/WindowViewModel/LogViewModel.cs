using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TDEduEnglish.Views.Pages;

namespace TDEduEnglish.ViewModels.WindowViewModel
{
    public class LogViewModel {
        private readonly AppNavigationService _navigationService;
        private readonly IRepository<User> _userRepository;
        private readonly IUserService _userService;
        private readonly ISessonService _sessonService;
        private readonly IAuthService _authService;


        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }
        public bool AcceptTerms { get; set; }

        

        public string LoginEmail { get; set; }
        public string LoginPassword { get; set; }

        public ICommand LoginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand CloseCommand { get; set; }


        public LogViewModel(AppNavigationService navigationService, IRepository<User> userRepository, IUserService userService, ISessonService sessonService, IAuthService authService) {
            _navigationService = navigationService;
            _userRepository = userRepository;
            _userService = userService;
            _sessonService = sessonService;
            _authService = authService;

            LoginCommand = new RelayCommand(async (parameter) => await Login());
            RegisterCommand = new RelayCommand(async (o) => await Register());
            CloseCommand = new RelayCommand((o) => Application.Current.Shutdown());

            //_userRepository.Add(new User {
            //    FullName = "Admin",
            //    Email = "admin@gmail.com",
            //    PasswordHash = "123456",
            //    Role = "SuperAdmin"
            //});
        }

        private async Task Login() {
            var user = await _authService.Login(LoginEmail, LoginPassword);
            if (user != null && user.Role == "User") {
                _sessonService.SetCurrentUser(user);
                MessageBox.Show($"Login successful!, Hello {user.FullName}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _navigationService?.NavigateToUserWindow();
            }
            else if (user != null && user.Role == "SuperAdmin") {
                _sessonService.SetCurrentUser(user);
                MessageBox.Show($"Login successful!, Hello Mr {user.FullName}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _navigationService?.NavigateToSuperAdminWindow();
            }
            else {
                MessageBox.Show("Invalid email or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private async Task Register() {
            if (AcceptTerms) {
                if (string.IsNullOrWhiteSpace(FullName) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(ConfirmPassword)) {
                    MessageBox.Show("Please fill in all required fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (Password != ConfirmPassword) {
                    MessageBox.Show("Passwords do not match.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var existingUser = await _userRepository.GetAll();
               
                if (existingUser.Any(u => u.Email.Equals(Email, StringComparison.OrdinalIgnoreCase))) {
                    MessageBox.Show("Email is already registered.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var newUser = new User {
                    FullName = FullName,
                    Email = Email,
                    PasswordHash = Password,
                    Role = "User",
                    JoinDate = DateTime.Now,
                    Level = "Beginner",
                    DateOfBirth = DateTime.Now,
                    PhoneNumber = PhoneNumber
                };

                await _userRepository.Add(newUser);
                MessageBox.Show($"Registration successful, Hello Mr {FullName}! You can now log in.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _navigationService?.NavigateToLogWindow();
            }
            else {
                MessageBox.Show("You must accept the terms and conditions to register.", "Error", MessageBoxButton.OK, MessageBoxImage.Error); return;
            }
        }
    }
}
