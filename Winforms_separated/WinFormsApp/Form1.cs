// using System;
// using System.Data.OleDb;
// using System.Windows.Forms;

// namespace WinFormsApp
// {
//     public partial class Form1 : Form
//     {
//         // private Button buttonSignUp;

//         public Form1()
//         {
//             InitializeComponent();
//             // InitializeSignUpButton();
//         }

//         // private void InitializeSignUpButton()
//         // {
//         //     // Create and configure the Sign Up button
//         //     buttonSignUp = new Button
//         //     {
//         //         Text = "Sign Up",
//         //         Font = new Font("Segoe UI", 10, FontStyle.Bold),
//         //         BackColor = Color.Orange,
//         //         ForeColor = Color.White,
//         //         Size = new Size(120, 40),
//         //         Location = new Point(350, 300) // Adjust the location below the login button
//         //     };
//         //     buttonSignUp.Click += ButtonSignUp_Click; // Handle the sign-up button click
//         //     this.Controls.Add(buttonSignUp);
//         // }

//         private void ButtonLogin_Click(object sender, EventArgs e)
//         {
//             string username = textBoxUsername.Text;
//             string password = textBoxPassword.Text;

//             // Input Validation
//             if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
//             {
//                 MessageBox.Show("Please enter both username and password.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                 return;
//             }

//             // // Check login details in MDB database
//             // if (username=="admin" && password=="12345")//ValidateLogin(username, password))
//             // {
//             //     MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
//             //     OpenHomePage();
//             // }
//             // else
//             // {
//             //     MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
//             // }
//         }

//         // private void ButtonSignUp_Click(object sender, EventArgs e)
//         // {
//         //     string username = textBoxUsername.Text;
//         //     string password = textBoxPassword.Text;

//         //     // Input Validation
//         //     if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
//         //     {
//         //         MessageBox.Show("Please enter both username and password.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//         //         return;
//         //     }

//         //     // Save the sign-up details to MDB database
//         //     if (SignUpUser(username, password))
//         //     {
//         //         MessageBox.Show("Sign Up successful! Please log in.", "Sign Up", MessageBoxButtons.OK, MessageBoxIcon.Information);
//         //         ShowLogin(); // Redirect back to the login form
//         //     }
//         //     else
//         //     {
//         //         MessageBox.Show("Username already exists.", "Sign Up Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
//         //     }
//         // }

//         // private bool SignUpUser(string username, string password)
//         // {
//         //     string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Winforms\user_details.MDB;";
//         //     string query = "INSERT INTO login (Username, Password) VALUES (?, ?)";

//         //     using (OleDbConnection connection = new OleDbConnection(connectionString))
//         //     {
//         //         try
//         //         {
//         //             connection.Open();
//         //             OleDbCommand command = new OleDbCommand(query, connection);
//         //             command.Parameters.AddWithValue("?", username);
//         //             command.Parameters.AddWithValue("?", password);

//         //             int rowsAffected = command.ExecuteNonQuery();
//         //             return rowsAffected > 0; // Return true if one row was inserted
//         //         }
//         //         catch (Exception ex)
//         //         {
//         //             MessageBox.Show("Error: " + ex.Message, "Sign Up Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
//         //             return false;
//         //         }
//         //     }
//         // }

//         // private bool ValidateLogin(string username, string password)
//         // {
//         //     string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Winforms\user_details.MDB;";
//         //     string query = "SELECT COUNT(*) FROM login WHERE Username = ? AND Password = ?";

//         //     using (OleDbConnection connection = new OleDbConnection(connectionString))
//         //     {
//         //         try
//         //         {
//         //             connection.Open();
//         //             OleDbCommand command = new OleDbCommand(query, connection);
//         //             command.Parameters.AddWithValue("?", username);
//         //             command.Parameters.AddWithValue("?", password);

//         //             int count = Convert.ToInt32(command.ExecuteScalar());
//         //             return count > 0; // Return true if matching credentials are found
//         //         }
//         //         catch (Exception ex)
//         //         {
//         //             MessageBox.Show("Error: " + ex.Message, "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
//         //             return false;
//         //         }
//         //     }
//         // }

//         private void OpenHomePage()
//         {
//             // Create and show the homepage form
//             HomePage homePage = new HomePage();
//             homePage.Show();
//             this.Hide(); // Optionally hide the login form after successful login
//         }

//         private void ShowLogin()
//         {
//             // Reset fields and show login screen again
//             textBoxUsername.Clear();
//             textBoxPassword.Clear();
//             // buttonSignUp.Visible = true; // Ensure the Sign Up button is visible again
//         }
//     }
// }
using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ButtonLogin_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text;
            string password = textBoxPassword.Text;

            // Input Validation
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check login credentials
            if (CheckLogin(username, password))
            {
                MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                OpenHomePage();
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ButtonSignUp_Click(object sender, EventArgs e)
        {
            SignUpForm signUpForm = new SignUpForm();
            signUpForm.ShowDialog(); // Show the Sign Up form as a modal dialog
        }

        private bool CheckLogin(string username, string password)
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:/Winforms/user_details.MDB;";
            
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    
                    string query = "SELECT COUNT(*) FROM login WHERE name = ? AND pass = ?";
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", username);
                        command.Parameters.AddWithValue("@pass", password);

                        int count = (int)command.ExecuteScalar();
                        return count > 0; // If count > 0, login is valid
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }
        private void CheckBoxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShowPassword.Checked)
            {
                // Show password
                textBoxPassword.UseSystemPasswordChar = false;
            }
            else
            {
                // Hide password
                textBoxPassword.UseSystemPasswordChar = true;
            }
        }
        
        private void OpenHomePage()
        {
            // Create and show the homepage form
            HomePage homePage = new HomePage();
            homePage.Show();
            this.Hide(); // Optionally hide the login form after successful login
        }
    }
}

