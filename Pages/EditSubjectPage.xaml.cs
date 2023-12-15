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
    /// Логика взаимодействия для EditSubjectPage.xaml
    /// </summary>
    public partial class EditSubjectPage : Page
    {
        public Subject Subject { get; set; }
        public EditSubjectPage(Subject subject)
        {
            InitializeComponent();
            Subject = subject;
        }

        private void ButtonGoToTableClick(object sender, RoutedEventArgs e)
        {
            CurrentData.CurrentData.CurrentFrame.Navigate(new TablePage("Предметы"));
        }
        private void ButtonSaveClick(object sender, RoutedEventArgs e)
        {
            if (FieldName.Text.Trim() == "")
            {
                MessageBox.Show("Поле не заполнено", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Subject.Name = FieldName.Text;

            Context.Instance.SaveChanges();
            CurrentData.CurrentData.CurrentFrame.Navigate(new TablePage("Предметы"));
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FieldName.Text = Subject.Name;
        }
    }
}
