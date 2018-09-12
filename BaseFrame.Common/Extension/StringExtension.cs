using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace BaseFrame.Common.Extension
{
    public static class StringExtension
    {
        #region 公共方法

        /// <summary>
        /// 将时间戳转换为DateTime对象
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime GetTime(this string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime;
            long.TryParse(timeStamp + "0000000", out lTime);
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// 将json字符串转换成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T FromJson<T>(this string json)
        {
            return string.IsNullOrWhiteSpace(json) ? default(T) :
                JsonConvert.DeserializeObject<T>(json,new JsonSerializerSettings (){ NullValueHandling = NullValueHandling.Ignore });
        }

        /// <summary>
        /// 格式化字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string FormatString(this string str, params object[] parameters)
        {
            return string.Format(str, parameters);
        }

        /// <summary>
        /// sha1加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Sha1(this string str)
        {
            var bytes = Encoding.Default.GetBytes(str);
            var provider = new SHA1CryptoServiceProvider();
            bytes = provider.ComputeHash(bytes);
            var sb = new StringBuilder();
            foreach (byte iByte in bytes)
            {
                sb.AppendFormat("{0:x2}", iByte);
            }
            return sb.ToString();
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 如果空显示表达式
        /// </summary>
        /// <param name="str"></param>
        /// <param name="exp">表达式</param>
        /// <returns></returns>
        public static string IfNullOrWhiteSpaceShow(this string str,string exp)
        {
            return str ?? exp;
        }

        /// <summary>
        /// 判断是否包含中文字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool HasChinese(this string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 加密方法：ToBase64(3DES((Body明文,Key))
        /// </summary>
        /// <param name="strEncrypt">Body明文</param>
        /// <param name="strKey">Key</param>
        /// <returns>加密字符串</returns>
        public static string Encrypt3DESToBase64(this string strEncrypt, string strKey)
        {
            return ToBase64(Encrypt3DES(Encoding.UTF8.GetBytes(strEncrypt), strKey));
        }

        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="strDecrypt">密文</param>
        /// <param name="strKey">Key</param>
        /// <returns></returns>
        public static string Decrypt3DESFromBase64(this string strDecrypt, string strKey)
        {
            return Encoding.UTF8.GetString(Decrypt3Des(FromBase64(strDecrypt), strKey));
        }


        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Sign(this string str)
        {
            return ToBase64(MD5(str));
        }


        /// <summary>
        /// 转换为base64编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToBase64(this string str)
        {
            byte[] s = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(s);
        }

        /// <summary>
        /// app对接签名
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SignApp(this string str)
        {
            return ToBase64FromStr(Md5Str(str));
        }

        /// <summary>
        /// 得到16进制小写md5字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string Md5Str(string str)
        {
            MD5 m = new MD5CryptoServiceProvider();
            byte[] bytes = m.ComputeHash(Encoding.UTF8.GetBytes(str));
            return bytes.Aggregate<byte, string>(null, (current, b) => current + b.ToString("x2"));
        }

        /// <summary>
        /// 将str转换为base64Str
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string ToBase64FromStr(string str)
        {
            byte[] b = Encoding.UTF8.GetBytes(str);
            //转成 Base64 形式的 System.String  
            return Convert.ToBase64String(b);
        }

        /// <summary>
        /// 转换为base16编码
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding">编码格式</param>
        /// <returns></returns>
        public static string Sign16(this string str, string encoding)
        {
            return MD5(str, encoding);
        }



        /// <summary>
        /// 获取中英文混排字符串的实际长度(字节数)
        /// </summary>
        /// <param name="str">要获取长度的字符串</param>
        /// <returns>字符串的实际长度值（字节数）</returns>
        public static int GetStringByteLength(this string str)
        {
            if (str.Equals(string.Empty))
                return 0;
            var strlen = 0;
            var strData = new ASCIIEncoding();
            //将字符串转换为ASCII编码的字节数字
            var strBytes = strData.GetBytes(str);
            for (var i = 0; i <= strBytes.Length - 1; i++)
            {
                if (strBytes[i] == 63)  //中文都将编码为ASCII编码63,即"?"号
                    strlen++;
                strlen++;
            }
            return strlen;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="arrEncrypt">明文</param>
        /// <param name="strKey">key</param>
        /// <returns></returns>
        private static byte[] Encrypt3DES(byte[] arrEncrypt, string strKey)
        {
            ICryptoTransform DESEncrypt = null;
            try
            {
                TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();

                DES.Key = ASCIIEncoding.ASCII.GetBytes(strKey);
                DES.Mode = CipherMode.ECB;

                DESEncrypt = DES.CreateEncryptor();
            }
            catch (Exception e)
            {
                return null;
            }

            return DESEncrypt.TransformFinalBlock(arrEncrypt, 0, arrEncrypt.Length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string ToBase64(byte[] str)
        {
            return Convert.ToBase64String(str);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static byte[] FromBase64(string str)
        {
            return Convert.FromBase64String(str);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arrDecrypt"></param>
        /// <param name="strKey"></param>
        /// <returns></returns>
        private static byte[] Decrypt3Des(byte[] arrDecrypt, string strKey)
        {
            ICryptoTransform desDecrypt = null;
            try
            {
                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();

                des.Key = Encoding.ASCII.GetBytes(strKey);
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.PKCS7;

                desDecrypt = des.CreateDecryptor();
            }
            catch (Exception)
            {
                return null;
            }

            return desDecrypt.TransformFinalBlock(arrDecrypt, 0, arrDecrypt.Length);
        }


        public static byte[] MD5(string str)
        {
            MD5 m = new MD5CryptoServiceProvider();
            return m.ComputeHash(Encoding.UTF8.GetBytes(str));
        }

        /// <summary>
        /// md5加密函数，易鑫提供
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="encoding">The encoding</param>
        /// <returns></returns>
        private static string MD5(string str, string encoding = "GBK")
        {
            byte[] result = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(Encoding.GetEncoding(encoding).GetBytes(str));
            StringBuilder output = new StringBuilder(16);
            for (int i = 0; i < result.Length; i++)
            {
                output.Append((result[i]).ToString("x2", System.Globalization.CultureInfo.InvariantCulture));
            }
            return output.ToString();
        }

        #endregion


        public static string ConvertSysPathToWebPath(this string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

            var array = path.Split('\\');

            if(array.Length>1)
            {
                return string.Join("/", array);
            }
            return path;
        }
    }
}
