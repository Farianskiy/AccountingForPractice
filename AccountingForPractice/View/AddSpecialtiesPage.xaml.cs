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
    /// Логика взаимодействия для AddSpecialtiesPage.xaml
    /// </summary>
    public partial class AddSpecialtiesPage : Page
    {
        private DatabaseContext db = new DatabaseContext();
        public AddSpecialtiesPage()
        {
            InitializeComponent();
        }

        private void btnAddSpecialty_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string specialtyName = txtSpecialtyName.Text;

                // Проверка, чтобы не добавлять пустое название специальности
                if (string.IsNullOrWhiteSpace(specialtyName))
                {
                    MessageBox.Show("Введите название специальности.");
                    return;
                }

                // Вызов метода добавления специальности в базу данных
                db.AddSpecialty(specialtyName);

                // Очистить текстовое поле после успешного добавления
                txtSpecialtyName.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

    }
}
