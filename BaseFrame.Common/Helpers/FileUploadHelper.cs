using System.IO;
using System.Web;

namespace BaseFrame.Common.Helpers
{
    public static class FileUploadHelper
    {
        public static void SaveFiles(this HttpContext httpContext,string virtualPath,string fileName)
        {
            var dirInfo = httpContext.Server.MapPath(virtualPath);

            if(!Directory.Exists(dirInfo))
            {
                Directory.CreateDirectory(dirInfo);
            }

            var files = httpContext.Request.Files;

            foreach(var key in files.AllKeys)
            {
                var file = files[key];

                var finalFileName =Path.Combine(httpContext.Server.MapPath(virtualPath),fileName);

                file.SaveAs(finalFileName);
            }
      
        }
    }
}
