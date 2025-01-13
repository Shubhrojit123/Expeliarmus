using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class SignUpForm : Form
    {
        public SignUpForm()
        {
            InitializeComponent();
        }

        private void ButtonRegister_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text;
            string password = textBoxPassword.Text;

            // Input Validation
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Save to Database
            try
            {
                string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:/Winforms/user_details.MDB;";
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO login (name, pass) VALUES (@name, @pass)";
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", username);
                        command.Parameters.AddWithValue("@pass", password);
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Sign up successful! Redirecting to login page.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // Close the Sign Up form
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CheckBoxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            // Toggle the password visibility
            textBoxPassword.UseSystemPasswordChar = !checkBoxShowPassword.Checked;
        }
    }
}
