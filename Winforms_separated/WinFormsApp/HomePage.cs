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

}