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

        private Constants.GradeNumerationType _numerationType;

        public GradesPage()
        {
            MainWindow.db.Pupils.Load();

            InitializeComponent();
            
            Loaded += (s, e) =>
            {
                _numerationType = (Constants.GradeNumerationType)MainWindow.ApplicationSetting.GradeNumerationTypeID;
                if (_numerationType == Constants.GradeNumerationType.Number)
                    GradeList.Columns[1].Visibility = Visibility.Collapsed;
                else
                    GradeList.Columns[1].Visibility = Visibility.Visible;
            };

            MainWindow.db.Grades.Load();
            Grades = MainWindow.db.Grades.Local;

            cvGrades = CollectionViewSource.GetDefaultView(GradeList.ItemsSource);

            SearchBox.TextChanged += cvGrades.Refresh;
        }

        #region Searching
        private ICollectionView cvGrades;

        private void cvsGrades_Filter(object sender, FilterEventArgs e)
        {
            var grade = e.Item as Grade;

            if (SearchBox.IsEmpty())
                e.Accepted = true;
            else if (($"{grade.Number}{grade.Letter}").ToLower().Contains(SearchBox.SearchText.Trim().ToLower()))
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
                var column = e.Column as DataGridBoundColumn;
                var property = (column.Binding as Binding).Path.Path;
                var newValue = (e.EditingElement as TextBox).Text.Trim();

                if (oldEditingValue == newValue)
                    return;

                switch (property)
                {
                    case "Number":
                        var accept = HandleNumberEditing(oldEditingValue, newValue);
                        if (accept == false)
                            e.Cancel = true;
                        break;
                    case "Letter":
                        var number = (e.Row.Item as Grade).Number;
                        accept = HandleLetterEditing(oldEditingValue, newValue.ToUpper(), number);
                        if (accept == false)
                            e.Cancel = true;
                        break;
                }
            }
        }

        private bool HandleNumberEditing(string oldNumberStr, string newNumberStr)
        {
            if (!int.TryParse(newNumberStr, out var newNumber) || newNumber <= 0)
            {
                GradeList.CancelEdit();
                MainWindow.StatusBar.Error("Номер класса должен быть целым числом, большим нуля.");
                return false;
            }

            // since there are no or only one grade in the list, it's not neccessary to check for duplicate
            if (Grades.Count <= 1)
                return true;

            if (_numerationType == Constants.GradeNumerationType.Number)
            {
                var areThereGradeNumberDuplicate = Grades.Any(grade => grade.Number == newNumber);
                if (areThereGradeNumberDuplicate)
                {
                    GradeList.CancelEdit();
                    MainWindow.StatusBar.Error("Такой номер класса уже существует.");
                    return false;
                }
                return true;
            }

            return true;
        }

        private bool HandleLetterEditing(string oldLetter, string newLetter, int number)
        {
            if (string.IsNullOrEmpty(newLetter) || newLetter.Length != 1
                || Helpers.ContainsOnlyRussianLetters(newLetter) == false)
            {
                GradeList.CancelEdit();
                MainWindow.StatusBar.Error("Буква класса должна быть задана одной русской буквой.");
                return false;
            }

            // since there are no or only one grade in the list, it's not neccessary to check for letter duplicate
            if (Grades.Count <= 1)
                return true;

            var areThereGradeDuplicate = Grades.Any(grade => grade.Number == number && grade.Letter == newLetter);

            if (areThereGradeDuplicate)
            {
                GradeList.CancelEdit();
                MainWindow.StatusBar.Error("Класс с таким номером и буквой уже существует.");
                return false;
            }

            return true;
        }
        #endregion

        #region Default grade
        // default grade's properties values used when user adds new grade to list
        private const int defaultNumber = 0;
        private const string defaultLetter = "";

        private bool IsDefaultGrade(Grade grade)
            => grade.Number == defaultNumber
                || grade.Letter == defaultLetter;

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
            newGrade.Number = defaultNumber;
            newGrade.Letter = defaultLetter;
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

                var areTherePupilsWithThisGrade = MainWindow.db.Pupils
                    .Any(pupil => pupil.Grade.Number == grade.Number && pupil.Grade.Letter == grade.Letter);

                if (areTherePupilsWithThisGrade)
                    MessageBox.Show($"Класс №{grade.Number} {grade.Letter} нельзя удалить, так как он указан у одного или более учников.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    Grades.Remove(grade);
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
                MainWindow.StatusBar.Error($"Укажите номер " +
                    $"{(_numerationType == Constants.GradeNumerationType.Number ? "и букву" : "")} " +
                    $"нового класса перед сохранением изменений.");
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

        public class GradeEqualityComparer : IEqualityComparer<Grade>
        {
            public bool Equals(Grade x, Grade y)
            {
                return x.Number == y.Number && x.Letter == y.Letter;
            }

            public int GetHashCode(Grade obj)
            {
                return obj == null ? 0 : obj.GetHashCode();
            }
        }
        private static readonly GradeEqualityComparer _gradeEqualityComparer = new GradeEqualityComparer();
    }
}
