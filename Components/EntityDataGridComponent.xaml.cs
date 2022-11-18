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
        public SearchBoxComponent SearchBox;
        
        public EntityHelper EH;

        public bool ShowDeletedEntities;
        
        private ICollectionView _collectionView;
        private ObservableCollection<object> _itemsSource;
        public ObservableCollection<object> ItemsSource
        {
            set
            {
                _itemsSource = value;
                _collectionView = new CollectionViewSource { Source = value }.View;
                InnerDataGrid.ItemsSource = _collectionView;
                SearchBox.TextChanged += _collectionView.Refresh;
            }
        }
        public void Refresh() => _collectionView.Refresh();

        public EntityDataGridComponent()
        {
            InitializeComponent();
        }
        
        public void LoadEntityPage(EntityPage entityPage)
        {
            // remove (unload) previous entityPage's columns, saving delete button columns
            if (InnerDataGrid.Columns.Count > 1)
                for (int i = 0; i < InnerDataGrid.Columns.Count-1; i++)
                    InnerDataGrid.Columns.RemoveAt(i);

            // load new entityPage's columns aside delte button column
            entityPage.Columns.ToList().ForEach(column =>
            {
                InnerDataGrid.Columns.Insert(0, column);
            });

            ItemsSource = entityPage.ItemsSource;
            EH = entityPage.EH;

            _collectionView.Filter = (obj) => {
                if (ShowDeletedEntities == true && EH.IsDeleted(obj) == false
                    || ShowDeletedEntities == false && EH.IsDeleted(obj) == true)
                    return false;
                if (SearchBox.IsEmpty())
                    return true;
                return EH.SearchTextMatcher(obj, SearchBox.SearchText);
            };
        }
        
        public void AddNewEntity()
        {
            var areThereEmptyEntity = _itemsSource.Any(entity => EH.IsBlank(entity));
            if (areThereEmptyEntity == true)
            {
                Helpers.Error($"Перед добавлнием нового {EH.Title.Singular.Genitive}, заполните данные предыдущего.");
                return;
            }
            var newEntity = EH.Builder();
            _itemsSource.Add(newEntity);
            InnerDataGrid.SelectedIndex = InnerDataGrid.Items.Count - 1;
            InnerDataGrid.ScrollIntoView(newEntity);
        }

        public void SaveChanges()
        {
            if (App.DB.ChangeTracker.HasChanges() == false)
            {
                Helpers.Inform("Нет изменений для сохранения.");
                return;
            }

            var validationResult = _itemsSource.ToList().All(entity =>
            {
                (var result, var message) = EH.Validator(entity);
                if (result == false)
                {
                    Helpers.Error($"Ошибка при проверке {EH.Title.Singular.Genitive}: {message}.");
                    return false;
                }
                return true;
            });

            if (validationResult == false)
                return;
            
            foreach (var entity1 in _itemsSource)
            {
                foreach (var entity2 in _itemsSource)
                {
                    if (entity1.Equals(entity2))
                        continue;
                    if (EH.Comparer(entity1, entity2) == true)
                    {
                        Helpers.Error($"Ошибка: {EH.Title.Singular.Nominative} с такими данными уже существует.");
                        return;
                    }
                }
            }

            Helpers.AskAndDoActionIfYes("Вы уверены, что хотите сохранить изменения?", () =>
            {
                App.DB.SaveChanges();
                Helpers.Inform("Изменения успешно сохранены.");
            });
        }


        public void DeleteSelectedEntity()
        {
            if (InnerDataGrid.SelectedItem == null)
                return;
            var entity = InnerDataGrid.SelectedItem;

            if (EH.IsBlank(entity))
            {
                _itemsSource.Remove(entity);
                Refresh();
                return;
            }
            
            var result = Helpers.Ask($"Вы действительно, хотите удалить {EH.Title.Singular.Accusative}?");
            if (result == false)
                return;
            (var deletionResult, var message) = EH.Deleter(entity);
            if (deletionResult == false)
            {
                Helpers.Error($"Нельзя удалить {EH.Title.Singular.Accusative} по причине: {message}.");
                return;
            }
            Refresh();
        }
    }
}
