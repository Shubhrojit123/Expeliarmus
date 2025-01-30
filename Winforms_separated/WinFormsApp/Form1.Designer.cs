namespace WinFormsApp;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    private System.Windows.Forms.Label labelTitle;
    private System.Windows.Forms.Label labelUsername;
    private System.Windows.Forms.Label labelPassword;
    private System.Windows.Forms.TextBox textBoxUsername;
    private System.Windows.Forms.TextBox textBoxPassword;
    private System.Windows.Forms.Button buttonLogin;
    private System.Windows.Forms.Button buttonSignUp;
    private System.Windows.Forms.CheckBox checkBoxShowPassword;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        this.labelTitle = new System.Windows.Forms.Label();
        this.labelUsername = new System.Windows.Forms.Label();
        this.labelPassword = new System.Windows.Forms.Label();
        this.textBoxUsername = new System.Windows.Forms.TextBox();
        this.textBoxPassword = new System.Windows.Forms.TextBox();
        this.buttonLogin = new System.Windows.Forms.Button();
        this.buttonSignUp = new System.Windows.Forms.Button();
        this.checkBoxShowPassword = new System.Windows.Forms.CheckBox(); // Add this line
        this.SuspendLayout();

        // 
        // labelTitle
        // 
        this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.labelTitle.ForeColor = System.Drawing.Color.Teal;
        this.labelTitle.Location = new System.Drawing.Point(120, 20);
        this.labelTitle.Name = "labelTitle";
        this.labelTitle.Size = new System.Drawing.Size(160, 40);
        this.labelTitle.TabIndex = 0;
        this.labelTitle.Text = "Login Page";
        this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

        // 
        // labelUsername
        // 
        this.labelUsername.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.labelUsername.ForeColor = System.Drawing.Color.DarkSlateGray;
        this.labelUsername.Location = new System.Drawing.Point(50, 80);
        this.labelUsername.Name = "labelUsername";
        this.labelUsername.Size = new System.Drawing.Size(80, 20);
        this.labelUsername.TabIndex = 1;
        this.labelUsername.Text = "Username:";

        // 
        // textBoxUsername
        // 
        this.textBoxUsername.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.textBoxUsername.Location = new System.Drawing.Point(150, 77);
        this.textBoxUsername.Name = "textBoxUsername";
        this.textBoxUsername.Size = new System.Drawing.Size(200, 25);
        this.textBoxUsername.TabIndex = 2;

        // 
        // labelPassword
        // 
        this.labelPassword.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.labelPassword.ForeColor = System.Drawing.Color.DarkSlateGray;
        this.labelPassword.Location = new System.Drawing.Point(50, 130);
        this.labelPassword.Name = "labelPassword";
        this.labelPassword.Size = new System.Drawing.Size(80, 20);
        this.labelPassword.TabIndex = 3;
        this.labelPassword.Text = "Password:";

        // 
        // textBoxPassword
        // 
        this.textBoxPassword.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.textBoxPassword.Location = new System.Drawing.Point(150, 127);
        this.textBoxPassword.Name = "textBoxPassword";
        this.textBoxPassword.Size = new System.Drawing.Size(200, 25);
        this.textBoxPassword.TabIndex = 4;
        this.textBoxPassword.UseSystemPasswordChar = true;

        // 
        // buttonLogin
        // 
        this.buttonLogin.BackColor = System.Drawing.Color.Teal;
        this.buttonLogin.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.buttonLogin.ForeColor = System.Drawing.Color.White;
        this.buttonLogin.Location = new System.Drawing.Point(150, 180);
        this.buttonLogin.Name = "buttonLogin";
        this.buttonLogin.Size = new System.Drawing.Size(100, 35);
        this.buttonLogin.TabIndex = 5;
        this.buttonLogin.Text = "Login";
        this.buttonLogin.UseVisualStyleBackColor = false;
        this.buttonLogin.Click += new System.EventHandler(this.ButtonLogin_Click);

        // buttonSignup:
        this.buttonSignUp = new System.Windows.Forms.Button();
        this.buttonSignUp.BackColor = System.Drawing.Color.LightSlateGray;
        this.buttonSignUp.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.buttonSignUp.ForeColor = System.Drawing.Color.White;
        this.buttonSignUp.Location = new System.Drawing.Point(270, 180);
        this.buttonSignUp.Name = "buttonSignUp";
        this.buttonSignUp.Size = new System.Drawing.Size(100, 35);
        this.buttonSignUp.TabIndex = 6;
        this.buttonSignUp.Text = "Sign Up";
        this.buttonSignUp.UseVisualStyleBackColor = false;
        this.buttonSignUp.Click += new System.EventHandler(this.ButtonSignUp_Click);
        this.Controls.Add(this.buttonSignUp);

        // 
        // checkBoxShowPassword
        // 
        this.checkBoxShowPassword.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.checkBoxShowPassword.Location = new System.Drawing.Point(150, 160); // Position the checkbox
        this.checkBoxShowPassword.Name = "checkBoxShowPassword";
        this.checkBoxShowPassword.Size = new System.Drawing.Size(150, 20);
        this.checkBoxShowPassword.TabIndex = 7;
        this.checkBoxShowPassword.Text = "Show Password";
        this.checkBoxShowPassword.UseVisualStyleBackColor = true;
        this.checkBoxShowPassword.CheckedChanged += new System.EventHandler(this.CheckBoxShowPassword_CheckedChanged); // Handle the checkbox change

        // Add the checkbox to the form controls
        this.Controls.Add(this.checkBoxShowPassword);

        // 
        // Form1
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.Color.AliceBlue;
        this.ClientSize = new System.Drawing.Size(400, 250);
        this.Controls.Add(this.buttonLogin);
        this.Controls.Add(this.textBoxPassword);
        this.Controls.Add(this.labelPassword);
        this.Controls.Add(this.textBoxUsername);
        this.Controls.Add(this.labelUsername);
        this.Controls.Add(this.labelTitle);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.Name = "Form1";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Login";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion
}
