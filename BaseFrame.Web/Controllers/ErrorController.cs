using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BaseFrame.Web.Controllers
{
    public class ErrorController : BaseController
    {
        /// <summary>
        /// 404页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Page404()
        {
            return View();
        }

        /// <summary>
        /// 500页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Page500()
        {
            return View();
        }
    }
}