using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Lind.DDD.Utils.ImageHelper
{
    public class ImageHelper
    {
        /// <summary>
        /// 在图片上输出文字
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="words"></param>
        /// <returns></returns>
        public static byte[] GetImage(string fileName, string words)
        {
            int fontSize = 12;
            Bitmap image = (Bitmap)Bitmap.FromFile(fileName);
            Graphics g = Graphics.FromImage(image);
            Font font = new Font("黑体", fontSize, FontStyle.Underline, GraphicsUnit.Pixel);
            g.DrawString(words, font, Brushes.YellowGreen, image.Width - fontSize / 2 * words.Length, image.Height - fontSize);
            MemoryStream mem = new MemoryStream();
            image.Save(mem, ImageFormat.Jpeg);
            g.Dispose();
            image.Dispose();
            return mem.ToArray();
        }

        /// <summary>
        /// 把文字加到图像流中
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="words"></param>
        /// <returns></returns>
        public static byte[] WordToImageStream(Stream stream, string words)
        {
            System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
            Bitmap bmap = new Bitmap(image, image.Width, image.Height);
            Graphics g = Graphics.FromImage(bmap);

            //定义笔迹的样式
            SolidBrush drawBrush = new SolidBrush(Color.Red);
            Font drawFont = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);
            int xPos = bmap.Height - (bmap.Height - 25);
            int yPos = 3;

            //往图片上添加的内容
            g.DrawString(words, drawFont, drawBrush, xPos, yPos);

            //画边框
            //Brush brush = new SolidBrush(Color.Black);
            //Pen pen = new Pen(brush, 1);
            //g.DrawRectangle(pen, new Rectangle(0, 0, Math.Abs(image.Width), Math.Abs(image.Height)));

            //保存修改好的图片
            MemoryStream returnImg = new MemoryStream();
            bmap.Save(returnImg, ImageFormat.Jpeg);
            return returnImg.ToArray();
        }
        /// <summary>
        /// 获取文字图片
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="words"></param>
        /// <returns></returns>
        public static byte[] GetImageWord(string fileName, Char c)
        {
            int fontSize = 12;
            Bitmap image = new Bitmap(fontSize, fontSize, PixelFormat.Format32bppArgb);
            image.MakeTransparent(Color.White);
            Graphics g = Graphics.FromImage(image);
            Font font = new Font("黑体", fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            g.DrawString(c.ToString(), font, Brushes.White, 0f, 0f);
            MemoryStream mem = new MemoryStream();
            image.Save(mem, ImageFormat.Jpeg);
            g.Dispose();
            image.Dispose();
            return mem.ToArray();
        }


        /// <summary> 
        /// 将 Stream 转成 byte[] 
        /// </summary> 
        public static byte[] StreamToBytes(Stream stream)
        {
            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);


            return bytes;
        }

        /// <summary> 
        /// 将 byte[] 转成 Stream 
        /// </summary> 
        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }




    }
}
