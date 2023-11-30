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
    /// <summary>
    /// Логика взаимодействия для EditStudentPage.xaml
    /// </summary>
    public partial class EditStudentPage : Page
    {
        private DatabaseContext db = new DatabaseContext();
        private int studentId;

        public EditStudentPage(Student selectedStudent)
        {
            InitializeComponent();

            // Заполняем поля данными выбранного студента
            txtSurname.Text = selectedStudent.SurName;
            txtName.Text = selectedStudent.Name;
            txtPatronymic.Text = selectedStudent.Patronymic;

            // Получаем информацию о группе студента из базы данных
            string groupName = GetGroupNameByStudentId(selectedStudent.Id);
            txtGroupName.Text = groupName;

            // Сохраняем Id студента для обновления в базе данных
            studentId = selectedStudent.Id;
        }

        private void EditStudent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем данные из полей
                string surname = txtSurname.Text;
                string name = txtName.Text;
                string patronymic = txtPatronymic.Text;
                string groupName = txtGroupName.Text;

                // Обновляем данные студента в базе данных
                db.UpdateStudent(studentId, surname, name, patronymic, groupName);

                // Возвращаемся на предыдущую страницу (например, страницу со списком студентов)
                contentFrame.Navigate(new TestDBPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при сохранении изменений: " + ex.Message);
            }
        }



        private string GetGroupNameByStudentId(int studentId)
        {
            // Метод для получения имени группы по Id студента
            string query = $"SELECT Groups.GroupId FROM Students JOIN Groups ON Students.GroupId = Groups.Id WHERE Students.Id = {studentId}";
            DataTable result = db.SelectData(query);
            return result.Rows.Count > 0 ? result.Rows[0]["GroupId"].ToString() : string.Empty;
        }
    }
}
