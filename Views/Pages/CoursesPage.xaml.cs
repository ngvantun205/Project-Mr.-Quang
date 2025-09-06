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

namespace TDEduEnglish.Views.Pages {
    /// <summary>
    /// Interaction logic for CoursesPage.xaml
    /// </summary>
    public partial class CoursesPage : Page {
        public CoursesPage() {
            InitializeComponent();
        }
        private void CourseTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            var selectedItem = CourseTreeView.SelectedItem;
        }
    }
}
