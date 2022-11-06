using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
    /// Логика взаимодействия для RichDataGrid.xaml
    /// </summary>
    public partial class RichDataGrid : UserControl
    {
        public Type ItemType
        {
            get { return (Type)GetValue(ItemTypeProperty); }
            set { SetValue(ItemTypeProperty, value); }
        }
        public static readonly DependencyProperty ItemTypeProperty =
            DependencyProperty.Register("type", typeof(Type), typeof(RichDataGrid), new PropertyMetadata(typeof(object)));

        public ObservableCollection<object> ItemsSource
        {
            get { return (ObservableCollection<object>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<object>), typeof(RichDataGrid), new PropertyMetadata(new ObservableCollection<object>()));

        public List<DataGridColumn> Columns
        {
            get { return (List<DataGridColumn>)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof(List<DataGridColumn>), typeof(RichDataGrid), new PropertyMetadata(new List<DataGridColumn>()));

        private ICollectionView dataGridCV;

        public event Predicate<object> Filter = (obj) => true;

        public RichDataGrid()
        {
            InitializeComponent();

            dataGridCV = CollectionViewSource.GetDefaultView(DataGrid.ItemsSource);
        }

        private void RichDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            Columns.ForEach(column => DataGrid.Columns.Add(column));
            dataGridCV.Filter += Filter;
        }
    }

    public class ItemsTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is Type type))
                return null;
            if (!(value is ObservableCollection<object> oc))
                return null;
            MethodInfo method = oc.GetType().GetMethod("OfType").MakeGenericMethod(new Type[] { type });
            return method.Invoke(oc, new object[] { });
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
