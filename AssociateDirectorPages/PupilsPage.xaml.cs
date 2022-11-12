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
    /// Логика взаимодействия для PupilsPage.xaml
    /// </summary>
    public partial class PupilsPage : Page
    {
        public ObservableCollection<Pupil> Pupils
        {
            get { return (ObservableCollection<Pupil>)GetValue(PupilsProperty); }
            set { SetValue(PupilsProperty, value); }
        }
        public static readonly DependencyProperty PupilsProperty =
            DependencyProperty.Register("Pupils", typeof(ObservableCollection<Pupil>), typeof(PupilsPage), new PropertyMetadata(null));

        public ObservableCollection<Grade> Grades
        {
            get { return (ObservableCollection<Grade>)GetValue(GradesProperty); }
            set { SetValue(GradesProperty, value); }
        }

        public static readonly DependencyProperty GradesProperty =
            DependencyProperty.Register("Grades", typeof(ObservableCollection<Grade>), typeof(PupilsPage), new PropertyMetadata(null));

        public PupilsPage()
        {
            InitializeComponent();

            MainWindow.db.Pupils.Load();
            Pupils = MainWindow.db.Pupils.Local;

            MainWindow.db.Grades.Load();
            Grades = MainWindow.db.Grades.Local;

            cvPupils = CollectionViewSource.GetDefaultView(PupilList.ItemsSource);

            SearchBox.TextChanged += cvPupils.Refresh;
        }

        #region Searching and filtering
        private ICollectionView cvPupils;

        private bool _showExcludedPupils = false;

        private void cvsPupils_Filter(object sender, FilterEventArgs e)
        {
            var pupil = e.Item as Pupil;

            if (SearchBox.IsEmpty())
            {
                e.Accepted = true;
                return;
            }
            var searchText = SearchBox.SearchText.Trim().ToLower();
            if (pupil.LastName.ToLower().Contains(searchText)
                || pupil.FirstName.ToLower().Contains(searchText)
                || pupil.Patronymic.ToLower().Contains(searchText))
                e.Accepted = true;
            else
                e.Accepted = false;
        }
        #endregion

        #region Editing handling
        private string oldEditingValue = "";

        private void PupilList_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (e.EditingEventArgs.Source.GetType() == typeof(TextBlock))
                oldEditingValue = (e.EditingEventArgs.Source as TextBlock).Text;
        }

        private void PupilList_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var newValue = (e.EditingElement as TextBox).Text.Trim();

                if (oldEditingValue == newValue)
                    return;

                var editingPupil = e.Row.Item as Pupil;

                var accept = HandlePropertyEditing(editingPupil, newValue);
                if (accept == false)
                    e.Cancel = true;
            }
        }

        private bool HandlePropertyEditing(Pupil editingPupil, string newValue)
        {
            if (string.IsNullOrEmpty(newValue))
            {
                PupilList.CancelEdit();
                MainWindow.StatusBar.Error("Поле не может быть пустым.");
                return false;
            }
            else if (Helpers.ContainsOnlyRussianLetters(newValue) == false)
            {
                PupilList.CancelEdit();
                MainWindow.StatusBar.Error("Поле должно содержать только русские буквы.");
                return false;
            }

            // since there are no or only one circle in the list, it's not neccessary to check for title duplicate
            if (Pupils.Count <= 1)
                return true;

            var areThereValueDuplicate = Pupils.
                Any(pupil => pupil.LastName == editingPupil.LastName
                    && pupil.FirstName == editingPupil.FirstName
                    && pupil.Patronymic == editingPupil.Patronymic);

            if (areThereValueDuplicate)
            {
                PupilList.CancelEdit();
                MainWindow.StatusBar.Error("Учитель с такими данными уже существует.");
                return false;
            }

            return true;
        }
        #endregion

        #region Default pupil
        private const string defaultName = "";
        private const bool defaultIsStudying = true;

        private bool AreThereDefaultPupil()
            => Pupils.Any(pupil => pupil.LastName == defaultName
                || pupil.FirstName == defaultName
                || pupil.Patronymic == defaultName
                || pupil.IsStudying == defaultIsStudying);
        #endregion

        #region Studying pupils buttons click handlers
        private void BtnAddPupil_Click(object sender, RoutedEventArgs e)
        {
            if (AreThereDefaultPupil())
            {
                MainWindow.StatusBar.Error("Для добавления ещё одного нового ученика, отредактируйте предыдущий.");
                return;
            }

            Pupil newPupil = new Pupil();
            newPupil.FirstName = defaultName;
            newPupil.LastName = defaultName;
            newPupil.Patronymic = defaultName;
            newPupil.IsStudying = defaultIsStudying;

            PupilList.SelectedIndex = PupilList.Items.Count - 1;

            PupilList.ScrollIntoView(newPupil);
        }
        #endregion

        #region Button handlers
        private void BtnExcludePupil_Click(object sender, RoutedEventArgs e)
        {
            List<Pupil> selectedPupils = PupilList.SelectedItems.Cast<Pupil>().ToList();
            if (selectedPupils.Count < 0)
            {
                MainWindow.StatusBar.Info("Выберите хотя бы одного ученика для удаления.");
                return;
            }

            var result = Helpers.AskQuestion(
                $"Вы уверены, что хотите удалить " +
                $"выбранн{(selectedPupils.Count > 1 ? "ых" : "ого")} " +
                $"ученик{(selectedPupils.Count > 1 ? "ов" : "а")}?");
            if (result == false)
                return;

            selectedPupils.ForEach(pupil => {
                var isAttendingAnyCircle = pupil.Circle_Pupil.Any(circle_pupil => circle_pupil.IsAttending == true);
                if (isAttendingAnyCircle)
                {
                    MessageBox.Show($"Ученика {pupil.FullName} нельзя исключить, так как он он посещает один или более кружков.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                pupil.IsStudying = false;
            });
            cvPupils.Refresh();
            MainWindow.StatusBar.Info($"Ученик{(selectedPupils.Count > 1 ? "и" : "")} успешно удал{(selectedPupils.Count > 1 ? "ены" : "ён")}.");
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
            BtnShowExcludedPupils.Visibility
                = BtnAddPupil.Visibility
                    = BtnExcludePupil.Visibility
                        = BtnExcludePupil.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

            BtnShowStudyingPupils.Visibility
                = BtnRestoreExcludedPupil.Visibility
                    = BtnRestoreExcludedPupil.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ToggleShowStudyingOrExcludedPupils(object sender, RoutedEventArgs e)
        {
            ToggleButtonsVisibilities();
            _showExcludedPupils = _showExcludedPupils == false ? true : false;
            SearchBox.Clear();
            cvPupils.Refresh();
        }

        private void BtnRestoreExcludedPupil_Click(object sender, RoutedEventArgs e)
        {
            var selectedPupils = PupilList.SelectedItems.Cast<Pupil>().ToList();
            if (selectedPupils.Count < 0)
            {
                MainWindow.StatusBar.Info("Выберите хотя бы одного ученика для восстановления.");
                return;
            }
            var result = Helpers.AskQuestion(
                    $"Вы уверены, что хотите восстановить " +
                    $"выбранн{(selectedPupils.Count > 1 ? "ых" : "ого")} " +
                    $"ученик{(selectedPupils.Count > 1 ? "ов" : "ка")}?");
            if (result == false)
                return;
            selectedPupils.ForEach(pupil => pupil.IsStudying = true);
            cvPupils.Refresh();
            MainWindow.StatusBar.Info($"круж{(selectedPupils.Count > 1 ? "ки" : "ок")} успешно восстановлены.");
        }
        #endregion
    }
}
