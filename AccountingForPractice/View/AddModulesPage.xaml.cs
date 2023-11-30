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
    /// Логика взаимодействия для AddModulesPage.xaml
    /// </summary>
    public partial class AddModulesPage : Page
    {
        private DatabaseContext db = new DatabaseContext();

        public AddModulesPage()
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

        private void btnAddModule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string moduleName = txtModuleName.Text;

                // Проверка, чтобы не добавлять модуль без названия
                if (string.IsNullOrWhiteSpace(moduleName))
                {
                    MessageBox.Show("Введите название модуля.");
                    return;
                }

                // Получаем выбранную специальность из ComboBox
                string selectedSpecialty = cmbSpecialties.SelectedItem as string;

                // Проверка, чтобы не добавлять модуль без выбранной специальности
                if (string.IsNullOrWhiteSpace(selectedSpecialty))
                {
                    MessageBox.Show("Выберите специальность для модуля.");
                    return;
                }

                // Получаем Id специальности по её названию
                int specializationId = db.GetSpecializationIdByName(selectedSpecialty);

                // Вызов метода добавления модуля в базу данных
                db.AddModule(moduleName, specializationId);

                // Очистить текстовые поля и сбросить выбор в ComboBox после успешного добавления
                txtModuleName.Clear();
                cmbSpecialties.SelectedIndex = -1; // Сбрасываем выбор в ComboBox
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
    }
}
