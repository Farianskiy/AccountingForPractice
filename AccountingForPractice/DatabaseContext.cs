using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using Npgsql;

namespace AccountingForPractice
{
    public class DatabaseContext
    {
        private NpgsqlConnection connection;

        public DatabaseContext()
        {
            string connectionString = "Host=127.0.0.1;Port=5432;Database=educationalpractice;Username=postgres;Password=12345";
            connection = new NpgsqlConnection(connectionString);
        }

        public void OpenConnection()
        {
            try
            {
                connection.Open();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Ошибка подключения к базе данных: " + ex.Message);
            }
        }

        public void CloseConnection()
        {
            connection.Close();
        }

        // Другие методы для выполнения SQL-запросов и работы с базой данных.

        public DataTable SelectData(string query)
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query, connection))
                {
                    adapter.Fill(dataTable);
                }
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Ошибка выполнения SQL-запроса: " + ex.Message);
            }

            return dataTable;
        }

        public void AddStudentWithDependencies(string surname, string name, string patronymic, string groupName, string module)
        {
            try
            {
                OpenConnection();

                // Получение Id группы по её имени
                int groupId = GetGroupIdByGroupName(groupName);

                // Получение Id специализации по Id группы
                int specializationId = GetSpecializationIdByGroupId(groupId);

                // Получение Id модуля по Id специализации и его имени
                int moduleId = GetModuleIdBySpecializationAndModule(specializationId, module);

                // Вставка данных о студенте
                string studentQuery = $"INSERT INTO Students (SurName, Name, Patronymic, GroupId) VALUES ('{surname}', '{name}', '{patronymic}', {groupId})";
                ExecuteNonQuery(studentQuery);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении студента: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        private int GetGroupIdByGroupName(string groupName)
        {
            string query = $"SELECT Id FROM Groups WHERE GroupId = '{groupName}'";
            DataTable result = SelectData(query);
            return result.Rows.Count > 0 ? (int)result.Rows[0]["Id"] : -1; // Возвращаем Id группы или -1, если не найдено
        }

        private int GetSpecializationIdByGroupId(int groupId)
        {
            string query = $"SELECT Specialization FROM Groups WHERE Id = {groupId}";
            DataTable result = SelectData(query);
            return result.Rows.Count > 0 ? (int)result.Rows[0]["Specialization"] : -1; // Возвращаем Id специализации или -1, если не найдено
        }

        private int GetModuleIdBySpecializationAndModule(int specializationId, string module)
        {
            string query = $"SELECT Id FROM Modules WHERE IdSpecialties = {specializationId} AND Module = '{module}'";
            DataTable result = SelectData(query);
            return result.Rows.Count > 0 ? (int)result.Rows[0]["Id"] : -1; // Возвращаем Id модуля или -1, если не найдено
        }

        public DataTable GetStudents()
        {
            string query = "SELECT id, SurName, Name, Patronymic FROM Students";
            return SelectData(query);
        }

        private void ExecuteNonQuery(string query)
        {
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка выполнения SQL-запроса: " + ex.Message);
            }
        }

        public void UpdateStudent(int studentId, string surname, string name, string patronymic, string groupName)
        {
            try
            {
                OpenConnection();

                // Обновление данных о студенте в таблице "Students"
                string updateStudentQuery = $"UPDATE Students SET SurName = '{surname}', Name = '{name}', Patronymic = '{patronymic}' WHERE Id = {studentId}";

                using (NpgsqlCommand cmdStudent = new NpgsqlCommand(updateStudentQuery, connection))
                {
                    cmdStudent.ExecuteNonQuery();
                }

                // Получение Id группы по её имени
                int groupId = GetGroupIdByGroupName(groupName);

                if (groupId != -1)
                {
                    // Обновление группы студента в таблице "Students"
                    string updateGroupQuery = $"UPDATE Students SET groupId = '{groupId}' WHERE Id = {studentId}";

                    using (NpgsqlCommand cmdGroup = new NpgsqlCommand(updateGroupQuery, connection))
                    {
                        cmdGroup.ExecuteNonQuery();
                    }
                }
                else
                {
                    Console.WriteLine("Группа не найдена в базе данных.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при обновлении студента: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public void DeleteStudent(int studentId)
        {
            try
            {
                OpenConnection();

                // Удаляем студента из базы данных
                string deleteQuery = $"DELETE FROM Students WHERE Id = @StudentId";

                using (NpgsqlCommand cmd = new NpgsqlCommand(deleteQuery, connection))
                {
                    // Добавляем параметр
                    cmd.Parameters.AddWithValue("@StudentId", studentId);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при удалении студента: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }


        public void AddSpecialty(string specialization)
        {
            try
            {
                OpenConnection();

                // Проверка, не существует ли уже специальности с таким названием
                string checkExistenceQuery = $"SELECT COUNT(*) FROM Specialties WHERE Specialization = '{specialization}'";
                int count = Convert.ToInt32(SelectData(checkExistenceQuery).Rows[0][0]);

                if (count > 0)
                {
                    MessageBox.Show("Специальность с таким названием уже существует.");
                    return;
                }

                // Вставка новой специальности
                string addSpecialtyQuery = $"INSERT INTO Specialties (Specialization) VALUES ('{specialization}')";
                ExecuteNonQuery(addSpecialtyQuery);

                MessageBox.Show("Специальность успешно добавлена.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении специальности: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public DataTable GetSpecialties()
        {
            string query = "SELECT Id, Specialization FROM Specialties";
            return SelectData(query);
        }

        public int GetSpecializationIdByName(string specializationName)
        {
            string query = $"SELECT Id FROM Specialties WHERE Specialization = '{specializationName}'";
            DataTable result = SelectData(query);
            return result.Rows.Count > 0 ? Convert.ToInt32(result.Rows[0]["Id"]) : -1; // Возвращаем Id специальности или -1, если не найдено
        }

        public void AddGroup(string groupName, int specializationId)
        {
            try
            {
                OpenConnection();

                // Проверка, не существует ли уже группы с таким названием и Id специальности
                string checkExistenceQuery = $"SELECT COUNT(*) FROM Groups WHERE GroupId = '{groupName}' AND Specialization = {specializationId}";
                int count = Convert.ToInt32(SelectData(checkExistenceQuery).Rows[0][0]);

                if (count > 0)
                {
                    MessageBox.Show("Группа с таким названием и специальностью уже существует.");
                    return;
                }

                // Вставка новой группы
                string addGroupQuery = $"INSERT INTO Groups (GroupId, Specialization) VALUES ('{groupName}', {specializationId})";
                ExecuteNonQuery(addGroupQuery);

                MessageBox.Show("Группа успешно добавлена.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении группы: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public void AddModule(string moduleName, int specializationId)
        {
            try
            {
                OpenConnection();

                // Проверка, не существует ли уже модуля с таким названием и Id специальности
                string checkExistenceQuery = $"SELECT COUNT(*) FROM Modules WHERE Module = '{moduleName}' AND IdSpecialties = {specializationId}";
                int count = Convert.ToInt32(SelectData(checkExistenceQuery).Rows[0][0]);

                if (count > 0)
                {
                    MessageBox.Show("Модуль с таким названием и специальностью уже существует.");
                    return;
                }

                // Вставка нового модуля
                string addModuleQuery = $"INSERT INTO Modules (IdSpecialties, Module) VALUES ({specializationId}, '{moduleName}')";
                ExecuteNonQuery(addModuleQuery);

                MessageBox.Show("Модуль успешно добавлен.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении модуля: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public void AddEnterpriseWithRequisites(string companyName, string fullNameOfTheHead, string tin, string kpp, string oprn, string bic, string checkingAccount, string corporateAccount, string bank, string telephone, string address)
        {
            try
            {
                OpenConnection();

                // Вставка данных в таблицу Requisites
                string requisitesQuery = $@"
            INSERT INTO Requisites (TIN, KPP, OPRN, BIC, CheckingAccount, CorporateAccount, Bank, Telephone, Address)
            VALUES ('{tin}', '{kpp}', '{oprn}', '{bic}', '{checkingAccount}', '{corporateAccount}', '{bank}', '{telephone}', '{address}')
            RETURNING Id;
        ";

                int requisitesId = Convert.ToInt32(SelectData(requisitesQuery).Rows[0]["Id"]);

                // Вставка данных в таблицу Enterprises
                string enterprisesQuery = $@"
            INSERT INTO Enterprises (CompanyName, FullNameOfTheHead, Requisites)
            VALUES ('{companyName}', '{fullNameOfTheHead}', {requisitesId});
        ";

                ExecuteNonQuery(enterprisesQuery);

                MessageBox.Show("Предприятие и реквизиты успешно добавлены.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении предприятия и реквизитов: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

    }
}

