using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace BaseFrame.Web.Common
{
    /// <summary>
    /// 该类继承了ActionResult，通过重写ExecuteResult方法，进行文件的下载
    /// </summary>
    public class LargeFileResult : FilePathResult
    {
        private const int BufferSize = 0x1000;
        public LargeFileResult(string fileName, string contentType) : base(fileName, contentType)
        {

        }

        public override void ExecuteResult(ControllerContext context)
        {
            HttpResponseBase response = context.HttpContext.Response;
            if (File.Exists(FileName))
            {
                FileStream fs = null;
                byte[] fileBuffer = new byte[BufferSize];//每次读取4096字节大小的数据
                try
                {
                    using (fs = File.OpenRead(FileName))
                    {
                        long totalLength = fs.Length;
                        response.ContentType = ContentType;
                        response.AddHeader("Content-Length", totalLength.ToString());
                        response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(Path.GetFileName(FileName)));
                        while (totalLength > 0 && response.IsClientConnected)//持续传输文件
                        {
                            int length = fs.Read(fileBuffer, 0, fileBuffer.Length);//每次读取4096个字节长度的内容
                            fs.Flush();
                            response.OutputStream.Write(fileBuffer, 0, length);//写入到响应的输出流
                            response.Flush();//刷新响应
                            totalLength = totalLength - length;
                        }
                        response.Close();//文件传输完毕，关闭相应流
                    }
                }
                catch (Exception ex)
                {
                    response.Write(ex.Message);
                }
                finally
                {
                    fs?.Close();//最后记得关闭文件流
                }
            }
        }
    }
}