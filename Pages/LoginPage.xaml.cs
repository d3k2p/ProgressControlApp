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
using ProgressControlApp;
using ProgressControlApp.DataBase;

namespace ProgressControlApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public void Login(string login, string password)
        {
            Context.Instance.User.ToList();
            List<User> users = new List<User>();
            users = Context.Instance.User.Where(u => u.Login == login).ToList();
            if (users.Count() == 0)
            {
                MessageBox.Show("Неверный логин", "Login", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            users = users.Where(u => u.Password == password).ToList();
            if (users.Count() == 0)
            {
                MessageBox.Show("Неверный пароль", "Password", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            CurrentData.CurrentData.CurrentUser = users.FirstOrDefault();
            CurrentData.CurrentData.CurrentFrame.Navigate(new MainPage());
        }
        public LoginPage()
        {
            InitializeComponent();
        }
        private void ButtonLoginClick(object sender, RoutedEventArgs e)
        {
            Login(FieldLogin.Text, FieldPassword.Password);
        }

        private void ButtonGuestClick(object sender, RoutedEventArgs e)
        {
            using (var context = new Context())
            {
                CurrentData.CurrentData.CurrentUser = new User() { Name = "guest", LastName = "-", Role = Context.Instance.Role.ToList()[2] };
                CurrentData.CurrentData.CurrentFrame.Navigate(new MainPage());
            }
        }
    }
}
