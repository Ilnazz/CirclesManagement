using CirclesManagement.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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

namespace CirclesManagement.AssociateDirectorPages
{
    /// <summary>
    /// Логика взаимодействия для ClassroomListPage.xaml
    /// </summary>
    public partial class ClassroomListPage : Page
    {
        public ObservableCollection<Classroom> ClassroomList
        {
            get { return (ObservableCollection<Classroom>)GetValue(ClassroomListProperty); }
            set { SetValue(ClassroomListProperty, value); }
        }
        public static readonly DependencyProperty ClassroomListProperty =
            DependencyProperty.Register("ClassroomList", typeof(ObservableCollection<Classroom>), typeof(ClassroomListPage), new PropertyMetadata(null));

        public ClassroomListPage()
        {
            InitializeComponent();

            MainWindow.db.Classrooms.Load();
            ClassroomList = MainWindow.db.Classrooms.Local;
        }
    }
}
