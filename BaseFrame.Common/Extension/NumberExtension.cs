namespace BaseFrame.Common.Extension
{
    /// <summary>
    /// 数字扩展类
    /// </summary>
    public static class NumberExtension
    {
        public static string NumberOrStringWhenZero(this int num,string exp)
        {
            return num == 0 ? exp : num.ToString();
        }

        /// <summary>
        /// 金额(分转元)
        /// </summary>
        /// <param name="fen"></param>
        /// <returns></returns>
        public static decimal Fen2Yuan(this string fen)
        {
            long lfen = 0;
            if (long.TryParse(fen, out lfen))
            {
                return Fen2Yuan(lfen);
            }
            return 0.00m;
        }

        /// <summary>
        /// 金额(分转元)
        /// </summary>
        /// <param name="fen"></param>
        /// <returns></returns>
        public static decimal Fen2Yuan(this long fen)
        {
            return fen / 100m;
        }

        /// <summary>
        /// 金额(分转元)
        /// </summary>
        /// <param name="fen"></param>
        /// <returns></returns>
        public static decimal Fen2Yuan(this int fen)
        {
            return fen / 100m;
        }
    }
}
