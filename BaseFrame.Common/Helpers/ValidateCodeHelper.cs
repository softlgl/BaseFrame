using System;
using System.Drawing;
using System.Linq;

namespace BaseFrame.Common.Helpers
{
    public class ValidateCodeHelper
    {
        public static string ValidateCode()
        {
            string checkCode = "";
            //生成随机生成器
            Random random = new Random();
            for (int i = 0; i < 4; i++)
            {
                var number = random.Next();
                char code;
                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else
                    code = (char)('A' + (char)(number % 26));
                checkCode += code.ToString();
            }
            return checkCode;
        }


        public static byte[] CreateCheckCodeImage(string code)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException(nameof(code));
            string[] checkCode = code.ToCharArray().Select(i=>i.ToString()).ToArray();
            //Bitmap image = new Bitmap((int)Math.Ceiling((checkCode.Length * 32.5)), 30);
            Bitmap image = new Bitmap(90,35);
            Graphics g = Graphics.FromImage(image);

            try
            {
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);

                //画图片的背景噪音线
                for (int i = 0; i < 20; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);

                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }

                //定义颜色
                Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
                //定义字体
                string[] f = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };

                for (int k = 0; k <= checkCode.Length - 1; k++)
                {
                    int cindex = random.Next(7);
                    int findex = random.Next(5);

                    Font drawFont = new Font(f[findex], 16, (FontStyle.Bold));



                    SolidBrush drawBrush = new SolidBrush(c[cindex]);

                    float x = 3.0F;
                    float y = 0.0F;
                    float width = 20.0F;
                    float height = 25.0F;
                    //int sjx = random.Next(10);
                    int sjx = 1;
                    int sjy = random.Next(image.Height - (int)height);

                    RectangleF drawRect = new RectangleF(x + sjx + (k * 20), y + sjy, width, height);

                    StringFormat drawFormat = new StringFormat();
                    drawFormat.Alignment = StringAlignment.Center;

                    g.DrawString(checkCode[k], drawFont, drawBrush, drawRect, drawFormat);
                }

                //画图片的前景噪音点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);

                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                //画图片的边框线
                g.DrawRectangle(new Pen(Color.White), 0, 0, image.Width - 1, image.Height - 1);

                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }
    }
}
