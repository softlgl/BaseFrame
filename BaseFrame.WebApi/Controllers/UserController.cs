using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BaseFrame.IService;
using BaseFrame.WebApi.Models;

namespace BaseFrame.WebApi.Controllers
{
    public class UserController : ApiControllerBase
    {
        public IUserService UserService { get; set; }

        [HttpGet]
        public ResponseResult Get(int id)
        {
            return SuccessResult(UserService.GetUserById(id));
        }
    }
}
