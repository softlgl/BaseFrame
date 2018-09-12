using System;
using System.Drawing;
using BaseFrame.Common.Extension;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;

namespace BaseFrame.Common.Helpers
{
    public class QRCodeHelper
    {

        //生成二维码方法一
        public static string CreateCodeSimple(string nr, int QRCodeScale, string filepath, string logoPath)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeScale = QRCodeScale;
            qrCodeEncoder.QRCodeVersion = 8;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            //System.Drawing.Image image = qrCodeEncoder.Encode("4408810820 深圳－广州 小江");
            System.Drawing.Image image = qrCodeEncoder.Encode(nr);
            string filename = $"{Guid.NewGuid()}.jpg";
            filepath = filepath + filename;
            BuildWatermark(image, logoPath, filepath);

            image.Dispose();
            //二维码解码
            //var codeDecoder = CodeDecoder(filepath);
            return filename;
        }

        /// <summary>
        /// 二维码解码
        /// </summary>
        /// <param name="filePath">图片路径</param>
        /// <returns></returns>
        public static string CodeDecoder(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
                return null;
            Bitmap myBitmap = new Bitmap(Image.FromFile(filePath));
            QRCodeDecoder decoder = new QRCodeDecoder();
            string decodedString = decoder.decode(new QRCodeBitmapImage(myBitmap));
            return decodedString;
        }

        /// <summary>
        /// 在二维码中间加入图片
        /// </summary>
        /// <param name="imgPhoto">原始图片</param>
        /// <param name="rMarkImgPath">中间图片地址</param>
        /// <param name="rDstImgPath">保存的地址</param>
        private static void BuildWatermark(Image imgPhoto, string rMarkImgPath, string rDstImgPath)
        {
            int squareLength = 50;
            var imgWarter = rMarkImgPath.IsNullOrWhiteSpace() ? null :
                Image.FromFile(rMarkImgPath);

            using (var g = Graphics.FromImage(imgPhoto))
            {
                if (imgWarter != null)
                {
                    g.DrawImage(imgWarter,
                        new Rectangle(imgPhoto.Width / 2 - squareLength / 2, imgPhoto.Height / 2 - squareLength / 2,
                            squareLength, squareLength),
                        0, 0, imgWarter.Width, imgWarter.Height, GraphicsUnit.Pixel);
                }

                imgPhoto.Save(rDstImgPath, System.Drawing.Imaging.ImageFormat.Png);
                imgPhoto.Dispose();
            }

        }
    }
}
