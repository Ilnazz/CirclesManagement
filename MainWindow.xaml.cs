using System;
using System.Collections.Generic;
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
        private User _currentUser;

        private readonly EntityPage[] _pages;
        private readonly string[] _pageTitles;
        private EntityPage _currentPage;

        public MainWindow(User user)
        {
            InitializeComponent();

            _currentUser = user;

            MainEntityDataGrid.SearchBox = SearchBox;

            _pages = new EntityPage[]
            {
                new CirclesPage(),
                //new ClassroomsPage(),
                //new GradesPage(),
                //new TeachersPage(),
                //new PupilsPage(),
                //new TimetablesPage(),
                //new LessonsPage()
            };


            _pageTitles = _pages.Select(p => p.Title).ToArray();

            NavigationTree.ItemsSource = _pageTitles;

            NavigationTree.SelectedItemChanged += (s, e) =>
            {
                MainEntityDataGrid.Visibility = Visibility.Visible;
                BtnSaveChanges.Visibility = Visibility.Visible;
                BtnAddEntity.Visibility = Visibility.Visible;
                BtnToggleShowDeletedEntities.Visibility = Visibility.Visible;

                _currentPage = _pages
                    .Where(p => p.Title == (NavigationTree.SelectedItem as string))
                    .First();

                // remove previous entityPage's columns
                if (MainEntityDataGrid.Columns.Count > 2)
                {
                    for (int i = MainEntityDataGrid.Columns.Count - 3; i > 0; i--)
                        MainEntityDataGrid.Columns.RemoveAt(i);
                }

                _currentPage.Columns.ToList().ForEach(column =>
                {
                    MainEntityDataGrid.Columns.Insert(0, column);
                });

                MainEntityDataGrid.ItemsSource = _currentPage.ItemsSource;

                MainEntityDataGrid.SearchTextMatcher = _currentPage.SearchTextMatcher;
                MainEntityDataGrid.IsEntityDeleted = _currentPage.IsEntityDeleted;
                MainEntityDataGrid.Filter = _currentPage.Filter;
                MainEntityDataGrid.EntityCreator = _currentPage.EntityCreator;
                MainEntityDataGrid.EntityValidator = _currentPage.EntityValidator;
                MainEntityDataGrid.ValidationErrorCallback = _currentPage.ValidationErrorCallback;
                MainEntityDataGrid.DeletingEntity = _currentPage.DeletingEntity;
            };
        }

        private void BtnUserLogOut_Click(object sender, RoutedEventArgs e)
        {
            Helpers.AskAndDoActionIfYes("Вы уверены, что хотите выйти из системы?", () =>
            {
                Helpers.Inform("Вы успешно вышли из системы.");
                var authorizationWindow = new AuthorizationWindow();
                authorizationWindow.Show();
                authorizationWindow.Activate();
                Close();
            });
        }

        private void BtnToggleWorkingAndDeletedEntities_Click(object sender, RoutedEventArgs e)
        {
            BtnToggleShowDeletedEntities.Content
                = BtnToggleShowDeletedEntities.Content.ToString() == "Показать удалённые"
                    ? "Показать активные" : "Показать удалённые";

            MainEntityDataGrid.ShowDeletedEntities = !MainEntityDataGrid.ShowDeletedEntities;
            MainEntityDataGrid.Refresh();
        }

        private void BtnSaveChanges_Click(object sender, RoutedEventArgs e)
            => MainEntityDataGrid.SaveChanges();

        private void BtnAddEntity_Click(object sender, RoutedEventArgs e)
            => MainEntityDataGrid.AddNewEntity();
    }
}
