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
using ProgressControlApp.DataBase;

namespace ProgressControlApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void ButtonGoOutClick(object sender, RoutedEventArgs e)
        {
            CurrentData.CurrentData.CurrentUser = null;
            CurrentData.CurrentData.CurrentFrame.Navigate(new LoginPage());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            NameUser.Text = "Пользователь: " + CurrentData.CurrentData.CurrentUser.Name + " " + CurrentData.CurrentData.CurrentUser.LastName;
            RoleUser.Text = "Роль: " + CurrentData.CurrentData.CurrentUser.Role.Name;
            if (CurrentData.CurrentData.CurrentUser.Role == Context.Instance.Role.ToList()[1])
            {
                ButtonUsers.IsEnabled = false;
            }
            if(CurrentData.CurrentData.CurrentUser.Role == Context.Instance.Role.ToList()[2])
            {
                ButtonProgress.IsEnabled = false;
            }
        }

        private void ButtonGroups_Click(object sender, RoutedEventArgs e)
        {
            CurrentData.CurrentData.CurrentFrame.Navigate(new TablePage((sender as Button).Content as string));
        }

        private void ButtonProgress_Click(object sender, RoutedEventArgs e)
        {
            CurrentData.CurrentData.CurrentFrame.Navigate(new TablePage((sender as Button).Content as string));
        }

        private void ButtonProfessors_Click(object sender, RoutedEventArgs e)
        {
            CurrentData.CurrentData.CurrentFrame.Navigate(new TablePage((sender as Button).Content as string));
        }

        private void ButtonUsers_Click(object sender, RoutedEventArgs e)
        {
            CurrentData.CurrentData.CurrentFrame.Navigate(new TablePage((sender as Button).Content as string));
        }

        private void ButtonSubject_Click(object sender, RoutedEventArgs e)
        {
            CurrentData.CurrentData.CurrentFrame.Navigate(new TablePage((sender as Button).Content as string));
        }

        private void ButtonStudent_Click(object sender, RoutedEventArgs e)
        {
            CurrentData.CurrentData.CurrentFrame.Navigate(new TablePage((sender as Button).Content as string));
        }
    }
}
