using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TDEduEnglish.ViewModels.SuperAdminViewModel;

namespace TDEduEnglish.Views.SuperAdminWindow {
    /// <summary>
    /// Interaction logic for ManageSpeakingWindow.xaml
    /// </summary>
    public partial class ManageSpeakingWindow : Window {
        public ManageSpeakingWindow() {
            InitializeComponent();
            this.DataContext = App.Provider?.GetRequiredService<ManageSpeakingViewModel>();
        }
    }
}
