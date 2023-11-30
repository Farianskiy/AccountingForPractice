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

namespace AccountingForPractice.View
{
    /// <summary>
    /// Логика взаимодействия для AddStudentPage.xaml
    /// </summary>
    public partial class AddStudentPage : Page
    {
        private DatabaseContext db = new DatabaseContext();
        public AddStudentPage()
        {
            InitializeComponent();
        }

        private void GoToAnotherPage_Click(object sender, RoutedEventArgs e)
        {
            contentFrame.Navigate(new TestDBPage());
        }

        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            string surname = txtSurname.Text;
            string name = txtName.Text;
            string patronymic = txtPatronymic.Text;
            string groupName = txtGroupName.Text;

            if (!string.IsNullOrEmpty(groupName))
            {
                db.AddStudentWithDependencies(surname, name, patronymic, groupName, "Модуль");

                // Очистить текстовые поля
                txtSurname.Text = "";
                txtName.Text = "";
                txtPatronymic.Text = "";
                txtGroupName.Text = "";
            }
            else
            {
                MessageBox.Show("Введите корректное наименование группы.");
            }
        }


    }
}
