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
    /// Логика взаимодействия для SpecialDataGrid.xaml
    /// </summary>
    public partial class SpecialDataGrid : UserControl
    {
        public ObservableCollection<DataGridColumn> Columns
        {
            get { return InnerDataGrid.Columns; }
            // bringing user added columns to left
            set {
                InnerDataGrid.Columns
                    .ToList()
                    .InsertRange(0, value);
            }
        }

        private ICollectionView _collectionView;
        public ObservableCollection<object> ItemsSource
        {
            get { return InnerDataGrid.ItemsSource as ObservableCollection<object>; }
            set {
                _collectionView = new CollectionViewSource { Source = value }.View;
                InnerDataGrid.ItemsSource = _collectionView;
            }
        }

        public Predicate<object> Filter
        {
            get { return _collectionView.Filter; }
            set { _collectionView.Filter = value; }
        }

        public void Refresh() => _collectionView.Refresh();

        public void AddItemAndScrollIntoView(object item)
        {
            ItemsSource.Add(item);
            InnerDataGrid.SelectedIndex = InnerDataGrid.Items.Count - 1;
            InnerDataGrid.ScrollIntoView(item);
        }

        public Action<object> DeletingItem;
        private void OnDeletingItem(object item)
        {
            DeletingItem?.Invoke(item);
        }

        public SpecialDataGrid()
        {
            InitializeComponent();
        }

        private bool _isEditingHappening;

        private void InnerDataGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender.GetType() == typeof(Button))
            {
                var btn = sender as Button;
                var tag = btn.Tag as string;

                if (tag == "Edit")
                {
                    _isEditingHappening = true;
                    InnerDataGrid.IsReadOnly = false;
                } else
                {
                    OnDeletingItem(InnerDataGrid.SelectedItem);
                }
            }
        }

        private void InnerDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _isEditingHappening = false;
            InnerDataGrid.IsReadOnly = false;
        }

        private void InnerDataGrid_LostFocus(object sender, RoutedEventArgs e)
        {
            _isEditingHappening = false;
            InnerDataGrid.IsReadOnly = false;
        }
    }
}
