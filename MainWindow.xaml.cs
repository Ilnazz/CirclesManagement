﻿using System;
using System.Collections.Generic;
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
using CirclesManagement.Pages;

namespace CirclesManagement
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static CirclesManagementEntities db = new CirclesManagementEntities();
        public static StatusBarComponent StatusBar;
        public static User CurrentUser;

        public MainWindow()
        {
            InitializeComponent();

            StatusBar = StatusBarComponentInstance;

            Navigation.AppWindow = this;

            Navigation.Next(("Страница авторизации", new AuthPage()));

            //TODO: On first running of this application it is neccessary to provide form to register associate director
        }

        private void BtnGoToPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            Navigation.Back();
        }

        private void BtnLogOut_Click(object sender, RoutedEventArgs e)
        {
            var result = Helpers.AskQuestion("Вы уверены, что хотите выйти из системы?");
            if (result == true)
            {
                CurrentUser = null;
                Navigation.IsUserAuthorized = false;
                Navigation.History.Clear();
                Navigation.Next(("Страница авторизации", new AuthPage()));
                StatusBar.Info("Вы успешно вышли из системы.");
            }
        }
    }
}
