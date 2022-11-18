using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CirclesManagement.Pages
{
    public abstract class EntityPage : UserControl
    {
        private ObservableCollection<DataGridColumn> _columns = new ObservableCollection<DataGridColumn>();
        public ObservableCollection<DataGridColumn> Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }

        public ObservableCollection<object> ItemsSource;

        public EntityHelper EH;
    }
}
