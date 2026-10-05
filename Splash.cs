using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace MissionPlanner
{
    public partial class Splash : Form
    {
        private Image _splashImage;

        public Splash()
        {
            InitializeComponent();

            TXT_version.Visible = false;
            label1.Visible = false;
            pictureBox1.Visible = false;

            _splashImage = LoadSplashImage();
            ApplySplash();

            Console.WriteLine("Splash .ctor");
        }

        private static Image LoadSplashImage()
        {
            try
            {
                var asm = Assembly.GetExecutingAssembly();
                foreach (var name in asm.GetManifestResourceNames())
                {
                    if (name.EndsWith("splashbg.png", StringComparison.OrdinalIgnoreCase))
                    {
                        using (var s = asm.GetManifestResourceStream(name))
                        using (var img = Image.FromStream(s))
                            return new Bitmap(img);
                    }
                }
            }
            catch { }

            string[] paths =
            {
                Path.Combine(Application.StartupPath, "splashbg.png"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "splashbg.png"),
                Path.Combine(Application.StartupPath, @"..\..\..\splashbg.png"),
                Path.Combine(Application.StartupPath, @"..\..\splashbg.png"),
                @"D:\MP_GCS\MP_GCS_APP\splashbg.png"
            };

            foreach (var p in paths)
            {
                try
                {
                    if (File.Exists(p))
                        using (var img = Image.FromFile(p))
                            return new Bitmap(img);
                }
                catch { }
            }

            return null;
        }

        private void ApplySplash()
        {
            if (_splashImage == null) return;
            BackgroundImage = _splashImage;
            BackgroundImageLayout = ImageLayout.Stretch;
            TXT_version.Visible = false;
            label1.Visible = false;
            pictureBox1.Visible = false;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            ApplySplash();
        }
    }
}