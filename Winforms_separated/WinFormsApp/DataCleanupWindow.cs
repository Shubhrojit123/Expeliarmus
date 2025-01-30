using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

namespace WinFormsApp
{
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
            this.Size = new System.Drawing.Size(450, 250);
            this.StartPosition = FormStartPosition.CenterParent;

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
}