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
        private EntityPage _currentPage;

        private static readonly EntityPage[] _pages = new EntityPage[]
        {
            new CirclesPage(),
            //new ClassroomsPage(),
            //new GradesPage(),
            //new TeachersPage(),
            //new PupilsPage(),
            //new TimetablesPage(),
            //new LessonsPage()
        };

        private readonly string[] _pageTitles = new string[]
        {
            "• Кружки",
            "• Кабинеты",
            "• Классы",
            "• Учителя",
            "• Ученики",
            "• Расписание",
            "• Уроки"
        };


        public MainWindow(User user)
        {
            InitializeComponent();

            _currentUser = user;

            MainEntityDataGrid.SearchBox = SearchBox;

            Navigation.ItemsSource = _pageTitles;
            Navigation.SelectionChanged += (s, e) =>
            {
                var pageIndex = Navigation.SelectedIndex;
                _currentPage = _pages[pageIndex];
                MainEntityDataGrid.LoadEntityPage(_currentPage);
            };
            Navigation.SelectedIndex = 0;
        }

        private void BtnUserLogOut_Click(object sender, RoutedEventArgs e)
        {
            Helpers.AskAndDoActionIfYes("Вы уверены, что хотите выйти из системы?", () =>
            {
                Helpers.Inform("Вы успешно вышли из системы.");
                var authorizationWindow = new AuthorizationWindow();
                authorizationWindow.Activate();
                Close();
            });
        }

        private void BtnToggleWorkingAndDeletedEntities_Click(object sender, RoutedEventArgs e)
        {
            BtnToggleShowDeletedEntities.Content
                = BtnToggleShowDeletedEntities.Content.ToString() == "Показать удалённые"
                    ? "Показать активные" : "Показать удалённые";

            BtnAddEntity.Visibility =
                BtnDeleteEntity.Visibility = BtnDeleteEntity.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

            MainEntityDataGrid.ShowDeletedEntities = !MainEntityDataGrid.ShowDeletedEntities;
            MainEntityDataGrid.Refresh();
        }

        private void BtnSaveChanges_Click(object sender, RoutedEventArgs e)
            => MainEntityDataGrid.SaveChanges();

        private void BtnAddEntity_Click(object sender, RoutedEventArgs e)
            => MainEntityDataGrid.AddNewEntity();

        private void BtnDeleteEntity_Click(object sender, RoutedEventArgs e)
            => MainEntityDataGrid.DeleteSelectedEntity();
    }
}
