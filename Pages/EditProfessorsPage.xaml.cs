using OfficeOpenXml.Style;
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
    /// Логика взаимодействия для EditProfessorsPage.xaml
    /// </summary>
    public partial class EditProfessorsPage : Page
    {
        public Professor Professor { get; set; }
        public EditProfessorsPage(Professor professor)
        {
            InitializeComponent();
            Professor = professor;
        }

        private void ButtonGoToTableClick(object sender, RoutedEventArgs e)
        {
            CurrentData.CurrentData.CurrentFrame.Navigate(new TablePage("Преподаватели"));
        }
        private void ButtonSaveClick(object sender, RoutedEventArgs e)
        {
            if (FieldName.Text.Trim() == "" && FieldLastName.Text.Trim() == "" && FieldSubject.SelectedItem == null)
            {
                MessageBox.Show("Не все поля заполнены", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Professor.Name = FieldName.Text;
            Professor.LastName = FieldLastName.Text;
            Professor.Subject = FieldSubject.SelectedItem as Subject;
            Context.Instance.SaveChanges();
            CurrentData.CurrentData.CurrentFrame.Navigate(new TablePage("Преподаватели"));
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FieldSubject.ItemsSource = Context.Instance.Subject.ToList();
            FieldName.Text = Professor.Name;
            FieldLastName.Text = Professor.LastName;
            FieldSubject.SelectedItem = Professor.Subject;
        }
    }
}
