using System;
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
            this.ClientSize = new Size(800, 500);
            this.Text = "Auto Attendance Transfer";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.LightCyan;

            // Start Button
            Button startButton = new Button
            {
                Text = "Start",
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                Size = new Size(100, 40),
                Location = new Point(250, 30)
            };
            startButton.Click += async (sender, e) => await StartButton_Click(sender, e);
            this.Controls.Add(startButton);

            // Stop Button
            Button stopButton = new Button
            {
                Text = "Stop",
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                BackColor = Color.Crimson,
                ForeColor = Color.White,
                Size = new Size(100, 40),
                Location = new Point(450, 30)
            };
            stopButton.Click += StopButton_Click;
            this.Controls.Add(stopButton);

            // Flying File (PictureBox with file icon)
            flyingFile = new PictureBox
            {
                Image = Image.FromFile("D:/Winforms/WinFormsApp/OIP.jpg"), // Replace with your file icon path
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(30, 30),
                Location = new Point(180, 150), // Starting position
            };
            this.Controls.Add(flyingFile);

            // Destination Target (PictureBox with folder icon)
            destinationTarget = new PictureBox
            {
                Image = Image.FromFile("D:/Winforms/WinFormsApp/vector-folder-icon.jpg"), // Replace with your folder icon path
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(45, 45),
                Location = new Point(580, 145), // Fixed position for the target
            };
            this.Controls.Add(destinationTarget);

            // Log Report Button
            Button logReportButton = new Button
            {
                Text = "Log Report",
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                BackColor = Color.Orange,
                ForeColor = Color.White,
                Size = new Size(100, 40),
                Location = new Point(100, 200)
            };
            logReportButton.Click += LogReportButton_Click;
            this.Controls.Add(logReportButton);

            // Add the Data Cleanup button
            Button dataCleanupButton = new Button
            {
                Text = "Data Cleanup",
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                BackColor = Color.Orange,
                ForeColor = Color.White,
                Size = new Size(100, 40),
                Location = new Point(200, 200) // Adjust the location as necessary
            };
            dataCleanupButton.Click += DataCleanupButton_Click;
            this.Controls.Add(dataCleanupButton);

            // Settings Button
            Button settingsButton = new Button
            {
                Text = "Settings",
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                BackColor = Color.Orange,
                ForeColor = Color.White,
                Size = new Size(100, 40),
                Location = new Point(300, 200)
            };
            settingsButton.Click += SettingsButton_Click;
            this.Controls.Add(settingsButton);

            // Location Button
            Button locationButton = new Button
            {
                Text = "Location",
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                BackColor = Color.Orange,
                ForeColor = Color.White,
                Size = new Size(100, 40),
                Location = new Point(400, 200)
            };
            locationButton.Click += LocationButton_Click;
            this.Controls.Add(locationButton);

            // Machine Type Button
            Button machinetypeButton = new Button
            {
                Text = "Machine Type",
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                BackColor = Color.Orange,
                ForeColor = Color.White,
                Size = new Size(100, 40),
                Location = new Point(500, 200)
            };
            machinetypeButton.Click += MachineTypeButton_Click;
            this.Controls.Add(machinetypeButton);

            // Close Button
            Button closeButton = new Button
            {
                Text = "Close",
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                BackColor = Color.Orange,
                ForeColor = Color.White,
                Size = new Size(100, 40),
                Location = new Point(600, 200) // Position next to "Machine Type" button
            };
            closeButton.Click += CloseButton_Click;
            this.Controls.Add(closeButton);

            // Log TextBox
            logTextBox = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true,
                Size = new Size(700, 200),
                Location = new Point(50, 250),
                BackColor = Color.White
            };
            this.Controls.Add(logTextBox);

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

            // Enable the Stop button and keep its original color
            var stopButton = this.Controls.OfType<Button>().FirstOrDefault(button => button.Text == "Stop");
            if (stopButton != null)
            {
                stopButton.Enabled = true;
                stopButton.BackColor = Color.Crimson; // Restore original color
                stopButton.ForeColor = Color.White; // Restore original text color
            }
            logTextBox.AppendText("Transfer started...\r\n");
            await AnimateFileTransferAsync(); // Start the asynchronous animation
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
                startButton.BackColor = Color.SeaGreen; // Restore original color
                startButton.ForeColor = Color.White; // Restore original text color
            }
            logTextBox.AppendText("Transfer stopped.\r\n");
        }

        private async Task AnimateFileTransferAsync()
        {
            while (isAnimating)
            {
                t += 0.007f; // Adjust this value for the desired speed

                int x = (int)((1 - t) * (1 - t) * startPosition.X + 2 * (1 - t) * t * controlPoint.X + t * t * endPosition.X);
                int y = (int)((1 - t) * (1 - t) * startPosition.Y + 2 * (1 - t) * t * controlPoint.Y + t * t * endPosition.Y);

                flyingFile.Location = new Point(x, y);

                // Log each position
                logTextBox.AppendText($"File at: ({x}, {y})\r\n");

                if (t >= 1.0f)
                {
                    t = 0.0f; // Reset progression
                    flyingFile.Location = startPosition; // Reset to start position
                    logTextBox.AppendText("File transfer completed.\r\n");
                }

                await Task.Delay(10);
            }
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
        private TextBox codeTextBox, companyTextBox, prefixTextBox, formatTextBox, sourcePathTextBox;
        private bool isEditMode = false;
        private bool isAddMode = false;

        public MachineTypeForm()
        {
            InitializeComponent();
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

            // Add some example rows
            machineTypeDataGridView.Rows.Add("MT001", "Company A", "PRE", "DDMMYYYY", "C:/Source/Files");
            machineTypeDataGridView.Rows.Add("MT002", "Company B", "FIX", "MMDDYYYY", "D:/Source/Files");

            this.Controls.Add(machineTypeDataGridView);
            machineTypeDataGridView.ClearSelection(); // Disable default row selection

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
            formatTextBox = new TextBox { Dock = DockStyle.Top };

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
            formatTextBox, formatLabel,
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
            addButton = new Button
            {
                Text = "Add",
                Size = new Size(75, 30),
                BackColor = Color.LightGreen
            };
            addButton.Click += AddButton_Click;
            buttonPanel.Controls.Add(addButton);

            // Edit Button
            editButton = new Button
            {
                Text = "Edit",
                Size = new Size(75, 30),
                BackColor = Color.LightBlue
            };
            editButton.Click += EditButton_Click;
            buttonPanel.Controls.Add(editButton);

            // Delete Button
            deleteButton = new Button
            {
                Text = "Delete",
                Size = new Size(75, 30),
                BackColor = Color.LightCoral
            };
            deleteButton.Click += DeleteButton_Click;
            buttonPanel.Controls.Add(deleteButton);

            // Save Button
            saveButton = new Button
            {
                Text = "Save",
                Size = new Size(75, 30),
                BackColor = Color.LightSkyBlue,
                Enabled = false
            };
            saveButton.Click += SaveButton_Click;
            buttonPanel.Controls.Add(saveButton);

            // Close Button
            closeButton = new Button
            {
                Text = "Close",
                Size = new Size(75, 30),
                BackColor = Color.OrangeRed
            };
            closeButton.Click += CloseButton_Click;
            buttonPanel.Controls.Add(closeButton);

            this.Controls.Add(buttonPanel);
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
            formatTextBox.Text = selectedRow.Cells["Format"].Value.ToString();
            sourcePathTextBox.Text = selectedRow.Cells["SourcePath"].Value.ToString();

            EnterEditMode();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(codeTextBox.Text) ||
                string.IsNullOrWhiteSpace(companyTextBox.Text) ||
                string.IsNullOrWhiteSpace(prefixTextBox.Text) ||
                string.IsNullOrWhiteSpace(formatTextBox.Text) ||
                string.IsNullOrWhiteSpace(sourcePathTextBox.Text))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            if (isAddMode)
            {
                machineTypeDataGridView.Rows.Add(codeTextBox.Text, companyTextBox.Text, prefixTextBox.Text, formatTextBox.Text, sourcePathTextBox.Text);
            }
            else if (isEditMode)
            {
                DataGridViewRow selectedRow = machineTypeDataGridView.SelectedRows[0];
                selectedRow.Cells["Code"].Value = codeTextBox.Text;
                selectedRow.Cells["Company"].Value = companyTextBox.Text;
                selectedRow.Cells["Prefix"].Value = prefixTextBox.Text;
                selectedRow.Cells["Format"].Value = formatTextBox.Text;
                selectedRow.Cells["SourcePath"].Value = sourcePathTextBox.Text;
            }

            MessageBox.Show("Changes saved successfully.");
            ExitEditMode();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (machineTypeDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to delete.");
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete the selected row?", "Confirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                machineTypeDataGridView.Rows.Remove(machineTypeDataGridView.SelectedRows[0]);
            }
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

        private void EnterEditMode()
        {
            machineTypeDataGridView.Visible = false;
            editModePanel.Visible = true;
            addButton.Enabled = false;
            editButton.Enabled = false;
            deleteButton.Enabled = false;
            saveButton.Enabled = true;
            closeButton.Text = "Cancel";
        }

        private void ExitEditMode()
        {
            isEditMode = false;
            isAddMode = false;

            machineTypeDataGridView.Visible = true;
            editModePanel.Visible = false;
            addButton.Enabled = true;
            editButton.Enabled = true;
            deleteButton.Enabled = true;
            saveButton.Enabled = false;
            closeButton.Text = "Close";
        }

        private void ClearFields()
        {
            codeTextBox.Clear();
            companyTextBox.Clear();
            prefixTextBox.Clear();
            formatTextBox.Clear();
            sourcePathTextBox.Clear();
        }
    }
  
    public class SettingsForm : Form
    {
        private DataGridView settingsDataGridView;
        private Button editButton, saveButton, closeButton, browseButton;
        private Panel editModePanel;
        private TextBox filePrefixTextBox, newFileFormatTextBox, olderThanTextBox, destinationPathTextBox;
        private CheckBox autoDeleteCheckBox;
        private bool isEditMode = false;

        public SettingsForm()
        {
            InitializeComponent();
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

            // Add some example rows
            settingsDataGridView.Rows.Add("1", "CSV", "D:/Data/File.csv");
            settingsDataGridView.Rows.Add("2", "XML", "D:/Data/File.xml");

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
            newFileFormatTextBox = new TextBox { Dock = DockStyle.Top };

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
            newFileFormatTextBox, newFileFormatLabel,
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

            // Edit Button
            editButton = new Button
            {
                Text = "Edit",
                Size = new Size(75, 30),
                BackColor = Color.LightGreen
            };
            editButton.Click += EditButton_Click;
            buttonPanel.Controls.Add(editButton);

            // Save Button
            saveButton = new Button
            {
                Text = "Save",
                Size = new Size(75, 30),
                BackColor = Color.LightSkyBlue,
                Enabled = false
            };
            saveButton.Click += SaveButton_Click;
            buttonPanel.Controls.Add(saveButton);

            // Close Button
            closeButton = new Button
            {
                Text = "Close",
                Size = new Size(75, 30),
                BackColor = Color.OrangeRed
            };
            closeButton.Click += CloseButton_Click;
            buttonPanel.Controls.Add(closeButton);

            this.Controls.Add(buttonPanel);
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (settingsDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to edit.");
                return;
            }

            // Toggle visibility
            settingsDataGridView.Visible = false;
            editModePanel.Visible = true;

            // Populate fields with selected row data
            DataGridViewRow selectedRow = settingsDataGridView.SelectedRows[0];
            filePrefixTextBox.Text = selectedRow.Cells["ID"].Value.ToString();
            newFileFormatTextBox.Text = selectedRow.Cells["FileFormat"].Value.ToString();
            destinationPathTextBox.Text = selectedRow.Cells["Path"].Value.ToString();

            // Update buttons
            editButton.Enabled = false;
            saveButton.Enabled = true;
            closeButton.Text = "Cancel";
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            // Validate Inputs
            if (string.IsNullOrEmpty(filePrefixTextBox.Text) ||
                string.IsNullOrEmpty(newFileFormatTextBox.Text) ||
                string.IsNullOrEmpty(destinationPathTextBox.Text))
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            // Save Logic (e.g., update DataGridView)
            DataGridViewRow selectedRow = settingsDataGridView.SelectedRows[0];
            selectedRow.Cells["ID"].Value = filePrefixTextBox.Text;
            selectedRow.Cells["FileFormat"].Value = newFileFormatTextBox.Text;
            selectedRow.Cells["Path"].Value = destinationPathTextBox.Text;

            MessageBox.Show("Settings have been saved successfully.");

            ExitEditMode();
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    destinationPathTextBox.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            if (isEditMode)
            {
                ExitEditMode();
            }
            else
            {
                this.Close();
            }
        }

        private void ExitEditMode()
        {
            isEditMode = false;

            // Toggle visibility
            settingsDataGridView.Visible = true;
            editModePanel.Visible = false;

            // Update buttons
            editButton.Enabled = true;
            saveButton.Enabled = false;
            closeButton.Text = "Close";
        }
    }

    public class LocationMaster : Form
    {
        private DataGridView dataGridView;
        private Button addButton, editButton, deleteButton, saveButton, cancelButton;

        public LocationMaster()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Location Master";
            this.ClientSize = new Size(300, 200);
            this.StartPosition = FormStartPosition.CenterParent;

            // DataGridView
            dataGridView = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 120,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                ScrollBars = ScrollBars.Both
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

        private void AddButton_Click(object sender, EventArgs e)
        {
            LocationEditForm editForm = new LocationEditForm(dataGridView);
            editForm.ShowDialog();
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                int selectedRowIndex = dataGridView.SelectedRows[0].Index;
                LocationEditForm editForm = new LocationEditForm(dataGridView, selectedRowIndex);
                editForm.ShowDialog();
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                dataGridView.Rows.RemoveAt(dataGridView.SelectedRows[0].Index);
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

        public LocationEditForm(DataGridView gridView, int rowIndex = -1)
        {
            dataGridView = gridView;
            selectedRowIndex = rowIndex;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = selectedRowIndex == -1 ? "Add Location" : "Edit Location";
            this.ClientSize = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;

            // TableLayoutPanel for aligning labels and textboxes
            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel
            {
                ColumnCount = 2, // Two columns: one for labels, one for textboxes
                RowCount = 5,    // Five rows for labels and textboxes
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(10),
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                AutoScroll = true
            };

            // Column styles to allow labels to wrap and take as much space as needed
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30)); // First column (for labels)
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70)); // Second column (for textboxes)

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
                locationNameTextBox.Text = dataGridView.Rows[selectedRowIndex].Cells[0].Value.ToString();
                cmsTextBox.Text = dataGridView.Rows[selectedRowIndex].Cells[1].Value.ToString();
                fortunaInTextBox.Text = dataGridView.Rows[selectedRowIndex].Cells[2].Value.ToString();
                fortunaOutTextBox.Text = dataGridView.Rows[selectedRowIndex].Cells[3].Value.ToString();
                isEditMode = true;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(locationNameTextBox.Text) || string.IsNullOrWhiteSpace(cmsTextBox.Text) ||
                string.IsNullOrWhiteSpace(fortunaInTextBox.Text) || string.IsNullOrWhiteSpace(fortunaOutTextBox.Text))
            {
                MessageBox.Show("Please fill all the fields.");
                return;
            }

            if (isEditMode)
            {
                dataGridView.Rows[selectedRowIndex].Cells[0].Value = locationNameTextBox.Text;
                dataGridView.Rows[selectedRowIndex].Cells[1].Value = cmsTextBox.Text;
                dataGridView.Rows[selectedRowIndex].Cells[2].Value = fortunaInTextBox.Text;
                dataGridView.Rows[selectedRowIndex].Cells[3].Value = fortunaOutTextBox.Text;
            }
            else
            {
                dataGridView.Rows.Add(locationNameTextBox.Text, cmsTextBox.Text, fortunaInTextBox.Text, fortunaOutTextBox.Text);
            }

            this.Close();
        }
    }
}
