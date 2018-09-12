using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BaseFrame.Common.Helpers
{
    public class ValidateHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minLen"></param>
        /// <param name="maxLen"></param>
        /// <returns></returns>
        public static bool CheckPasswordFormat(string str, int minLen = 6, int maxLen = 8)
        {
            var reg = @"^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{" + minLen + "," + maxLen + "}$";

            return Regex.IsMatch(str, reg);
        }


        /// <summary>  
        /// 验证身份证合理性  
        /// </summary>
        /// <param name="idNumber"></param>
        /// <returns></returns>  
        public static bool CheckIdCard(string idNumber)
        {
            if (idNumber.Length == 18)
            {
                bool check = CheckIdCard18(idNumber);
                return check;
            }
            else if (idNumber.Length == 15)
            {
                bool check = CheckIdCard15(idNumber);
                return check;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 验证车牌号
        /// </summary>
        /// <param name="vehicleNumber">车牌号码</param>
        /// <returns></returns>
        public static bool IsVehicleNumber(string vehicleNumber)
        {
            bool result = false;

            if (vehicleNumber.Equals("未知"))
            {
                return true;
            }

            if (vehicleNumber.Length == 7)
            {
                string express = @"^[京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领A-Z]{1}[A-Z]{1}[A-Z0-9]{4}[A-Z0-9挂学警港澳]{1}$";
                result = Regex.IsMatch(vehicleNumber, express);
            }
            return result;
        }

        /// <summary>
        /// 校验VIN规则
        /// </summary>
        /// <param name="vin"></param>
        /// <returns></returns>
        public static bool VINCheck(string vin)
        {

            Dictionary<int, int> vinMapWeight = new Dictionary<int, int>()
            {
                { 1, 8 },
                { 2, 7 },
                { 3, 6 },
                { 4, 5 },
                { 5, 4 },
                { 6, 3 },
                { 7, 2 },
                { 8, 10 },
                { 9, 0 },
                { 10, 9 },
                { 11, 8 },
                { 12, 7 },
                { 13, 6 },
                { 14, 5 },
                { 15, 4 },
                { 16, 3 },
                { 17, 2 },
            };
            Dictionary<char, int> vinMapValue = new Dictionary<char, int>()
            {
                { '0', 0 },{ '1', 1 },{ '2', 2 },{ '3', 3 },{ '4', 4 },
                { '5', 5 },{ '6', 6 },{ '7', 7 },{ '8', 8 },{ '9', 9 },
                { 'A', 1 },{ 'B', 2 },{ 'C', 3 },{ 'D', 4 },{ 'E', 5 },
                { 'F', 6 },{ 'G', 7 },{ 'H', 8 },{ 'J', 1 },{ 'K', 2 },
                { 'L', 3 },{ 'M', 4 },{ 'N', 5 },{ 'P', 7 },{ 'R', 9 },
                { 'S', 2 },{ 'T', 3 },{ 'U', 4 },{ 'V', 5 },{ 'W', 6 },
                { 'X', 7 },{ 'Y', 8 },{ 'Z', 9 }
            };

            if (string.IsNullOrWhiteSpace(vin))
                return false;

            if (vin.Length != 17)
                return false;
            if (vin.IndexOf("I") >= 0 || vin.IndexOf("O") >= 0 || vin.IndexOf("Q") >= 0)
                return false;


            var vinArr = vin.ToCharArray();
            var sum = 0;
            for (int i = 0; i < vinArr.Length; i++)
            {
                sum += vinMapValue[vinArr[i]] * vinMapWeight[i + 1];
            }
            if (sum % 11 == 10)
            {
                if (vinArr[8] != 'X')
                    return false;
            }
            else
            {
                if (sum % 11 != vinMapValue[vinArr[8]])
                    return false;
            }

            return true;
        }


        /// <summary>  
        /// 18位身份证号码验证  
        /// </summary>  
        private static bool CheckIdCard18(string idNumber)
        {
            long n;
            if (long.TryParse(idNumber.Remove(17), out n) == false
                || n < Math.Pow(10, 16) || long.TryParse(idNumber.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证  
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(idNumber.Remove(2), StringComparison.Ordinal) == -1)
            {
                return false;//省份验证  
            }
            string birth = idNumber.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time;
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证  
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] ai = idNumber.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(wi[i]) * int.Parse(ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != idNumber.Substring(17, 1).ToLower())
            {
                return false;//校验码验证  
            }
            return true;//符合GB11643-1999标准  
        }


        /// <summary>  
        /// 16位身份证号码验证  
        /// </summary>  
        private static bool CheckIdCard15(string idNumber)
        {
            long n = 0;
            if (long.TryParse(idNumber, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证  
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(idNumber.Remove(2), StringComparison.Ordinal) == -1)
            {
                return false;//省份验证  
            }
            string birth = idNumber.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time;
            return DateTime.TryParse(birth, out time) != false;
        }
    }
}
