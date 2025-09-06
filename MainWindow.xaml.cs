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

namespace TDEduEnglish {

    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            var navigationService = new AppNavigationService(MainFrame);

            this.DataContext = new HomeViewModel(navigationService);
        }
      
    }
}
