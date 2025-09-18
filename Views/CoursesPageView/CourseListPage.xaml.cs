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
using TDEduEnglish.ViewModels.CoursePageViewModel;

namespace TDEduEnglish.Views.CoursesPageView {
    /// <summary>
    /// Interaction logic for CourseListPage.xaml
    /// </summary>
    public partial class CourseListPage : Page {
        public CourseListPage(AppNavigationService appNavigationService) {
            InitializeComponent();
            this.DataContext = App.Provider?.GetRequiredService<CourseListViewModel>();
        }
    }
}
