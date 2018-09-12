using System.ComponentModel;
using BaseFrame.Common.Helpers;

namespace BaseFrame.WebApi.Models
{
    public class FailResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        private ResponseCode _code = ResponseCode.OK;
        public ResponseCode Code
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
            }
        }

        /// <summary>
        /// 消息描述
        /// </summary>
        private string _msg;
        public string Msg
        {
            get
            {
                return !string.IsNullOrEmpty(_msg) ? _msg : EnumHelper.GetDescription(Code);
            }
            set
            {
                _msg = value;
            }
        }
    }

    public enum ResponseCode
    {
        [Description("请求成功")]
        OK = 200,
        [Description("权限不足")]
        Unauthorized = 401,
        [Description("请求资源不存在")]
        NotFound = 404,
        [Description("服务端内部异常")]
        InternalServerError = 500,
    }
}