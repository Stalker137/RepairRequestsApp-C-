using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace RepairRequestsApp
{
    public partial class RegisterForm : Form
    {
        private string connectionString = @"Data Source=ARTEM\SQLEXPRESS;Initial Catalog=Repair;Integrated Security=True;Encrypt=False";

        public RegisterForm()
        {
            InitializeComponent();
            LoadRoles();
        }

        private void LoadRoles()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT RoleID, RoleName FROM Roles";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            cmbRoles.Items.Add(new RoleItem
                            {
                                RoleID = Convert.ToInt32(reader["RoleID"]),
                                RoleName = reader["RoleName"].ToString()
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки ролей: " + ex.Message);
                }
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (cmbRoles.SelectedItem == null)
            {
                MessageBox.Show("Выберите роль!");
                return;
            }

            string username = txtNewUsername.Text;
            string password = txtNewPassword.Text;
            int roleID = ((RoleItem)cmbRoles.SelectedItem).RoleID;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @username";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@username", username);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("Пользователь с таким логином уже существует.");
                            return;
                        }
                    }

                    string insertQuery = "INSERT INTO Users (Username, Password, RoleID) VALUES (@username, @password, @roleID)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password); // Пароль без хэширования
                        cmd.Parameters.AddWithValue("@roleID", roleID);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Регистрация успешна!");
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка регистрации: " + ex.Message);
                }
            }
        }

        private class RoleItem
        {
            public int RoleID { get; set; }
            public string RoleName { get; set; }
            public override string ToString()
            {
                return RoleName;
            }
        }
    }
}