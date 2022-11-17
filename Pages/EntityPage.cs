using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CirclesManagement.Pages
{
    public abstract class EntityPage : Page
    {
        private ObservableCollection<DataGridColumn> _columns = new ObservableCollection<DataGridColumn>();
        public ObservableCollection<DataGridColumn> Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }

        public ObservableCollection<object> ItemsSource;

        public Func<object, string, bool> SearchTextMatcher;
        public Func<object, bool> IsEntityDeleted;
        public Predicate<object> Filter;

        public Func<object> EntityCreator;

        public Func<object, bool> EntityValidator;
        public Action<object> ValidationErrorCallback;

        public Action<object> DeletingEntity;
    }
}
