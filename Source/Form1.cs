using System;
using System.Drawing;
using System.Windows.Forms;
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using MaterialSkin;
using MaterialSkin.Controls;

namespace qr_code
{
    public partial class Form1 : MaterialForm
    {
    [DllImport("user32", CharSet = CharSet.Auto)]
        internal extern static bool PostMessage(IntPtr hWnd, uint Msg, uint WParam, uint LParam);
        [DllImport("user32", CharSet = CharSet.Auto)]
#pragma warning disable CS0108
        internal extern static bool ReleaseCapture();
#pragma warning restore CS0108
        const uint WM_SYSCOMMAND = 0x0112;
        const uint DOMOVE = 0xF012;
        const uint DOSIZE = 0xF008;
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;
        private bool m_aeroEnabled;
        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;
        private const int WM_ACTIVATEAPP = 0x001C;
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);
        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
            );
        public struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                m_aeroEnabled = CheckAeroEnabled();
                CreateParams cp = base.CreateParams;
                if (!m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW; return cp;
            }
        }
        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0; DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCPAINT:
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        MARGINS margins = new MARGINS()
                        {
                            bottomHeight = 1,
                            leftWidth = 0,
                            rightWidth = 0,
                            topHeight = 0
                        }; DwmExtendFrameIntoClientArea(this.Handle, ref margins);
                    }
                    break;
                default: break;
            }
            base.WndProc(ref m);
        }

        // =======================================================================================================================
        // =======================================================================================================================
        // =======================================================================================================================

        Form f;
        String Out, theme;
        int form2 = 0;
        public Form1()
        {
            InitializeComponent();
            // Дефолтная тема
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            theme = Properties.Settings.Default.DarkMode;
            if ((theme == "") || (theme == " ") || (theme == "0"))
            {
                materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
                materialSkinManager.ColorScheme = new ColorScheme(Primary.Green500, Primary.Green700, Primary.Green100, Accent.Blue200, TextShade.WHITE);
            }
            if (theme == "1")
            {
                b_w.Checked = true;
                materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
                materialSkinManager.ColorScheme = new ColorScheme(Primary.Green500, Primary.Green700, Primary.Green100, Accent.Yellow200, TextShade.WHITE);
            }
        }

        private void B_w_CheckedChanged(object sender, EventArgs e)
        {
            if (b_w.Checked)
            {
                //Включение тёмной темы (галочка)
                var materialSkinManager = MaterialSkinManager.Instance;
                materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
                materialSkinManager.ColorScheme = new ColorScheme(Primary.Green500, Primary.Green700, Primary.Green100, Accent.Yellow200, TextShade.WHITE);
                Properties.Settings.Default.DarkMode = "1";
                Properties.Settings.Default.Save();
            }
            if (!b_w.Checked)
            {
                //Выключение тёмной темы (галочка) (включение дефолтной темы)
                var materialSkinManager = MaterialSkinManager.Instance;
                materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
                materialSkinManager.ColorScheme = new ColorScheme(Primary.Green500, Primary.Green700, Primary.Green100, Accent.Blue200, TextShade.WHITE);
                Properties.Settings.Default.DarkMode = "0";
                Properties.Settings.Default.Save();
            }
        }

        private void MaterialRaisedButton1_Click(object sender, EventArgs e)
        {
        foreach (Form f in Application.OpenForms)
            {
                if (f.Name == "Form2")
                {
                    form2 = 1;
                }
                else { form2 = 0; }
            }
            if (form2 == 0)
            {
                f = new Form2();
                f.Show();
            }
        }

        private void MaterialRaisedButton2_Click(object sender, EventArgs e)
        {
            string qrtext = textBox1.Text; 
            QRCodeEncoder encoder = new QRCodeEncoder();
            Bitmap qrcode = encoder.Encode(qrtext); 
            pictureBox1.Image = qrcode as Image; 
        }

        private void MaterialRaisedButton3_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog(); 
            save.FileName = "";
            save.Filter = "PNG|*.png|JPEG|*.jpg|GIF|*.gif|BMP|*.bmp";
            if (save.ShowDialog() == DialogResult.OK) 
            {
                pictureBox1.Image.Save(save.FileName); 
            }
        }

        private void MaterialRaisedButton4_Click(object sender, EventArgs e)
        {
            OpenFileDialog load = new OpenFileDialog(); 
            load.FileName = "";
            load.Filter = "PNG|*.png|JPEG|*.jpg|GIF|*.gif|BMP|*.bmp";
            if (load.ShowDialog() == DialogResult.OK) 
            {
                pictureBox1.ImageLocation = load.FileName; 
            }
        }

        private void MaterialRaisedButton5_Click(object sender, EventArgs e)
        {
            QRCodeDecoder decoder = new QRCodeDecoder();
            Out = decoder.Decode(new QRCodeBitmapImage(pictureBox1.Image as Bitmap));                                                            
            textBox2.Text = Out;
        }

        private void MaterialRaisedButton6_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(textBox2.Text);
        }

        private void materialRaisedButton7_Click(object sender, EventArgs e)
        {
            textBox1.Text = Clipboard.GetText();
        }
    }
}
