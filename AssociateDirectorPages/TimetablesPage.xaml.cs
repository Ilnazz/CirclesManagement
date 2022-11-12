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
    /// Логика взаимодействия для TimetablesPage.xaml
    /// </summary>
    public partial class TimetablesPage : Page
    {
        public ObservableCollection<WeekDay> WeekDays
        {
            get { return (ObservableCollection<WeekDay>)GetValue(WeekDaysProperty); }
            set { SetValue(WeekDaysProperty, value); }
        }
        public static readonly DependencyProperty WeekDaysProperty =
            DependencyProperty.Register("WeekDays", typeof(ObservableCollection<WeekDay>), typeof(TimetablesPage), new PropertyMetadata(null));

        public ObservableCollection<Timetable> Timetables
        {
            get { return (ObservableCollection<Timetable>)GetValue(TimetablesProperty); }
            set { SetValue(TimetablesProperty, value); }
        }
        public static readonly DependencyProperty TimetablesProperty =
            DependencyProperty.Register("Timetables", typeof(ObservableCollection<Timetable>), typeof(TimetablesPage), new PropertyMetadata(null));

        public TimetablesPage()
        {
            InitializeComponent();

            MainWindow.db.WeekDays.Load();
            WeekDays = MainWindow.db.WeekDays.Local;

            MainWindow.db.Timetables.Load();
            Timetables = MainWindow.db.Timetables.Local;
        }
    }
}
