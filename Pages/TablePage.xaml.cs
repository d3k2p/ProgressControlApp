using Microsoft.Win32;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для TablePage.xaml
    /// </summary>
    public partial class TablePage : Page
    {
        public Group Group { get; set; }
        private void CreatingAcademicAchievements(User user)
        {
            bool readOnly = true;
            if(CurrentData.CurrentData.CurrentUser.Role == Context.Instance.Role.ToList()[0])
                readOnly = false;
            MainTable.ItemsSource = null;
            MainTable.Columns.Clear();
            if (!readOnly)
            {
                if (FieldGroup.SelectedItem == null)
                    return;
                Group = FieldGroup.SelectedItem as Group;
            }
            else
            {
                Student studentCurrent = Context.Instance.Student.Where(s => s.User.ID == CurrentData.CurrentData.CurrentUser.ID).FirstOrDefault();
                Group = studentCurrent.Group;
            }
            if (Group == null) return;

            List<DataGridRow> rows = new List<DataGridRow>();

            DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
            dataGridTextColumn.Header = "ФИ";
            dataGridTextColumn.Binding = new Binding("Student.FullName");
            dataGridTextColumn.IsReadOnly = true;
            MainTable.Columns.Add(dataGridTextColumn);

            List<Subject> subjects = Context.Instance.Subject.ToList(); ;

            for (int i = 0; i < subjects.Count; i++)
            {
                dataGridTextColumn = new DataGridTextColumn();
                dataGridTextColumn.Header = subjects[i].Name;
                dataGridTextColumn.Binding = new Binding("Marks[" + i + "].Value");
                dataGridTextColumn.IsReadOnly=readOnly;
                MainTable.Columns.Add(dataGridTextColumn);
            }

            List<Student> students = Context.Instance.Student.Where(s => s.Group.ID == Group.ID).ToList();

            foreach (Student student in students)
            {
                List<Mark> marks = new List<Mark>();

                foreach (Subject subject in subjects)
                {
                    Mark mark = Context.Instance.Mark.Where(m => m.Student.ID == student.ID && m.Subject.ID == subject.ID).FirstOrDefault();
                    marks.Add(mark == null ? new Mark() { Student = student, Subject = subject } : mark);
                }

                rows.Add(new DataGridRow(student, subjects, marks));
            }

            MainTable.ItemsSource = rows;
        }

        class DataGridRow
        {
            public Student Student { get; set; }
            public List<Subject> Subjects { get; set; }
            public List<Mark> Marks { get; set; }
            public DataGridRow(Student student, List<Subject> subjects, List<Mark> marks)
            {
                Student = student;
                Subjects = subjects;
                Marks = marks;
            }
        }
        private void ExportToExcel(string table)
        {
            LicenseContext license = LicenseContext.NonCommercial;
            ExcelPackage.LicenseContext = license;

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add(table);

                for (int i = 0; i < MainTable.Columns.Count(); i++)
                    excelWorksheet.Cells[1, i + 1].Value = MainTable.Columns[i].Header;

                for (int row = 0; row < MainTable.Items.Count; row++)
                {
                    for (int col = 0; col < MainTable.Columns.Count; col++)
                    {
                        switch (table)
                        {
                            case "Пользователи":
                                List<User> users = MainTable.ItemsSource as List<User>;
                                excelWorksheet.Cells[row + 2, 1].Value = users[row].Name;
                                excelWorksheet.Cells[row + 2, 2].Value = users[row].LastName;
                                excelWorksheet.Cells[row + 2, 3].Value = users[row].Login;
                                excelWorksheet.Cells[row + 2, 4].Value = users[row].Role.Name;
                                break;

                            case "Группы":
                                List<Group> groups = MainTable.ItemsSource as List<Group>;
                                excelWorksheet.Cells[row + 2, 1].Value = groups[row].Name;
                                break;
                            case "Предметы":
                                List<Subject> subjects = MainTable.ItemsSource as List<Subject>;
                                excelWorksheet.Cells[row + 2, 1].Value = subjects[row].Name;
                                break;
                            case "Преподаватели":
                                List<Professor> professors = MainTable.ItemsSource as List<Professor>;
                                excelWorksheet.Cells[row + 2, 1].Value = professors[row].Name;
                                excelWorksheet.Cells[row + 2, 2].Value = professors[row].LastName;
                                excelWorksheet.Cells[row + 2, 3].Value = professors[row].Subject.Name;
                                break;
                            case "Студенты":
                                List<Student> students = MainTable.ItemsSource as List<Student>;
                                excelWorksheet.Cells[row + 2, 1].Value = students[row].User.Name;
                                excelWorksheet.Cells[row + 2, 2].Value = students[row].User.LastName;
                                excelWorksheet.Cells[row + 2, 3].Value = students[row].Group.Name;
                                break;
                            case "Успеваемость":
                                List<DataGridRow> rows = MainTable.ItemsSource as List<DataGridRow>;
                                excelWorksheet.Cells[row + 2, 1].Value = rows[row].Student.FullName;
                                for(int i = 0; i < rows[row].Marks.Count(); i++)
                                    excelWorksheet.Cells[row + 2, i + 2].Value = rows[row].Marks[i].Value;
                                break;
                        }
                    }
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog() { FileName = table, DefaultExt = ".xlsx" };
                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        excelPackage.SaveAs(new FileInfo(saveFileDialog.FileName));
                        MessageBox.Show("Успешно сохранено", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch
                    {
                        MessageBox.Show("Не удалось сохранить файл.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        public DataGridTextColumn CreateColumn(string header, string binding)
        {
            var column = new DataGridTextColumn()
            {
                Binding = new Binding(binding),
                Header = header,
                IsReadOnly = true,
            };
            return column;
        }
        public string Table { get; set; }
        public TablePage(string table)
        {
            InitializeComponent();
            Table = table;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string searchText = FieldSearch.Text.ToLower().Trim();

            FieldGroup.ItemsSource = Context.Instance.Group.ToList();
            if (FieldSearch.Text == null)
                FieldSearch.Text = "";
            if (CurrentData.CurrentData.CurrentUser.Role == Context.Instance.Role.ToList()[2] ||
                CurrentData.CurrentData.CurrentUser.Role == Context.Instance.Role.ToList()[1])
            {
                ButtonAdd.IsEnabled = false;
                ButtonDelete.IsEnabled = false;                
            }

            switch (Table)
            {
                case "Преподаватели":
                    MainTable.Columns.Clear();
                    MainTable.Columns.Add(CreateColumn("Имя", "Name"));
                    MainTable.Columns.Add(CreateColumn("Фамилия", "LastName"));
                    MainTable.Columns.Add(CreateColumn("Предмет", "Subject.Name"));
                    MainTable.ItemsSource = Context.Instance.Professor.Include("Subject").ToList()
                        .Where
                            (p => p.Name.ToLower().Contains(searchText) ||
                            p.LastName.ToLower().Contains(searchText) || 
                            p.Subject.Name.ToLower().Contains(searchText));
                    break;
                case "Предметы":
                    MainTable.Columns.Clear();
                    MainTable.Columns.Add(CreateColumn("Название", "Name"));
                    MainTable.ItemsSource = Context.Instance.Subject.ToList()
                        .Where
                            (s => s.Name.ToLower().Contains(searchText));
                    break;
                case "Пользователи":
                    MainTable.Columns.Clear();
                    MainTable.Columns.Add(CreateColumn("Имя", "Name"));
                    MainTable.Columns.Add(CreateColumn("Фамилия", "LastName"));
                    MainTable.Columns.Add(CreateColumn("Логин", "Login"));
                    MainTable.Columns.Add(CreateColumn("Роль", "Role.Name"));
                    MainTable.ItemsSource = Context.Instance.User.Include("Role").ToList()
                        .Where
                        (u => u.FullName.ToLower().Contains(searchText) ||
                         u.Login.ToLower().Contains(searchText) ||
                         u.Role.Name.ToLower().Contains(searchText));
                    break;
                case "Группы":
                    MainTable.Columns.Clear();
                    MainTable.Columns.Add(CreateColumn("Название", "Name"));
                    MainTable.ItemsSource = Context.Instance.Group.ToList()
                        .Where
                            (g => g.Name.ToLower().Contains(searchText));
                    break;
                case "Студенты":
                    MainTable.Columns.Clear();
                    MainTable.Columns.Add(CreateColumn("Имя", "User.Name"));
                    MainTable.Columns.Add(CreateColumn("Фамилия", "User.LastName"));
                    MainTable.Columns.Add(CreateColumn("Группа", "Group.Name"));
                    MainTable.ItemsSource = Context.Instance.Student.Include("User").Include("Group").ToList()
                        .Where
                            (s => s.FullName.ToLower().Contains(searchText) ||
                            s.Group.Name.ToLower().Contains(searchText));
                    break;
                case "Успеваемость":
                    FieldSearch.IsEnabled = false;
                    if (CurrentData.CurrentData.CurrentUser.Role == Context.Instance.Role.ToList()[0])
                    {
                        FieldGroup.Visibility = Visibility.Visible;
                    }
                    CreatingAcademicAchievements(CurrentData.CurrentData.CurrentUser);
                    break;
            }
        }
        private void ButtonGoToMenuClick(object sender, RoutedEventArgs e)
        {
            CurrentData.CurrentData.CurrentFrame.Navigate(new MainPage());
        }

        private void ButtonAddClick(object sender, RoutedEventArgs e)
        {
            switch (Table)
            {
                case "Преподаватели":
                    Professor professor = new Professor();
                    Context.Instance.Professor.Add(professor);
                    CurrentData.CurrentData.CurrentFrame.Navigate(new EditProfessorsPage(professor));
                    break;
                case "Предметы":
                    Subject subject = new Subject();
                    Context.Instance.Subject.Add(subject);
                    CurrentData.CurrentData.CurrentFrame.Navigate(new EditSubjectPage(subject));
                    break;
                case "Пользователи":
                    User user = new User();
                    Context.Instance.User.Add(user);
                    CurrentData.CurrentData.CurrentFrame.Navigate(new EditUsersPage(user));
                    break;
                case "Группы":
                    Group group = new Group();
                    Context.Instance.Group.Add(group);
                    CurrentData.CurrentData.CurrentFrame.Navigate(new EditGroupsPage(group));
                    break;
                case "Студенты":
                    Student student = new Student();
                    Context.Instance.Student.Add(student);
                    CurrentData.CurrentData.CurrentFrame.Navigate(new EditStudentPage(student));
                    break;
            }
        }

        private void ButtonDeleteClick(object sender, RoutedEventArgs e)
        {
            if (MainTable.SelectedItem == null)
                return;
            MessageBoxResult result = MessageBox.Show("Действительно хотите удалить?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
                return;
            switch (Table)
            {
                case "Преподаватели":
                    Context.Instance.Professor.Remove(MainTable.SelectedItem as Professor);
                    Context.Instance.SaveChanges();
                    MainTable.ItemsSource = Context.Instance.Professor.ToList();
                    break;
                case "Предметы":
                    Subject subject = MainTable.SelectedItem as Subject;
                    if (subject.Professor.Count() != 0)
                    {
                        MessageBox.Show("Этот предмет ведут", "Удаление", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    Context.Instance.Subject.Remove(subject);
                    Context.Instance.SaveChanges();
                    MainTable.ItemsSource = Context.Instance.Subject.ToList();
                    break;
                case "Пользователи":
                    User user = MainTable.SelectedItem as User;
                    if (CurrentData.CurrentData.CurrentUser == MainTable.SelectedItem)
                    {
                        MessageBox.Show("Нельзя удалить самого себя", "Удаление", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    Student student = Context.Instance.Student.Where(s => s.User.ID == user.ID).FirstOrDefault();
                    if (student == null)
                    {
                        Context.Instance.User.Remove(user);
                        Context.Instance.SaveChanges();
                        return;
                    }
                    Context.Instance.Student.Remove(student);
                    Context.Instance.User.Remove(user);
                    Context.Instance.SaveChanges();
                    MainTable.ItemsSource = Context.Instance.User.ToList();
                    break;
                case "Группы":
                    if ((MainTable.SelectedItem as Group).Student.Count() != 0)
                    {
                        MessageBox.Show("В группе есть студенты", "Удаление", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    Context.Instance.Group.Remove(MainTable.SelectedItem as Group);
                    Context.Instance.SaveChanges();
                    MainTable.ItemsSource = Context.Instance.Group.ToList();
                    break;
                case "Студенты":
                    Context.Instance.Student.Remove(MainTable.SelectedItem as Student);
                    Context.Instance.SaveChanges();
                    MainTable.ItemsSource = Context.Instance.Student.ToList();
                    break;
            }
        }

        private void ButtonExcel_Click(object sender, RoutedEventArgs e)
        {
            ExportToExcel(Table);
        }

        private void MainTable_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (MainTable.SelectedItem == null)
                return;
            switch (Table)
            {
                case "Преподаватели":
                    CurrentData.CurrentData.CurrentFrame.Navigate(new EditProfessorsPage(MainTable.SelectedItem as Professor));
                    break;
                case "Предметы":
                    CurrentData.CurrentData.CurrentFrame.Navigate(new EditSubjectPage(MainTable.SelectedItem as Subject));
                    break;
                case "Пользователи":
                    CurrentData.CurrentData.CurrentFrame.Navigate(new EditUsersPage(MainTable.SelectedItem as User));
                    break;
                case "Группы":
                    CurrentData.CurrentData.CurrentFrame.Navigate(new EditGroupsPage(MainTable.SelectedItem as Group));
                    break;
                case "Студенты":
                    CurrentData.CurrentData.CurrentFrame.Navigate(new EditStudentPage(MainTable.SelectedItem as Student));
                    break;
            }
        }

        private void MainTable_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            
            string value = ((TextBox)e.EditingElement).Text;
            int index = e.Column.DisplayIndex - 1;
            Mark mark = (e.EditingElement.DataContext as DataGridRow).Marks[index];
            bool isContains = Context.Instance.Mark.Where(m => m.ID == mark.ID).Any();

            if (string.IsNullOrEmpty(value) && isContains)
            {
                Context.Instance.Mark.Remove(mark);
                Context.Instance.SaveChanges();
                return;
            }
            if (value.Length > 1)
            {
                MessageBox.Show("Максимум 1 символа!");
                ((TextBox)e.EditingElement).Text = "";
                return;
            }

            mark.Value = value;

            if (!isContains)
                Context.Instance.Mark.Add(mark);

            Context.Instance.SaveChanges();
        }

        private void FieldGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CreatingAcademicAchievements(CurrentData.CurrentData.CurrentUser);
        }

        private void FieldSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Page_Loaded(null, null);
        }
    }
}
