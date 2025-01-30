using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WinFormsApp
{
    public class LocationMaster : Form
    {
        private DataGridView dataGridView;
        private Button addButton, editButton, deleteButton;
        private string connectionString; // Declare connectionString as a class-level variable

        public LocationMaster()
        {
            InitializeDatabase();  // Initialize the database first
            InitializeComponent();
            LoadDataFromDatabase();
        }

        private void InitializeDatabase()
        {
            // Get the base directory of the executable
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            // Define the path to the Data folder
            string dataFolderPath = Path.Combine(baseDirectory, "Data");
            // Define the path to the database file
            string databaseFilePath = Path.Combine(dataFolderPath, "user_details.MDB");

            // Check if the database file exists
            if (!File.Exists(databaseFilePath))
            {
                MessageBox.Show("Database file not found! Ensure it is placed in the 'Data' folder beside the executable.");
                throw new FileNotFoundException("Database file not found.");
            }

            // Construct and store the connection string in the class-level variable
            connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={databaseFilePath};";
        }

        private void InitializeComponent()
        {
            this.Text = "Location Master";
            this.ClientSize = new Size(450, 300);
            this.StartPosition = FormStartPosition.CenterParent;

            // DataGridView
            dataGridView = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 200,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            dataGridView.Columns.Add("Location", "Location");
            dataGridView.Columns.Add("CMS", "CMS");
            dataGridView.Columns.Add("FortunaIn", "Fortuna In");
            dataGridView.Columns.Add("FortunaOut", "Fortuna Out");

            this.Controls.Add(dataGridView);

            // Buttons
            FlowLayoutPanel buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                FlowDirection = FlowDirection.LeftToRight
            };

            addButton = new Button { Text = "Add", Size = new Size(75, 30) };
            addButton.Click += AddButton_Click;
            buttonPanel.Controls.Add(addButton);

            editButton = new Button { Text = "Edit", Size = new Size(75, 30) };
            editButton.Click += EditButton_Click;
            buttonPanel.Controls.Add(editButton);

            deleteButton = new Button { Text = "Delete", Size = new Size(75, 30) };
            deleteButton.Click += DeleteButton_Click;
            buttonPanel.Controls.Add(deleteButton);

            this.Controls.Add(buttonPanel);
        }

        private void LoadDataFromDatabase()
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM location_master";
                    OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView.Rows.Clear();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        dataGridView.Rows.Add(row["Location"], row["CMS"], row["FortunaIn"], row["FortunaOut"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            LocationEditForm editForm = new LocationEditForm(
                string.Empty,  // Location (empty for a new entry)
                string.Empty,  // CMS (empty for a new entry)
                string.Empty,  // FortunaIn (empty for a new entry)
                string.Empty,  // FortunaOut (empty for a new entry)
                dataGridView,  // Pass the DataGridView
                -1             // Indicate that this is a new entry (no row index)
            );
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                string location = editForm.LocationText;
                string cms = editForm.CMSText;
                string fortunaIn = editForm.FortunaInText;
                string fortunaOut = editForm.FortunaOutText;

                try
                {
                    using (OleDbConnection connection = new OleDbConnection(connectionString))
                    {
                        connection.Open();
                        string query = "INSERT INTO location_master (Location, CMS, FortunaIn, FortunaOut) VALUES (@Location, @CMS, @FortunaIn, @FortunaOut)";
                        using (OleDbCommand command = new OleDbCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Location", location);
                            command.Parameters.AddWithValue("@CMS", cms);
                            command.Parameters.AddWithValue("@FortunaIn", fortunaIn);
                            command.Parameters.AddWithValue("@FortunaOut", fortunaOut);
                            command.ExecuteNonQuery();
                        }
                    }

                    LoadDataFromDatabase();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding data: {ex.Message}");
                }
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView.SelectedRows[0];
                string originalLocation = selectedRow.Cells["Location"].Value.ToString();

                LocationEditForm editForm = new LocationEditForm(
                    selectedRow.Cells["Location"].Value.ToString(),
                    selectedRow.Cells["CMS"].Value.ToString(),
                    selectedRow.Cells["FortunaIn"].Value.ToString(),
                    selectedRow.Cells["FortunaOut"].Value.ToString(),
                    dataGridView,
                    dataGridView.SelectedRows[0].Index
                );

                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    string location = editForm.LocationText;
                    string cms = editForm.CMSText;
                    string fortunaIn = editForm.FortunaInText;
                    string fortunaOut = editForm.FortunaOutText;

                    try
                    {
                        using (OleDbConnection connection = new OleDbConnection(connectionString))
                        {
                            connection.Open();
                            string query = "UPDATE location_master SET Location = @Location, CMS = @CMS, FortunaIn = @FortunaIn, FortunaOut = @FortunaOut WHERE Location = @OriginalLocation";
                            using (OleDbCommand command = new OleDbCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@Location", location);
                                command.Parameters.AddWithValue("@CMS", cms);
                                command.Parameters.AddWithValue("@FortunaIn", fortunaIn);
                                command.Parameters.AddWithValue("@FortunaOut", fortunaOut);
                                command.Parameters.AddWithValue("@OriginalLocation", originalLocation);
                                command.ExecuteNonQuery();
                            }
                        }

                        LoadDataFromDatabase();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error editing data: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a row to edit.");
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView.SelectedRows[0];
                string location = selectedRow.Cells["Location"].Value.ToString();

                try
                {
                    using (OleDbConnection connection = new OleDbConnection(connectionString))
                    {
                        connection.Open();
                        string query = "DELETE FROM location_master WHERE Location = @Location";
                        using (OleDbCommand command = new OleDbCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Location", location);
                            command.ExecuteNonQuery();
                        }
                    }

                    LoadDataFromDatabase();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting data: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }
    }
}
