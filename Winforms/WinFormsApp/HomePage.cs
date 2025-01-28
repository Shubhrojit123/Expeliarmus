using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

namespace WinFormsApp
{
    public partial class HomePage : Form
    {
        private PictureBox flyingFile;
        private PictureBox destinationTarget;
        private PictureBox arodeklogo;
        private PictureBox parklogo;
        private Point startPosition;
        private Point controlPoint;
        private Point endPosition;
        private float t; // Parameter for curve progression
        private bool isAnimating; // Track if the animation is running
        private bool isPaused; // Track if the animation is paused
        private TextBox logTextBox; // For displaying logs

        public HomePage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties
            this.ClientSize = new Size(385, 500);
            this.Text = "Auto Attendance Transfer";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(64, 64, 64); // Rich black background for luxury feel


            // Get the current directory of the application
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Path to the "Images" folder relative to the application's directory
            string imagesDirectory = Path.Combine(appDirectory, "Images");

            // Arodek logo
            parklogo = new PictureBox
            {
                Image = Image.FromFile(Path.Combine(imagesDirectory, "Park-Hotel-Logo.png")),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(55, 55),
                Location = new Point(162, 105),
                BackColor = Color.Transparent // Transparent to blend with form
            };
            this.Controls.Add(parklogo);

            // Start Button
            Button startButton = new Button
            {
                Text = "Start",
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                BackColor = Color.White, // Gold for elegance
                ForeColor = Color.Black, // Black text for contrast
                Size = new Size(80, 30),
                Location = new Point(70, 20)
            };
            startButton.Click += async (sender, e) => await StartButton_Click(sender, e);
            this.Controls.Add(startButton);

            // Stop Button
            Button stopButton = new Button
            {
                Text = "Stop",
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                BackColor = Color.White, // Deep red for prominence
                ForeColor = Color.Black, // White text for clarity
                Size = new Size(80, 30),
                Location = new Point(230, 20)
            };

            stopButton.Click += StopButton_Click;
            this.Controls.Add(stopButton);

            // Flying File (PictureBox with file icon)
            flyingFile = new PictureBox
            {
                Image = Image.FromFile(Path.Combine(imagesDirectory, "OIP.jpg")),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(15, 15),
                Location = new Point(50, 140), // Starting position
                BackColor = Color.Transparent // Transparent to blend with form
            };
            this.Controls.Add(flyingFile);

            // Destination Target (PictureBox with folder icon)
            destinationTarget = new PictureBox
            {
                Image = Image.FromFile(Path.Combine(imagesDirectory, "vector-folder-icon.jpg")),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(20, 20),
                Location = new Point(305, 140),
                BackColor = Color.Transparent // Transparent to blend with form
            };
            this.Controls.Add(destinationTarget);


            // Log Report Button
            Button logReportButton = new Button
            {
                Text = "Log Report",
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                BackColor = Color.White, // Deep blue for contrast
                ForeColor = Color.Black,
                Size = new Size(90, 30),
                Location = new Point(46, 190)
            };
            logReportButton.Click += LogReportButton_Click;
            this.Controls.Add(logReportButton);

            // Data Cleanup Button
            Button dataCleanupButton = new Button
            {
                Text = "Data Cleanup",
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                BackColor = Color.White,
                ForeColor = Color.Black,
                Size = new Size(90, 30),
                Location = new Point(146, 190)
            };
            dataCleanupButton.Click += DataCleanupButton_Click;
            this.Controls.Add(dataCleanupButton);

            // Settings Button
            Button settingsButton = new Button
            {
                Text = "Settings",
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                BackColor = Color.White,
                ForeColor = Color.Black,
                Size = new Size(90, 30),
                Location = new Point(246, 190)
            };
            settingsButton.Click += SettingsButton_Click;
            this.Controls.Add(settingsButton);

            // Location Button
            Button locationButton = new Button
            {
                Text = "Location",
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                BackColor = Color.White,
                ForeColor = Color.Black,
                Size = new Size(90, 30),
                Location = new Point(46, 230)
            };
            locationButton.Click += LocationButton_Click;
            this.Controls.Add(locationButton);

