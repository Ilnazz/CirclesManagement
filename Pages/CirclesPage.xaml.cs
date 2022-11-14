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

namespace CirclesManagement.Pages
{
    /// <summary>
    /// Логика взаимодействия для CirclesPage.xaml
    /// </summary>
    public partial class CirclesPage : EntitiesPage
    {
        private ObservableCollection<Circle> _circles;

        public CirclesPage()
        {
            InitializeComponent();

            MainWindow.db.Circles.Load();
            _circles = MainWindow.db.Circles.Local;
            CircleList.ItemsSource = _circles.Cast<object>() as ObservableCollection<object>;

            UserPage.SearchBox.TextChanged += CircleList.Refresh;

            CircleList.Filter += (obj) =>
            {
                var circle = obj as Circle;

                var accept = false;
                if (ShowDeletedEntities == true && circle.IsWorking == false
                    || ShowDeletedEntities == false && circle.IsWorking == true)
                    accept = true;
                
                if (accept == true)
                {
                    if (UserPage.SearchBox.IsEmpty()
                        || Helpers.IsMatchSearchText(circle.Title)
                        || Helpers.IsMatchSearchText(circle.MaxNumberOfPupils.ToString()))
                        accept = true;
                    else
                        accept = false;
                }

                return accept;
            };
        }

        #region Editing handling
        private string oldEditingValue = ""; // circle title at beginning of editing

        private void CircleList_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (e.EditingEventArgs.Source.GetType() == typeof(TextBlock))
                oldEditingValue = (e.EditingEventArgs.Source as TextBlock).Text;
        }

        private void CircleList_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var column = e.Column as DataGridBoundColumn;
                var property = (column.Binding as Binding).Path.Path;
                var newValue = (e.EditingElement as TextBox).Text.Trim();

                if (oldEditingValue == newValue)
                    return;

                switch (property)
                {
                    case "Title":
                        var accept = HandleTitleEditing(oldEditingValue, newValue);
                        if (accept == false)
                            e.Cancel = true;
                        break;
                    case "MaxNumberOfPupils":
                        accept = HandleMaxNumberOfPupilsEditing(oldEditingValue, newValue);
                        if (accept == false)
                            e.Cancel = true;
                        break;
                }
            }
        }

        private bool HandleTitleEditing(string oldTitle, string newTitle)
        {
            if (string.IsNullOrEmpty(newTitle))
            {
                CircleList.CancelEdit();
                Helpers.Error("Название кружка не может быть пустым.");
                return false;
            }
            else if (Helpers.ContainsOnlyRussianLetters(newTitle) == false)
            {
                CircleList.CancelEdit();
                Helpers.Error("Название кружка должно содержать только русские буквы.");
                return false;
            }

            // since there are no or only one circle in the list, it's not neccessary to check for title duplicate
            if (_circles.Count <= 1)
                return true;

            var areThereTitleDuplicate = _circles.Any(circle => circle.Title == newTitle);
            if (areThereTitleDuplicate)
            {
                CircleList.CancelEdit();
                Helpers.Error("Кружок с таким названием уже существует.");
                return false;
            }

            return true;
        }

        private bool HandleMaxNumberOfPupilsEditing(string oldNumberStr, string newNumberStr)
        {
            if (!int.TryParse(newNumberStr, out var newNumber) || newNumber <= 0)
            {
                CircleList.CancelEdit();
                Helpers.Error("Макс. число учеников должно быть целым числом, большим нуля.");
                return false;
            }

            return true;
        }
        #endregion

        #region Default circle
        // default circle field values used when user adds new circle to list
        private const string defaultTitle = "Новый кружок";
        private const int defaultMaxNumberOfPupils = 0;
        private const bool defaultIsWorking = true;

        private bool IsDefaultCircle(Circle circle)
        {
            return circle.Title == defaultTitle
                || circle.MaxNumberOfPupils == defaultMaxNumberOfPupils;
        }

        private bool AreThereDefaultCircle()
            => _circles.Any(circle => IsDefaultCircle(circle));
        #endregion

        #region Working circles buttons click handlers
        public override void AddEntity()
        {
            if (AreThereDefaultCircle())
            {
                Helpers.Error("Для добавления ещё одного нового кружка, отредактируйте предыдущий.");
                return;
            }

            Circle newCircle = new Circle();
            newCircle.Title = defaultTitle;
            newCircle.IsWorking = defaultIsWorking;
            newCircle.MaxNumberOfPupils = defaultMaxNumberOfPupils;
            _circles.Add(newCircle);

            CircleList.SelectedIndex = CircleList.Items.Count - 1;

            CircleList.ScrollIntoView(newCircle);
        }

        private void BtnDeleteCircle_Click(object sender, RoutedEventArgs e)
        {
            List<Circle> selectedCircles = CircleList.SelectedItems.Cast<Circle>().ToList();
            if (selectedCircles.Count < 0)
            {
                Helpers.Error("Выберите хотя бы один кружок для удаления.");
                return;
            }

            var result = Helpers.Ask(
                $"Вы уверены, что хотите удалить " +
                $"выбранн{(selectedCircles.Count > 1 ? "ые" : "ый")} " +
                $"круж{(selectedCircles.Count > 1 ? "ки" : "ок")}?");
            if (result == false)
                return;

            selectedCircles.ForEach(circle => {
                if (IsDefaultCircle(circle)) // if it is default circle it's not allowed to mark it as deleted, simple delete it
                    _circles.Remove(circle);
                else
                {
                    var isPresentInTimetable = circle.Timetables.Count != 0;
                    if (isPresentInTimetable)
                    {
                        MessageBox.Show($"Кружок {circle.Title} нельзя удалить, так как он указан в расписании занятий.", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    var isAttendedByPupils = circle.Circle_Pupil.Any(circle_pupil => circle_pupil.IsAttending == true);
                    if (isPresentInTimetable)
                    {
                        MessageBox.Show($"Кружок №{circle.Title} нельзя удалить, так как его посещюат ученики.", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    circle.IsWorking = false;
                }
            });
            cvCircles.Refresh();
            Helpers.Inform($"Круж{(selectedCircles.Count > 1 ? "ки" : "ок")} успешно удал{(selectedCircles.Count > 1 ? "ены" : "ён")}.");
        }

        private void BtnSaveChangesInDB_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.db.ChangeTracker.HasChanges() == false)
            {
                Helpers.Inform("Нет изменений для сохранения.");
                return;
            }
            else if (AreThereDefaultCircle())
            {
                Helpers.Error("Укажите наименование и макс. число учеников для нового кружка перед сохранением изменений.");
                return;
            }

            var result = Helpers.Ask("Вы уверены, что хотите сохранить изменения?");
            if (result == true)
            {
                MainWindow.db.SaveChanges();
                Helpers.Inform("Изменения успешно сохранены.");
            }
        }
        #endregion
    }
}
