using CirclesManagement.Classes;
using CirclesManagement.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CirclesManagement.Components
{
    /// <summary>
    /// Логика взаимодействия для EntityListComponent.xaml
    /// </summary>
    public partial class EntityDataGridComponent : UserControl
    {
        private ICollectionView _collectionView;
        private ObservableCollection<object> _itemsSource;
        public ObservableCollection<object> ItemsSource
        {
            set
            {
                _itemsSource = value;
                _collectionView = new CollectionViewSource { Source = value }.View;
                InnerDataGrid.ItemsSource = _collectionView;
                _searchBox.TextChanged += _collectionView.Refresh;
            }
        }

        private SearchBoxComponent _searchBox;
        public SearchBoxComponent SearchBox
        {
            get { return _searchBox; }
            set { _searchBox = value;  }
        }

        public void LoadEntityPage(EntityPage entityPage)
        {
            // remove previous entityPage's columns
            if (InnerDataGrid.Columns.Count > 2)
                for (int i = InnerDataGrid.Columns.Count - 3; i > 0; i--)
                    InnerDataGrid.Columns.RemoveAt(i);

            entityPage.Columns.ToList().ForEach(column =>
            {
                InnerDataGrid.Columns.Insert(0, column);
            });

            MainEntityDataGrid.ItemsSource = _currentPage.ItemsSource;
            MainEntityDataGrid.EH = _currentPage.EH;
        }

        public EntityHelper EH
        {
            set {
                EH = value;
            }
        }

        public Predicate<object> Filter
        {
            get { return _collectionView.Filter; }
            set {
                _collectionView.Filter = (obj) => {
                    if (ShowDeletedEntities == true && EH.IsDeleted(obj) == false
                        || ShowDeletedEntities == false && EH.IsDeleted(obj) == true)
                        return false;
                    if (_searchBox.IsEmpty())
                        return true;
                    return EH.SearchTextMatcher(obj, _searchBox.SearchText);
                };
            }
        }

        public void Refresh() => _collectionView.Refresh();

        public bool ShowDeletedEntities;

        public EntityDataGridComponent()
        {
            InitializeComponent();
        }
        
        public void AddNewEntity()
        {
            var areThereEmptyEntity = _itemsSource.Any(entity => IsEntityEmpty(entity));
            if (areThereEmptyEntity == true)
            {
                Helpers.Error($"Перед добавлнием нового {EH.Title.Singular.GenitiveCase}, заполните данные предыдущего.");
                return;
            }
            var newEntity = EH.Builder();
            _itemsSource.Add(newEntity);
            InnerDataGrid.SelectedIndex = InnerDataGrid.Items.Count - 1;
            InnerDataGrid.ScrollIntoView(newEntity);
        }

        public Func<object, bool> EntityValidator;
        public Action<object> ValidationErrorCallback;
        public void SaveChanges()
        {
            if (App.DB.ChangeTracker.HasChanges() == false)
            {
                Helpers.Inform("Нет изменений для сохранения.");
                return;
            }

            var validationResult = _itemsSource.ToList().All(entity =>
            {
                if (EntityValidator(entity) == false)
                {
                    ValidationErrorCallback(entity);
                    return false;
                }
                return true;
            });

            if (validationResult == false)
                return;

            Helpers.AskAndDoActionIfYes("Вы уверены, что хотите сохранить изменения?", () =>
            {
                App.DB.SaveChanges();
                Helpers.Inform("Изменения успешно сохранены.");
            });
        }

        public Predicate<object> IsEntityEmpty;
        public Func<object, bool> EntityDeleter;

        private void InnerDataGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is DependencyObject srcObj == false)
                return;

            Button btn = null;
            DataGridRow row = null;
            while (btn == null || row == null)
            {
                if (srcObj is Button)
                    btn = srcObj as Button;
                else if (srcObj is DataGridRow)
                    row = srcObj as DataGridRow;
                srcObj = VisualTreeHelper.GetParent(srcObj);
                if (srcObj == null)
                    return;
            }
            if (btn == null) // click was not on the button
                return;

            var entity = row.Item;

            if (IsEntityEmpty(entity))
            {
                _itemsSource.Remove(entity);
                Refresh();
            }
            else
            {
                var result = Helpers.Ask($"Вы уверены, что хотите удалить {}?");
                if (result == false)
                    return false;
                var wasDeleted = EntityDeleter.Invoke(entity);
                if (wasDeleted)
                    Refresh();
            }
        }
    }
}
