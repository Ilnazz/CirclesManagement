using System;
using System.Collections.Generic;
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
using CirclesManagement.Pages;

namespace CirclesManagement
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private EntityPage[] _entityPages;
        private EntityPage _currentEntityPage;

        public MainWindow()
        {
            InitializeComponent();

            TBUser.Text = $"Пользователь: {App.CurrentUser.Name}";

            LoadPagesAccordingToUserRole();

            Navigation.ItemsSource = _entityPages.Select(page => page.Title);
            Navigation.SelectionChanged += () =>
            {
                if (App.DB.ChangeTracker.HasChanges() == true)
                {
                    Helpers.Error("Перед переходом на другую страницу, сохраните изменения.");
                }

                if (Navigation.SelectedIndex != -1)
                {
                    _currentEntityPage = _entityPages[Navigation.SelectedIndex];
                    _currentEntityPage.SearchBox = SearchBox;

                    if (_currentEntityPage.HasAddFunction == true)
                        BtnAdd.Visibility = Visibility.Visible;
                    else
                        BtnAdd.Visibility = Visibility.Collapsed;

                    if (_currentEntityPage.HasDeleteFunction == true)
                    {
                        BtnDelete.Visibility = Visibility.Visible;
                        BtnToggleShowDeletedEntities.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        BtnDelete.Visibility = Visibility.Collapsed;
                        BtnToggleShowDeletedEntities.Visibility = Visibility.Collapsed;
                    }

                    if (_currentEntityPage.HasEditFunction == true)
                        BtnEdit.Visibility = Visibility.Visible;
                    else
                        BtnEdit.Visibility = Visibility.Collapsed;

                    if (_currentEntityPage.HasCreateLessonFunction == true)
                        BtnCreateLesson.Visibility = Visibility.Visible;
                    else
                        BtnCreateLesson.Visibility = Visibility.Collapsed;

                    if (MainFrame.CanGoBack)
                        MainFrame.GoBack();
                    MainFrame.Navigate(_currentEntityPage);
                }
            };
        }

        private void LoadPagesAccordingToUserRole()
        {
            if (App.CurrentUser.Role.ID is 1)
                _entityPages = new EntityPage[]
                {
                    new CirclesPage(),
                    new ClassroomsPage(),
                    new GradesPage(),
                    new PupilsPage(),
                    new TeachersPage(),
                    new GroupsPage(),
                    new TimetablesPage(),
                    new LessonsPage()
                };
            else
                _entityPages = new EntityPage[]
                {
                    new GroupsPageOfTeacher(App.CurrentUser.Teacher),
                    new TimetablesPageOfTeacher(App.CurrentUser.Teacher),
                    new LessonsPageOfTeacher(App.CurrentUser.Teacher)
                };
        }

        private void BtnUserLogOut_Click(object sender, RoutedEventArgs e)
        {
            if (App.DB.ChangeTracker.HasChanges() == true)
                _currentEntityPage?.DiscardChanges();

            var authorizationWindow = new AuthorizationWindow();
            authorizationWindow.Show();
            authorizationWindow.Activate();
            Close();    
        }

        private void BtnToggleWorkingAndDeletedEntities_Click(object sender, RoutedEventArgs e)
        {
            if (_currentEntityPage == null)
                return;
            BtnToggleShowDeletedEntities.Content
                = BtnToggleShowDeletedEntities.Content.ToString() == "Показать удалённые"
                    ? "Показать активные" : "Показать удалённые";

            BtnAdd.Visibility =
                BtnDelete.Visibility = BtnDelete.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

            _currentEntityPage.ShowDeletedEntities = !_currentEntityPage.ShowDeletedEntities;
        }

        private void BtnSaveChanges_Click(object sender, RoutedEventArgs e)
            => _currentEntityPage?.SaveChanges();

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
            => _currentEntityPage?.AddNewEntity();

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
            => _currentEntityPage?.DeleteEntity();

        private void BtnDiscardChanges_Click(object sender, RoutedEventArgs e)
            => _currentEntityPage?.DiscardChanges();

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
            => _currentEntityPage?.EditHandler();

        private void BtnCreateLesson_Click(object sender, RoutedEventArgs e)
            => _currentEntityPage?.CreateLessonFunction();
    }
}
