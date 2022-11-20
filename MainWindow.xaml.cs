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
        private readonly EntityPage[] _entityPages = new EntityPage[]
        {
            new CirclesPage(),
            new ClassroomsPage(),
            new GradesPage(),
            new PupilsPage(),
            new TeachersPage(),
            new TimetablesPage()
            //new LessonsPage()
        };

        private EntityPage _currentEntityPage;
        
        public MainWindow()
        {
            InitializeComponent();

            Closing += (s, e) =>
            {
                Helpers.AskAndDoActionIfYes("Вы уверены, что хотите выйти из приложения? Несохранённые изменения будут потеряны.", () =>
                {
                    if (App.DB.ChangeTracker.HasChanges() == true)
                        _currentEntityPage?.DiscardChanges();
                });
            };

            Navigation.ItemsSource = _entityPages.Select(page => page.Title);
            Navigation.BeforeSelectionChanged += (oldIndex, newIndex) =>
            {
                if (App.DB.ChangeTracker.HasChanges() == true)
                {
                    Helpers.Inform($"Перед переходом на другую страницу сохраните/отмените изменения на текущей.");
                    return false;
                }

                if (newIndex != -1)
                {
                    _currentEntityPage = _entityPages[newIndex];
                    _currentEntityPage.SearchBox = SearchBox;
                    if (MainFrame.CanGoBack)
                        MainFrame.GoBack();
                    MainFrame.Navigate(_currentEntityPage);
                }

                return true;
            };
        }

        private void BtnUserLogOut_Click(object sender, RoutedEventArgs e)
        {
            Helpers.WarnAndDoActionIfYes("Вы уверены, что хотите выйти из системы? Несохранённые изменения будут потеряны.", () =>
            {
                if (App.DB.ChangeTracker.HasChanges() == true)
                    _currentEntityPage?.DiscardChanges();

                var authorizationWindow = new AuthorizationWindow();
                authorizationWindow.Show();
                authorizationWindow.Activate();
                Close();
            });
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
            => _currentEntityPage?.DeleteSelectedEntity();

        private void BtnDiscardChanges_Click(object sender, RoutedEventArgs e)
            => _currentEntityPage?.DiscardChanges();
    }
}
