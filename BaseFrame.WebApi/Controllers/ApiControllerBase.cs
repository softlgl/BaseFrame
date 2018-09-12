using System.Web.Http;
using BaseFrame.WebApi.Models;

namespace BaseFrame.WebApi.Controllers
{
    
    public class ApiControllerBase : ApiController
    {
        /// <summary>
        /// 成功状态返回结果
        /// </summary>
        /// <param name="result">返回的数据</param>
        /// <returns></returns>
        protected ResponseResult SuccessResult(object result)
        {
            return new ResponseResult { Status = ResultStatus.Success, Result = result };
        }

        /// <summary>
        /// 成功状态返回结果
        /// </summary>
        /// <param name="result">返回的数据</param>
        /// <returns></returns>
        protected ResponseResult SuccessResult(string result)
        {
            return new ResponseResult { Status = ResultStatus.Success, Result = new { Code = ResponseCode.OK, Msg = result } };
        }

        /// <summary>
        /// 自定义状态返回结果
        /// </summary>
        /// <param name="status"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected ResponseResult Result(ResultStatus status, object result)
        {
            return new ResponseResult { Status = status, Result = result };
        }

        /// <summary>
        /// 失败状态返回结果
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="msg">失败信息</param>
        /// <returns></returns>
        protected ResponseResult FailResult(ResponseCode code, string msg = null)
        {
            return new ResponseResult { Status = ResultStatus.Fail, Result = new FailResult { Code = code, Msg = msg } };
        }

        /// <summary>
        /// 异常状态返回结果
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="msg">异常信息</param>
        /// <returns></returns>
        protected ResponseResult ErrorResult(ResponseCode code, string msg = null)
        {
            return new ResponseResult { Status = ResultStatus.Error, Result = new FailResult { Code = code, Msg = msg } };
        }
    }
}