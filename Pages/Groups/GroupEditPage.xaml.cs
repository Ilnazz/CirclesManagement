using CirclesManagement.Components;
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

namespace CirclesManagement.Pages
{
    /// <summary>
    /// Логика взаимодействия для GroupEditPage.xaml
    /// </summary>
    public partial class GroupEditPage : EntityPage
    {
        public GroupEditPage(Group group)
        {
            InitializeComponent();

            EH = new EntityHelper
            {
                Builder = () =>
                {
                    var newGroup_Pupil = new Group_Pupil
                    {
                        Pupil = null,
                        IsAttending = true
                    };
                    group.Group_Pupil.Add(newGroup_Pupil);
                    return newGroup_Pupil;
                },

                IsBlank = (obj) =>
                {
                    var group_pupil = obj as Group_Pupil;
                    return group_pupil.Pupil == null;
                },

                Validator = (obj) =>
                {
                    var group_pupil = obj as Group_Pupil;
                    if (group_pupil.Pupil == null)
                        return (false, "необходимо выбрать ученика");
                    return (true, "");
                },

                Comparer = (obj1, obj2) =>
                {
                    var group_pupil1 = obj1 as Group_Pupil;
                    var group_pupil2 = obj2 as Group_Pupil;
                    if (group_pupil1.Pupil == group_pupil2.Pupil)
                        return (false, $"нельзя записать одного ученика в группу дважды");
                    return (true, "");
                },

                SearchTextMatcher = (obj, searchText) =>
                {
                    var group_pupil = obj as Group_Pupil;
                    return group_pupil.Pupil.FullName.Contains(searchText);
                },

                Deleter = (obj) => (false, ""),

                IsDeleted = (obj) => false
            };

            EntitiesSource = new ObservableCollection<object>(group.Group_Pupil);

            DataContext = new
            {
                Pupils = App.DB.Pupils.Local.Where(pupil => pupil.IsStudying == true)
            };
        }
    }
}
