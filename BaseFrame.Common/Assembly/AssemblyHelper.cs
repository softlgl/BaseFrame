using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BaseFrame.Common.Extension;

namespace BaseFrame.Common.Assembly
{
    public class AssemblyHelper
    {
        #region 加载程序集

        public static List<System.Reflection.Assembly> GetAllAssembly(string dllName)
        {
            List<string> plugiinPath = FindPlugin(dllName);
            var list = new List<System.Reflection.Assembly>();
            foreach (string fileName in plugiinPath)
            {
                try
                {
                    string asmName = Path.GetFileNameWithoutExtension(fileName);
                    if (asmName != string.Empty)
                    {
                        System.Reflection.Assembly asm = System.Reflection.Assembly.LoadFrom(fileName);
                        list.Add(asm);
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return list;
        }
        //查找所有插件的路径
        private static List<string> FindPlugin(string dllName)
        {
            List<string> pluginPath = new List<string>();

            string path = AppDomain.CurrentDomain.BaseDirectory;
            string dir = string.Empty;
            string[] dllList;
            if (Directory.Exists(Path.Combine(path, "bin")))
            {
                dir = Path.Combine(path, "bin");
            }
            else if (Directory.Exists(path))
            {
                dir = path.EndsWith(@"\") ? path.Substring(0, path.Length - 1) : path;
            }
            if (!dir.IsNullOrWhiteSpace())
            {
                dllList = Directory.GetFiles(dir, dllName);
                if (dllList.Length > 0)
                {
                    pluginPath.AddRange(dllList.Select(item => Path.Combine(dir, item.Substring(dir.Length + 1))));
                }
            }
            return pluginPath;
        }

        #endregion
    }
}
