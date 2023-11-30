using System;
using System.Collections.Generic;
using System.Data;
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
using static AccountingForPractice.View.TestDBPage;

namespace AccountingForPractice.View
{
    public partial class TestDBPage : Page
    {
        public class Student
        {
            public int Id { get; set; }
            public string SurName { get; set; }
            public string Name { get; set; }
            public string Patronymic { get; set; }
        }

        private DatabaseContext db = new DatabaseContext();

        public TestDBPage()
        {
            InitializeComponent();
            LoadStudents();
        }

        private void GoToAnotherPage_Click(object sender, RoutedEventArgs e)
        {
            contentFrame.Navigate(new AddStudentPage());
        }

        private void LoadStudents()
        {
            try
            {
                db.OpenConnection();

                DataTable studentsTable = db.GetStudents();

                // Проверяем, есть ли данные в таблице, прежде чем устанавливать источник данных
                if (studentsTable != null && studentsTable.Rows.Count > 0)
                {
                    List<Student> studentsList = new List<Student>();

                    foreach (DataRow row in studentsTable.Rows)
                    {
                        Student student = new Student
                        {
                            Id = Convert.ToInt32(row["ID"]),
                            SurName = row["Surname"].ToString(),
                            Name = row["Name"].ToString(),
                            Patronymic = row["Patronymic"].ToString(),
                        };

                        studentsList.Add(student);
                    }

                    dataGrid.ItemsSource = studentsList;
                }
                else
                {
                    // Очищаем источник данных, если данных нет
                    dataGrid.ItemsSource = null;
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                MessageBox.Show("Произошла ошибка при загрузке студентов: " + ex.Message);
            }
            finally
            {
                db.CloseConnection();
            }
        }

        private void EditStudent_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, выбран ли студент в DataGrid
            if (dataGrid.SelectedItem != null)
            {
                // Получаем выбранного студента
                Student selectedStudent = (Student)dataGrid.SelectedItem;

                // Создаем экземпляр страницы редактирования и передаем выбранного студента
                EditStudentPage editStudentPage = new EditStudentPage(selectedStudent);

                // Открываем страницу редактирования внутри текущего Frame
                contentFrame.Navigate(editStudentPage);
            }
            else
            {
                // Если студент не выбран, выводим сообщение об ошибке или предупреждение
                MessageBox.Show("Выберите студента для редактирования.");
            }
        }

        private void DeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверяем, выбран ли студент в DataGrid
                if (dataGrid.SelectedItem != null)
                {
                    // Получаем выбранного студента
                    Student selectedStudent = (Student)dataGrid.SelectedItem;

                    // Получаем Id выбранного студента
                    int studentId = selectedStudent.Id;

                    // Вызываем метод удаления студента из базы данных
                    db.DeleteStudent(studentId);

                    // Возвращаемся на предыдущую страницу (например, страницу со списком студентов)
                    contentFrame.Navigate(new TestDBPage());
                }
                else
                {
                    // Если студент не выбран, выводим сообщение об ошибке или предупреждение
                    MessageBox.Show("Выберите студента для удаления.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при удалении студента: " + ex.Message);
            }
        }


    }
}

