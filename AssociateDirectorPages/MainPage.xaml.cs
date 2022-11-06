using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

using CirclesManagement.Classes;

namespace CirclesManagement.AssociateDirectorPages
{
    /// <summary>
    /// Логика взаимодействия для AssociateDirectorMainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void BtnGoToRegisterTeacherPage_Click(object sender, RoutedEventArgs e)
        {
            Navigation.Next(("Страница регистрации учителя", new RegisterTeacherPage()));
        }
    }
}

//var q = from p in MainWindow.db.Pupils
//        join g in MainWindow.db.Grades on p.GradeID equals g.ID
//        select new
//        {
//            LastName = p.LastName,
//            FirstName = p.FirstName,
//            Patronymic = p.Patronymic,
//            Grade = g.Title
//        };
//ObservableCollection<Pupil> EditingPupilList = (ObservableCollection<Pupil>)q.AsEnumerable();

//DGPupilList_CBGrades.ItemsSource = MainWindow.db.Grades.Select(g => g.Title).ToList();

//MainWindow.db.Teachers.Load();
//var EditingTeacherList = MainWindow.db.Teachers.Local.ToBindingList();
//DGTeacherList.ItemsSource = EditingTeacherList;

//DGTimetable.ItemsSource = MainWindow.db.Timetables.Select(t => t.Time).ToList();
//DGTimetable_CBClassrooms.ItemsSource = MainWindow.db.Classrooms.Select(c => c.Title).ToList();
//DGTimetable_CBTeachers.ItemsSource = MainWindow.db.Teachers.ToList().Select(t => $"{t.LastName} {t.FirstName.Substring(0,1)}. {t.Patronymic.Substring(0,1)}.").ToList();
//DGTimetable_CBCircles.ItemsSource = MainWindow.db.Circles.Select(c => c.Title).ToList();
//DGTimetable_CBWeekDays.ItemsSource = MainWindow.db.WeekDays.Select(wd => wd.Title).ToList();