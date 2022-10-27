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

using CirclesManagement.Components;

namespace CirclesManagement.Pages
{
    /// <summary>
    /// Логика взаимодействия для AssociateDirectorMainPage.xaml
    /// </summary>
    public partial class AssociateDirectorMainPage : Page
    {
        public AssociateDirectorMainPage()
        {
            InitializeComponent();

            //DGCirclesList.ItemsSource = MainWindow.db.Circles.ToList();

            //DGPupilsList.ItemsSource = MainWindow.db.Pupils.ToList();
            //DGPupilsList_CBGrade.ItemsSource = MainWindow.db.Grades.Select(g => g.Title).ToList();

            //DGTimetable.ItemsSource = MainWindow.db.Timetables.ToList();
            //DGTeacherList.ItemsSource = MainWindow.db.Teachers.ToList();
            DGCirclesList.DataContext = MainWindow.db.Circles.ToList();
        }

        private void BGoToRegisterTeacherPage_Click(object sender, RoutedEventArgs e)
        {
            Navigation.Next(("Страница регистрации учителя", new RegisterTeacherPage()));
        }
    }
}
