using ProgressControlApp.DataBase;
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

namespace ProgressControlApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditStudentPage.xaml
    /// </summary>
    public partial class EditStudentPage : Page
    {
        public Student Student { get; set; }
        public EditStudentPage(Student student)
        {
            InitializeComponent();
            Student = student;
        }

        private void ButtonGoToTableClick(object sender, RoutedEventArgs e)
        {
            CurrentData.CurrentData.CurrentFrame.Navigate(new TablePage("Студенты"));
        }
        private void ButtonSaveClick(object sender, RoutedEventArgs e)
        {
            if (FieldStudent.SelectedItem == null && FieldGroup.SelectedItem == null)
            {
                MessageBox.Show("Не все поля заполнены", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            User user = FieldStudent.SelectedItem as User;
            Group group = FieldGroup.SelectedItem as Group;
            List<Student> students = Context.Instance.Student.Where(s => s.Group.ID == group.ID && s.User.ID == user.ID).ToList();
            if (students.Count() != 0)
            {
                MessageBox.Show("Студент уже существует", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Student.User = user;
            Student.Group = group;
            Context.Instance.SaveChanges();
            CurrentData.CurrentData.CurrentFrame.Navigate(new TablePage("Студенты"));
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Role role = Context.Instance.Role.ToList()[1];
            FieldStudent.ItemsSource = Context.Instance.User.Where(u => u.Role.ID == role.ID).ToList();
            FieldGroup.ItemsSource = Context.Instance.Group.ToList();
            FieldStudent.SelectedItem = Student.User;
            FieldGroup.SelectedItem = Student.Group;
        }
    }
}
