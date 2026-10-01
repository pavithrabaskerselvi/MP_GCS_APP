using System;
using System.Drawing;
using System.Windows.Forms;

namespace MissionPlanner
{
    public class LoginForm : Form
    {
        TextBox txtEmail, txtPass;
        Button btnLogin;
        Label lblMsg;

        public LoginForm()
        {
            Text = "CHENNAIDRONEACADEMY - Login";
            ClientSize = new Size(380, 430);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(14, 26, 54);
            try { Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); } catch { }

            Controls.Add(new Label
            {
                Text = "CHENNAI DRONE ACADEMY",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 15, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Bounds = new Rectangle(0, 30, 380, 40)
            });
            Controls.Add(new Label
            {
                Text = "Ground Control Station - Login",
                ForeColor = Color.FromArgb(120, 170, 255),
                TextAlign = ContentAlignment.MiddleCenter,
                Bounds = new Rectangle(0, 72, 380, 24)
            });

            Controls.Add(MakeLabel("Email", 120));
            txtEmail = new TextBox { Bounds = new Rectangle(40, 142, 300, 28), Font = new Font("Segoe UI", 11) };
            Controls.Add(txtEmail);

            Controls.Add(MakeLabel("Password", 185));
            txtPass = new TextBox { Bounds = new Rectangle(40, 207, 300, 28), Font = new Font("Segoe UI", 11), UseSystemPasswordChar = true };
            Controls.Add(txtPass);

            lblMsg = new Label
            {
                ForeColor = Color.Salmon,
                TextAlign = ContentAlignment.MiddleCenter,
                Bounds = new Rectangle(20, 245, 340, 40)
            };
            Controls.Add(lblMsg);

            btnLogin = new Button
            {
                Text = "LOGIN",
                Bounds = new Rectangle(40, 295, 300, 42),
                BackColor = Color.FromArgb(0, 90, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += DoLogin;
            Controls.Add(btnLogin);
            AcceptButton = btnLogin;

            var lnk = new LinkLabel
            {
                Text = "New user? Create account",
                LinkColor = Color.FromArgb(120, 170, 255),
                TextAlign = ContentAlignment.MiddleCenter,
                Bounds = new Rectangle(40, 350, 300, 24)
            };
            lnk.LinkClicked += (s, e) => OpenRegister();
            Controls.Add(lnk);
        }

        Label MakeLabel(string text, int y)
        {
            return new Label { Text = text, ForeColor = Color.White, Bounds = new Rectangle(40, y, 300, 20) };
        }

        async void DoLogin(object sender, EventArgs e)
        {
            lblMsg.ForeColor = Color.Salmon;
            lblMsg.Text = "";
            if (txtEmail.Text.Trim() == "" || txtPass.Text == "")
            {
                lblMsg.Text = "Enter email and password";
                return;
            }

            btnLogin.Enabled = false;
            btnLogin.Text = "Please wait...";
            var err = await AuthService.SignIn(txtEmail.Text.Trim(), txtPass.Text);
            btnLogin.Enabled = true;
            btnLogin.Text = "LOGIN";

            if (err == null)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                lblMsg.Text = err;
            }
        }

        void OpenRegister()
        {
            using (var reg = new RegisterForm())
            {
                if (reg.ShowDialog(this) == DialogResult.OK)
                {
                    txtEmail.Text = reg.Email;
                    txtPass.Text = "";
                    lblMsg.ForeColor = Color.LightGreen;
                    lblMsg.Text = "Account created. Please login.";
                }
            }
        }
    }
}