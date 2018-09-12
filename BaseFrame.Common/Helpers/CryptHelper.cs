using System;
using System.Security.Cryptography;
using System.Text;

namespace BaseFrame.Common.Helpers
{
    public class CryptHelper
    {
        // strKey 密钥,必须为24位 加密
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="strEncrypt"></param>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public static string Encrypt3DESToBase64(string strEncrypt, string strKey)
        {
            return ToBase64(Encrypt3DES(Encoding.UTF8.GetBytes(strEncrypt), strKey));
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="strDecrypt"></param>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public static string Decrypt3DESFromBase64(string strDecrypt, string strKey)
        {
            return Encoding.UTF8.GetString(Decrypt3DES(FromBase64(strDecrypt), strKey));
        }

        // strKey 密钥,必须为24位
        /// <summary>
        /// des 加密
        /// </summary>
        /// <param name="strEncrypt"></param>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public static byte[] Encrypt3DES(string strEncrypt, string strKey)
        {
            return Encrypt3DES(Encoding.UTF8.GetBytes(strEncrypt), strKey);
        }
        /// <summary>
        /// des解密
        /// </summary>
        /// <param name="strDecrypt"></param>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public static byte[] Decrypt3DES(string strDecrypt, string strKey)
        {
            return Decrypt3DES(Encoding.UTF8.GetBytes(strDecrypt), strKey);
        }

        public static byte[] Encrypt3DES(byte[] arrEncrypt, string strKey)
        {
            ICryptoTransform DESEncrypt = null;
            try
            {
                TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();

                DES.Key = ASCIIEncoding.ASCII.GetBytes(strKey);
                DES.Mode = CipherMode.ECB;

                DESEncrypt = DES.CreateEncryptor();
            }
            catch (System.Exception e)
            {
                return null;
            }

            return DESEncrypt.TransformFinalBlock(arrEncrypt, 0, arrEncrypt.Length);
        }

        public static byte[] Decrypt3DES(byte[] arrDecrypt, string strKey)
        {
            ICryptoTransform DESDecrypt = null;
            try
            {
                TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();

                DES.Key = ASCIIEncoding.ASCII.GetBytes(strKey);
                DES.Mode = CipherMode.ECB;
                DES.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

                DESDecrypt = DES.CreateDecryptor();
            }
            catch (System.Exception e)
            {
                return null;
            }

            return DESDecrypt.TransformFinalBlock(arrDecrypt, 0, arrDecrypt.Length);
        }

        public static string EncryptDES(string pToEncrypt, string sKey)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = ms.ToArray().ToString();
                ms.Close();
                return str;
            }
        }

        public static string DecryptDES(string pToDecrypt, string sKey)
        {
            byte[] inputByteArray = Encoding.Default.GetBytes(pToDecrypt);

            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }

                string str = ms.ToArray().ToString();
                ms.Close();
                return str;
            }
        }


        // 加密字符串   
        public static string EncryptString(string sInputString, string sKey)
        {
            byte[] data = Encoding.UTF8.GetBytes(sInputString);
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = new byte[8];
            ICryptoTransform desencrypt = DES.CreateEncryptor();
            byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
            return Convert.ToBase64String(result);
        }

        // 解密字符串   
        public static string DecryptString(string sInputString, string sKey)
        {
            byte[] data = Convert.FromBase64String(sInputString);
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = new byte[8];
            ICryptoTransform desencrypt = DES.CreateDecryptor();
            byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
            return Encoding.UTF8.GetString(result);
        }

        public static byte[] MD5(string str)
        {
            return MD5(Encoding.UTF8.GetBytes(str));
        }

        public static byte[] MD5(byte[] str)
        {
            MD5 m = new MD5CryptoServiceProvider();
            /*byte[] s = m.ComputeHash(str);
            string md5 = BitConverter.ToString(s);   
            md5 = md5.Replace("-", "");
            md5 = md5.Trim();
            return md5;*/
            return m.ComputeHash(str);
        }

        public static string MD5Str(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            byte[] result = MD5(str);            
            return BitConverter.ToString(result).Replace("-", "");          
        }

        public static string MD5ToBase64(string str)
        {
            return ToBase64(MD5(str));
        }

        public static string ToBase64(string str)
        {
            byte[] s = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(s);
        }

        public static string ToBase64(byte[] str)
        {
            return Convert.ToBase64String(str);
        }

        public static byte[] FromBase64(string str)
        {
            return Convert.FromBase64String(str);
        }

        public static string FromBase64ToStr(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            return Encoding.UTF8.GetString(Convert.FromBase64String(str));
        }

        public static string Utf8ToUnicode(byte[] str)
        {
            byte[] s = Encoding.Convert(Encoding.UTF8, Encoding.Default, str, 0, str.Length);
            return Encoding.Default.GetString(s, 0, s.Length);
        }

        public static string GetSecretKey()
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()), 0, 18).Replace("=", "");
        }
    }
}
