﻿using System;
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
    /// Логика взаимодействия для CirclesPage.xaml
    /// </summary>
    public partial class CirclesPage : Page
    {
        public ObservableCollection<Circle> Circles
        {
            get { return (ObservableCollection<Circle>)GetValue(CirclesProperty); }
            set { SetValue(CirclesProperty, value); }
        }
        public static readonly DependencyProperty CirclesProperty =
            DependencyProperty.Register("Circles", typeof(ObservableCollection<Circle>), typeof(CirclesPage), new PropertyMetadata(null));

        public CirclesPage()
        {
            InitializeComponent();

            MainWindow.db.Circles.Load();
            Circles = MainWindow.db.Circles.Local;

            cvCircles = CollectionViewSource.GetDefaultView(CircleList.ItemsSource);

            SearchBox.TextChanged += cvCircles.Refresh;
        }

        #region Searching and filtering
        private ICollectionView cvCircles;

        private bool showDeletedCircles = false;

        private void cvsCircles_Filter(object sender, FilterEventArgs e)
        {
            var circle = e.Item as Circle;

            if (showDeletedCircles == true && circle.IsWorking == false)
                e.Accepted = true;
            else if (showDeletedCircles == false && circle.IsWorking == true)
                e.Accepted = true;
            else
                e.Accepted = false;
            if (e.Accepted)
            {
                if (SearchBox.IsEmpty())
                    return;
                if (circle.Title.ToLower().Contains(SearchBox.SearchText.Trim().ToLower())
                    || circle.MaxNumberOfPupils.ToString().ToLower().Contains(SearchBox.SearchText.Trim().ToLower()))
                    e.Accepted = true;
                else
                    e.Accepted = false;
            }
        }
        #endregion

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

                var editingCellTB = e.EditingElement as TextBox;
                editingCellTB.Text = editingCellTB.Text.Trim();

                switch (property)
                {
                    case "Title":
                        var accept = HandleTitleEditing(oldEditingValue, editingCellTB.Text);
                        if (accept == false)
                            e.Cancel = true;
                        break;
                    case "MaxNumberOfPupils":
                        accept = HandleMaxNumberOfPupilsEditing(oldEditingValue, editingCellTB.Text);
                        if (accept == false)
                            e.Cancel = true;
                        break;
                }
            }
        }

        private bool HandleTitleEditing(string oldTitle, string newTitle)
        {
            // return, if there are no changes in the title
            if (oldTitle == newTitle)
                return true;

            if (string.IsNullOrEmpty(newTitle))
            {
                CircleList.CancelEdit();
                MainWindow.StatusBar.Error("Название кружка не может быть пустым.");
                return false;
            }
            else if (Helpers.ContainsOnlyRussianLetters(newTitle) == false)
            {
                CircleList.CancelEdit();
                MainWindow.StatusBar.Error("Название кружка должно содержать только русские буквы.");
                return false;
            }

            // since there are no or only one circle in the list, it's not neccessary to check for title duplicate
            if (Circles.Count <= 1)
                return true;

            var areThereTitleDuplicates = Circles.Any(circle => circle.Title == newTitle);
            if (areThereTitleDuplicates)
            {
                CircleList.CancelEdit();
                MainWindow.StatusBar.Error("Кружок с таким названием уже существует.");
                return false;
            }

            return true;
        }

        private bool HandleMaxNumberOfPupilsEditing(string oldNumberStr, string newNumberStr)
        {
            if (!int.TryParse(newNumberStr, out var newNumber) || newNumber <= 0)
            {
                CircleList.CancelEdit();
                MainWindow.StatusBar.Error("Макс. число учеников должно быть целым числом, большим нуля.");
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
            => Circles.Any(circle => IsDefaultCircle(circle));
        #endregion

        #region Working circles buttons click handlers
        private void BtnAddCircle_Click(object sender, RoutedEventArgs e)
        {
            if (AreThereDefaultCircle())
            {
                MainWindow.StatusBar.Error("Для добавления ещё одного нового кружка, отредактируйте предыдущий.");
                return;
            }

            Circle newCircle = new Circle();
            newCircle.Title = defaultTitle;
            newCircle.IsWorking = defaultIsWorking;
            newCircle.MaxNumberOfPupils = defaultMaxNumberOfPupils;
            Circles.Add(newCircle);

            CircleList.SelectedIndex = CircleList.Items.Count - 1;

            CircleList.ScrollIntoView(newCircle);
        }

        private void BtnDeleteCircle_Click(object sender, RoutedEventArgs e)
        {
            List<Circle> selectedCircles = CircleList.SelectedItems.Cast<Circle>().ToList();
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

            selectedCircles.ForEach(circle => {
                if (IsDefaultCircle(circle)) // if it is default circle it's not allowed to mark it as deleted, simple delete it
                    Circles.Remove(circle);
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
            MainWindow.StatusBar.Info($"Круж{(selectedCircles.Count > 1 ? "ки" : "ок")} успешно удал{(selectedCircles.Count > 1 ? "ены" : "ён")}.");
        }

        private void BtnSaveChangesInDB_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.db.ChangeTracker.HasChanges() == false)
            {
                MainWindow.StatusBar.Info("Нет изменений для сохранения.");
                return;
            }
            else if (AreThereDefaultCircle())
            {
                MainWindow.StatusBar.Error("Укажите наименование и макс. число учеников для нового кружка перед сохранением изменений.");
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
            showDeletedCircles = showDeletedCircles == false ? true : false;
            SearchBox.Clear();
            cvCircles.Refresh();
        }

        private void BtnRestoreDeletedCircle_Click(object sender, RoutedEventArgs e)
        {
            List<Circle> selectedCircles = CircleList.SelectedItems.Cast<Circle>().ToList();
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
            cvCircles.Refresh();
            MainWindow.StatusBar.Info($"круж{(selectedCircles.Count > 1 ? "ки" : "ок")} успешно восстановлены.");
        }
        #endregion
    }
}