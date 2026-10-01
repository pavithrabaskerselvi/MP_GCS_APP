using System;
using System.Drawing;
using System.Windows.Forms;

namespace MissionPlanner
{
    public class RegisterForm : Form
    {
        TextBox txtEmail, txtPass, txtConfirm;
        Button btnRegister;
        Label lblMsg;

        public string Email { get { return txtEmail.Text.Trim(); } }

        public RegisterForm()
        {
            Text = "CHENNAIDRONEACADEMY - Register";
            ClientSize = new Size(380, 470);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(14, 26, 54);
            try { Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); } catch { }

            Controls.Add(new Label
            {
                Text = "Create Account",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 15, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Bounds = new Rectangle(0, 30, 380, 40)
            });

            Controls.Add(MakeLabel("Email", 90));
            txtEmail = new TextBox { Bounds = new Rectangle(40, 112, 300, 28), Font = new Font("Segoe UI", 11) };
            Controls.Add(txtEmail);

            Controls.Add(MakeLabel("Password (min 6 characters)", 155));
            txtPass = new TextBox { Bounds = new Rectangle(40, 177, 300, 28), Font = new Font("Segoe UI", 11), UseSystemPasswordChar = true };
            Controls.Add(txtPass);

            Controls.Add(MakeLabel("Confirm password", 220));
            txtConfirm = new TextBox { Bounds = new Rectangle(40, 242, 300, 28), Font = new Font("Segoe UI", 11), UseSystemPasswordChar = true };
            Controls.Add(txtConfirm);

            lblMsg = new Label
            {
                ForeColor = Color.Salmon,
                TextAlign = ContentAlignment.MiddleCenter,
                Bounds = new Rectangle(20, 280, 340, 40)
            };
            Controls.Add(lblMsg);

            btnRegister = new Button
            {
                Text = "REGISTER",
                Bounds = new Rectangle(40, 335, 300, 42),
                BackColor = Color.FromArgb(0, 90, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Click += DoRegister;
            Controls.Add(btnRegister);
            AcceptButton = btnRegister;

            var back = new LinkLabel
            {
                Text = "Back to login",
                LinkColor = Color.FromArgb(120, 170, 255),
                TextAlign = ContentAlignment.MiddleCenter,
                Bounds = new Rectangle(40, 395, 300, 24)
            };
            back.LinkClicked += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            Controls.Add(back);
        }

        Label MakeLabel(string text, int y)
        {
            return new Label { Text = text, ForeColor = Color.White, Bounds = new Rectangle(40, y, 300, 20) };
        }

        async void DoRegister(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            if (Email == "" || txtPass.Text == "")
            {
                lblMsg.Text = "Enter email and password";
                return;
            }
            if (txtPass.Text != txtConfirm.Text)
            {
                lblMsg.Text = "Passwords do not match";
                return;
            }

            btnRegister.Enabled = false;
            btnRegister.Text = "Please wait...";
            var err = await AuthService.SignUp(Email, txtPass.Text);
            btnRegister.Enabled = true;
            btnRegister.Text = "REGISTER";

            if (err == null)
            {
                // force a normal login after registering
                AuthService.UserEmail = null;
                AuthService.IdToken = null;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                lblMsg.Text = err;
            }
        }
    }
}