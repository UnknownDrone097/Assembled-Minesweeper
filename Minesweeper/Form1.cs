using System;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Minesweeper
{
    public partial class Form1 : Form
    {
        int GameSize = 15;
        double Difficulty = 0.1;
        Panel[,] Panels { get; set; }
        static int Tilesize = 25;
        string BombURL = "https://media.istockphoto.com/vectors/cartoon-bomb-illustration-vector-id842671590?k=6&m=842671590&s=612x612&w=0&h=D1A2a--svXcKUyosW2-StYy-2cUNz7c_Zf3RlcqJDK8=";
        Image Bomb { get; set; }
        string FlagURL = "https://clipartstation.com/wp-content/uploads/2018/09/flag-clipart-png-1.png";
        Image Flag { get; set; }
        Game CurrentGame { get; set; }
        Form form1;
        Form formg;
        public Form1()
        {
            InitializeComponent();
            form1 = this;
            Bomb = GetImage(BombURL, 0, 0, Tilesize, Tilesize);
            Flag = GetImage(FlagURL, 0, 0, Tilesize, Tilesize);
            SizeTxt.Text = GameSize.ToString();
            DifficultyTxt.Text = Difficulty.ToString();
        }

        private void SizeTxt_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(SizeTxt.Text, out int size)) { MessageBox.Show("Size must be an int"); return; };
            if (size < 2 || size > 100) { MessageBox.Show("Size must be between 2 and 100"); return; }
            GameSize = size;
        }

        private void DifficultyTxt_TextChanged(object sender, EventArgs e)
        {
            if (!double.TryParse(DifficultyTxt.Text, out double difficulty)) { MessageBox.Show("Difficulty must be a double"); return; };
            if (difficulty < 0 || difficulty > 1) { MessageBox.Show("Difficulty must be between 0 and 1"); return; }
            Difficulty = difficulty;
        }
        private void GoBtn_Click(object sender, EventArgs e)
        {
            //Generate new form
            Form Minesweeper = new Form();
            formg = Minesweeper;
            Minesweeper.Name = "Minesweeper";
            Minesweeper.Text = "Minesweeper";
            Minesweeper.FormClosing += Minesweeper_FormClosing;
            Minesweeper.ClientSize = new Size(Tilesize * GameSize, Tilesize * GameSize);

            form1.Hide();

            //Populate form with game
            CurrentGame = new Game(GameSize, GameSize).Initialize(Difficulty);
            Panels = new Panel[GameSize, GameSize];
            for (int i = 0; i < GameSize; i++)
            {
                for (int ii = 0; ii < GameSize; ii++)
                {
                    var newPanel = new Panel
                    {
                        Size = new Size(Tilesize, Tilesize),
                        Location = new Point(Tilesize * i, Tilesize * ii)
                    };
                    Panels[i, ii] = newPanel;
                    newPanel.Click += newPanel_Click;
                    newPanel.BackgroundImageLayout = ImageLayout.Center;
                    newPanel.BorderStyle = BorderStyle.FixedSingle;
                    newPanel.Paint += new PaintEventHandler(newPanel_Paint);
                    newPanel.BackColor = Color.LimeGreen;
                    Minesweeper.Controls.Add(newPanel);
                }
            }

            Minesweeper.Show();
        }
        private void Minesweeper_FormClosing(object sender, FormClosingEventArgs e)
        {
            form1.Show();
        }
        private void newPanel_Paint(object sender, PaintEventArgs e)
        {
            if (CurrentGame.KnownBoard[(sender as Panel).Location.X / (sender as Panel).Size.Height, (sender as Panel).Location.Y / (sender as Panel).Size.Height] == false) { return; }
            if (CurrentGame.Mines[(sender as Panel).Location.X / (sender as Panel).Size.Height, (sender as Panel).Location.Y / (sender as Panel).Size.Height] == true) { (sender as Panel).BackgroundImage = Bomb; ; return; }
            (sender as Panel).BackColor = Color.BlanchedAlmond;
            if (CurrentGame.Board[(sender as Panel).Location.X / (sender as Panel).Size.Height, (sender as Panel).Location.Y / (sender as Panel).Size.Height] == 0) { return; }
            using (Font myFont = new Font("Arial", 14))
            {
                e.Graphics.DrawString(CurrentGame.Board[(sender as Panel).Location.X / (sender as Panel).Size.Height, (sender as Panel).Location.Y / (sender as Panel).Size.Height].ToString(), myFont, Brushes.Black, new Point(2, 2));
            }
        }
        private void newPanel_Click(object sender, EventArgs e)
        {
            Panel p = sender as Panel;
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button.Equals(MouseButtons.Left)) { CurrentGame.Reveal(p.Location.X / Tilesize, p.Location.Y / Tilesize); }
            if (me.Button.Equals(MouseButtons.Right)) { CurrentGame.Flag(p.Location.X / Tilesize, p.Location.Y / Tilesize); }
            if (CurrentGame.Victory())
            {
                for (int i = 0; i < GameSize; i++)
                { for (int ii = 0; ii < GameSize; ii++) { CurrentGame.KnownBoard[i, ii] = true; CurrentGame.Flags[i, ii] = false; } }
                MessageBox.Show("A winner is you!");
            }
            PanelUpdate();
        }
        private void PanelUpdate()
        {
            for (int i = 0; i < GameSize; i++)
            {
                for (int ii = 0; ii < GameSize; ii++)
                {
                    if (CurrentGame.Flags[i, ii] == true) { Panels[i, ii].BackgroundImage = Flag; continue; }
                    if (CurrentGame.Flags[i, ii] == false) { Panels[i, ii].BackgroundImage = null; }
                    if (CurrentGame.KnownBoard[i, ii] == true)
                    {
                        newPanel_Paint(Panels[i, ii], new PaintEventArgs(Panels[i, ii].CreateGraphics(), new Rectangle()));
                    }
                }
            }
        }
        private Image GetImage(string PictureURL, int x1, int y1, int x2, int y2)
        {
            var request = WebRequest.Create(PictureURL);
            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                Bitmap b = Bitmap.FromStream(stream) as Bitmap;
                Rectangle rectangle = new Rectangle(x1, y1, x2, y2);
                System.Drawing.Imaging.PixelFormat format = b.PixelFormat;
                Bitmap b2 = b.Clone(rectangle, format);
                b2 = new Bitmap(b2, new Size(x2, y2));
                b2 = ResizeImage(b, x2, y2);
                return b2 as Image;
            };
        }
        //Taken from Github
        Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}