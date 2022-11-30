using CirclesManagement.Classes;
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
    /// Логика взаимодействия для PupilsPage.xaml
    /// </summary>
    public partial class PupilsPage : EntityPage
    {
        public PupilsPage()
        {
            InitializeComponent();

            EH = new EntityHelper
            {
                Builder = () =>
                {
                    var newPupil = new Pupil
                    {
                        LastName = "",
                        FirstName = "",
                        Patronymic = "",
                        IsStudying = true,
                        Grade = null
                    };
                    App.DB.Pupils.Local.Add(newPupil);
                    return newPupil;
                },

                IsBlank = (obj) =>
                {
                    var pupil = obj as Pupil;
                    return pupil.LastName == ""
                        && pupil.FirstName == ""
                        && pupil.Patronymic == ""
                        && pupil.Grade == null;
                },

                Validator = (obj) =>
                {
                    var pupil = obj as Pupil;
                    if (string.IsNullOrWhiteSpace(pupil.LastName))
                        return (false, "фамилия ученика не может быть пустой");
                    else if (string.IsNullOrWhiteSpace(pupil.FirstName))
                        return (false, "имя ученика не может быть пустым");
                    else if (string.IsNullOrWhiteSpace(pupil.Patronymic))
                        return (false, "отчество ученика не может быть пустым");
                    else if (pupil.Grade == null)
                        return (false, $"у ученика \"{pupil.FullName}\" должен быть задан класс");
                    return (true, "");
                },

                Comparer = (obj1, obj2) =>
                {
                    var pupil1 = obj1 as Pupil;
                    var pupil2 = obj2 as Pupil;
                    if (pupil1.LastName.ToLower().Trim() == pupil2.LastName.ToLower().Trim()
                        && pupil1.FirstName.ToLower().Trim() == pupil2.FirstName.ToLower().Trim()
                        && pupil1.Patronymic.ToLower().Trim() == pupil2.Patronymic.ToLower().Trim())
                            return (false, $"ученик с ФИО {pupil1.LastName} {pupil1.FirstName} {pupil1.Patronymic} уже существует");
                    return (true, "");
                },

                SearchTextMatcher = (obj, searchText) =>
                {
                    var pupil = obj as Pupil;
                    return pupil.LastName.Contains(searchText)
                        || pupil.FirstName.Contains(searchText)
                        || pupil.Patronymic.Contains(searchText)
                        || (pupil.Grade != null && pupil.Grade.Title.Contains(searchText));
                },

                Deleter = (obj) =>
                {
                    var pupil = obj as Pupil;

                    var isAttendingAnyCircle = pupil.Group_Pupil.Any(group_pupil => group_pupil.IsAttending == true);
                    if (isAttendingAnyCircle)
                        return (false, $"ученик $\"{pupil.FullName}\" посещает как минимум один кружок(и)");

                    pupil.IsStudying = false;
                    return (true, "");
                },

                IsDeleted = (obj) =>
                {
                    var pupil = obj as Pupil;
                    return pupil.IsStudying == false;
                },
                
                SavePreparator = (obj) =>
                {
                    var pupil = obj as Pupil;
                    pupil.LastName = Helpers.Capitalize(pupil.LastName);
                    pupil.FirstName = Helpers.Capitalize(pupil.FirstName);
                    pupil.Patronymic = Helpers.Capitalize(pupil.Patronymic);
                }
            };

            dataGridGroupping = (
                new PropertyGroupDescription("Grade.Title"),
                FindResource("PupilsDataGridGroupStyle") as GroupStyle
            );

            EntitiesSource = new ObservableCollection<object>(App.DB.Pupils.Local);

            DataContext = new
            {
                Grades = App.DB.Grades.Local.Where(grade => grade.IsActive == true)
            };
        }
    }
}