            // Machine Type Button
            Button machinetypeButton = new Button
            {
                Text = "Machine Type",
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                BackColor = Color.White,
                ForeColor = Color.Black,
                Size = new Size(90, 30),
                Location = new Point(146, 230)
            };
            machinetypeButton.Click += MachineTypeButton_Click;
            this.Controls.Add(machinetypeButton);

            // Close Button
            Button closeButton = new Button
            {
                Text = "Close",
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                BackColor = Color.White,
                ForeColor = Color.Black,
                Size = new Size(90, 30),
                Location = new Point(246, 230)
            };
            closeButton.Click += CloseButton_Click;
            this.Controls.Add(closeButton);

            // Log TextBox
            logTextBox = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true,
                Size = new Size(300, 180),
                Location = new Point(42, 280),
                BackColor = Color.White,
                ForeColor = Color.Black
            };
            this.Controls.Add(logTextBox);

            // Arodek logo
            arodeklogo = new PictureBox
            {
                Image = Image.FromFile(Path.Combine(imagesDirectory, "arodek-logo.png")),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(30, 30),
                Location = new Point(342, 465),
                BackColor = Color.Transparent // Transparent to blend with form
            };
            this.Controls.Add(arodeklogo);

            // "Powered By" label
            Label poweredByLabel = new Label
            {
                Text = "\u00A9 Powered By",
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(262, 473) // Position it beside the logo
            };
            this.Controls.Add(poweredByLabel);

            // Initialize curve points
            startPosition = flyingFile.Location;
            endPosition = new Point(destinationTarget.Location.X + 30, destinationTarget.Location.Y + 15); // Center of the target
            controlPoint = new Point((startPosition.X + endPosition.X) / 2, startPosition.Y - 150); // Control point above the path

            t = 0; // Reset progression
            isAnimating = false; // Initially, the animation is not running
            isPaused = false; // Initially, the animation is not paused

