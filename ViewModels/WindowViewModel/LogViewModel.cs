using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TDEduEnglish.Views.Pages;

namespace TDEduEnglish.ViewModels.WindowViewModel {
    public class LogViewModel : Bindable, INotifyPropertyChanged {
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

        private bool isSuccessPopupVisible;
        public bool IsSuccessPopupVisible {
            get => isSuccessPopupVisible; set {
                Set(ref isSuccessPopupVisible, value);
                OnPropertyChanged(nameof(IsSuccessPopupVisible));
            }
        }
        private bool isRegisterErrorPopupVisible;
        public bool IsRegisterErrorPopupVisible {
            get => isRegisterErrorPopupVisible; set {
                Set(ref isRegisterErrorPopupVisible, value);
                OnPropertyChanged(nameof(IsRegisterErrorPopupVisible));
            }
        }
        private bool isLoginErrorPopupVisible;
        public bool IsLoginErrorPopupVisible {
            get => isLoginErrorPopupVisible; set {
                Set(ref isLoginErrorPopupVisible, value);
                OnPropertyChanged(nameof(IsLoginErrorPopupVisible));
            }
        }
        private string successtext;
        public string SuccessText {
            get => successtext; set {
                Set(ref successtext, value);
                OnPropertyChanged(nameof(SuccessText));
            }
        }
        private string registeErrorText;
        public string RegisterErrorText {
            get => registeErrorText; set {
                Set(ref registeErrorText, value);
                OnPropertyChanged(nameof(RegisterErrorText));
            }
        }
        private string loginErrorText;
        public string LoginErrorText {
            get => loginErrorText; set {
                Set(ref loginErrorText, value);
                OnPropertyChanged(nameof(LoginErrorText));
            }
        }



        public string LoginEmail { get; set; }
        public string LoginPassword { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand ContinueCommand { get; set; }
        public ICommand TryAgainCommand { get; set; }


        public LogViewModel(AppNavigationService navigationService, IRepository<User> userRepository, IUserService userService, ISessonService sessonService, IAuthService authService) {
            _navigationService = navigationService;
            _userRepository = userRepository;
            _userService = userService;
            _sessonService = sessonService;
            _authService = authService;


            LoginCommand = new RelayCommand(async (parameter) => await Login());
            RegisterCommand = new RelayCommand(async (o) => await Register());
            CloseCommand = new RelayCommand((o) => Application.Current.Shutdown());
            TryAgainCommand = new RelayCommand(o => TryAgain());
            ContinueCommand = new RelayCommand(o => Continue());

            //_userRepository.Add(new User {
            //    FullName = "Admin",
            //    Email = "admin@gmail.com",
            //    PasswordHash = "12345678",
            //    Role = "SuperAdmin"
            //});
        }
        private void Continue() {
            if (_sessonService.CurrentUser == null) {
                _navigationService?.NavigateToLogWindow();
                return;
            }
            if (_sessonService.CurrentUser.Role == "User") {
                _navigationService?.NavigateToUserWindow();
            }
            else if (_sessonService.CurrentUser.Role == "SuperAdmin") {
                _navigationService?.NavigateToSuperAdminWindow();
            }

        }

        private async Task Login() {
            var user = await _authService.Login(LoginEmail, LoginPassword);
            if (string.IsNullOrWhiteSpace(LoginEmail) || string.IsNullOrWhiteSpace(LoginPassword)) {
                LoginErrorText = "Please fill in all required fields.";
                IsLoginErrorPopupVisible = true;
                return;
            }
            if (LoginEmail.IndexOf('@') == -1 || LoginEmail.IndexOf('.') == -1) {
                LoginErrorText = "Please enter a valid email address.";
                IsLoginErrorPopupVisible = true;
                return;
            }
            if (LoginPassword.Length < 8) {
                LoginErrorText = "Password must be at least 8 characters long.";
                IsLoginErrorPopupVisible = true;
                return;
            }
            if (user != null && user.Role == "User") {
                _sessonService.SetCurrentUser(user);
                SuccessText = $"Login successful!, Welcome back {user.FullName}";
                IsSuccessPopupVisible = true;
            }
            else if (user != null && user.Role == "SuperAdmin") {
                _sessonService.SetCurrentUser(user);
                SuccessText = $"Login successful!, Welcome back {user.FullName}";
                IsSuccessPopupVisible = true;
            }
            else {
                LoginErrorText = "Invalid email or password.";
                IsLoginErrorPopupVisible = true;
            }

        }
        private void TryAgain() {
            IsLoginErrorPopupVisible = false;
            IsRegisterErrorPopupVisible = false;
        }
        private async Task Register() {
            if (AcceptTerms) {
                if (string.IsNullOrWhiteSpace(FullName) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(ConfirmPassword)) {
                    RegisterErrorText = "Please fill in all required fields.";
                    IsRegisterErrorPopupVisible = true;
                    return;
                }
                if (Email.IndexOf('@') == -1 || Email.IndexOf('.') == -1) {
                    RegisterErrorText = "Please enter a valid email address.";
                    IsRegisterErrorPopupVisible = true;
                    return;
                }
                if (string.IsNullOrEmpty(PhoneNumber)) {
                    RegisterErrorText = "Phone number is required.";
                    IsRegisterErrorPopupVisible = true;
                    return;
                }
                if (Password.Length < 8) {
                    RegisterErrorText = "Password must be at least 8 characters long.";
                    IsRegisterErrorPopupVisible = true;
                    return;
                }
                if (Password != ConfirmPassword) {
                    RegisterErrorText = "Passwords do not match.";
                    IsRegisterErrorPopupVisible = true;
                    return;
                }
                var existingUser = await _userRepository.GetAll();

                if (existingUser.Any(u => u.Email.Equals(Email, StringComparison.OrdinalIgnoreCase))) {
                    RegisterErrorText = "An account with this email already exists.";
                    IsRegisterErrorPopupVisible = true;
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
                SuccessText = "Registration successful! You can now log in.";
                IsSuccessPopupVisible = true;
            }
            else {
                RegisterErrorText = "You must accept the terms and conditions to register.";
                IsRegisterErrorPopupVisible = true;
                return;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
