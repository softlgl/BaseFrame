using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using BaseFrame.Common.Helpers;

namespace BaseFrame.Common.Extension
{
    public static class ListExtension
    {
        /// <summary>
        /// 获取导出Excel stream
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datas">list集合</param>
        /// <param name="columns">列名集合</param>
        /// <param name="header">Excel header名称</param>
        /// <returns></returns>
        public static Stream GetExportList<T>(this List<T> datas, string[] columns, string header)
        {
            DataTable dt = datas.ToDataTable(columns);
            return ExportExcelHelper.ExportExcelReport(dt, header);
        }

        /// <summary>
        /// Convert a List{T} to a DataTable.
        /// </summary>
        public static DataTable ToDataTable<T>(this List<T> items,string[] columns=null)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            if (columns==null||columns.Length==0)
            {
                foreach (PropertyInfo prop in props)
                {
                    Type t = GetCoreType(prop.PropertyType);
                    tb.Columns.Add(prop.Name, t);
                }
            }
            else
            {
                foreach (string prop in columns)
                {
                    tb.Columns.Add(prop);
                }
            }

            foreach (T item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }
            return tb;
        }

        /// <summary>
        /// Determine of specified type is nullable
        /// </summary>
        public static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// Return underlying type if type is Nullable otherwise return the type
        /// </summary>
        public static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                return Nullable.GetUnderlyingType(t);
            }
            return t;
        }
    }
}
