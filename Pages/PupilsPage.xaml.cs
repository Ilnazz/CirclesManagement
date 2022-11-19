using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
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

using CirclesManagement.Classes;
using CirclesManagement.Components;

namespace CirclesManagement.Pages
{
    /// <summary>
    /// Логика взаимодействия для CirclesPage.xaml
    /// </summary>
    public partial class PupilsPage : EntityPage
    {
        public PupilsPage()
        {
            InitializeComponent();

            App.DB.Pupils.Load();
            ItemsSource = new ObservableCollection<object>(App.DB.Pupils.Local);

            EntityCollectionsForComboBoxes = new
            {
                Grades = App.DB.Grades.Local
            };

            EH = new EntityHelper
            {
                Title = new EntityHelper.Word()
                {
                    Singular = new EntityHelper.WordCases()
                    {
                        Nominative = "ученик",
                        Genitive = "ученика",
                        Dative = "ученику",
                        Accusative = "ученика",
                        Ablative = "учеником",
                        Prepositional = "ученике"
                    },
                    Plural = new EntityHelper.WordCases()
                    {
                        Nominative = "ученики",
                        Genitive = "учеников",
                        Dative = "ученикам",
                        Accusative = "учеников",
                        Ablative = "учениками",
                        Prepositional = "учениках"
                    }
                },

                Builder = () =>
                {
                    var newPupil = new Pupil();
                    newPupil.LastName = "";
                    newPupil.FirstName = "";
                    newPupil.Patronymic = "";
                    newPupil.IsStudying = true;
                    App.DB.Pupils.Local.Add(newPupil);
                    return newPupil;
                },

                IsBlank = (obj) =>
                {
                    var pupil = obj as Pupil;
                    return pupil.LastName == ""
                        && pupil.FirstName == ""
                        && pupil.Patronymic == "";
                },

                IsDeleted = (obj) =>
                {
                    var pupil = obj as Pupil;
                    return pupil.IsStudying == false;
                },

                Comparer = (obj1, obj2) =>
                {
                    var pupil1 = obj1 as Pupil;
                    var pupil2 = obj2 as Pupil;
                    return pupil1.LastName == pupil2.LastName
                        && pupil1.FirstName == pupil2.FirstName
                        && pupil1.Patronymic == pupil2.Patronymic;
                },

                SearchTextMatcher = (obj, searchText) =>
                {
                    var pupil = obj as Pupil;
                    return pupil.LastName.Contains(searchText)
                        || pupil.FirstName.Contains(searchText)
                        || pupil.Patronymic.Contains(searchText);
                },

                Validator = (obj) =>
                {
                    var pupil = obj as Pupil;
                    if (string.IsNullOrWhiteSpace(pupil.LastName))
                        return (false, "фамилия не может быть пустой");
                    else if (string.IsNullOrWhiteSpace(pupil.FirstName))
                        return (false, "имя не может быть пустым");
                    else if (string.IsNullOrWhiteSpace(pupil.Patronymic))
                        return (false, "отчество не может быть пустым");
                    return (true, "");
                },

                Deleter = (obj) =>
                {
                    var pupil = obj as Pupil;

                    var isAttendingAnyCircle = pupil.Circle_Pupil
                        .Any(circle_pupil => circle_pupil.IsAttending == true);
                    if (isAttendingAnyCircle)
                        return (false, "посещает кружок(и)");
                    
                    pupil.IsStudying = false;
                    return (true, "");
                }
            };
        }
    }
}
