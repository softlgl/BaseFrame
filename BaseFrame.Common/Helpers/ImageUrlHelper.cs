using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using BaseFrame.Common.Extension;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Zip;
using NPOI.SS.UserModel;

namespace BaseFrame.Common.Helpers
{
    public class ImageUrlHelper
    {
        //private static string path = HttpContext.Current.Server.MapPath("~");

        //常规置换
        //private static string oldCarDir = HttpContext.Current.Server.MapPath("~/UploadImage/{1}/{0}/OldCar");
        //private static string newCarUrl = HttpContext.Current.Server.MapPath("~/UploadImage/{1}/{0}/NewCar");

        /// <summary>
        /// 获取二手车登记证图片Url
        /// </summary>
        /// <param name="id">工单ID</param>
        /// <returns></returns>
        public static List<string> GetRegistrationImage(long id)
        {
            List<string> list = new List<string>();
            string oldCarDir = HttpContext.Current.Server.MapPath("~/UploadImage/{1}/{0}/OldCar");
            oldCarDir = string.Format(oldCarDir, id, "NormalSubstitution");
            DirectoryInfo folder = new DirectoryInfo(oldCarDir);
            foreach (var file in folder.GetFiles())
            {
                if (file.Name.Contains("RegistrationImage1") || file.Name.Contains("RegistrationImage2") || file.Name.Contains("RegistrationImage3"))
                {
                    list.Add(file.FullName);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取报废证明图片Url
        /// </summary>
        /// <param name="id">工单ID</param>
        /// <returns></returns>
        public static List<string> GetScrapImage(long id)
        {
            List<string> list = new List<string>();
            string oldCarDir = HttpContext.Current.Server.MapPath("~/UploadImage/{1}/{0}/OldCar");
            oldCarDir = string.Format(oldCarDir, id, "ScrapSubstitution");
            DirectoryInfo folder = new DirectoryInfo(oldCarDir);
            foreach (var file in folder.GetFiles())
            {
                if (file.Name.Contains("ScrapImage1") || file.Name.Contains("ScrapImage2") || file.Name.Contains("ScrapImage3"))
                {
                    list.Add(file.FullName);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取身份证明Url
        /// </summary>
        /// <param name="id">工单ID</param>
        /// <returns></returns>
        public static List<string> GetIdentityProve(long id, int orderType)
        {
            List<string> list = new List<string>();
            string oldCarDir = HttpContext.Current.Server.MapPath("~/UploadImage/{1}/{0}/OldCar");
            string newCarUrl = HttpContext.Current.Server.MapPath("~/UploadImage/{1}/{0}/NewCar");
            if (orderType == 1)
            {
                oldCarDir = string.Format(oldCarDir, id, "NormalSubstitution");
                newCarUrl = string.Format(newCarUrl, id, "NormalSubstitution");
            }
            if (orderType == 2)
            {
                oldCarDir = string.Format(oldCarDir, id, "ScrapSubstitution");
                newCarUrl = string.Format(newCarUrl, id, "ScrapSubstitution");
            }

            DirectoryInfo folder = new DirectoryInfo(oldCarDir);
            foreach (var file in folder.GetFiles())
            {
                if (file.Name.Contains("IdCardImage"))
                {
                    list.Add(file.FullName);
                }
            }

            DirectoryInfo folder1 = new DirectoryInfo(newCarUrl);
            foreach (var file in folder1.GetFiles())
            {
                if (file.Name.Contains("IdCardImage") || file.Name.Contains("RelationshipImage1") || file.Name.Contains("RelationshipImage2") || file.Name.Contains("RelationshipImage3"))
                {
                    list.Add(file.FullName);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取旧车发票Url
        /// </summary>
        /// <param name="id">工单ID</param>
        /// <returns></returns>
        public static List<string> GetOldCarInvoice(long id, int orderType)
        {
            List<string> list = new List<string>();
            string oldCarDir = HttpContext.Current.Server.MapPath("~/UploadImage/{1}/{0}/OldCar");
            if (orderType == 1)
            {
                oldCarDir = string.Format(oldCarDir, id, "NormalSubstitution");
            }
            if (orderType == 2)
            {
                oldCarDir = string.Format(oldCarDir, id, "ScrapSubstitution");
            }

            DirectoryInfo folder = new DirectoryInfo(oldCarDir);
            foreach (var file in folder.GetFiles())
            {
                if (file.Name.Contains("InvoiceImage"))
                {
                    list.Add(file.FullName);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取新车发票Url
        /// </summary>
        /// <param name="id">工单ID</param>
        /// <returns></returns>
        public static List<string> GetNewCarInvoice(long id, int orderType)
        {
            List<string> list = new List<string>();
            string newCarUrl = HttpContext.Current.Server.MapPath("~/UploadImage/{1}/{0}/NewCar");
            if (orderType == 1)
            {
                newCarUrl = string.Format(newCarUrl, id, "NormalSubstitution");
            }
            if (orderType == 2)
            {
                newCarUrl = string.Format(newCarUrl, id, "ScrapSubstitution");
            }

            DirectoryInfo folder = new DirectoryInfo(newCarUrl);
            foreach (var file in folder.GetFiles())
            {
                if (file.Name.Contains("InvoiceImage"))
                {
                    list.Add(file.FullName);
                }
            }
            return list;
        }

        public static int GetPictureIdx(IWorkbook workbook, byte[] bytes, string url)
        {
            string ext = Path.GetExtension(url);
            int pictureIdx = 0;

            switch (ext.ToLower())
            {
                case ".png":
                    pictureIdx = workbook.AddPicture(bytes, PictureType.PNG);
                    return pictureIdx;

                case ".jpg":
                    pictureIdx = workbook.AddPicture(bytes, PictureType.JPEG);
                    return pictureIdx;

                case ".jpeg":
                    pictureIdx = workbook.AddPicture(bytes, PictureType.JPEG);
                    return pictureIdx;

                default:
                    return pictureIdx;
            }
        }

        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Compress(string input)
        {
            string result = string.Empty;
            byte[] buffer = Encoding.UTF8.GetBytes(input);
            using (MemoryStream outputStream = new MemoryStream())
            {
                using (BZip2OutputStream zipStream = new BZip2OutputStream(outputStream))
                {
                    zipStream.Write(buffer, 0, buffer.Length);
                    zipStream.Close();
                }
                return Convert.ToBase64String(outputStream.ToArray());
            }
        }

        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Decompress(string input)
        {
            string result = string.Empty;
            byte[] buffer = Convert.FromBase64String(input);
            using (Stream inputStream = new MemoryStream(buffer))
            {
                BZip2InputStream zipStream = new BZip2InputStream(inputStream);

                using (StreamReader reader = new StreamReader(zipStream, Encoding.UTF8))
                {
                    //输出
                    result = reader.ReadToEnd();
                }
            }

            return result;
        }

        #region 压缩文件
        /// <summary>
        /// 功能：压缩文件（暂时只压缩文件夹下一级目录中的文件，文件夹及其子级被忽略）
        /// </summary>
        /// <param name="dirPath">被压缩的文件夹夹路径</param>
        /// <param name="zipFilePath">生成压缩文件的路径，为空则默认与被压缩文件夹同一级目录，名称为：文件夹名+.zip</param>
        /// <param name="err">出错信息</param>
        /// <returns>是否压缩成功</returns>
        public static bool ZipFile(string dirPath, string zipFilePath,string[] fileNames,  out string err)
        {
            err = "";
            if (dirPath == string.Empty)
            {
                err = "要压缩的文件夹不能为空！";
                return false;
            }
            if (!Directory.Exists(dirPath))
            {
                err = "要压缩的文件夹不存在！";
                return false;
            }
            //压缩文件名为空时使用文件夹名＋.zip
            if (string.IsNullOrWhiteSpace(zipFilePath))
            {
                if (dirPath.EndsWith("\\"))
                {
                    dirPath = dirPath.Substring(0, dirPath.Length - 1);
                }
                zipFilePath = dirPath + ".zip";
            }

            try
            {
                //string[] filenames = Directory.GetFiles(dirPath);
                using (ZipOutputStream s = new ZipOutputStream(File.Create(zipFilePath)))
                {
                    s.SetLevel(9);
                    byte[] buffer = new byte[4096];
                    foreach (string file in fileNames)
                    {
                        if(file.IsNullOrWhiteSpace() || !File.Exists(file))continue;
                        ZipEntry entry = new ZipEntry(Path.GetFileName(file));
                        entry.DateTime = DateTime.Now;
                        s.PutNextEntry(entry);
                        using (FileStream fs = File.OpenRead(file))
                        {
                            int sourceBytes;
                            do
                            {
                                sourceBytes = fs.Read(buffer, 0, buffer.Length);
                                s.Write(buffer, 0, sourceBytes);
                            } while (sourceBytes > 0);
                        }
                    }
                    s.Finish();
                    s.Close();
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
                return false;
            }
            return true;
        }

        #endregion

        #region 解压缩
        /// <summary>
        /// 功能：解压zip格式的文件。
        /// </summary>
        /// <param name="zipFilePath">压缩文件路径</param>
        /// <param name="unZipDir">解压文件存放路径,为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹</param>
        /// <param name="err">出错信息</param>
        /// <returns>解压是否成功</returns>
        public static bool UnZipFile(string zipFilePath, string unZipDir, out string err)
        {
            err = "";
            if (zipFilePath == string.Empty)
            {
                err = "压缩文件不能为空！";
                return false;
            }
            if (!File.Exists(zipFilePath))
            {
                err = "压缩文件不存在！";
                return false;
            }
            //解压文件夹为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹
            if (unZipDir == string.Empty)
                unZipDir = zipFilePath.Replace(Path.GetFileName(zipFilePath), Path.GetFileNameWithoutExtension(zipFilePath));
            if (!unZipDir.EndsWith("\\"))
                unZipDir += "\\";
            if (!Directory.Exists(unZipDir))
                Directory.CreateDirectory(unZipDir);

            try
            {
                using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
                {

                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        string directoryName = Path.GetDirectoryName(theEntry.Name);
                        string fileName = Path.GetFileName(theEntry.Name);
                        if (directoryName.Length > 0)
                        {
                            Directory.CreateDirectory(unZipDir + directoryName);
                        }
                        if (!directoryName.EndsWith("\\"))
                            directoryName += "\\";
                        if (fileName != String.Empty)
                        {
                            using (FileStream streamWriter = File.Create(unZipDir + theEntry.Name))
                            {

                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        streamWriter.Write(data, 0, size);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }//while
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
                return false;
            }
            return true;
        }
        //解压结束
        #endregion
    }
}



