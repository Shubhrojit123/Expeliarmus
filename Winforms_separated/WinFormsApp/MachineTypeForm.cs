using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WinFormsApp
{
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
			this.ClientSize = new Size(450, 400);
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

}