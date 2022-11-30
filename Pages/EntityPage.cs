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
using System.Windows.Media;

namespace CirclesManagement.Pages
{
    public abstract class EntityPage : Page
    {
        public bool HasAddFunction = true;
        public bool HasDeleteFunction = true;

        public bool HasEditFunction = false;
        public Action EditHandler;

        public bool HasCreateLessonFunction = false;
        public Action CreateLessonFunction;

        private DataGrid _innerDataGrid = new DataGrid()
        {
            AutoGenerateColumns = false,
            CanUserAddRows = false,
            CanUserDeleteRows = false,
            EnableColumnVirtualization = true,
            EnableRowVirtualization = true,
            SelectionMode = DataGridSelectionMode.Single,
            HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
        };
        public DataGrid InnerDataGrid
        {
            get { return _innerDataGrid; }
            set { _innerDataGrid = value; }
        }

        protected (PropertyGroupDescription, GroupStyle) dataGridGroupping = (null, null);

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
                if (_entitiesSource.Count > 0)
                {
                    if (dataGridGroupping.Item1 != null && dataGridGroupping.Item2 != null)
                    {
                        _collectionView.GroupDescriptions.Add(dataGridGroupping.Item1);
                        _innerDataGrid.GroupStyle.Add(dataGridGroupping.Item2);
                    }
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

            _innerDataGrid.LoadingRow += (s, e) =>
            {
                e.Row.Header = (e.Row.GetIndex() + 1).ToString();
            };
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

        public void DeleteEntity()
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

                changedEntries.ForEach(entry =>
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
                });

                _collectionView.Refresh();
            });
        }

        public void DiscardAddedEntities()
        {
            var changedEntries = App.DB.ChangeTracker.Entries()
                    .Where(x => x.State != EntityState.Unchanged).ToList();

            changedEntries.ForEach(entry =>
            {
                if (entry.State == EntityState.Added)
                {
                    entry.State = EntityState.Detached;
                    _entitiesSource.Remove(entry.Entity);
                }
            });
        }

        public void DiscardModifications()
        {
            var changedEntries = App.DB.ChangeTracker.Entries()
                    .Where(x => x.State != EntityState.Unchanged).ToList();

            changedEntries.ForEach(entry =>
            {
                if (entry.State == EntityState.Modified)
                {
                    entry.CurrentValues.SetValues(entry.OriginalValues);
                    entry.State = EntityState.Unchanged;
                }
            });
        }
    }
}