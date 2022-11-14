using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    /// 
    public partial class UserPage : Page
    {
        public static SearchBoxComponent SearchBox;

        private static readonly EntitiesPage[] _pages = new EntitiesPage[]
        {
            new CirclesPage(),
            //new ClassroomsPage(),
            //new GradesPage(),
            //new TeachersPage(),
            //new PupilsPage(),
            //new TimetablesPage(),
            //new LessonsPage()
        };

        private static readonly string[] _pageTitles = _pages.Select(p => p.Title).ToArray();

        private EntitiesPage _currentPage;

        public UserPage()
        {
            SearchBox = SearchBoxInstance;

            NavigationTree.ItemsSource = _pageTitles;
            NavigationTree.SelectedItemChanged += (s, e) =>
            {
                _currentPage = _pages
                    .Where(p => p.Title == (NavigationTree.SelectedItem as string))
                    .First();
                FrameForPage.Navigate(_currentPage);
            };
        }

        private void BtnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            Helpers.AskAndDoActionIfYes("Вы уверены, что хотите сохранить изменения?", () =>
            {
                Helpers.Inform("Изменения сохранены.");
                MainWindow.db.SaveChanges();
            });
        }

        private void ToggleWorkingAndDeletedEntities(object sender, RoutedEventArgs e)
        {
            BtnShowDeletedEntities.Visibility
                = BtnShowDeletedEntities.Visibility == Visibility.Visible? Visibility.Collapsed : Visibility.Visible;
            BtnShowWorkingEntities.Visibility
                = BtnShowWorkingEntities.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;

            _currentPage.ShowDeletedEntities = !_currentPage.ShowDeletedEntities;
        }

        private void BtnAddEntity_Click(object sender, RoutedEventArgs e)
        {
            _currentPage.AddEntity();
        }
    }
}
