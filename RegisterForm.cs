using System;
using System.Drawing;
using System.Windows.Forms;

namespace MissionPlanner
{
    public class RegisterForm : CdaThemedForm
    {
        CdaInputBox txtEmail, txtPass, txtConfirm;
        CdaRoundButton btnRegister;
        Label lblMsg;

        public string Email { get { return txtEmail.Value.Trim(); } }

        public RegisterForm() : base(600, false)
        {
            AddHeader();
            AddTabs(false, BackToLogin);

            txtEmail = AddField("Email Address", "you@example.com", false, 196);
            txtPass = AddField("Password (min 6 characters)", "Create a password", true, 276);
            txtConfirm = AddField("Confirm Password", "Re-enter your password", true, 356);

            lblMsg = MakeLabel(Card, "", 30, 430, 340, 36, 9.5f, FontStyle.Regular,
                CdaTheme.Error, ContentAlignment.MiddleCenter);

            btnRegister = new CdaRoundButton { Text = "Register", Bounds = new Rectangle(30, 474, 340, 46) };
            btnRegister.Click += DoRegister;
            Card.Controls.Add(btnRegister);
            AcceptButton = btnRegister;

            AddFooterLink("Already have an account? Login", 25, 5, 540, BackToLogin);

            Shown += (s, e) => { Activate(); txtEmail.Inner.Focus(); };
        }

        void BackToLogin()
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        async void DoRegister(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            if (Email == "" || txtPass.Value == "")
            {
                lblMsg.Text = "Enter email and password";
                return;
            }
            if (txtPass.Value != txtConfirm.Value)
            {
                lblMsg.Text = "Passwords do not match";
                return;
            }

            btnRegister.Enabled = false;
            btnRegister.Text = "Please wait...";
            var err = await AuthService.SignUp(Email, txtPass.Value);
            btnRegister.Enabled = true;
            btnRegister.Text = "Register";

            if (err == null)
            {
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