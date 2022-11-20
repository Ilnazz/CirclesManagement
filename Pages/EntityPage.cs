using CirclesManagement.Classes;
using CirclesManagement.Components;
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

namespace CirclesManagement.Pages
{
    public abstract class EntityPage : Page
    {
        private readonly DataGrid _innerDataGrid = new DataGrid()
        {
            AutoGenerateColumns = false,
            CanUserAddRows = false,
            CanUserDeleteRows = false,
            EnableColumnVirtualization = true,
            EnableRowVirtualization = true,
            SelectionMode = DataGridSelectionMode.Single
        };

        protected PropertyGroupDescription[] propertyGroupDescriptions = null;

        public ObservableCollection<DataGridColumn> DataGridColumns
        {
            get { return _innerDataGrid.Columns; }
            set { value.ToList().ForEach(column => _innerDataGrid.Columns.Add(column)); }
        }

        protected EntityHelper EH;

        private ICollectionView _collectionView;
        private ObservableCollection<object> _entitiesSource;
        protected ObservableCollection<object> EntitiesSource
        {
            set
            {
                _entitiesSource = value;
                _collectionView = new CollectionViewSource { Source = value }.View;
                if (propertyGroupDescriptions != null)
                {
                    propertyGroupDescriptions
                        .ToList().ForEach(pgd => _collectionView.GroupDescriptions.Add(pgd));
                    _innerDataGrid.GroupStyle.Add(FindResource("DataGridGroupStyle") as GroupStyle);
                }
                _innerDataGrid.ItemsSource = _collectionView;
            }
        }

        private SearchBoxComponent _searchBox;
        public SearchBoxComponent SearchBox
        {
            get { return _searchBox; }
            set {
                _searchBox = value;
                
                _searchBox.TextChanged += _collectionView.Refresh;

                _collectionView.Filter = (obj) => {
                    if (_showDeletedEntites == true && EH.IsDeleted(obj) == false
                        || _showDeletedEntites == false && EH.IsDeleted(obj) == true)
                        return false;
                    if (SearchBox.IsEmpty())
                        return true;
                    return EH.SearchTextMatcher(obj, SearchBox.SearchText);
                };
            }
        }
        
        private bool _showDeletedEntites = false;
        public bool ShowDeletedEntities
        {
            get { return _showDeletedEntites; }
            set
            {
                _showDeletedEntites = value;
                _collectionView.Refresh();
            }
        }

        public EntityPage() {
            Content = _innerDataGrid;
        }

        public void AddNewEntity()
        {
            var isThereBlankEntity = _entitiesSource.Any(entity => EH.IsBlank(entity));
            if (isThereBlankEntity == true)
            {
                Helpers.Error("Перед добавлнием новой записи, заполните данные предыдущей.");
                return;
            }

            var newEntity = EH.Builder();
            _entitiesSource.Add(newEntity);
            _innerDataGrid.SelectedIndex = _innerDataGrid.Items.Count - 1;
            _innerDataGrid.ScrollIntoView(newEntity);
        }

        public void DeleteSelectedEntity()
        {
            if (_innerDataGrid.SelectedItem == null)
                return;

            var entity = _innerDataGrid.SelectedItem;

            // Если сущность была только что созданной, просто удалить её из списка
            if (EH.IsBlank(entity))
            {
                _entitiesSource.Remove(entity);
                _collectionView.Refresh();
                return;
            }

            Helpers.WarnAndDoActionIfYes("Вы действительно хотите удалить данную запись?", () =>
            {
                (var deletionResult, var deletionMessage) = EH.Deleter(entity);
                if (deletionResult == false)
                {
                    Helpers.Error($"Нельзя удалить данную запись по причине: {deletionMessage}.");
                    return;
                }

                _collectionView.Refresh();
            });
        }

        public void SaveChanges()
        {
            if (App.DB.ChangeTracker.HasChanges() == false)
            {
                Helpers.Inform("Нет изменений для сохранения.");
                return;
            }

            var areAllEntitiesValid = _entitiesSource
                .Where(e => EH.IsDeleted(e) == false)
                .All(entity =>
            {
                (var validationResult, var validationMessage) = EH.Validator(entity);
                if (validationResult == false)
                {
                    Helpers.Error($"{Helpers.Capitalize(validationMessage)}.");
                    return false;
                }

                var isDuplicate = _entitiesSource
                    .Where(e => EH.IsDeleted(e) == false)
                    .Any(otherEntity =>
                {
                    // Сравнение той же самой сущности с собой же
                    if (entity.Equals(otherEntity)) 
                        return false;

                    (var comparisonResult, var comparisonMessage) = EH.Comparer(entity, otherEntity);
                    if (comparisonResult == false)
                    {
                        Helpers.Error($"{Helpers.Capitalize(comparisonMessage)}.");
                        return true;
                    }
                    
                    return false;
                });

                return isDuplicate == false;
            });

            if (areAllEntitiesValid == false)
                return;

            // Выполнить подготовку сущностей к сохранению
            _entitiesSource.ToList().ForEach(entity => EH.SavePreparator?.Invoke(entity));

            App.DB.SaveChanges();
            Helpers.Inform("Изменения сохранены.");
        }

        public void DiscardChanges()
        {
            if (App.DB.ChangeTracker.HasChanges() == false)
            {
                Helpers.Inform("Нет изменений для отмены.");
                return;
            }

            Helpers.WarnAndDoActionIfYes("Вы действительно хотите отменить изменения?", () =>
            {
                var changedEntries = App.DB.ChangeTracker.Entries()
                    .Where(x => x.State != EntityState.Unchanged).ToList();

                foreach (var entry in changedEntries)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            entry.CurrentValues.SetValues(entry.OriginalValues);
                            entry.State = EntityState.Unchanged;
                            break;
                        case EntityState.Added:
                            entry.State = EntityState.Detached;
                            _entitiesSource.Remove(entry.Entity);
                            break;
                    }
                }
                _collectionView.Refresh();
                Helpers.Inform("Изменения отменены.");
            });
        }
    }
}