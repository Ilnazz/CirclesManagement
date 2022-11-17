using CirclesManagement.Classes;
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
        public ObservableCollection<DataGridColumn> Columns
        {
            get { return InnerDataGrid.Columns; }
        }

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

        public Func<object, string, bool> SearchTextMatcher;
        public Func<object, bool> IsEntityDeleted;
        public Predicate<object> Filter
        {
            get { return _collectionView.Filter; }
            set {
                _collectionView.Filter = (obj) => {
                    if (ShowDeletedEntities == true && IsEntityDeleted(obj) == false
                        || ShowDeletedEntities == false && IsEntityDeleted(obj) == true)
                        return false;
                    if (_searchBox.IsEmpty())
                        return true;
                    return SearchTextMatcher.Invoke(obj, _searchBox.SearchText);
                };
            }
        }

        public void Refresh() => _collectionView.Refresh();

        public bool ShowDeletedEntities;

        public EntityDataGridComponent()
        {
            InitializeComponent();
        }
        
        public Func<object> EntityCreator;
        public void AddNewEntity()
        {
            var newEntity = EntityCreator();
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

            var validationResult = App.DB.ChangeTracker.Entries().ToList().All(entry =>
            {
                if (EntityValidator(entry.Entity) == false)
                {
                    ValidationErrorCallback(entry.Entity);
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

        public Action<object> DeletingEntity;

        private void InnerDataGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is DependencyObject srcObj == false)
                return;

            while (srcObj != null)
            {
                if (srcObj is Button btn)
                {
                    // delete button was clicked
                    DeletingEntity.Invoke(InnerDataGrid.SelectedItem);
                    return;
                }
                srcObj = VisualTreeHelper.GetParent(srcObj);
            }
        }
    }
}
