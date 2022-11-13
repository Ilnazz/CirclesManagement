using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class SpecialDataGrid : DataGrid
    {
        public ObservableCollection<DataGridColumn> CustomColumns
        {
            get { return (ObservableCollection<DataGridColumn>)GetValue(CustomColumnsProperty); }
            set { SetValue(CustomColumnsProperty, value); }
        }
        public static readonly DependencyProperty CustomColumnsProperty =
            DependencyProperty.Register("CustomColumns", typeof(ObservableCollection<DataGridColumn>), typeof(SpecialDataGrid), new PropertyMetadata(new ObservableCollection<DataGridColumn>()));

        public SpecialDataGrid()
        {
            InitializeComponent();

            AutoGenerateColumns = false;
            CanUserAddRows = false;
            CanUserDeleteRows = false;
            CanUserReorderColumns = false;
            EnableColumnVirtualization = true;
            EnableRowVirtualization = true;

            // bringing user added columns to left
            CustomColumns.CollectionChanged += (s, e) =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    List<DataGridColumn> newColumns = new List<DataGridColumn>();
                    foreach (DataGridColumn newColumn in e.NewItems)
                        Columns.Insert(0, newColumn);
                }
            };
        }
    }
}
