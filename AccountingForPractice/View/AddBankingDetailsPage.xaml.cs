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
    /// Логика взаимодействия для AddBankingDetailsPage.xaml
    /// </summary>
    public partial class AddBankingDetailsPage : Page
    {
        private DatabaseContext db = new DatabaseContext();

        public AddBankingDetailsPage()
        {
            InitializeComponent();
        }

        private void btnAddEnterprise_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string companyName = txtCompanyName.Text;
                string fullNameOfTheHead = txtFullNameOfTheHead.Text;
                string tin = txtTIN.Text;
                string kpp = txtKPP.Text;
                string oprn = txtOPRN.Text;
                string bic = txtBIC.Text;
                string checkingAccount = txtCheckingAccount.Text;
                string corporateAccount = txtCorporateAccount.Text;
                string bank = txtBank.Text;
                string telephone = txtTelephone.Text;
                string address = txtAddress.Text;

                // Вызов метода добавления предприятия и реквизитов в базу данных
                db.AddEnterpriseWithRequisites(companyName, fullNameOfTheHead, tin, kpp, oprn, bic, checkingAccount, corporateAccount, bank, telephone, address);

                // Очистить текстовые поля после успешного добавления
                txtCompanyName.Clear();
                txtFullNameOfTheHead.Clear();
                txtTIN.Clear();
                txtKPP.Clear();
                txtOPRN.Clear();
                txtBIC.Clear();
                txtCheckingAccount.Clear();
                txtCorporateAccount.Clear();
                txtBank.Clear();
                txtTelephone.Clear();
                txtAddress.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
    }
}
