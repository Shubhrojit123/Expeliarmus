using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WinFormsApp
{
    public class LocationEditForm : Form
    {
        private Label locationNameLabel, cmsLabel, fortunaInLabel, fortunaOutLabel;
        private TextBox locationNameTextBox, cmsTextBox, fortunaInTextBox, fortunaOutTextBox;
        private Button saveButton, cancelButton;
        private bool isEditMode;
        private int selectedRowIndex;
        private DataGridView dataGridView;
        private string connectionString;

        // Database connection string
        //private string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:/Winforms/user_details.MDB;";

        // Public properties for accessing the text field values
        public string LocationText => locationNameTextBox.Text;
        public string CMSText => cmsTextBox.Text;
        public string FortunaInText => fortunaInTextBox.Text;
        public string FortunaOutText => fortunaOutTextBox.Text;

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

            // Construct the connection string dynamically
            string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={databaseFilePath};";
            //connection = new OleDbConnection(connectionString);
        }

        public LocationEditForm(string location = "", string cms = "", string fortunaIn = "", string fortunaOut = "", DataGridView gridView = null, int rowIndex = -1)
        {
            InitializeDatabase();
            dataGridView = gridView;
            selectedRowIndex = rowIndex;
            InitializeComponent();

            if (rowIndex != -1) // If editing an existing row
            {
                locationNameTextBox.Text = location;
                cmsTextBox.Text = cms;
                fortunaInTextBox.Text = fortunaIn;
                fortunaOutTextBox.Text = fortunaOut;
                isEditMode = true;
            }
        }

        private void InitializeComponent()
        {
            this.Text = selectedRowIndex == -1 ? "Add Location" : "Edit Location";
            this.ClientSize = new Size(450, 300);
            this.StartPosition = FormStartPosition.CenterParent;

            // TableLayoutPanel for aligning labels and textboxes
            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 5,
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(10),
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                AutoScroll = true
            };

            // Column styles
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));

            // Labels and TextBoxes
            locationNameLabel = new Label { Text = "Location Name:" };
            locationNameTextBox = new TextBox();

            cmsLabel = new Label { Text = "CMS Code:" };
            cmsTextBox = new TextBox();

            fortunaInLabel = new Label { Text = "Fortuna In:" };
            fortunaInTextBox = new TextBox();

            fortunaOutLabel = new Label { Text = "Fortuna Out:" };
            fortunaOutTextBox = new TextBox();

            // Add controls to the TableLayoutPanel
            tableLayoutPanel.Controls.Add(locationNameLabel, 0, 0);
            tableLayoutPanel.Controls.Add(locationNameTextBox, 1, 0);

            tableLayoutPanel.Controls.Add(cmsLabel, 0, 1);
            tableLayoutPanel.Controls.Add(cmsTextBox, 1, 1);

            tableLayoutPanel.Controls.Add(fortunaInLabel, 0, 2);
            tableLayoutPanel.Controls.Add(fortunaInTextBox, 1, 2);

            tableLayoutPanel.Controls.Add(fortunaOutLabel, 0, 3);
            tableLayoutPanel.Controls.Add(fortunaOutTextBox, 1, 3);

            this.Controls.Add(tableLayoutPanel);

            // Buttons
            saveButton = new Button { Text = "Save", Location = new Point(50, 200), Width = 100 };
            cancelButton = new Button { Text = "Cancel", Location = new Point(200, 200), Width = 100 };

            saveButton.Click += SaveButton_Click;
            cancelButton.Click += (sender, e) => this.Close();

            this.Controls.Add(saveButton);
            this.Controls.Add(cancelButton);

            // Load data for editing if it's in edit mode
            if (selectedRowIndex != -1)
            {
                locationNameTextBox.Text = dataGridView.Rows[selectedRowIndex].Cells[0].Value?.ToString();
                cmsTextBox.Text = dataGridView.Rows[selectedRowIndex].Cells[1].Value?.ToString();
                fortunaInTextBox.Text = dataGridView.Rows[selectedRowIndex].Cells[2].Value?.ToString();
                fortunaOutTextBox.Text = dataGridView.Rows[selectedRowIndex].Cells[3].Value?.ToString();
                isEditMode = true;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LocationText) || string.IsNullOrWhiteSpace(CMSText) ||
                string.IsNullOrWhiteSpace(FortunaInText) || string.IsNullOrWhiteSpace(FortunaOutText))
            {
                MessageBox.Show("Please fill all the fields.");
                return;
            }

            try
            {
                using (var connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    string query;

                    if (isEditMode)
                    {
                        // Update existing record in database
                        query = "UPDATE location_master SET CMS = @CMS, FortunaIn = @FortunaIn, FortunaOut = @FortunaOut WHERE Location = @Location";
                    }
                    else
                    {
                        // Insert new record into database
                        query = "INSERT INTO location_master (Location, CMS, FortunaIn, FortunaOut) VALUES (@Location, @CMS, @FortunaIn, @FortunaOut)";
                    }

                    using (var command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Location", locationNameTextBox.Text);
                        command.Parameters.AddWithValue("@CMS", cmsTextBox.Text);
                        command.Parameters.AddWithValue("@FortunaIn", fortunaInTextBox.Text);
                        command.Parameters.AddWithValue("@FortunaOut", fortunaOutTextBox.Text);

                        command.ExecuteNonQuery();
                    }
                }

                // Update DataGridView
                if (isEditMode)
                {
                    dataGridView.Rows[selectedRowIndex].Cells[0].Value = LocationText;
                    dataGridView.Rows[selectedRowIndex].Cells[1].Value = CMSText;
                    dataGridView.Rows[selectedRowIndex].Cells[2].Value = FortunaInText;
                    dataGridView.Rows[selectedRowIndex].Cells[3].Value = FortunaOutText;
                }
                else
                {
                    dataGridView.Rows.Add(LocationText, CMSText, FortunaInText, FortunaOutText);
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
    }
}