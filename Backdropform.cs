using System.Drawing;
using System.Windows.Forms;

namespace MissionPlanner
{
    // Blank full-screen dark background. Stays behind splash, login and home screens
    // so the desktop / Visual Studio code is never visible.
    public class BackdropForm : Form
    {
        public BackdropForm()
        {
            Text = "CHENNAIDRONEACADEMY";
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            Bounds = Screen.PrimaryScreen.Bounds;
            BackColor = Color.FromArgb(10, 18, 40);
            ShowInTaskbar = false;
        }
    }
}