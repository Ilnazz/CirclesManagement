using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using CirclesManagement.ADO;
using CirclesManagement.Pages;

namespace CirclesManagement
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static CirclesManagementEntities db = new CirclesManagementEntities();
        public static User CurrentUser;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = CurrentUser;
            MainFrame.Navigate(new AuthPage());

            //TODO: On first running of this application it is neccessary to provide form to register associate director
        }

        
    }
}
