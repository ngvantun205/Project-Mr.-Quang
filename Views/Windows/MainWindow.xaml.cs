using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TDEduEnglish.Services;
using TDEduEnglish.ViewModels;
using TDEduEnglish.Views.Pages;

namespace TDEduEnglish {

    public partial class MainWindow : Window {
        private readonly AppNavigationService _navigationService;

        public MainWindow(AppNavigationService navigationService) {
            InitializeComponent();

            _navigationService = navigationService;
            _navigationService.SetFrame(MainFrame);

            this.DataContext = App.Provider?.GetRequiredService<HomeViewModel>();
            _navigationService.NavigateTo<HomePage>();
        }
    }

}
