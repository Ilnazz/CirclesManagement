using CirclesManagement.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
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
    /// Логика взаимодействия для NavigationComponent.xaml
    /// </summary>
    public partial class NavigationComponent : UserControl
    {
        public IEnumerable<string> ItemsSource
        {
            get { return (IEnumerable<string>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable<string>), typeof(NavigationComponent), new PropertyMetadata(new List<string>()));

        public event Action SelectionChanged
        {
            add { InnerListBox.SelectionChanged += (s, e) => value(); }
            remove { InnerListBox.SelectionChanged -= (s, e) => value(); }
        }

        public int SelectedIndex
        {
            get { return InnerListBox.SelectedIndex; }
            set { InnerListBox.SelectedIndex = value; }
        }

        public NavigationComponent()
        {
            InitializeComponent();
        }
    }
}
