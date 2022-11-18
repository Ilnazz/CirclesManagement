using CirclesManagement.Classes;
using CirclesManagement.Components;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CirclesManagement
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly CirclesManagementEntities DB = new CirclesManagementEntities();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            RegisterGlobalExceptionHandler();
        }

        private void RegisterGlobalExceptionHandler()
        {
            DispatcherUnhandledException += (sender, e) =>
            {
                e.Handled = true;
                HandleException(e.Exception);
            };
            Dispatcher.UnhandledException += (sender, e) =>
            {
                e.Handled = true;
                HandleException(e.Exception);
            };

            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                HandleException(e.Exception);
            };
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                HandleException(e.ExceptionObject as Exception);
            };

        }

        private void HandleException(Exception e)
        {
            Helpers.Error(e.Message);
        }
    }
}
