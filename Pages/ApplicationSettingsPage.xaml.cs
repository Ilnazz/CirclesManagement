using CirclesManagement.Classes;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для ApplicationSettingsPage.xaml
    /// </summary>
    public partial class ApplicationSettingsPage : Page
    {
        public ApplicationSettingsPage()
        {
            InitializeComponent();

            if (MainWindow.ApplicationSetting.GradeNumerationTypeID == (int)Constants.GradeNumerationType.Number)
                RBNumber.IsChecked = true;
            else
                RBNumberAndLetter.IsChecked = true;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (RBNumber.IsChecked ?? false)
                MainWindow.ApplicationSetting.GradeNumerationTypeID = (int)Constants.GradeNumerationType.Number;
            else if (RBNumberAndLetter.IsChecked ?? false)
                MainWindow.ApplicationSetting.GradeNumerationTypeID = (int)Constants.GradeNumerationType.NumberAndLetter;
        }
        
        private void BtnSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.db.ChangeTracker.HasChanges() == false)
            {
                MainWindow.StatusBar.Info("Нет изменений для сохранения.");
                return;
            }
            MainWindow.db.SaveChanges();
        }

        private void BtnResetSettings_Click(object sender, RoutedEventArgs e)
        {
            var result = Helpers.AskQuestion("Вы уверены, что хотите восстановить настройки по умолчанию?");
            if (result == true)
            {
                MainWindow.ApplicationSetting.GradeNumerationTypeID = (int)Constants.GradeNumerationType.Number;
                RBNumber.IsChecked = true;
                MainWindow.db.SaveChanges();
            }
        }

        private void BtnDeleteAllData_Click(object sender, RoutedEventArgs e)
        {
            var result = Helpers.AskQuestionWarning("Вы уверены, что хотите удалить все данные?");
            if (result == true)
            {
                MainWindow.db.Users.Local.Clear();
                MainWindow.db.Teachers.Local.Clear();

                MainWindow.db.Grades.Local.Clear();
                MainWindow.db.Pupils.Local.Clear();
                MainWindow.db.Circles.Local.Clear();
                MainWindow.db.Circle_Pupil.Local.Clear();

                MainWindow.db.Lessons.Local.Clear();
                MainWindow.db.Lesson_Pupil.Local.Clear();

                MainWindow.db.Timetables.Local.Clear();

                MainWindow.db.SaveChanges();
                // TODO: unathorize, go to main page
                // TODO: think about settings of application,...
            }
        }
    }
}
