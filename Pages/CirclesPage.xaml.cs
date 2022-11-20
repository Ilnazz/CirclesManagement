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

            EH = new EntityHelper
            {
                Builder = () =>
                {
                    var newCircle = new Circle
                    {
                        Title = "",
                        IsWorking = true,
                        MaxNumberOfPupils = 0
                    };
                    App.DB.Circles.Local.Add(newCircle);
                    return newCircle;
                },

                IsBlank = (obj) =>
                {
                    var circle = obj as Circle;
                    return circle.Title == "" && circle.MaxNumberOfPupils == 0;
                },

                Validator = (obj) =>
                {
                    var circle = obj as Circle;
                    if (string.IsNullOrWhiteSpace(circle.Title))
                        return (false, "название кружка не может быть пустым");
                    else if (circle.MaxNumberOfPupils <= 0)
                        return (false, $"максимальное число учеников кружка \"{circle.Title}\" должно быть больше нуля");
                    return (true, "");
                },

                Comparer = (obj1, obj2) =>
                {
                    var circle1 = obj1 as Circle;
                    var circle2 = obj2 as Circle;
                    if (circle1.Title.ToLower().Trim() == circle2.Title.ToLower().Trim())
                        return (false, $"кружок с названием \"{circle1.Title}\" уже существует");
                    return (true, "");
                },

                SearchTextMatcher = (obj, searchText) =>
                {
                    var circle = obj as Circle;
                    return circle.Title.Contains(searchText)
                        || circle.MaxNumberOfPupils.ToString().Contains(searchText);
                },

                Deleter = (obj) =>
                {
                    var circle = obj as Circle;
                    var isPresentInTimetable = circle.Timetables.Count > 0;
                    if (isPresentInTimetable)
                        return (false, $"кружок {circle.Title} указан в расписании занятий");

                    var isAttendedByPupils = circle.Circle_Pupil
                        .Any(circle_pupil => circle_pupil.IsAttending == true);
                    if (isAttendedByPupils)
                        return (false, $"кружок {circle.Title} посещюат ученики");

                    circle.IsWorking = false;
                    return (true, "");
                },

                IsDeleted = (obj) =>
                {
                    var circle = obj as Circle;
                    return circle.IsWorking == false;
                },

                SavePreparator = (obj) =>
                {
                    var circle = obj as Circle;
                    circle.Title = Helpers.Capitalize(circle.Title);
                }
            };

            EntitiesSource = new ObservableCollection<object>(App.DB.Circles.Local);
        }
    }
}
