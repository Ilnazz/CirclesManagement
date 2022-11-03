using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

using CirclesManagement.Components;

namespace CirclesManagement.Pages
{
    /// <summary>
    /// Логика взаимодействия для AssociateDirectorMainPage.xaml
    /// </summary>
    public partial class AssociateDirectorMainPage : Page
    {
        public BindingList<Circle> EditingCircleList
        {
            get { return (BindingList<Circle>)GetValue(EditingCircleListProperty); }
            set { SetValue(EditingCircleListProperty, value); }
        }
        public static readonly DependencyProperty EditingCircleListProperty =
            DependencyProperty.Register("EditingCircleList", typeof(BindingList<Circle>), typeof(AssociateDirectorMainPage), new PropertyMetadata(null));

        private const string defaultCircleTitle = "Новый кружок";
        private const bool defaultCircleIsWorking = true;

        public AssociateDirectorMainPage()
        {
            InitializeComponent();

            DGCircleList.CanUserAddRows = false;
            DGCircleList.CanUserDeleteRows = false;
            DGCircleList.EnableColumnVirtualization = true;
            DGCircleList.EnableRowVirtualization = true;

            MainWindow.db.Circles.Load();
            EditingCircleList = MainWindow.db.Circles.Local.ToBindingList();
            DGCircleList.ItemsSource = EditingCircleList;
        }

        private bool AreThereDefaultCircle()
        {
            return false;
        }
        
        private void DGCircleList_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                TextBox editingTB = e.EditingElement as TextBox;
                editingTB.Text = editingTB.Text.Trim();

                if (EditingCircleList.Count == 0) return;

                string editingCircleTitle = editingTB.Text;

                bool areThereCircleWithTheSameTitle = EditingCircleList
                    .Any(circle => circle.Title == editingCircleTitle);

                if (areThereCircleWithTheSameTitle)
                {
                    DGCircleList.CancelEdit();
                    StatusBar.Warning("Кружок с таким названием уже существует!");
                }
            }
        }

        private void BCircleListAddCircle_Click(object sender, RoutedEventArgs e)
        {
            bool isPreviousAddedCircleNotEdited
                = EditingCircleList.Any(circle => circle.Title == defaultCircleTitle
                        && circle.IsWorking == defaultCircleIsWorking);

            if (isPreviousAddedCircleNotEdited) {
                StatusBar.Warning("Для добавления ещё одного нового кружка, отредактируйте предыдущий.");
                return;
            }

            Circle newCircle = EditingCircleList.AddNew();
            newCircle.Title = defaultCircleTitle;
            newCircle.IsWorking = defaultCircleIsWorking;

            DGCircleList.SelectedIndex = DGCircleList.Items.Count - 1;

            DGCircleList.ScrollIntoView(newCircle);
        }

        private void BCircleListDeleteCircle_Click(object sender, RoutedEventArgs e)
        {
            List<Circle> selectedCircles = DGCircleList.SelectedItems.Cast<Circle>().ToList();
            if (selectedCircles.Count > 0)
            {
                string message = $"Вы уверены, что хотите удалить выбранн{(selectedCircles.Count > 1 ? "ые" : "ую")} запи{(selectedCircles.Count > 1 ? "си" : "сь")}?";
                var result = MessageBox.Show(message, "Подтверждение",
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                    selectedCircles.ForEach(c => EditingCircleList.Remove(c));
            }
            else
                StatusBar.Info($"Выберите хотя бы одну запись для удаления.");
        }

        private void BCircleListSaveChangesInDB_Click(object sender, RoutedEventArgs e)
        {
            if (!MainWindow.db.ChangeTracker.HasChanges())
            {
                StatusBar.Info($"Нет изменений для сохранения.");
                return;
            }
            //if ()
            var result = MessageBox.Show("Вы уверены, что хотите сохранить изменения?", "Подтверждение",
                MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                int savedChangesNumber = MainWindow.db.SaveChanges();
                StatusBar.Info($"Изменения успешно сохранены. Произведено записей в БД: {savedChangesNumber}");
            }
        }

        private void BGoToRegisterTeacherPage_Click(object sender, RoutedEventArgs e)
        {
            Navigation.Next(("Страница регистрации учителя", new RegisterTeacherPage()));
        }

    }
}

//EditingCircleList.ListChanged += EditingCircleList_ListChanged;

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