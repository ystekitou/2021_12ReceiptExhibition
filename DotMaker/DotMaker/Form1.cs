using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DotMaker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // ビデオキャプチャデバイスを選択するダイアログの生成
            var form = new VideoCaptureDeviceForm();
            // 選択ダイアログを開く
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // 選択されたデバイスをVideoSourcePlayerのソースに設定
                videoSourcePlayer1.VideoSource = form.VideoDevice;
                // ビデオキャプチャのスタート
                videoSourcePlayer1.Start();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 閉じるときの処理
            if (videoSourcePlayer1.VideoSource != null && videoSourcePlayer1.VideoSource.IsRunning)
            {
                videoSourcePlayer1.VideoSource.SignalToStop();
                videoSourcePlayer1.VideoSource = null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            pictureBox1.Image = null;
            // 現在のフレームをビットマップに保存
            var bmp = videoSourcePlayer1.GetCurrentVideoFrame();
            bmp.Save("a.bmp");
        }
        Bitmap pic1Img;
        private void button2_Click(object sender, EventArgs e)
        {
            // Bitmapオブジェクト生成
            Bitmap bmp;
            
            // BitmapをpictureBoxから取得
            bmp = new Bitmap("a.bmp");

            pic1Img = new Bitmap(bmp.Width, bmp.Height);
             // Bitmap処理の高速化開始
             BitmapPlus bmpP = new BitmapPlus(bmp);
            bmpP.BeginAccess();
            for (int y = 170; y < 580; y+=26)
            {
                for (int x = 930; x < 1340; x+=26)
                {

                    int r = 0, g = 0, b = 0;

                    for(int y2 = y; y2 < y+26; y2++)
                    {
                        for (int x2 = x; x2 < x + 26; x2++)
                        {
                            
                            Color col = bmpP.GetPixel(x2, y2);
                            r += col.R;
                            g += col.G;
                            b += col.B;
                            
                        }
                    }

                    r /= (26 * 26);
                    g /= (26 * 26);
                    b /= (26 * 26);
                    for (int y2 = y; y2 < (y + 26); y2++)
                    {
                        for (int x2 = x; x2 < (x + 26); x2++)
                        {
                            pic1Img.SetPixel(x2, y2, Color.FromArgb(255, r, g, b));
                        }
                    }

                    // Bitmapの色取得
                    //Color bmpCol = bmp.GetPixel(i, j);
                    //Color bmpCol = bmpP.GetPixel(x, y);
                    
                    //pic1Img.SetPixel(x, y, bmpCol);

                    //bmpP.SetPixel(x, y, Color.FromArgb(0, 0, 0, 0));
                    //pic1Img.SetPixel(x, y, Color.FromArgb(0, 0, 0, 0));
                    /*
                    if (bmpCol.R == (byte)0)
                    {
                        // Bitmapの色設定
                        //bmp.SetPixel(i, j, Color.FromArgb(255, 255, 0, 0));
                        bmpP.SetPixel(x, y, Color.FromArgb(255, 255, 0, 0));
                    }
                    else
                    {
                        // Bitmapの色設定
                        //bmp.SetPixel(i, j, Color.FromArgb(255, 0, 0, 0));
                        bmpP.SetPixel(x, y, Color.FromArgb(255, 0, 0, 0));
                    }
                    */
                }
            }
            // Bitmap処理の高速化終了
            bmpP.EndAccess();
            
            bmp.Dispose();


            pictureBox1.Image = pic1Img;
            pic1Img.Save("b.bmp");
            //bmp.Dispose();

        }


        List<System.Drawing.Drawing2D.GraphicsPath> paths = new List<System.Drawing.Drawing2D.GraphicsPath>();

        private void videoSourcePlayer1_MouseDown(object sender, MouseEventArgs e)
        {
            var point1 = e.Location;
            var point2 = new Point(0, 0);

            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddLine(point1, point2);
            paths.Add(path);

            pictureBox1.Invalidate();
        }

        private void videoSourcePlayer1_Paint(object sender, PaintEventArgs e)
        {
            foreach (var path in paths)
            {
                e.Graphics.DrawPath(Pens.Red, path);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void videoSourcePlayer1_MouseLeave(object sender, EventArgs e)
        {

        }
    }
    
}
