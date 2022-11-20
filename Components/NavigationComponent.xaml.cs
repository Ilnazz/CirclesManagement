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
            set {
                SetValue(ItemsSourceProperty, value);

                int index = 0;
                InnerListBox.ItemsSource = ItemsSource.Select(item => new { Item = item, Index = index++ });
            }
        }
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable<string>), typeof(NavigationComponent), new PropertyMetadata(new List<string>()));

        // Происходит перед выбором следующего элемента
        public Func<int, int, bool> BeforeSelectionChanged = (_, __) => true;
        
        public NavigationComponent()
        {
            InitializeComponent();
        }

        private void Item_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var newIndex = (int)(e.Source as TextBlock).Tag;

            if (newIndex == InnerListBox.SelectedIndex)
            {
                // Клик на том же элементе, ничего не менятеся
                return;
            }

            var accept = BeforeSelectionChanged(InnerListBox.SelectedIndex, newIndex);
            if (accept == false)
                e.Handled = true;
        }
    }
}
