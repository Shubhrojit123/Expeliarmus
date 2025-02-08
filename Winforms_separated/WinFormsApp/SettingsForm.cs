using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WinFormsApp
{
    public class SettingsForm : Form
    {
        private DataGridView settingsDataGridView;
        private Button addButton, editButton, saveButton, closeButton, browseButton;
        private Panel editModePanel;
        private TextBox filePrefixTextBox, destinationPathTextBox, olderThanTextBox;
        private CheckBox autoDeleteCheckBox;
        private DateTimePicker newFileFormatPicker;
        private bool isEditMode = false;
        private OleDbConnection connection;

        public SettingsForm()
        {
            InitializeComponent();
            InitializeDatabase();
            LoadSettingsData();
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

            // Construct the connection string dynamically
            string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={databaseFilePath};";
            connection = new OleDbConnection(connectionString);
        }

        private void LoadSettingsData()
        {
            string query = "SELECT Id, Fl_Prfx, Nw_Fl_Frmt, Dstntn_Path FROM settings";

            try
            {
                connection.Open();
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // Clear existing rows in DataGridView
                settingsDataGridView.Rows.Clear();

                if (dataTable.Rows.Count > 0)
                {
                    // Load existing records into DataGridView
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string filePrefix = row["Fl_Prfx"].ToString();
                        string newFileFormat = DateTime.ParseExact(row["Nw_Fl_Frmt"].ToString(), "yyMMdd", null).ToString("yyMMdd");
                        string fileFormat = $"{filePrefix}{newFileFormat}.tas";
                        string path = row["Dstntn_Path"].ToString();

                        settingsDataGridView.Rows.Add(row["Id"], fileFormat, path);
                    }
                }
                else
                {
                    // No records found, add a default row
                    string defaultPrefix = "A";
                    string defaultDate = DateTime.Now.ToString("yyMMdd");
                    string defaultFileFormat = $"{defaultPrefix}{defaultDate}.tas";

                    settingsDataGridView.Rows.Add("1", defaultFileFormat, "D:"); // ID left blank
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading settings data: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void InitializeComponent()
        {
            this.Text = "Settings";
            this.ClientSize = new Size(450, 400);
            this.StartPosition = FormStartPosition.CenterParent;

            // DataGridView
            settingsDataGridView = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 150,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                ScrollBars = ScrollBars.Both
            };

            settingsDataGridView.Columns.Add("ID", "ID");
            settingsDataGridView.Columns.Add("FileFormat", "FileFormat");
            settingsDataGridView.Columns.Add("Path", "Path");

            this.Controls.Add(settingsDataGridView);

            // Edit Mode Panel
            editModePanel = new Panel
            {
                Dock = DockStyle.Fill,
                Visible = false
            };

            // File Prefix
            Label filePrefixLabel = new Label { Text = "File Prefix:", Dock = DockStyle.Top };
            filePrefixTextBox = new TextBox { Dock = DockStyle.Top };

            // New File Format
            Label newFileFormatLabel = new Label { Text = "New File Format:", Dock = DockStyle.Top };
            FlowLayoutPanel newFileFormatPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true
            };

            newFileFormatPicker = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Width = 150
            };

            Label tasSuffixLabel = new Label
            {
                Text = ".tas",
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft
            };

            newFileFormatPanel.Controls.Add(newFileFormatPicker);
            newFileFormatPanel.Controls.Add(tasSuffixLabel);

            // Destination Path
            Label destinationPathLabel = new Label { Text = "Destination Path:", Dock = DockStyle.Top };
            FlowLayoutPanel destinationPathPanel = new FlowLayoutPanel { Dock = DockStyle.Top, FlowDirection = FlowDirection.LeftToRight };
            destinationPathTextBox = new TextBox { Width = 300 };
            browseButton = new Button { Text = "Browse", Width = 75 };
            browseButton.Click += BrowseButton_Click;
            destinationPathPanel.Controls.Add(destinationPathTextBox);
            destinationPathPanel.Controls.Add(browseButton);

            // Auto Delete
            autoDeleteCheckBox = new CheckBox { Text = "Auto Delete", Dock = DockStyle.Top };

            // Older Than
            Label olderThanLabel = new Label { Text = "Older Than (Days):", Dock = DockStyle.Top };
            olderThanTextBox = new TextBox { Dock = DockStyle.Top };

            // Add controls to Edit Panel
            editModePanel.Controls.AddRange(new Control[] {
            olderThanTextBox, olderThanLabel,
            autoDeleteCheckBox,
            destinationPathPanel, destinationPathLabel,
            newFileFormatPanel, newFileFormatLabel,
            filePrefixTextBox, filePrefixLabel
        });

            this.Controls.Add(editModePanel);

            // Buttons Panel
            FlowLayoutPanel buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                FlowDirection = FlowDirection.LeftToRight
            };

            //// Add Button
            //addButton = new Button
            //{
            //    Text = "Add",
            //    Size = new Size(75, 30),
            //    BackColor = Color.LightBlue,
            //    ForeColor = Color.White
            //};
            //addButton.Click += AddButton_Click;
            //buttonPanel.Controls.Add(addButton);

            // Edit Button
            editButton = new Button
            {
                Text = "Edit",
                Size = new Size(75, 30),
                BackColor = Color.LightBlue,
                ForeColor = Color.White
            };
            editButton.Click += EditButton_Click;
            buttonPanel.Controls.Add(editButton);

            // Save Button
            saveButton = new Button
            {
                Text = "Save",
                Size = new Size(75, 30),
                BackColor = Color.LightBlue,
                ForeColor = Color.White,
                Enabled = false
            };
            saveButton.Click += SaveButton_Click;
            buttonPanel.Controls.Add(saveButton);

            // Close Button
            closeButton = new Button
            {
                Text = "Close",
                Size = new Size(75, 30),
                BackColor = Color.LightBlue,
                ForeColor = Color.White
            };
            closeButton.Click += CloseButton_Click;
            buttonPanel.Controls.Add(closeButton);

            this.Controls.Add(buttonPanel);
        }

        //private void AddButton_Click(object sender, EventArgs e)
        //{
        //    // Switch to Edit Mode for adding new entry
        //    settingsDataGridView.Visible = false;
        //    editModePanel.Visible = true;

        //    // Clear fields for new entry
        //    filePrefixTextBox.Clear();
        //    newFileFormatPicker.Value = DateTime.Now;
        //    destinationPathTextBox.Clear();
        //    olderThanTextBox.Clear();
        //    autoDeleteCheckBox.Checked = false;

        //    // Update buttons for Add Mode
        //    editButton.Enabled = false;
        //    saveButton.Enabled = true;
        //    closeButton.Text = "Cancel";
        //    isEditMode = false;
        //}

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (settingsDataGridView.SelectedRows.Count > 0)
            {
                // Get selected row
                DataGridViewRow selectedRow = settingsDataGridView.SelectedRows[0];

                // Extract and split the FileFormat column (e.g., A250122.tas)
                string fileFormat = selectedRow.Cells["FileFormat"].Value.ToString(); // e.g., "A250122"
                string filePrefix = fileFormat.Substring(0, 1); // Extract prefix (e.g., "A")
                string datePart = fileFormat.Substring(1, 6);   // Extract date part (e.g., "250122")

                // Parse date part to DateTimePicker
                DateTime parsedDate;
                if (DateTime.TryParseExact(datePart, "yyMMdd", null, System.Globalization.DateTimeStyles.None, out parsedDate))
                {
                    newFileFormatPicker.Value = parsedDate;
                }
                else
                {
                    MessageBox.Show("Invalid date format in the selected row.");
                    return;
                }

                // Populate other fields
                filePrefixTextBox.Text = filePrefix;
                destinationPathTextBox.Text = selectedRow.Cells["Path"].Value.ToString();
                //autoDeleteCheckBox.Checked = Convert.ToBoolean(selectedRow.Cells["Auto_Delete"].Value); // Checkbox
                //olderThanTextBox.Text = selectedRow.Cells["Older_Than_Days"].Value.ToString();      // Days value

                // Enable Edit Mode UI
                isEditMode = true;
                editModePanel.Visible = true;
                settingsDataGridView.Visible = false;
                saveButton.Enabled = true;
            }
            else
            {
                MessageBox.Show("Please select a row to edit.");
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            // Validate Inputs
            if (string.IsNullOrEmpty(filePrefixTextBox.Text) || string.IsNullOrEmpty(destinationPathTextBox.Text))
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            // Generate FileFormat by merging File Prefix and New File Format
            string fileFormat = $"{filePrefixTextBox.Text}{newFileFormatPicker.Value:yyMMdd}.tas";

            // Convert checkbox value to Yes/No
            string autoDeleteValue = autoDeleteCheckBox.Checked ? "Yes" : "No";

            // SQL Insert or Update Query
            string query = "INSERT INTO settings (Fl_Prfx, Nw_Fl_Frmt, Dstntn_Path, At_Dlt, Oldr_Thn) VALUES (?, ?, ?, ?, ?)";

            try
            {
                connection.Open();
                using (OleDbCommand cmd = new OleDbCommand(query, connection))
                {
                    // Add Parameters
                    cmd.Parameters.AddWithValue("?", filePrefixTextBox.Text);
                    cmd.Parameters.AddWithValue("?", newFileFormatPicker.Value.ToString("yyMMdd"));
                    cmd.Parameters.AddWithValue("?", destinationPathTextBox.Text);
                    cmd.Parameters.AddWithValue("?", autoDeleteValue); // Store checkbox value
                    cmd.Parameters.AddWithValue("?", olderThanTextBox.Text);  // Store days value

                    if (isEditMode)
                        cmd.Parameters.AddWithValue("?", settingsDataGridView.SelectedRows[0].Cells["ID"].Value.ToString());

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Data saved successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving data: " + ex.Message);
            }
            finally
            {
                connection.Close(); // Ensure connection is closed before loading data
            }

            // Reload data and update UI
            LoadSettingsData();
            settingsDataGridView.Visible = true;
            editModePanel.Visible = false;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            if (closeButton.Text == "Cancel")
            {
                settingsDataGridView.Visible = true;
                editModePanel.Visible = false;
            }
            else
            {
                this.Close();
            }
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            // Open folder browser dialog
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    destinationPathTextBox.Text = folderDialog.SelectedPath;
                }
            }
        }
    }

}