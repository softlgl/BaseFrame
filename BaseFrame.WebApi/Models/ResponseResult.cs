using System.ComponentModel;

namespace BaseFrame.WebApi.Models
{
    public class ResponseResult
    {
        /// <summary>
        /// 请求状态
        /// </summary>
        public ResultStatus Status { get; set; }
        /// <summary>
        /// 返回结果
        /// </summary>
        public object Result { get; set; }
    }

    public enum ResultStatus
    {
        [Description("请求成功")]
        Success = 1,
        [Description("请求失败")]
        Fail = 0,
        [Description("请求异常")]
        Error = -1,
    }
}