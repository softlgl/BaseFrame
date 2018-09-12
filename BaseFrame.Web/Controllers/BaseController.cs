using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaseFrame.Web.Common;

namespace BaseFrame.Web.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 大文件下载
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        protected FilePathResult LargeFile(string fileName, string contentType)
        {
            return new LargeFileResult(fileName, contentType);
        }
    }
}