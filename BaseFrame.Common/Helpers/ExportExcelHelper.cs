using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Reflection;
using BaseFrame.Common.log4net;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace BaseFrame.Common.Helpers
{
    public static class ExportExcelHelper
    {
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public static Stream ExportExcelReport(DataTable dt, string header)
        {
            try
            {
                if (dt.Columns.Count == 0) return null;
                int columnCount = dt.Columns.Count;
                int startIndex = 0;  //从第几行开始,默认从零开始

                HSSFWorkbook workbook = new HSSFWorkbook(); //创建Excel文档
                ISheet sheet = workbook.CreateSheet(); //创建工作表格

                #region 设置标题
                IRow row = sheet.CreateRow(startIndex);//在工作表格中添加一行
                row.HeightInPoints = 40;
                ICell headerCell = row.CreateCell(0);

                ICellStyle headerStyle = workbook.CreateCellStyle(); //设置格的样式      
                headerStyle.Alignment = HorizontalAlignment.Center; //左右居中
                headerStyle.VerticalAlignment = VerticalAlignment.Center;  //上下居中

                IFont headerFont = workbook.CreateFont(); //设置字体
                headerFont.FontHeightInPoints = 18;
                headerFont.Boldweight = (short)FontBoldWeight.Bold;
                headerStyle.SetFont(headerFont);

                headerCell.CellStyle = headerStyle;
                headerCell.SetCellValue(header);
                CellRangeAddress range = new CellRangeAddress(0, 0, 0, columnCount - 1);
                sheet.AddMergedRegion(range);
                startIndex++;
                #endregion

                #region  创建列和列名称
                row = sheet.CreateRow(startIndex); //在工作表格中添加一行
                row.HeightInPoints = 20;  //行的高度
                for (int i = 0; i < columnCount; i++)
                {
                    string columnName = dt.Columns[i].ColumnName; //列名称
                    sheet.SetColumnWidth(i, columnName.Length * 3 * 256); //宽度
                    ICell cell = row.CreateCell(i); //在行中添加一列 

                    ICellStyle style = workbook.CreateCellStyle(); //设置格的样式      
                    style.Alignment = HorizontalAlignment.Center; //左右居中
                    style.VerticalAlignment = VerticalAlignment.Center;  //上下居中
                    //style.FillForegroundColor = HSSFColor.Black.Index;
                    //style.FillPattern = FillPattern.SolidForeground;

                    IFont font = workbook.CreateFont(); //设置字体
                    font.FontHeightInPoints = 13;
                    font.Boldweight = (short)FontBoldWeight.Bold;
                    style.SetFont(font);

                    cell.CellStyle = style;
                    cell.SetCellValue(columnName);//设置列的内容
                }
                startIndex++;
                #endregion

                #region 填充数据
                ICellStyle dataStyle = workbook.CreateCellStyle(); //设置格的样式      
                dataStyle.Alignment = HorizontalAlignment.Center; //左右居中
                dataStyle.VerticalAlignment = VerticalAlignment.Center;  //上下居中        
                for (int i = 0; i < dt.Rows.Count; i++) //遍历DataTable行
                {
                    DataRow dataRow = dt.Rows[i];
                    row = sheet.CreateRow(startIndex + i); //在工作表中添加一行

                    for (int j = 0; j < columnCount; j++) //遍历DataTable列
                    {
                        ICell cell = row.CreateCell(j);//在行中添加一列              
                        cell.CellStyle = dataStyle;
                        cell.SetCellValue(dataRow[j].ToString()); //设置列的内容	 
                    }
                }
                #endregion

                var ms = new MemoryStream();

                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                return ms;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 导出Excel模版
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Stream ExportExcelTemplate<T>()
        {
            try
            {
                int index = 0;  //从第几行开始,默认从零开始
                HSSFWorkbook workbook = new HSSFWorkbook(); //创建Excel文档
                ISheet sheet = workbook.CreateSheet(); //创建工作表格

                #region  创建列和列名称
                IRow row = sheet.CreateRow(0); //在工作表格中添加一行
                //row.HeightInPoints = 20;  //行的高度
                var type = typeof(T);
                var Properties = type.GetProperties();
                foreach (var p in Properties)
                {
                    var attr = p.GetCustomAttribute<DisplayNameAttribute>();
                    if (attr != null)
                    {
                        var displayName = attr.DisplayName; //列名称
                        sheet.SetColumnWidth(index, 6 * 2 * 256); //宽度
                        ICell cell = row.CreateCell(index); //在行中添加一列 

                        ICellStyle style = workbook.CreateCellStyle(); //设置格的样式      
                        style.Alignment = HorizontalAlignment.Center; //左右居中
                        style.VerticalAlignment = VerticalAlignment.Center;  //上下居中
                        style.FillForegroundColor = HSSFColor.Green.Index;
                        style.FillPattern = FillPattern.SolidForeground;

                        IFont font = workbook.CreateFont(); //设置字体
                        //font.FontHeightInPoints = 13;
                        font.Color = new HSSFColor.White().Indexed;
                        font.Boldweight = (short)FontBoldWeight.Bold;
                        style.SetFont(font);

                        cell.CellStyle = style;
                        cell.SetCellValue(displayName);//设置列的内容
                        index++;
                    }
                }
                #endregion

                var ms = new MemoryStream();
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                return ms;
            }
            catch (Exception ex)
            {
                Log4Helper.Error("ExportExcelReport异常", ex);
                throw;
            }
        }

        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IList<T> ImportExcelToList<T>(Stream s) where T : class, new()
        {
            int index = 2;
            try
            {
                IList<T> list = new List<T>();
                IWorkbook wk = new HSSFWorkbook(s);
                ISheet sheet = wk.GetSheetAt(0); //获取第一个sheet               
                IRow headrow = sheet.GetRow(0);  //获取第一行

                List<string> headlist = new List<string>();
                //创建列                                
                for (int i = headrow.FirstCellNum; i < headrow.LastCellNum; i++)
                {
                    headlist.Add(headrow.GetCell(i).StringCellValue);//这里没有考虑数据格式转换，会出现bug
                }

                var fields = headlist.ToArray();
                //遍历每一行数据
                for (int i = sheet.FirstRowNum + 1; i < sheet.LastRowNum + 1; i++)
                {
                    T t = new T();
                    IRow row = sheet.GetRow(i);
                    for (int j = 0; j < fields.Length; j++)
                    {
                        ICell cell = row.GetCell(j);
                        if (cell == null) continue;
                        object cellValue;
                        switch (cell.CellType)
                        {
                            case CellType.String: //文本
                                cellValue = cell.StringCellValue;
                                break;

                            case CellType.Numeric: //数值
                                cellValue = Convert.ToInt64(cell.NumericCellValue);//Double转换为int
                                break;

                            case CellType.Boolean: //bool
                                cellValue = cell.BooleanCellValue;
                                break;

                            case CellType.Blank: //空白
                                cellValue = null;
                                break;

                            default:
                                cellValue = "ERROR";
                                break;
                        }
                        var type = typeof(T);
                        var Properties = type.GetProperties();
                        foreach (var p in Properties)
                        {
                            var attr = p.GetCustomAttribute<DisplayNameAttribute>();
                            if (attr != null)
                            {
                                var displayName = attr.DisplayName; //列名称
                                if (displayName.Equals(fields[j]))
                                {
                                    if (!p.PropertyType.IsGenericType)
                                        cellValue = cellValue == null ? null : Convert.ChangeType(cellValue, p.PropertyType);
                                    else //泛型Nullable<>
                                    {
                                        Type genericTypeDefinition = p.PropertyType.GetGenericTypeDefinition();
                                        if (genericTypeDefinition == typeof(Nullable<>))
                                        {
                                            cellValue = cellValue == null
                                                ? null
                                                : Convert.ChangeType(cellValue, Nullable.GetUnderlyingType(p.PropertyType));
                                        }
                                    }
                                    p.SetValue(t, cellValue, null);
                                }
                            }
                        }
                    }
                    list.Add(t);
                    index++;
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception($"第{index}行数据出现问题:{ex.Message}");
            }
        }
    }
}
