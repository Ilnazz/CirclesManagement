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
    /// Логика взаимодействия для CircleListPage.xaml
    /// </summary>
    public partial class CircleListPage : Page
    {
        public ObservableCollection<Circle> CircleList
        {
            get { return (ObservableCollection<Circle>)GetValue(CircleListProperty); }
            set { SetValue(CircleListProperty, value); }
        }
        public static readonly DependencyProperty CircleListProperty =
            DependencyProperty.Register("CircleList", typeof(ObservableCollection<Circle>), typeof(CircleListPage), new PropertyMetadata(null));
        
        public CircleListPage()
        {
            InitializeComponent();

            MainWindow.db.Circles.Load();
            CircleList = MainWindow.db.Circles.Local;
            
            cvCircleList = CollectionViewSource.GetDefaultView(DGCircleList.ItemsSource);

            SearchBox.OnTextChanged += cvCircleList.Refresh;
        }

        #region Filtering realization
        private ICollectionView cvCircleList;

        private bool showWorkingCircles = true;
        
        private void cvsCircleList_Filter(object sender, FilterEventArgs e)
        {
            if (AreThereDefaultCircleInList())
                return;
            if (e.Item is Circle circle)
            {
                if (showWorkingCircles == true && circle.IsWorking == true)
                    e.Accepted = true;
                else if (showWorkingCircles == false && circle.IsWorking == false)
                    e.Accepted = true;
                else
                    e.Accepted = false;
                if (e.Accepted)
                {
                    if (SearchBox.IsEmpty())
                        return;
                    if (circle.Title.ToLower().Contains(SearchBox.SearchText.Trim().ToLower()))
                        e.Accepted = true;
                    else
                        e.Accepted = false;
                }
            }
        }
        #endregion

        #region DataGrid editing event handlers
        private string savedEditingCircleTitle; // circle title at beginning of editing
        
        private void DGCircleList_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (e.EditingEventArgs.Source.GetType() == typeof(TextBlock))
                savedEditingCircleTitle = (e.EditingEventArgs.Source as TextBlock).Text;
        }

        private void DGCircleList_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                if (e.Column is DataGridBoundColumn column)
                {
                    var bindingPath = (column.Binding as Binding).Path.Path;
                    var editingCellTB = e.EditingElement as TextBox;
                    editingCellTB.Text = editingCellTB.Text.Trim();

                    if (bindingPath == "Title")
                        HandleCircleTitleEditEnding(editingCellTB.Text);
                    else if (bindingPath == "MaxNumberOfPupils")
                        HandleCircleMaxNumberOfPupilsEditEnding(editingCellTB.Text);
                }
            }
        }

        private void HandleCircleTitleEditEnding(string newCircleTitle)
        {
            // return, if there are no changes in the title
            if (newCircleTitle == savedEditingCircleTitle) return;

            if (string.IsNullOrEmpty(newCircleTitle))
            {
                DGCircleList.CancelEdit();
                MainWindow.StatusBar.Error("Название кружка не может быть пустым.");
                return;
            }
            else if (!Helpers.ContainsOnlyRussianLetters(newCircleTitle))
            {
                DGCircleList.CancelEdit();
                MainWindow.StatusBar.Error("Название кружка должно содержать только русские буквы.");
                return;
            }

            // since there are no or only one circle in the list, it's not neccessary to check for title duplicate
            if (CircleList.Count <= 1) return;

            var circleTitleDuplicates = CircleList
                .ToList()
                .FindAll(circle => circle.Title == newCircleTitle).Count;

            if (circleTitleDuplicates >= 1)
            {
                DGCircleList.CancelEdit();
                MainWindow.StatusBar.Error("Кружок с таким названием уже существует.");
            }
        }

        private void HandleCircleMaxNumberOfPupilsEditEnding(string s)
        {
            if (!int.TryParse(s, out var newMaxNumberOfPupils) || newMaxNumberOfPupils <= 0)
            {
                DGCircleList.CancelEdit();
                MainWindow.StatusBar.Error("Макс. число учеников должно быть целым числом, большим нуля.");
                return;
            }
        }
        #endregion

        #region Default circle
        // default circle field values used when user adds new circle to list
        private const string defaultCircleTitle = "Новый кружок";
        private const int defaultCircleMaxNumberOfPupils = 0;
        private const bool defaultCircleIsWorking = true;
        
        private bool IsDefaultCircle(Circle circle)
        {
            return circle.Title == defaultCircleTitle
                && circle.MaxNumberOfPupils == defaultCircleMaxNumberOfPupils
                && circle.IsWorking == defaultCircleIsWorking;
        }

        private bool AreThereDefaultCircleInList()
        {
            return CircleList.Any(circle => IsDefaultCircle(circle));
        }
        #endregion

        #region Working circles buttons click handlers
        private void BtnAddCircle_Click(object sender, RoutedEventArgs e)
        {
            if (AreThereDefaultCircleInList())
            {
                MainWindow.StatusBar.Error("Для добавления ещё одного нового кружка, отредактируйте предыдущий.");
                return;
            }

            Circle newCircle = new Circle();
            newCircle.Title = defaultCircleTitle;
            newCircle.IsWorking = defaultCircleIsWorking;
            newCircle.MaxNumberOfPupils = defaultCircleMaxNumberOfPupils;
            CircleList.Add(newCircle);

            DGCircleList.SelectedIndex = DGCircleList.Items.Count - 1;

            DGCircleList.ScrollIntoView(newCircle);
        }

        private void BtnDeleteCircle_Click(object sender, RoutedEventArgs e)
        {
            List<Circle> selectedCircles = DGCircleList.SelectedItems.Cast<Circle>().ToList();
            if (selectedCircles.Count < 0)
            {
                MainWindow.StatusBar.Info("Выберите хотя бы один кружок для удаления.");
                return;
            }
            var result = Helpers.AskQuestion(
                $"Вы уверены, что хотите удалить " +
                $"выбранн{(selectedCircles.Count > 1 ? "ые" : "ый")} " +
                $"круж{(selectedCircles.Count > 1 ? "ки" : "ок")}?");
            if (result == false)
                return;
            selectedCircles.ForEach(c => {
                if (IsDefaultCircle(c)) // if it is default circle it's not allowed to mark it as deleted
                    CircleList.Remove(c);
                else
                    c.IsWorking = false;
            });
            MainWindow.StatusBar.Info($"Круж{(selectedCircles.Count > 1 ? "ки" : "ок")} успешно удал{(selectedCircles.Count > 1 ? "ены" : "ён")}.");
            cvCircleList.Refresh();
        }

        private void BtnSaveChangesInDB_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.db.ChangeTracker.HasChanges() == false)
            {
                MainWindow.StatusBar.Info("Нет изменений для сохранения.");
                return;
            }
            else if (AreThereDefaultCircleInList())
            {
                MainWindow.StatusBar.Error("Задайте наименование нового кружка перед сохранением изменений.");
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

        #region Wroking and deleted circles show buttons click handlers
        private void ToggleButtonsVisibilities()
        {
            BtnShowDeletedCircles.Visibility
                = BtnAddCircle.Visibility
                    = BtnDeleteCircle.Visibility
                        = BtnDeleteCircle.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

            BtnShowWorkingCircles.Visibility
                = BtnRestoreDeletedCircle.Visibility
                    = BtnRestoreDeletedCircle.Visibility == Visibility.Collapsed? Visibility.Visible : Visibility.Collapsed;
        }

        private void ToggleShowWorkingOrDeletedCircles(object sender, RoutedEventArgs e)
        {
            ToggleButtonsVisibilities();
            showWorkingCircles = !showWorkingCircles;
            SearchBox.Clear();
            cvCircleList.Refresh();
        }

        private void BtnRestoreDeletedCircle_Click(object sender, RoutedEventArgs e)
        {
            List<Circle> selectedCircles = DGCircleList.SelectedItems.Cast<Circle>().ToList();
            if (selectedCircles.Count < 0)
            {
                MainWindow.StatusBar.Info("Выберите хотя бы один кружок для восстановления.");
                return;
            }
            var result = Helpers.AskQuestion(
                    $"Вы уверены, что хотите восстановить " +
                    $"выбранн{(selectedCircles.Count > 1 ? "ые" : "ый")} " +
                    $"круж{(selectedCircles.Count > 1 ? "ки" : "ок")}?");
            if (result == false)
                return;
            selectedCircles.ForEach(c => c.IsWorking = true);
            MainWindow.StatusBar.Info($"круж{(selectedCircles.Count > 1 ? "ки" : "ок")} успешно восстановлены.");
            cvCircleList.Refresh();
        }
        #endregion
    }
}
