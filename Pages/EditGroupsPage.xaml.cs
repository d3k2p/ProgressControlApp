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
using ProgressControlApp.CurrentData;
using ProgressControlApp.DataBase;

namespace ProgressControlApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditGroupsPage.xaml
    /// </summary>
    public partial class EditGroupsPage : Page
    {
        public Group Group { get; set; }
        public EditGroupsPage(Group group)
        {
            InitializeComponent();
            Group = group;
        }

        private void ButtonGoToTableClick(object sender, RoutedEventArgs e)
        {
            CurrentData.CurrentData.CurrentFrame.Navigate(new TablePage("Группы"));
        }
        private void ButtonSaveClick(object sender, RoutedEventArgs e)
        {
            if (FieldName.Text.Trim() == "")
            {
                MessageBox.Show("Поле не заполнено", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            List<Group> groups = Context.Instance.Group.Where(s => s.Name.Trim() == FieldName.Text.Trim()).ToList();
            if (groups.Count() != 0)
            {
                MessageBox.Show("Группа уже существует", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Group.Name = FieldName.Text;

            Context.Instance.SaveChanges();
            CurrentData.CurrentData.CurrentFrame.Navigate(new TablePage("Группы"));
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FieldName.Text = Group.Name;
        }
    }
}
