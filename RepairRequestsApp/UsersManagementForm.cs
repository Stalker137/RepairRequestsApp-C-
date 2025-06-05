using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace RepairRequestsApp
{
    public partial class UsersManagementForm : Form
    {
        private string connectionString = @"Data Source=ARTEM\SQLEXPRESS;Initial Catalog=Repair;Integrated Security=True;Encrypt=False";

        public UsersManagementForm()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            dgvUsers.Rows.Clear();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT u.UserID, u.Username, r.RoleName FROM Users u JOIN Roles r ON u.RoleID = r.RoleID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            dgvUsers.Rows.Add(
                                reader["UserID"],
                                reader["Username"],
                                reader["RoleName"]
                            );
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки пользователей: " + ex.Message);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            RegisterForm form = new RegisterForm();
            form.ShowDialog();
            LoadUsers();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите пользователя для редактирования.");
                return;
            }

            int userID = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["UserID"].Value);
            EditUserForm form = new EditUserForm(userID);
            form.ShowDialog();
            LoadUsers();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите пользователя для удаления.");
                return;
            }

            int userID = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["UserID"].Value);

            if (MessageBox.Show("Удалить пользователя?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM Users WHERE UserID = @userID";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@userID", userID);
                            cmd.ExecuteNonQuery();
                        }
                        LoadUsers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка удаления пользователя: " + ex.Message);
                    }
                }
            }
        }
    }
}