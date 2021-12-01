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
            


            //videoSourcePlayer1.VideoSource = new AForge.Controls.VideoSourcePlayer();
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
            bmp.Save("a.png");
        }
        Bitmap pic1Img;
        private void button2_Click(object sender, EventArgs e)
        {
            // Bitmapオブジェクト生成
            Bitmap bmp;
            
            // BitmapをpictureBoxから取得
            bmp = new Bitmap("a.png");

            pic1Img = new Bitmap(bmp.Width, bmp.Height);

            int dotSize = 64;
            Bitmap bmpDot = new Bitmap(dotSize, dotSize);

            double margin = 0.2;
            int imageWidth = (int)(bmp.Height * (1- margin*2));
            int interval = imageWidth / dotSize;

            Rectangle rect = new Rectangle((int)(bmp.Width* margin), (int)(bmp.Width * margin), imageWidth, imageWidth);
            //Rectangle rect = new Rectangle((int)(bmp.Width * margin), (int)(bmp.Width * margin), 400, 400);
            Bitmap bmpNew = bmp.Clone(rect, bmp.PixelFormat);

            bmp.Dispose();
            // Bitmap処理の高速化開始
            BitmapPlus bmpP = new BitmapPlus(bmpNew);
            bmpP.BeginAccess();
            int dotXIndex = 0;
            int dotYIndex = 0;

            for (int y = 0; y < imageWidth; y+= interval)
            {
                Console.WriteLine(dotXIndex);
                dotXIndex = 0;
                for (int x = 0; x < imageWidth; x+= interval)
                {
                    if (dotXIndex >= dotSize || dotYIndex >= dotSize) 
                        break;
                    int r = 0, g = 0, b = 0;
                    
                    for(int y2 = y; (y2 < y + interval && y2 < imageWidth); y2++)
                    {
                        for (int x2 = x; (x2 < x + interval && x2 < imageWidth); x2++)
                        {

                            Color col = bmpP.GetPixel(x2, y2);
                            r += col.R;
                            g += col.G;
                            b += col.B;
                            break;
                            
                        }
                        break;
                    }
                    
                    /*
                    r /= (interval * interval);
                    g /= (interval * interval);
                    b /= (interval * interval);
                    */

                    r = Math.Min(255, r);
                    g = Math.Min(255, g);
                    b = Math.Min(255, b);

                    
                    r = (r < 128) ? 0 : 255;
                    g = (g < 128) ? 0 : 255;
                    b = (b < 128) ? 0 : 255;
                    

                    for (int y2 = y; y2 < (y + interval); y2++)
                    {
                        for (int x2 = x; x2 < (x + interval); x2++)
                        {
                           pic1Img.SetPixel(x2, y2, Color.FromArgb(255, r, g, b));
                        }
                    }

                    if(dotXIndex % 2 == 0 && dotYIndex % 2 == 0)
                    bmpDot.SetPixel(dotXIndex, dotYIndex, Color.FromArgb(255, r, g, b));

                    if(dotXIndex >= 64)
                    {
                        Console.WriteLine("a");
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
                    dotXIndex++;
                }
                dotYIndex++;
            }
            // Bitmap処理の高速化終了
            bmpP.EndAccess();

            bmpNew.Save("test.bmp");


            bmpDot.Save("dot.bmp");
            bmpDot.Dispose();


            pictureBox1.Image = Image.FromFile("dot.bmp");
            pic1Img.Save("b.bmp");
            pic1Img.Dispose();
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
