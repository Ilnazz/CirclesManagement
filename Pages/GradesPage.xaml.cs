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
    /// Логика взаимодействия для GradesPage.xaml
    /// </summary>
    public partial class GradesPage : EntityPage
    {
        public GradesPage()
        {
            InitializeComponent();

            App.DB.Grades.Load();
            ItemsSource = new ObservableCollection<object>(App.DB.Grades.Local);

            EH = new EntityHelper
            {
                Title = new EntityHelper.Word()
                {
                    Singular = new EntityHelper.WordCases()
                    {
                        Nominative = "класс",
                        Genitive = "класса",
                        Dative = "классу",
                        Accusative = "класс",
                        Ablative = "классом",
                        Prepositional = "классе"
                    },
                    Plural = new EntityHelper.WordCases()
                    {
                        Nominative = "классы",
                        Genitive = "классов",
                        Dative = "классам",
                        Accusative = "классы",
                        Ablative = "классами",
                        Prepositional = "классах"
                    }
                },
                
                Builder = () =>
                {
                    var newGrade = new Grade();
                    newGrade.Title = "";
                    newGrade.IsActive = true;
                    App.DB.Grades.Local.Add(newGrade);
                    return newGrade;
                },

                IsBlank = (obj) =>
                {
                    var grade = obj as Grade;
                    return grade.Title == "";
                },

                IsDeleted = (obj) =>
                {
                    var grade = obj as Grade;
                    return grade.IsActive == false;
                },

                Comparer = (obj1, obj2) =>
                {
                    var grade1 = obj1 as Grade;
                    var grade2 = obj2 as Grade;
                    return grade1.Title == grade2.Title;
                },

                SearchTextMatcher = (obj, searchText) =>
                {
                    var grade = obj as Grade;
                    return grade.Title.Contains(searchText);
                },

                Validator = (obj) =>
                {
                    var grade = obj as Grade;
                    if (string.IsNullOrWhiteSpace(grade.Title))
                        return (false, "название не может быть пустым");
                    else if (grade.Title.Length > 3)
                        return (false, "название должно содеражть одну-две цифры и (опционально) одну букву");
                    return (true, "");
                },

                Deleter = (obj) =>
                {
                    var grade = obj as Grade;

                    var isConnectedWithPupil = grade.Pupils.Count > 0;
                    if (isConnectedWithPupil)
                        return (false, "указан у ученика(ов)");
                    
                    grade.IsActive = false;
                    return (true, "");
                }
            };
        }
    }
}
