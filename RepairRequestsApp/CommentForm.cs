using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml.Linq;

namespace RepairRequestsApp
{
    public partial class CommentForm : Form
    {
        private string connectionString = @"Data Source=ARTEM\SQLEXPRESS;Initial Catalog=Repair;Integrated Security=True;Encrypt=False";
        private int requestID;
        private int userID;

        public CommentForm(int requestID, int userID)
        {
            InitializeComponent();
            this.requestID = requestID;
            this.userID = userID;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string commentText = txtComment.Text.Trim();
            if (string.IsNullOrEmpty(commentText))
            {
                MessageBox.Show("Введите комментарий!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Comments (RequestID, EmployeeID, CommentText) VALUES (@requestID, @employeeID, @commentText)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@requestID", requestID);
                        cmd.Parameters.AddWithValue("@employeeID", userID);
                        cmd.Parameters.AddWithValue("@commentText", commentText);
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Комментарий добавлен!");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка добавления комментария: " + ex.Message);
                }
            }
        }
    }
}