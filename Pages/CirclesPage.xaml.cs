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
    public partial class CirclesPage : EntitiesPage
    {
        private ObservableCollection<Circle> _circles;

        public CirclesPage(SearchBoxComponent searchBox)
        {
            InitializeComponent();

            MainWindow.db.Circles.Load();
            _circles = MainWindow.db.Circles.Local;
            CircleList.ItemsSource = new ObservableCollection<object>(_circles);

            searchBox.TextChanged += CircleList.Refresh;

            CircleList.Filter += (obj) =>
            {
                var circle = obj as Circle;

                var accept = false;
                if (ShowDeletedEntities == true && circle.IsWorking == false
                    || ShowDeletedEntities == false && circle.IsWorking == true)
                    accept = true;
                
                if (accept == true)
                {
                    if (searchBox.IsEmpty()
                        || searchBox.IsMatchSearchText(circle.Title)
                        || searchBox.IsMatchSearchText(circle.MaxNumberOfPupils.ToString()))
                        accept = true;
                    else
                        accept = false;
                }

                return accept;
            };

            CircleList.DeletingItem += (obj) =>
            {
                var circle = obj as Circle;

                var result = Helpers.Ask("Вы уверены, что хотите удалить данный кружок?");
                if (result == false)
                    return;

                var isPresentInTimetable = circle.Timetables.Count != 0;
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
                CircleList.Refresh();
                Helpers.Inform($"Кружок успешно удалён.");
            };


        }

        public override void AddEntity()
        {
            Circle newCircle = new Circle();
            newCircle.Title = "";
            newCircle.IsWorking = true;
            newCircle.MaxNumberOfPupils = 0;

            CircleList.AddItemAndScrollIntoView(newCircle);
        }

        private bool IsValidCircle(Circle circle)
        {
            return true;
        }

        public override void SaveChanges()
        {
            if (MainWindow.db.ChangeTracker.HasChanges() == false)
            {
                Helpers.Inform("Нет изменений для сохранения.");
                return;
            }

            foreach (var entry in MainWindow.db.ChangeTracker.Entries())
            {
                var circle = entry.Entity as Circle;
                if (IsValidCircle(circle) == false)
                {
                    Helpers.Error($"{circle.Title} не прошёл проверку :/");
                    return;
                }
            }

            Helpers.AskAndDoActionIfYes("Вы уверены, что хотите сохранить изменения?", () =>
            {
                MainWindow.db.SaveChanges();
                Helpers.Inform("Изменения успешно сохранены.");
            });
        }
    }
}
