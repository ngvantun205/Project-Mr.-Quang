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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TDEduEnglish.Services;
using TDEduEnglish.ViewModels;

namespace TDEduEnglish.Views.Pages
{
    public partial class QuizzesPage : Page
    {
        public QuizzesPage(AppNavigationService navigationService) {
            InitializeComponent();
            this.DataContext = App.Provider?.GetRequiredService<QuizzesViewModel>();
        }
    }
}