            this.ResumeLayout(true);
        }

        private async Task StartButton_Click(object sender, EventArgs e)
        {
            isAnimating = true;

            // Disable the Start button and change its color to grey
            if (sender is Button startButton)
            {
                startButton.Enabled = false;
                startButton.BackColor = Color.LightGray;
                startButton.ForeColor = Color.DarkGray; // Change text color to dark grey
            }

            // Enable the Stop button and change its color to active
            var stopButton = this.Controls.OfType<Button>().FirstOrDefault(button => button.Text == "Stop");
            if (stopButton != null)
            {
                stopButton.Enabled = true;
                stopButton.BackColor = Color.White; // Active color for Stop button
                stopButton.ForeColor = Color.Black;
            }

            // Retrieve file transfer details from the database
            string fileName = string.Empty;
            string sourcePath = string.Empty;
            string destinationPath = string.Empty;
            Dictionary<string, string> locationMappings = new Dictionary<string, string>();

            try
            {
                string databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "user_details.MDB");

                // Retrieve data from the database
                using (OleDbConnection connection = new OleDbConnection($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={databasePath};"))
                {
                    connection.Open();

                    // Get file name and source path
                    using (OleDbCommand command = new OleDbCommand("SELECT Fl_Nm_Frmt, Fl_Src_Pth FROM machine_type_master", connection))
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            fileName = reader["Fl_Nm_Frmt"].ToString();
                            sourcePath = Path.Combine(reader["Fl_Src_Pth"].ToString(), fileName);
                        }
                    }

                    // Get destination path and file naming details
                    using (OleDbCommand command = new OleDbCommand("SELECT Dstntn_Path, Fl_Prfx, Nw_Fl_Frmt FROM settings", connection))
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string prefix = reader["Fl_Prfx"].ToString();
                            string newFileName = reader["Nw_Fl_Frmt"].ToString();
                            destinationPath = Path.Combine(reader["Dstntn_Path"].ToString(), $"{prefix}{newFileName}.tas");
                        }
                    }

                    // Get location mappings
                    using (OleDbCommand command = new OleDbCommand("SELECT FortunaOut, cms FROM location_master", connection))
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string fortunaOut = reader["FortunaOut"].ToString();
                            string cms = reader["cms"].ToString();
                            locationMappings[fortunaOut] = cms;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logTextBox.AppendText($"Error retrieving data from database: {ex.Message}\r\n");
                return;
            }

            logTextBox.AppendText($"Starting file conversion...\r\nSource: {sourcePath}\r\nDestination: {destinationPath}\r\n");

            var conversionTask = Task.Run(() =>
            {
                try
                {
                    using (StreamReader reader = new StreamReader(sourcePath))
                    using (StreamWriter writer = new StreamWriter(destinationPath))
                    {
                        int lineNumber = 1;

                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            // Extract components from the source line
                            string fortunaCode = line.Substring(0, 4); // AA13
                            string serialNumber = line.Substring(4, 6); // 000134
                            string dateCode = line.Substring(10, 8); // 02012025
                            string numericCode = line.Substring(18, 8); // 20015869
                            string cmsCode = line.Substring(26, 5); // CMS01

                            // Transform components
                            string newFortunaCode = locationMappings.ContainsKey(fortunaCode) ? locationMappings[fortunaCode] : fortunaCode;
                            string newCmsCode = cmsCode == "CMS01" ? "CMS81" : cmsCode == "CMS02" ? "CMS82" : cmsCode;
                            string newLine = $"{lineNumber.ToString("D6")}{newFortunaCode}{serialNumber}{dateCode}{numericCode}{newCmsCode}";

                            // Write to destination file
                            writer.WriteLine(newLine);

                            lineNumber++;
                        }
                    }

                    logTextBox.Invoke((Action)(() => logTextBox.AppendText("File conversion completed successfully.\r\n")));
                }
                catch (Exception ex)
                {
                    logTextBox.Invoke((Action)(() => logTextBox.AppendText($"Error during file conversion: {ex.Message}\r\n")));
                }
            });

            await AnimateFileTransferAsync(conversionTask); // Start the animation with the file conversion task
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            isAnimating = false;

            // Disable the Stop button and change its color to grey
            if (sender is Button stopButton)
            {
                stopButton.Enabled = false;
                stopButton.BackColor = Color.LightGray;
                stopButton.ForeColor = Color.DarkGray; // Change text color to dark grey
            }

            // Enable the Start button and keep its original color
            var startButton = this.Controls.OfType<Button>().FirstOrDefault(button => button.Text == "Start");
            if (startButton != null)
            {
                startButton.Enabled = true;
                startButton.BackColor = Color.White; // Restore original color
                startButton.ForeColor = Color.Black; // Restore original text color
            }

            logTextBox.AppendText("Transfer stopped.\r\n");
        }

        private async Task AnimateFileTransferAsync(Task fileTransferTask)
        {
            // Adjust the end position to stop just above the destination file image
            Point adjustedEndPosition = new Point(endPosition.X - 27, endPosition.Y - 12);

            do
            {
                t = 0;
                while (t < 1.0f && isAnimating)
                {
                    t += 0.007f; // Adjust this value for the desired speed

                    int x = (int)((1 - t) * (1 - t) * startPosition.X + 2 * (1 - t) * t * controlPoint.X + t * t * adjustedEndPosition.X);
                    int y = (int)((1 - t) * (1 - t) * startPosition.Y + 2 * (1 - t) * t * controlPoint.Y + t * t * adjustedEndPosition.Y);

                    flyingFile.Location = new Point(x, y);

                    await Task.Delay(10); // Smooth animation
                }
            } while (isAnimating && !fileTransferTask.IsCompleted);

            isAnimating = false;
        }

        private void LogReportButton_Click(object sender, EventArgs e)
        {
            LogReportForm logReportForm = new LogReportForm();
            logReportForm.ShowDialog();
        }

        private void DataCleanupButton_Click(object sender, EventArgs e)
        {
            // Open the new Data Cleanup window
            DataCleanupWindow dataCleanupWindow = new DataCleanupWindow();
            dataCleanupWindow.ShowDialog();  // Use ShowDialog for modal window
        }

        private void LocationButton_Click(object sender, EventArgs e)
        {
            LocationMaster locationMaster = new LocationMaster();
            locationMaster.ShowDialog();
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            SettingsForm processSetup = new SettingsForm();
            processSetup.ShowDialog();
        }

        private void MachineTypeButton_Click(object sender, EventArgs e)
        {
            MachineTypeForm machinetypemaster = new MachineTypeForm();
            machinetypemaster.ShowDialog();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close(); // Closes the current form
        }

    }

    public class MachineTypeForm : Form
    {
        private DataGridView machineTypeDataGridView;
        private Button addButton, editButton, deleteButton, saveButton, closeButton, browseButton;
        private Panel editModePanel;
        private TextBox codeTextBox, companyTextBox, prefixTextBox, sourcePathTextBox;
        private DateTimePicker datePicker;
        private bool isEditMode = false;
        private bool isAddMode = false;

        // Dynamic database connection string
        private string connectionString;

        public MachineTypeForm()
        {
            InitializeComponent();
            SetupDatabaseConnection();
            LoadDataFromDatabase();
        }

        private void SetupDatabaseConnection()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory; // Path to the executable directory
            string dataFolderPath = Path.Combine(baseDirectory, "Data"); // Path to Data folder
            string databaseFilePath = Path.Combine(dataFolderPath, "user_details.MDB"); // Path to the MDB file

            // Ensure the database file exists
            if (!File.Exists(databaseFilePath))
            {
                MessageBox.Show("Database file not found! Ensure it is placed in the 'Data' folder beside the executable.");
                throw new FileNotFoundException("Database file not found.");
            }

            // Connection string for the database
            connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={databaseFilePath};";
        }

        private void InitializeComponent()
        {
            this.Text = "Machine Type Master";
            this.ClientSize = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterParent;

            // DataGridView
            machineTypeDataGridView = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 200,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                ScrollBars = ScrollBars.Both
            };

            machineTypeDataGridView.Columns.Add("Code", "Code");
            machineTypeDataGridView.Columns.Add("Company", "Company");
            machineTypeDataGridView.Columns.Add("Prefix", "Prefix");
            machineTypeDataGridView.Columns.Add("Format", "Format");
            machineTypeDataGridView.Columns.Add("SourcePath", "SourcePath");

            this.Controls.Add(machineTypeDataGridView);

            // Edit Mode Panel
            editModePanel = new Panel
            {
                Dock = DockStyle.Fill,
                Visible = false
            };

            // Machine Type Code
            Label codeLabel = new Label { Text = "Machine Type Code:", Dock = DockStyle.Top };
            codeTextBox = new TextBox { Dock = DockStyle.Top };

            // Company Name
            Label companyLabel = new Label { Text = "Company Name:", Dock = DockStyle.Top };
            companyTextBox = new TextBox { Dock = DockStyle.Top };

            // File Name Prefix
            Label prefixLabel = new Label { Text = "File Name Prefix:", Dock = DockStyle.Top };
            prefixTextBox = new TextBox { Dock = DockStyle.Top };

            // File Name Format
            Label formatLabel = new Label { Text = "File Name Format:", Dock = DockStyle.Top };
            FlowLayoutPanel formatPanel = new FlowLayoutPanel { Dock = DockStyle.Top, FlowDirection = FlowDirection.LeftToRight };

            datePicker = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Width = 150
            };

            TextBox tasTextBox = new TextBox
            {
                Text = "TAS",
                ReadOnly = true,
                Width = 50,
                BackColor = SystemColors.Control,
                BorderStyle = BorderStyle.None,
                TextAlign = HorizontalAlignment.Center
            };

            formatPanel.Controls.Add(datePicker);
            formatPanel.Controls.Add(new Label { Text = ".", AutoSize = true });
            formatPanel.Controls.Add(tasTextBox);

            // File Source Path
            Label sourcePathLabel = new Label { Text = "Source Path:", Dock = DockStyle.Top };
            FlowLayoutPanel sourcePathPanel = new FlowLayoutPanel { Dock = DockStyle.Top, FlowDirection = FlowDirection.LeftToRight };
            sourcePathTextBox = new TextBox { Width = 400 };
            browseButton = new Button { Text = "Browse", Width = 75 };
            browseButton.Click += BrowseButton_Click;
            sourcePathPanel.Controls.Add(sourcePathTextBox);
            sourcePathPanel.Controls.Add(browseButton);

            // Add controls to Edit Panel
            editModePanel.Controls.AddRange(new Control[] {
            sourcePathPanel, sourcePathLabel,
            formatPanel, formatLabel,
            prefixTextBox, prefixLabel,
            companyTextBox, companyLabel,
            codeTextBox, codeLabel
        });

            this.Controls.Add(editModePanel);

            // Buttons Panel
            FlowLayoutPanel buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                FlowDirection = FlowDirection.LeftToRight
            };

            // Add Button
            addButton = new Button { Text = "Add", Size = new Size(75, 30) };
            addButton.Click += AddButton_Click;
            buttonPanel.Controls.Add(addButton);

            // Edit Button
            editButton = new Button { Text = "Edit", Size = new Size(75, 30) };
            editButton.Click += EditButton_Click;
            buttonPanel.Controls.Add(editButton);

            // Delete Button
            deleteButton = new Button { Text = "Delete", Size = new Size(75, 30) };
            deleteButton.Click += DeleteButton_Click;
            buttonPanel.Controls.Add(deleteButton);

            // Save Button
            saveButton = new Button { Text = "Save", Size = new Size(75, 30), Enabled = false };
            saveButton.Click += SaveButton_Click;
            buttonPanel.Controls.Add(saveButton);

            // Close Button
            closeButton = new Button { Text = "Close", Size = new Size(75, 30) };
            closeButton.Click += CloseButton_Click;
            buttonPanel.Controls.Add(closeButton);

            this.Controls.Add(buttonPanel);
        }

        private void LoadDataFromDatabase()
        {
            machineTypeDataGridView.Rows.Clear();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                string query = "SELECT * FROM machine_type_master";
                OleDbCommand command = new OleDbCommand(query, connection);

                try
                {
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        machineTypeDataGridView.Rows.Add(
                            reader["Mchn_Typ_Cd"].ToString(),
                            reader["Com_Nm"].ToString(),
                            reader["Fl_Nm_Prfx"].ToString(),
                            reader["Fl_Nm_Frmt"].ToString(),
                            reader["Fl_Src_Pth"].ToString()
                        );
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading data: {ex.Message}");
                }
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            ClearFields();
            isAddMode = true;
            EnterEditMode();
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (machineTypeDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to edit.");
                return;
            }

            isEditMode = true;
            isAddMode = false;

            DataGridViewRow selectedRow = machineTypeDataGridView.SelectedRows[0];
            codeTextBox.Text = selectedRow.Cells["Code"].Value.ToString();
            companyTextBox.Text = selectedRow.Cells["Company"].Value.ToString();
            prefixTextBox.Text = selectedRow.Cells["Prefix"].Value.ToString();
            sourcePathTextBox.Text = selectedRow.Cells["SourcePath"].Value.ToString();

            string[] formatSplit = selectedRow.Cells["Format"].Value.ToString().Split('.');
            datePicker.Value = DateTime.ParseExact(formatSplit[0], "ddMMyyyy", null);

            EnterEditMode();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(codeTextBox.Text) ||
                string.IsNullOrWhiteSpace(companyTextBox.Text) ||
                string.IsNullOrWhiteSpace(sourcePathTextBox.Text))
            {
                MessageBox.Show("Some fields are necessary.");
                return;
            }

            string formattedDate = datePicker.Value.ToString("ddMMyyyy") + ".TAS";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand command;

                if (isAddMode)
                {
                    string insertQuery = "INSERT INTO machine_type_master (Mchn_Typ_Cd, Com_Nm, Fl_Nm_Prfx, Fl_Nm_Frmt, Fl_Src_Pth) " +
                                         "VALUES (?, ?, ?, ?, ?)";
                    command = new OleDbCommand(insertQuery, connection);
                    command.Parameters.AddWithValue("?", codeTextBox.Text);
                    command.Parameters.AddWithValue("?", companyTextBox.Text);
                    command.Parameters.AddWithValue("?", prefixTextBox.Text);
                    command.Parameters.AddWithValue("?", formattedDate);
                    command.Parameters.AddWithValue("?", sourcePathTextBox.Text);
                }
                else if (isEditMode)
                {
                    string updateQuery = "UPDATE machine_type_master SET Com_Nm = ?, Fl_Nm_Prfx = ?, Fl_Nm_Frmt = ?, Fl_Src_Pth = ? " +
                                         "WHERE Mchn_Typ_Cd = ?";
                    command = new OleDbCommand(updateQuery, connection);
                    command.Parameters.AddWithValue("?", companyTextBox.Text);
                    command.Parameters.AddWithValue("?", prefixTextBox.Text);
                    command.Parameters.AddWithValue("?", formattedDate);
                    command.Parameters.AddWithValue("?", sourcePathTextBox.Text);
                    command.Parameters.AddWithValue("?", codeTextBox.Text);
                }
                else
                {
                    return;
                }

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show(isAddMode ? "Record added successfully." : "Record updated successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving data: {ex.Message}");
                }
            }

            ExitEditMode();
            LoadDataFromDatabase();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (machineTypeDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to delete.");
                return;
            }

            DataGridViewRow selectedRow = machineTypeDataGridView.SelectedRows[0];
            string codeToDelete = selectedRow.Cells["Code"].Value.ToString();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                string deleteQuery = "DELETE FROM machine_type_master WHERE Mchn_Typ_Cd = ?";
                OleDbCommand command = new OleDbCommand(deleteQuery, connection);
                command.Parameters.AddWithValue("?", codeToDelete);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Record deleted successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting data: {ex.Message}");
                }
            }

            LoadDataFromDatabase();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            if (isEditMode || isAddMode)
            {
                ExitEditMode();
            }
            else
            {
                this.Close();
            }
        }

        //private void EnterEditMode()
        //{
        //    editModePanel.Dock = DockStyle.Fill; // Make the edit panel fill the form
        //    editModePanel.Visible = true;
        //    saveButton.Enabled = true;
        //    machineTypeDataGridView.Enabled = false;
        //    addButton.Enabled = false;
        //    editButton.Enabled = false;
        //    deleteButton.Enabled = false;
        //}
        private void EnterEditMode()
        {
            editModePanel.Visible = true; // Show the edit panel
            machineTypeDataGridView.Visible = false; // Hide the DataGridView
            saveButton.Enabled = true;

            addButton.Enabled = false;
            editButton.Enabled = false;
            deleteButton.Enabled = false;
        }

        //private void ExitEditMode()
        //{
        //    editModePanel.Visible = false;
        //    saveButton.Enabled = false;
        //    machineTypeDataGridView.Enabled = true;
        //    addButton.Enabled = true;
        //    editButton.Enabled = true;
        //    deleteButton.Enabled = true;

        //    isEditMode = false;
        //    isAddMode = false;
        //    ClearFields();
        //}
        private void ExitEditMode()
        {
            editModePanel.Visible = false; // Hide the edit panel
            machineTypeDataGridView.Visible = true; // Show the DataGridView
            saveButton.Enabled = false;

            machineTypeDataGridView.Enabled = true;
            addButton.Enabled = true;
            editButton.Enabled = true;
            deleteButton.Enabled = true;

            isEditMode = false;
            isAddMode = false;
            ClearFields();
        }

        private void ClearFields()
        {
            codeTextBox.Clear();
            companyTextBox.Clear();
            prefixTextBox.Clear();
            sourcePathTextBox.Clear();
            datePicker.Value = DateTime.Today;
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    sourcePathTextBox.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }
    }

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

                // Add rows to DataGridView
                foreach (DataRow row in dataTable.Rows)
                {
                    string filePrefix = row["Fl_Prfx"].ToString();
                    string newFileFormat = DateTime.ParseExact(row["Nw_Fl_Frmt"].ToString(), "yyMMdd", null).ToString("yyMMdd");
                    string fileFormat = $"{filePrefix}{newFileFormat}.tas";
                    string path = row["Dstntn_Path"].ToString();

                    settingsDataGridView.Rows.Add(row["Id"], fileFormat, path);
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
            this.ClientSize = new Size(500, 400);
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

            // Add Button
            addButton = new Button
            {
                Text = "Add",
                Size = new Size(75, 30),
                BackColor = Color.LightBlue,
                ForeColor = Color.White
            };
            addButton.Click += AddButton_Click;
            buttonPanel.Controls.Add(addButton);

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

        private void AddButton_Click(object sender, EventArgs e)
        {
            // Switch to Edit Mode for adding new entry
            settingsDataGridView.Visible = false;
            editModePanel.Visible = true;

            // Clear fields for new entry
            filePrefixTextBox.Clear();
            newFileFormatPicker.Value = DateTime.Now;
            destinationPathTextBox.Clear();
            olderThanTextBox.Clear();
            autoDeleteCheckBox.Checked = false;

            // Update buttons for Add Mode
            editButton.Enabled = false;
            saveButton.Enabled = true;
            closeButton.Text = "Cancel";
            isEditMode = false;
        }

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
                if (DateTime.TryParseExact(datePart, "ddMMyy", null, System.Globalization.DateTimeStyles.None, out parsedDate))
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
            string query = isEditMode
                ? "UPDATE settings SET Fl_Prfx = ?, Nw_Fl_Frmt = ?, Dstntn_Path = ?, At_Dlt = ?, Oldr_Thn = ? WHERE Id = ?"
                : "INSERT INTO settings (Fl_Prfx, Nw_Fl_Frmt, Dstntn_Path, At_Dlt, Oldr_Thn) VALUES (?, ?, ?, ?, ?)";

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

    public class LocationMaster : Form
    {
        private DataGridView dataGridView;
        private Button addButton, editButton, deleteButton;
        private string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:/Winforms/user_details.MDB;";

        public LocationMaster()
        {
            InitializeComponent();
            LoadDataFromDatabase();
        }

        private void InitializeComponent()
        {
            this.Text = "Location Master";
            this.ClientSize = new Size(400, 300);
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
            //LocationEditForm editForm = new LocationEditForm(dataGridView);
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

                //LocationEditForm editForm = new LocationEditForm(
                //    selectedRow.Cells["Location"].Value.ToString(),
                //    selectedRow.Cells["CMS"].Value.ToString(),
                //    selectedRow.Cells["FortunaIn"].Value.ToString(),
                //    selectedRow.Cells["FortunaOut"].Value.ToString()
                //);
                LocationEditForm editForm = new LocationEditForm(
                    selectedRow.Cells["Location"].Value.ToString(),
                    selectedRow.Cells["CMS"].Value.ToString(),
                    selectedRow.Cells["FortunaIn"].Value.ToString(),
                    selectedRow.Cells["FortunaOut"].Value.ToString(),
                    dataGridView,  // Pass the DataGridView
                    dataGridView.SelectedRows[0].Index // Pass the selected row index
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

    public class DataCleanupWindow : Form
    {
        private DateTimePicker fromDatePicker;
        private DateTimePicker toDatePicker;
        private Button okButton;
        private Button closeButton;

        public DataCleanupWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Form settings
            this.Text = "Delete Log";
            this.Size = new System.Drawing.Size(400, 250);

            // From Date Label and Picker
            Label fromLabel = new Label();
            fromLabel.Text = "From Date:";
            fromLabel.Location = new System.Drawing.Point(10, 20);
            this.Controls.Add(fromLabel);

            fromDatePicker = new DateTimePicker();
            fromDatePicker.Format = DateTimePickerFormat.Custom;
            fromDatePicker.CustomFormat = "dd-MM-yyyy";
            fromDatePicker.Location = new System.Drawing.Point(120, 20);
            this.Controls.Add(fromDatePicker);

            // To Date Label and Picker
            Label toLabel = new Label();
            toLabel.Text = "To Date:";
            toLabel.Location = new System.Drawing.Point(10, 60);
            this.Controls.Add(toLabel);

            toDatePicker = new DateTimePicker();
            toDatePicker.Format = DateTimePickerFormat.Custom;
            toDatePicker.CustomFormat = "dd-MM-yyyy";
            toDatePicker.Location = new System.Drawing.Point(120, 60);
            this.Controls.Add(toDatePicker);

            // Ok Button
            okButton = new Button();
            okButton.Text = "Ok";
            okButton.Location = new System.Drawing.Point(40, 100);
            okButton.Click += OkButton_Click;
            this.Controls.Add(okButton);

            // Close Button
            closeButton = new Button();
            closeButton.Text = "Close";
            closeButton.Location = new System.Drawing.Point(140, 100);
            closeButton.Click += CloseButton_Click;
            this.Controls.Add(closeButton);
        }

        // Ok Button click event handler
        private void OkButton_Click(object sender, EventArgs e)
        {
            DateTime fromDate = fromDatePicker.Value;
            DateTime toDate = toDatePicker.Value;

            // Execute the cleanup process here
            MessageBox.Show($"Data cleanup started from {fromDate.ToString("dd-MM-yyyy")} to {toDate.ToString("dd-MM-yyyy")}");
            // You can call your cleanup process method here
        }

        // Close Button click event handler
        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    public class LogReportForm : Form
    {
        public LogReportForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Log Report";
            this.ClientSize = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;

            // Fields
            CheckBox allLocationsCheckBox = new CheckBox
            {
                Text = "All Location",
                Location = new Point(20, 20),
                AutoSize = true
            };
            this.Controls.Add(allLocationsCheckBox);

            CheckBox empCodeJunkCharCheckBox = new CheckBox
            {
                Text = "EmpCode With Junk Char",
                Location = new Point(20, 50),
                AutoSize = true
            };
            this.Controls.Add(empCodeJunkCharCheckBox);

            Label selectLocationLabel = new Label
            {
                Text = "Select Location:",
                Location = new Point(20, 80),
                AutoSize = true
            };
            this.Controls.Add(selectLocationLabel);

            ComboBox selectLocationDropDown = new ComboBox
            {
                Location = new Point(150, 75),
                Size = new Size(200, 25)
            };
            selectLocationDropDown.Items.AddRange(new[] { "Location 1", "Location 2", "Location 3" });
            this.Controls.Add(selectLocationDropDown);

            Label fromDateLabel = new Label
            {
                Text = "From Date:",
                Location = new Point(20, 120),
                AutoSize = true
            };
            this.Controls.Add(fromDateLabel);

            DateTimePicker fromDatePicker = new DateTimePicker
            {
                Location = new Point(150, 115),
                Size = new Size(200, 25),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd-MM-yyyy"
            };
            this.Controls.Add(fromDatePicker);

            Label toDateLabel = new Label
            {
                Text = "To Date:",
                Location = new Point(20, 160),
                AutoSize = true
            };
            this.Controls.Add(toDateLabel);

            DateTimePicker toDatePicker = new DateTimePicker
            {
                Location = new Point(150, 155),
                Size = new Size(200, 25),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd-MM-yyyy"
            };
            this.Controls.Add(toDatePicker);

            // Buttons
            Button okButton = new Button
            {
                Text = "Ok",
                Location = new Point(100, 220),
                Size = new Size(80, 30)
            };
            okButton.Click += (sender, e) => MessageBox.Show("Report Generated!");
            this.Controls.Add(okButton);

            Button cancelButton = new Button
            {
                Text = "Cancel",
                Location = new Point(200, 220),
                Size = new Size(80, 30)
            };
            cancelButton.Click += (sender, e) => this.Close();
            this.Controls.Add(cancelButton);
        }
    }

    public class LocationEditForm : Form
    {
        private Label locationNameLabel, cmsLabel, fortunaInLabel, fortunaOutLabel;
        private TextBox locationNameTextBox, cmsTextBox, fortunaInTextBox, fortunaOutTextBox;
        private Button saveButton, cancelButton;
        private bool isEditMode;
        private int selectedRowIndex;
        private DataGridView dataGridView;

        // Database connection string
        private string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:/Winforms/user_details.MDB;";

        // Public properties for accessing the text field values
        public string LocationText => locationNameTextBox.Text;
        public string CMSText => cmsTextBox.Text;
        public string FortunaInText => fortunaInTextBox.Text;
        public string FortunaOutText => fortunaOutTextBox.Text;

        public LocationEditForm(string location = "", string cms = "", string fortunaIn = "", string fortunaOut = "", DataGridView gridView = null, int rowIndex = -1)
        {
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
            this.ClientSize = new Size(400, 300);
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