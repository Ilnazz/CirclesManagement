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
    /// Логика взаимодействия для ClassroomsPage.xaml
    /// </summary>
    public partial class ClassroomsPage : Page
    {
        public ObservableCollection<Classroom> Classrooms
        {
            get { return (ObservableCollection<Classroom>)GetValue(ClassroomsProperty); }
            set { SetValue(ClassroomsProperty, value); }
        }
        public static readonly DependencyProperty ClassroomsProperty =
            DependencyProperty.Register("Classrooms", typeof(ObservableCollection<Classroom>), typeof(ClassroomsPage), new PropertyMetadata(null));

        public ClassroomsPage()
        {
            InitializeComponent();

            MainWindow.db.Classrooms.Load();
            Classrooms = MainWindow.db.Classrooms.Local;

            cvClassrooms = CollectionViewSource.GetDefaultView(ClassroomList.ItemsSource);

            SearchBox.TextChanged += cvClassrooms.Refresh;
        }

        #region Searching
        private ICollectionView cvClassrooms;

        private void cvsClassrooms_Filter(object sender, FilterEventArgs e)
        {
            var classroom = e.Item as Classroom;

            if (SearchBox.IsEmpty())
                e.Accepted = true;
            else if (classroom.Number.ToString().Contains(SearchBox.SearchText.Trim().ToLower())
                || classroom.Title.ToLower().Contains(SearchBox.SearchText.Trim().ToLower()))
                e.Accepted = true;
            else
                e.Accepted = false;
        }
        #endregion

        #region Editing handling
        private string oldEditingValue; // classroom number/title at beginning of editing

        private void ClassroomList_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
            => oldEditingValue = (e.EditingEventArgs.Source as TextBlock).Text;

        private void ClassroomList_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var column = e.Column as DataGridBoundColumn;
                var property = (column.Binding as Binding).Path.Path;

                var editingCellTB = e.EditingElement as TextBox;
                editingCellTB.Text = editingCellTB.Text.Trim();

                switch (property)
                {
                    case "Number":
                        var accept = HandleNumberEditing(oldEditingValue, editingCellTB.Text);
                        if (accept == false)
                            e.Cancel = true;
                        break;
                    case "Title":
                        HandleTitleEditing(oldEditingValue, editingCellTB.Text);
                        break;
                }
            }
        }

        private bool HandleNumberEditing(string oldNumberStr, string newNumberStr)
        {
            // return, if there are no changes in the number
            if (oldNumberStr == newNumberStr)
                return true;

            if (!int.TryParse(newNumberStr, out var newNumber) || newNumber < 0)
            {
                ClassroomList.CancelEdit();
                MainWindow.StatusBar.Error("Номер кабинета должен быть целым неотрицательным числом.");
                return false;
            }

            // since there are no or only one classroom in the list, it's not neccessary to check for number duplicate
            if (Classrooms.Count <= 1)
                return true;

            var areThereNumberDuplicates = Classrooms.Any(classroom => classroom.Number == newNumber);
            if (areThereNumberDuplicates)
            {
                ClassroomList.CancelEdit();
                MainWindow.StatusBar.Error("Кабинет с таким номером уже существует.");
                return false;
            }

            return true;
        }

        private bool HandleTitleEditing(string oldTitle, string newTitle)
        {
            // return, if there are no changes in the title
            if (oldTitle == newTitle)
                return true;

            if (string.IsNullOrEmpty(newTitle))
            {
                ClassroomList.CancelEdit();
                MainWindow.StatusBar.Error("Название кабинета не может быть пустым.");
                return false;
            }
            else if (Helpers.ContainsOnlyRussianLetters(newTitle) == false)
            {
                ClassroomList.CancelEdit();
                MainWindow.StatusBar.Error("Название кабинета должно содержать только русские буквы.");
                return false;
            }

            // since there are no or only one classroom in the list, it's not neccessary to check for title duplicate
            if (Classrooms.Count <= 1)
                return true;

            var areThereTitleDuplicates = Classrooms.Any(classroom => classroom.Title == newTitle);
            if (areThereTitleDuplicates)
            {
                ClassroomList.CancelEdit();
                MainWindow.StatusBar.Error("Кабинет с таким названием уже существует.");
                return false;
            }

            return true;
        }
        #endregion

        #region Default classroom
        // default classroom's properties values used when user adds new classroom to list
        private const int defaultNumber = 0;
        private const string defaultTitle = "Новый кабинет";

        private bool IsDefaultClassroom(Classroom classroom)
            => classroom.Number == defaultNumber
                || classroom.Title == defaultTitle;

        private bool AreThereDefaultClassroom()
            => Classrooms.Any(classroom => IsDefaultClassroom(classroom));
        #endregion

        #region Buttons click handlers
        private void BtnAddClassroom_Click(object sender, RoutedEventArgs e)
        {
            if (AreThereDefaultClassroom())
            {
                MainWindow.StatusBar.Error("Для добавления ещё одного нового кабинета, отредактируйте предыдущий.");
                return;
            }

            Classroom newClassroom = new Classroom();
            newClassroom.Number = defaultNumber;
            newClassroom.Title = defaultTitle;
            Classrooms.Add(newClassroom);

            ClassroomList.SelectedIndex = ClassroomList.Items.Count - 1;

            ClassroomList.ScrollIntoView(newClassroom);
        }

        private void BtnDeleteClassroom_Click(object sender, RoutedEventArgs e)
        {
            List<Classroom> selectedClassrooms = ClassroomList.SelectedItems.Cast<Classroom>().ToList();
            if (selectedClassrooms.Count < 0)
            {
                MainWindow.StatusBar.Info("Выберите хотя бы один кабинет для удаления.");
                return;
            }

            var result = Helpers.AskQuestion(
                $"Вы уверены, что хотите удалить " +
                $"выбранн{(selectedClassrooms.Count > 1 ? "ые" : "ый")} " +
                $"кабинет{(selectedClassrooms.Count > 1 ? "ы" : "")}?");
            if (result == false)
                return;

            selectedClassrooms.ForEach(classroom => {
                var isPresentInTimetable = classroom.Timetables.Count != 0;
                if (isPresentInTimetable)
                    MessageBox.Show($"Кабинет №{classroom.Number} нельзя удалить, так как он указан в расписании занятий.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    Classrooms.Remove(classroom);
            });
            cvClassrooms.Refresh();
            MainWindow.StatusBar.Info($"Кабинет{(selectedClassrooms.Count > 1 ? "ы" : "")} успешно удал{(selectedClassrooms.Count > 1 ? "ены" : "ён")}.");
        }

        private void BtnSaveChangesInDB_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.db.ChangeTracker.HasChanges() == false)
            {
                MainWindow.StatusBar.Info("Нет изменений для сохранения.");
                return;
            }
            else if (AreThereDefaultClassroom())
            {
                MainWindow.StatusBar.Error("Укажите номер и название нового кабинета перед сохранением изменений.");
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
    }
}
