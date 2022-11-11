using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

using CirclesManagement.Classes;
using CirclesManagement.Components;

namespace CirclesManagement.AssociateDirectorPages
{
    /// <summary>
    /// Логика взаимодействия для TeachersPage.xaml
    /// </summary>
    public partial class TeachersPage : Page
    {
        public ObservableCollection<Teacher> Teachers
        {
            get { return (ObservableCollection<Teacher>)GetValue(TeachersProperty); }
            set { SetValue(TeachersProperty, value); }
        }
        public static readonly DependencyProperty TeachersProperty =
            DependencyProperty.Register("Teachers", typeof(ObservableCollection<Teacher>), typeof(TeachersPage), new PropertyMetadata(null));

        public TeachersPage()
        {
            InitializeComponent();

            MainWindow.db.Teachers.Load();
            Teachers = MainWindow.db.Teachers.Local;

            cvTeachers = CollectionViewSource.GetDefaultView(TeacherList.ItemsSource);

            SearchBox.TextChanged += cvTeachers.Refresh;
        }

        #region Searching and filtering
        private ICollectionView cvTeachers;

        private bool showDismissedTeachers = false;

        private void cvsTeachers_Filter(object sender, FilterEventArgs e)
        {
            var teacher = e.Item as Teacher;

            if (showDismissedTeachers == true && teacher.IsWorking == false)
                e.Accepted = true;
            else if (showDismissedTeachers == false && teacher.IsWorking == true)
                e.Accepted = true;
            else
                e.Accepted = false;
            if (e.Accepted)
            {
                if (SearchBox.IsEmpty())
                {
                    e.Accepted = true;
                    return;
                }
                var searchText = SearchBox.SearchText.Trim().ToLower();
                if (teacher.LastName.ToLower().Contains(searchText)
                    || teacher.FirstName.ToLower().Contains(searchText)
                    || teacher.Patronymic.ToLower().Contains(searchText))
                    e.Accepted = true;
                else
                    e.Accepted = false;
            }
        }
        #endregion

        #region Editing handling
        private string oldEditingValue = "";

        private void TeacherList_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (e.EditingEventArgs.Source.GetType() == typeof(TextBlock))
                oldEditingValue = (e.EditingEventArgs.Source as TextBlock).Text;
        }

        private void TeacherList_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var newValue = (e.EditingElement as TextBox).Text.Trim();

                if (oldEditingValue == newValue)
                    return;

                var editingTeacher = e.Row.Item as Teacher;

                var accept = HandlePropertyEditing(editingTeacher, newValue);
                if (accept == false)
                    e.Cancel = true;
            }
        }

        private bool HandlePropertyEditing(Teacher editingTeacher, string newValue)
        {
            if (string.IsNullOrEmpty(newValue))
            {
                TeacherList.CancelEdit();
                MainWindow.StatusBar.Error("Поле не может быть пустым.");
                return false;
            }
            else if (Helpers.ContainsOnlyRussianLetters(newValue) == false)
            {
                TeacherList.CancelEdit();
                MainWindow.StatusBar.Error("Поле должно содержать только русские буквы.");
                return false;
            }

            // since there are no or only one circle in the list, it's not neccessary to check for title duplicate
            if (Teachers.Count <= 1)
                return true;

            var areThereValueDuplicate = Teachers.
                Any(teacher => teacher.LastName == editingTeacher.LastName
                    && teacher.FirstName == editingTeacher.FirstName
                    && teacher.Patronymic == editingTeacher.Patronymic);

            if (areThereValueDuplicate)
            {
                TeacherList.CancelEdit();
                MainWindow.StatusBar.Error("Учитель с такими данными уже существует.");
                return false;
            }

            return true;
        }
        #endregion

        #region Button handlers
        private void BtnDismissTeacher_Click(object sender, RoutedEventArgs e)
        {
            List<Teacher> selectedTeachers = TeacherList.SelectedItems.Cast<Teacher>().ToList();
            if (selectedTeachers.Count < 0)
            {
                MainWindow.StatusBar.Info("Выберите хотя бы одного учителя для удаления.");
                return;
            }

            var result = Helpers.AskQuestion(
                $"Вы уверены, что хотите удалить " +
                $"выбранн{(selectedTeachers.Count > 1 ? "ых" : "ого")} " +
                $"учител{(selectedTeachers.Count > 1 ? "ей" : "я")}?");
            if (result == false)
                return;

            selectedTeachers.ForEach(teacher => {
                var isPresentInTimetable = teacher.Timetables.Count != 0;
                if (isPresentInTimetable)
                {
                    MessageBox.Show($"Учителя {teacher.FullName} нельзя уволить, так как он указан в расписании занятий.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                teacher.IsWorking = false;
            });
            cvTeachers.Refresh();
            MainWindow.StatusBar.Info($"Учител{(selectedTeachers.Count > 1 ? "ей" : "я")} успешно удал{(selectedTeachers.Count > 1 ? "ены" : "ён")}.");
        }

        private void BtnSaveChangesInDB_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.db.ChangeTracker.HasChanges() == false)
            {
                MainWindow.StatusBar.Info("Нет изменений для сохранения.");
                return;
            }

            var result = Helpers.AskQuestion("Вы уверены, что хотите сохранить изменения?");
            if (result == true)
            {
                MainWindow.db.SaveChanges();
                MainWindow.StatusBar.Info("Изменения успешно сохранены.");
            }
        }
        #endregion

        #region Wroking and deleted Teachers show buttons click handlers
        private void ToggleButtonsVisibilities()
        {
            BtnShowDismissedTeachers.Visibility
                = BtnDismissTeacher.Visibility
                    = BtnDismissTeacher.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

            BtnShowWorkingTeachers.Visibility
                = BtnShowWorkingTeachers.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ToggleShowWorkingOrDismissedTeachers(object sender, RoutedEventArgs e)
        {
            ToggleButtonsVisibilities();
            showDismissedTeachers = showDismissedTeachers == false ? true : false;
            SearchBox.Clear();
            cvTeachers.Refresh();
        }
        #endregion
    }
}
