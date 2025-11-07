using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TDEduEnglish.ViewModels.SuperAdminViewModel {
    internal class ManageUserViewModel : Bindable, INotifyPropertyChanged {
        private readonly AppNavigationService _navigationService;
        private readonly IUserService _userService;

        private ObservableCollection<User> users;
        public ObservableCollection<User> Users {
            get => users; set {
                Set(ref users, value);
                OnPropertyChanged(nameof(Users));
            }
        }
        private User selectedUser;
        public User SelectedUser { get => selectedUser; set {
                Set(ref selectedUser, value);
            }
        }

        public ICommand AddUserCommand { get; set; }
        public ICommand DeleteUserCommand { get; set; }
        public ICommand UpdateUserCommand { get; set; }
        public ManageUserViewModel(AppNavigationService navigationService, IUserService userService) {
            _navigationService = navigationService;
            _userService = userService;

            AddUserCommand = new RelayCommand(async o => await AddUser());
            DeleteUserCommand = new RelayCommand(async o => await DeleteUser(SelectedUser));
            UpdateUserCommand = new RelayCommand(async o => await UpdateUser());

             _ = LoadData();
        }
        private async Task LoadData() {
            var userlist = await _userService.GetAll();
            if (userlist != null) {
                Users = new ObservableCollection<User>(userlist);
            }
            else Users = new ObservableCollection<User>();
        }
        private async Task AddUser() {
            var newuser = new User() { Role = "User", PasswordHash = "12345678"};
            await _userService.Add(newuser);
            ;
            await LoadData();
        }
        private async Task DeleteUser(object o) {
            if(o is  User user) {
                var result = MessageBox.Show($"Are you sure you want to delete user '{user.FullName}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if(result == MessageBoxResult.Yes) {
                    await _userService.Delete(user.UserId);
                    Users.Remove(user);
                    MessageBox.Show("Delete user successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else {
                MessageBox.Show("Please select user to delete");
            }
        }
        private async Task UpdateUser() {
            foreach(var user in Users) {
                await _userService.Update(user);
            }
            await LoadData();
            MessageBox.Show("Update user successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyname = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
