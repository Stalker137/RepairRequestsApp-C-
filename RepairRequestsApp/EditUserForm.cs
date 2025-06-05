using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace RepairRequestsApp
{
    public partial class EditUserForm : Form
    {
        private string connectionString = @"Data Source=ARTEM\SQLEXPRESS;Initial Catalog=Repair;Integrated Security=True;Encrypt=False";
        private int userID;
        private int roleID;
        private int? clientID = null;

        public EditUserForm() : this(0, 0)
        {
            InitializeComponent();
        }

        public EditUserForm(int userID) : this(userID, 0)
        {
            InitializeComponent();
            this.userID = userID;
        }

        public EditUserForm(int userID, int roleID) : this(userID, roleID, null)
        {
            InitializeComponent();
            this.userID = userID;
            this.roleID = roleID;
        }

        public EditUserForm(int userID, int roleID, int? clientID) : base()
        {
            InitializeComponent();
            this.userID = userID;
            this.roleID = roleID;
            this.clientID = clientID;

            if (clientID.HasValue && clientID > 0)
            {
                LoadUserData();
            }
        }

        private void LoadUserData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Name, ContactInfo FROM Clients WHERE ClientID = @clientID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@clientID", clientID.Value);
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            txtName.Text = reader["Name"].ToString();
                            txtContactInfo.Text = reader["ContactInfo"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки данных клиента: " + ex.Message);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string contactInfo = txtContactInfo.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Введите название клиента!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    if (clientID.HasValue && clientID > 0)
                    {
                        string query = "UPDATE Clients SET Name = @name, ContactInfo = @contactInfo WHERE ClientID = @clientID";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@name", name);
                            cmd.Parameters.AddWithValue("@contactInfo", contactInfo);
                            cmd.Parameters.AddWithValue("@clientID", clientID.Value);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string query = "INSERT INTO Clients (Name, ContactInfo) VALUES (@name, @contactInfo)";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@name", name);
                            cmd.Parameters.AddWithValue("@contactInfo", contactInfo);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Клиент сохранён!");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка сохранения клиента: " + ex.Message);
                }
            }
        }
    }
}