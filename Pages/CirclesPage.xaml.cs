using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

using CirclesManagement.Classes;
using CirclesManagement.Components;

namespace CirclesManagement.Pages
{
    /// <summary>
    /// Логика взаимодействия для CirclesPage.xaml
    /// </summary>
    public partial class CirclesPage : EntityPage
    {
        private ObservableCollection<Circle> _circles;

        public CirclesPage()
        {
            InitializeComponent();

            App.DB.Circles.Load();
            _circles = App.DB.Circles.Local;
            ItemsSource = new ObservableCollection<object>(_circles);

            EntityCreator += () =>
            {
                var newCircle = new Circle();
                newCircle.Title = "";
                newCircle.IsWorking = true;
                newCircle.MaxNumberOfPupils = 20;
                return newCircle;
            };

            IsEntityDeleted += (obj) =>
            {
                var circle = obj as Circle;
                return circle.IsWorking == false;
            };

            SearchTextMatcher += (obj, searchText) =>
            {
                var circle = obj as Circle;

                return circle.Title.Contains(searchText)
                    || circle.MaxNumberOfPupils.ToString().Contains(searchText);
            };

            EntityValidator += (obj) =>
            {
                var circle = obj as Circle;
                return string.IsNullOrWhiteSpace(circle.Title) == false
                    && Helpers.ContainsOnlyRussianLetters(circle.Title) == true
                    && circle.MaxNumberOfPupils != 0;
            };

            ValidationErrorCallback += (obj) =>
            {
                var circle = obj as Circle;
                Helpers.Error($"Ошибка валидации кружка \"{circle.Title}\".");
            };

            DeletingEntity += (obj) =>
            {
                var circle = obj as Circle;

                // if circle is empty delete it from list

                var result = Helpers.Ask("Вы уверены, что хотите удалить данный кружок?");
                if (result == false)
                    return;

                var isPresentInTimetable = circle.Timetables.Count > 0;
                if (isPresentInTimetable)
                {
                    Helpers.Error("Данный кружок нельзя удалить, так как он указан в расписании занятий.");
                    return;
                }
                var isAttendedByPupils = circle.Circle_Pupil.Any(circle_pupil => circle_pupil.IsAttending == true);
                if (isPresentInTimetable)
                {
                    Helpers.Error("Данный кружок нельзя удалить, так как его посещюат ученики.");
                    return;
                }
                circle.IsWorking = false;
                Helpers.Inform($"Кружок успешно удалён.");
            };
        }
    }
}
