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
    /// Логика взаимодействия для GradesPage.xaml
    /// </summary>
    public partial class GradesPage : EntityPage
    {
        public GradesPage()
        {
            InitializeComponent();

            EH = new EntityHelper
            {
                Builder = () =>
                {
                    var newGrade = new Grade
                    {
                        Title = "",
                        IsActive = true
                    };
                    App.DB.Grades.Local.Add(newGrade);
                    return newGrade;
                },

                IsBlank = (obj) =>
                {
                    var grade = obj as Grade;
                    return grade.Title == "";
                },

                Validator = (obj) =>
                {
                    var grade = obj as Grade;
                    if (string.IsNullOrWhiteSpace(grade.Title))
                        return (false, "название класса не может быть пустым");
                    else if (Helpers.IsValidGradeTitle(grade.Title) == false)
                        return (false, $"название класса \"{grade.Title}\" - некорректно; " +
                            $"оно должно содеражть одну-две цифры и (опционально) одну букву");
                    return (true, "");
                },

                Comparer = (obj1, obj2) =>
                {
                    var grade1 = obj1 as Grade;
                    var grade2 = obj2 as Grade;
                    if (grade1.Title.ToLower().Trim() == grade2.Title.ToLower().Trim())
                        return (false, $"класс с названием \"{grade1.Title}\" уже существует");
                    return (true, "");
                },

                SearchTextMatcher = (obj, searchText) =>
                {
                    var grade = obj as Grade;
                    return grade.Title.Contains(searchText);
                },

                Deleter = (obj) =>
                {
                    var grade = obj as Grade;

                    var isConnectedWithPupil = grade.Pupils.Count > 0;
                    if (isConnectedWithPupil)
                        return (false, $"есть ученики, которые состоят классе \"{grade.Title}\"");

                    grade.IsActive = false;
                    return (true, "");
                },

                IsDeleted = (obj) =>
                {
                    var grade = obj as Grade;
                    return grade.IsActive == false;
                },

                SavePreparator = (obj) =>
                {
                    var grade = obj as Grade;
                    grade.Title = Helpers.Capitalize(grade.Title);
                }
            };

            EntitiesSource = new ObservableCollection<object>(App.DB.Grades.Local);
        }
    }
}
