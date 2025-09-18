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
using TDEduEnglish.ViewModels.WindowViewModel;

namespace TDEduEnglish.Views.Windows {
    /// <summary>
    /// Interaction logic for SuperAdminWindow.xaml
    /// </summary>
    public partial class SuperAdminWindow : Window {
        public SuperAdminWindow() {
            InitializeComponent();
            this.DataContext = App.Provider?.GetRequiredService<SuperAdminViewModel>();
        }
    }
}
