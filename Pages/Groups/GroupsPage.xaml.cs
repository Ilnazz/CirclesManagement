﻿using CirclesManagement.Classes;
using CirclesManagement.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Логика взаимодействия для GroupsPage.xaml
    /// </summary>
    public partial class GroupsPage : EntityPage
    {
        public GroupsPage()
        {
            InitializeComponent();

            EH = new EntityHelper
            {
                Builder = () =>
                {
                    var newGroup = new Group
                    {
                        Teacher = null,
                        Circle = null,
                        IsActive = true
                    };
                    App.DB.Groups.Local.Add(newGroup);
                    return newGroup;
                },

                IsBlank = (obj) =>
                {
                    var group = obj as Group;
                    return group.Teacher == null
                        && group.Circle == null;
                },

                Validator = (obj) =>
                {
                    var group = obj as Group;
                    if (group.Teacher == null)
                        return (false, "необходимо выбрать учителя");
                    else if (group.Circle == null)
                        return (false, "необходимо выбрать кружок");
                    return (true, "");
                },

                Comparer = (obj1, obj2) =>
                {
                    var group1 = obj1 as Group;
                    var group2 = obj2 as Group;
                    if (group1.Teacher == group2.Teacher
                        && group1.Circle == group2.Circle)
                            return (false, $"учитель \"{group1.Teacher.FullName}\" уже ведёт кружок \"{group1.Circle.Title}\"");
                    return (true, "");
                },

                SearchTextMatcher = (obj, searchText) =>
                {
                    var group = obj as Group;
                    return group.Teacher.FullName.Contains(searchText)
                        || group.Circle.Title.Contains(searchText);
                },

                Deleter = (obj) =>
                {
                    var group = obj as Group;
                    var isAttendedByAnyPupil = group.Group_Pupil.Any(group_pupil => group_pupil.IsAttending == true);
                    if (isAttendedByAnyPupil)
                        return (false, $"Кружок \"{group.Title}\" нельзя удалить, так как его посещает как минимум один ученик");
                    group.IsActive = false;
                    return (true, "");
                },

                IsDeleted = (obj) =>
                {
                    var group = obj as Group;
                    return group.IsActive == false;
                },

                SavePreparator = (obj) =>
                {
                    var group = obj as Group;
                }
            };

            EntitiesSource = new ObservableCollection<object>(App.DB.Groups.Local);

            DataContext = new
            {
                Teachers = App.DB.Teachers.Local.Where(teacher => teacher.IsWorking == true),
                Circles = App.DB.Circles.Local.Where(circle => circle.IsWorking == true)
            };

            HasEditFunction = true;
            EditHandler += () =>
            {
                if (InnerDataGrid.SelectedItem == null)
                    return;
                var selectedGroup = InnerDataGrid.SelectedItem as Group;
                var groupEditWindow = new GroupEditWindow(selectedGroup);
                groupEditWindow.ShowDialog();
                groupEditWindow.Activate();
            };
        }
    }
}
