using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TDEduEnglish.Services;

namespace TDEduEnglish.ViewModels {
    internal class RegisterViewModel {
        private readonly AppNavigationService _navigationService;
        private readonly IUserService _userService;
        private readonly IRepository<User> _userRepository;

        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }
        public bool AcceptTerms { get; set; }

        public ICommand RegisterCommand { get; set; }

        public RegisterViewModel(AppNavigationService navigationService, IUserService userService, IRepository<User> userRepository) {
            _navigationService = navigationService;
            _userService = userService;
            _userRepository = userRepository;

            RegisterCommand = new RelayCommand(async (o) => await Register());
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
                //var existingUser = await _userRepository.GetAll();
                var existingUser = new List<User> {
                    new User { Email = "user1@gmail.com" },
                };
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
                    Level = "Beginner"
                };

                //await _userRepository.Add(newUser);
                MessageBox.Show($"Registration successful, Hello Mr {FullName}! You can now log in.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else {
                MessageBox.Show("You must accept the terms and conditions to register.", "Error", MessageBoxButton.OK, MessageBoxImage.Error); return;
            }
        }
    }
}
