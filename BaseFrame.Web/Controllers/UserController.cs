using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaseFrame.IService;

namespace BaseFrame.Web.Controllers
{
    public class UserController : BaseController
    {
        public IUserService UserService { get; set; }
        public JsonResult GetUserById(int uid)
        {
            return Json(UserService.GetUserById(uid), JsonRequestBehavior.AllowGet);
        }
    }
}