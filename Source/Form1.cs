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
        Form f;
        String Out, theme;
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
            f = new Form2();
            f.Show();
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
