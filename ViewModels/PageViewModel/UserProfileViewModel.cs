using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TDEduEnglish.Services;

namespace TDEduEnglish.ViewModels {
    internal class UserProfileViewModel {
        private readonly AppNavigationService _appNavigationService;
        private readonly IRepository<User> _userRepository;
        private readonly ISessonService _sessionService;
        private readonly IUserService _userService;

        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }


        public ICommand UpdateCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public UserProfileViewModel(AppNavigationService appNavigationService, IRepository<User> userRepository, ISessonService sessionService, IUserService userService) {
            _appNavigationService = appNavigationService;
            _userRepository = userRepository;
            _sessionService = sessionService;
            _userService = userService;
            var currentUser = _sessionService.GetCurrentUser();
            
            if (currentUser != null) {
                FullName = currentUser.FullName;
                Email = currentUser.Email;
                PhoneNumber = currentUser.PhoneNumber;
                DateOfBirth = currentUser.DateOfBirth;
            }


            UpdateCommand = new RelayCommand(async (o) => await UpdateProfile());
            CancelCommand = new RelayCommand((o) => Cancel());
        }

        private async Task UpdateProfile() {
            var currentUser = _sessionService.GetCurrentUser();
            if (currentUser != null) {
                currentUser.FullName = FullName;
                currentUser.PhoneNumber = PhoneNumber;
                currentUser.DateOfBirth = DateOfBirth;
                await _userService.Update(currentUser);
                MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else {
                MessageBox.Show("No user is currently logged in.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel() {
            var currentUser = _sessionService.GetCurrentUser();
            FullName = currentUser?.FullName ?? string.Empty;
            Email = currentUser?.Email ?? string.Empty;
            PhoneNumber = currentUser?.PhoneNumber ?? string.Empty;
            DateOfBirth = currentUser?.DateOfBirth ?? DateTime.MinValue;
        }
    }
}
