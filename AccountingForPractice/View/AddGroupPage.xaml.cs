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

namespace AccountingForPractice.View
{
    /// <summary>
    /// Логика взаимодействия для AddGroupPage.xaml
    /// </summary>
    public partial class AddGroupPage : Page
    {
        private DatabaseContext db = new DatabaseContext();

        public AddGroupPage()
        {
            InitializeComponent();

            // Заполним выпадающий список специальностей данными из базы данных
            LoadSpecialties();
        }

        private void LoadSpecialties()
        {
            try
            {
                // Получаем данные о специальностях из базы данных
                DataTable specialtiesTable = db.GetSpecialties();

                // Заполняем выпадающий список
                foreach (DataRow row in specialtiesTable.Rows)
                {
                    cmbSpecialties.Items.Add(row["Specialization"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке специальностей: " + ex.Message);
            }
        }

        private void btnAddGroup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string groupName = txtGroupName.Text;

                // Проверка, чтобы не добавлять группу без названия
                if (string.IsNullOrWhiteSpace(groupName))
                {
                    MessageBox.Show("Введите название группы.");
                    return;
                }

                // Получаем выбранную специальность из ComboBox
                string selectedSpecialty = cmbSpecialties.SelectedItem as string;

                // Проверка, чтобы не добавлять группу без выбранной специальности
                if (string.IsNullOrWhiteSpace(selectedSpecialty))
                {
                    MessageBox.Show("Выберите специальность для группы.");
                    return;
                }

                // Получаем Id специальности по её названию
                int specializationId = db.GetSpecializationIdByName(selectedSpecialty);

                // Вызов метода добавления группы в базу данных
                db.AddGroup(groupName, specializationId);

                // Очистить текстовые поля и сбросить выбор в ComboBox после успешного добавления
                txtGroupName.Clear();
                cmbSpecialties.SelectedIndex = -1; // Сбрасываем выбор в ComboBox
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
    }
}
