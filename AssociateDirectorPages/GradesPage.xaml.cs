using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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
    /// Логика взаимодействия для GradesPage.xaml
    /// </summary>
    public partial class GradesPage : Page
    {
        public ObservableCollection<Grade> Grades
        {
            get { return (ObservableCollection<Grade>)GetValue(GradesProperty); }
            set { SetValue(GradesProperty, value); }
        }
        public static readonly DependencyProperty GradesProperty =
            DependencyProperty.Register("Grades", typeof(ObservableCollection<Grade>), typeof(GradesPage), new PropertyMetadata(null));

        public GradesPage()
        {
            MainWindow.db.Pupils.Load();

            InitializeComponent();
            
            MainWindow.db.Grades.Load();
            Grades = MainWindow.db.Grades.Local;

            cvGrades = CollectionViewSource.GetDefaultView(GradeList.ItemsSource);

            SearchBox.TextChanged += cvGrades.Refresh;
        }

        #region Searching
        private ICollectionView cvGrades;

        private bool _showDeleteGrades;

        private void cvsGrades_Filter(object sender, FilterEventArgs e)
        {
            var grade = e.Item as Grade;

            if (SearchBox.IsEmpty())
            {
                e.Accepted = true;
                return;
            }
            else if (grade.Title.Contains(SearchBox.SearchText.Trim().ToLower()))
                e.Accepted = true;
            else
                e.Accepted = false;
        }
        #endregion

        #region Editing handling
        private string oldEditingValue; // grade number/letter at beginning of editing

        private void GradeList_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (e.EditingEventArgs.Source.GetType() == typeof(TextBlock))
                oldEditingValue = (e.EditingEventArgs.Source as TextBlock).Text;
        }

        private void GradeList_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var editingTB = e.EditingElement as TextBox;
                editingTB.Text = editingTB.Text.Trim().ToUpper();
                var newValue = editingTB.Text;

                if (oldEditingValue == newValue)
                    return;

                var accept = HandleTitleEditing(newValue);
                if (accept == false)
                    e.Cancel = true;
            }
        }

        private bool HandleTitleEditing(string newTitle)
        {
            if (Helpers.IsCorrectGradeTitle(newTitle) == false)
            {
                GradeList.CancelEdit();
                MainWindow.StatusBar.Error("Название класса должно быть задано одной/двумя цифрами и одной русской буквой.");
                return false;
            }

            // since there are no or only one grade in the list, it's not neccessary to check for letter duplicate
            if (Grades.Count <= 1)
                return true;

            var isItTitleDuplicate = Grades.Any(grade => grade.Title == newTitle);

            if (isItTitleDuplicate)
            {
                GradeList.CancelEdit();
                MainWindow.StatusBar.Error("Класс с таким названием уже существует.");
                return false;
            }

            return true;
        }
        #endregion

        #region Default grade
        // default grade's properties values used when user adds new grade to list
        private const string defaultTitle = "";

        private bool IsDefaultGrade(Grade grade)
            => grade.Title == defaultTitle;

        private bool AreThereDefaultGrade()
            => Grades.Any(grade => IsDefaultGrade(grade));
        #endregion

        #region Buttons click handlers
        private void BtnAddGrade_Click(object sender, RoutedEventArgs e)
        {
            if (AreThereDefaultGrade())
            {
                MainWindow.StatusBar.Error("Для добавления ещё одного нового класса, отредактируйте предыдущий.");
                return;
            }

            Grade newGrade = new Grade();
            newGrade.Title = defaultTitle;
            Grades.Add(newGrade);

            GradeList.SelectedIndex = GradeList.Items.Count - 1;

            GradeList.ScrollIntoView(newGrade);
        }
        
        private void BtnDeleteGrade_Click(object sender, RoutedEventArgs e)
        {
            List<Grade> selectedGrades = GradeList.SelectedItems.Cast<Grade>().ToList();
            if (selectedGrades.Count < 0)
            {
                MainWindow.StatusBar.Info("Выберите хотя бы один класс для удаления.");
                return;
            }

            var result = Helpers.AskQuestion(
                $"Вы уверены, что хотите удалить " +
                $"выбранн{(selectedGrades.Count > 1 ? "ые" : "ый")} " +
                $"класс{(selectedGrades.Count > 1 ? "ы" : "")}?");
            if (result == false)
                return;

            selectedGrades.ForEach(grade => {

                var isTherePupilWithThisGrade = MainWindow.db.Pupils
                    .Any(pupil => pupil.Grade.Title == grade.Title);

                if (isTherePupilWithThisGrade)
                    MessageBox.Show($"Класс {grade.Title} нельзя удалить, так как он указан у одного или более учеников.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    grade.IsActive = false;
            });
            cvGrades.Refresh();
            MainWindow.StatusBar.Info($"Класс{(selectedGrades.Count > 1 ? "ы" : "")} успешно удал{(selectedGrades.Count > 1 ? "ены" : "ён")}.");
        }

        private void BtnSaveChangesInDB_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.db.ChangeTracker.HasChanges() == false)
            {
                MainWindow.StatusBar.Info("Нет изменений для сохранения.");
                return;
            }
            else if (AreThereDefaultGrade())
            {
                MainWindow.StatusBar.Error($"Укажите название нового класса перед сохранением изменений.");
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

        #region Wroking and deleted grades show buttons click handlers
        private void ToggleButtonsVisibilities()
        {
            BtnShowDeletedGrades.Visibility
                = BtnAddGrade.Visibility
                    = BtnDeleteGrade.Visibility
                        = BtnDeleteGrade.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

            BtnShowWorkingGrades.Visibility
                = BtnRestoreDeletedGrade.Visibility
                    = BtnRestoreDeletedGrade.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ToggleShowActiveOrDeletedGrades(object sender, RoutedEventArgs e)
        {
            ToggleButtonsVisibilities();
            _showDeleteGrades = _showDeleteGrades == false ? true : false;
            SearchBox.Clear();
            cvGrades.Refresh();
        }

        private void BtnRestoreDeletedGrade_Click(object sender, RoutedEventArgs e)
        {
            List<Grade> selectedGrades = GradeList.SelectedItems.Cast<Grade>().ToList();
            if (selectedGrades.Count < 0)
            {
                MainWindow.StatusBar.Info("Выберите хотя бы один класс для восстановления.");
                return;
            }
            var result = Helpers.AskQuestion(
                    $"Вы уверены, что хотите восстановить " +
                    $"выбранн{(selectedGrades.Count > 1 ? "ые" : "ый")} " +
                    $"класс{(selectedGrades.Count > 1 ? "ы" : "")}?");
            if (result == false)
                return;
            selectedGrades.ForEach(grade => grade.IsActive = true);
            cvGrades.Refresh();
            MainWindow.StatusBar.Info($"Класс{(selectedGrades.Count > 1 ? "ы" : "")} успешно восстановлен{(selectedGrades.Count > 1 ? "ы" : "")}.");
        }
        #endregion
    }
}
