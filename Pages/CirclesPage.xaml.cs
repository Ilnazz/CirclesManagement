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
    public partial class CirclesPage : EntityPage
    {
        public CirclesPage()
        {
            InitializeComponent();

            App.DB.Circles.Load();
            ItemsSource = new ObservableCollection<object>(App.DB.Circles.Local);

            EH = new EntityHelper
            {
                Title = new EntityHelper.Word()
                {
                    Singular = new EntityHelper.WordCases()
                    {
                        Nominative = "кружок",
                        Genitive = "кружка",
                        Dative = "кружку",
                        Accusative = "кружок",
                        Ablative = "кружком",
                        Prepositional = "кружке"
                    },
                    Plural = new EntityHelper.WordCases()
                    {
                        Nominative = "кружки",
                        Genitive = "кружков",
                        Dative = "кружкам",
                        Accusative = "кружки",
                        Ablative = "кружками",
                        Prepositional = "кружках"
                    }
                },

                Builder = () =>
                {
                    var newCircle = new Circle();
                    newCircle.Title = "";
                    newCircle.IsWorking = true;
                    newCircle.MaxNumberOfPupils = 0;
                    return newCircle;
                },

                IsBlank = (obj) =>
                {
                    var circle = obj as Circle;
                    return circle.Title == "" && circle.MaxNumberOfPupils == 0;
                },

                IsDeleted = (obj) =>
                {
                    var circle = obj as Circle;
                    return circle.IsWorking == false;
                },

                SearchTextMatcher = (obj, searchText) =>
                {
                    var circle = obj as Circle;
                    return circle.Title.Contains(searchText)
                        || circle.MaxNumberOfPupils.ToString().Contains(searchText);
                },

                Validator = (obj) =>
                {
                    var circle = obj as Circle;
                    if (string.IsNullOrWhiteSpace(circle.Title))
                        return (false, "Название не может быть пустым");
                    else if (Helpers.ContainsOnlyRussianLetters(circle.Title) == false)
                        return (false, "Название должно содержать только русские буквы");
                    else if (circle.MaxNumberOfPupils <= 0)
                        return (false, "Максимальное число учеников должно быть больше нуля");
                    return (true, "");
                },

                Deleter = (obj) =>
                {
                    var circle = obj as Circle;

                    var isPresentInTimetable = circle.Timetables.Count > 0;
                    if (isPresentInTimetable)
                        return (false, "указан в расписании занятий.");
                    
                    var isAttendedByPupils = circle.Circle_Pupil
                        .Any(circle_pupil => circle_pupil.IsAttending == true);
                    if (isPresentInTimetable)
                        return (false, "посещюат ученики");

                    circle.IsWorking = false;
                    return (true, "");
                }
            };
        }
    }
}
