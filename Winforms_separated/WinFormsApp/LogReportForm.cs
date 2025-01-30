using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

namespace WinFormsApp
{
    public class LogReportForm : Form
    {
        public LogReportForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Log Report";
            this.ClientSize = new Size(450, 300);
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
}