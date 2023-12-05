using CirclesManagement.Classes;
using CirclesManagement.Components;
using System;
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
using System.Windows.Shapes;

namespace CirclesManagement.Pages
{
    /// <summary>
    /// Логика взаимодействия для GroupEditWindow.xaml
    /// </summary>
    public partial class GroupEditWindow : Window
    {
        public Group CurrentGroup
        {
            get { return (Group)GetValue(CurrentGroupProperty); }
            set { SetValue(CurrentGroupProperty, value); }
        }
        public static readonly DependencyProperty CurrentGroupProperty =
            DependencyProperty.Register("CurrentGroup", typeof(Group), typeof(GroupEditWindow), new PropertyMetadata(null));

        public GroupEditWindow(Group group)
        {
            InitializeComponent();

            if (App.CurrentUser.Role.ID is 1)
                TBInformation.Text = $"Группа учителя {group.Teacher.LastNameAndInitials} по кружку {group.Circle.Title}";
            else
                TBInformation.Text = $"Группа учеников по кружку {group.Circle.Title}";

            CurrentGroup = group;

            var groupEditPage = new GroupEditPage(group);

            BtnAdd.Click += (s, e) => groupEditPage.AddNewEntity();
            BtnDelete.Click += (s, e) => groupEditPage.DeleteEntity();
            BtnSaveChanges.Click += (s, e) => groupEditPage.SaveChanges();

            FrameForPage.Navigate(groupEditPage);

            Closing += (s, e) =>
            {
                if (App.DB.ChangeTracker.HasChanges() == true)
                {
                    groupEditPage.DiscardAddedEntities();
                }
            };
        }
    }
}
