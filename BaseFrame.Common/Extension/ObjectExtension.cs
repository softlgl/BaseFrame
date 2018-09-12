using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace BaseFrame.Common.Extension
{
    public static class ObjectExtension
    {
        /// <summary>
        /// 将object转成json字符串
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="isIndent">是否缩进（带有时间格式数据需要为1）</param>
        /// <returns>json字符串</returns>
        public static string ToJson(this object obj, bool isIndent = false)
        {
            var settings = new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd HH:mm:ss"
            };

            return JsonConvert.SerializeObject(obj, isIndent ? Formatting.Indented : Formatting.None, settings);
        }

        /// <summary>
        /// 将object转换成xml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectToXml<T>(this T obj)
        {
            string result = string.Empty;
            XmlSerializer xsl = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                xsl.Serialize(ms, obj);
                result = Encoding.UTF8.GetString(ms.ToArray());
            }
            return result;
        }

        /// <summary>
        /// 将xml转换成object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlStr"></param>
        /// <returns></returns>
        public static T XmlToObject<T>(this string xmlStr)
        {
            T t = default(T);
            XmlSerializer xsl = new XmlSerializer(typeof(T));
            using (Stream ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlStr)))
            {
                using (System.Xml.XmlReader reeder = System.Xml.XmlReader.Create(ms))
                {
                    Object obj = xsl.Deserialize(reeder);
                    t = (T)obj;
                }
            }
            return t;
        }

        /// <summary>
        /// 将HttpPostedFileBase流转成byte[]
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static byte[] ConvertToByte(this HttpPostedFileBase file)
        {
            byte[] imageByte = null;
            using (BinaryReader br = new BinaryReader(file.InputStream))
            {
                imageByte = br.ReadBytes(file.ContentLength);
            }
            return imageByte;
        }

        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        public static string GetTimeStamp(this DateTime dateTime)
        {
            var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
    }
}
