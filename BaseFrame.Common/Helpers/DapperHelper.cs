using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Dapper;

namespace BaseFrame.Common.Helpers
{
    public static class DapperHelper
    {
        // Remember to add <remove name="LocalSqlServer" > in ConnectionStringssection if using this, as otherwise it would be the first one.

        /// <summary>
        /// 获取一个打开的链接
        /// </summary>
        /// <param name="name">The name of the connection string (optional).</param>
        /// <returns></returns>
        public static IDbConnection GetOpenConnection(string name = null)
        {
            string connString = "";
            connString = name == null ? ConfigurationManager.ConnectionStrings[0].ConnectionString : ConfigurationManager.ConnectionStrings[name].ConnectionString;
            var connection = new SqlConnection(connString);
            connection.Open();
            return connection;
        }

        /// <summary>
        /// 插入多条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="entities"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        public static int InsertMultiple<T>(string sql, IEnumerable<T> entities, string connectionName = null) where T : class, new()
        {
            using (var cnn = GetOpenConnection(connectionName))
            {
                var tran = cnn.BeginTransaction();
                try
                {
                    int records = 0;

                    foreach (T entity in entities)
                    {
                        records += cnn.Execute(sql, entity);
                    }
                    tran.Commit();
                    return records;
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IList<T> list)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in list)
            {
                for (int i = 0; i < values.Length; i++)
                    values[i] = props[i].GetValue(item) ?? DBNull.Value;
                table.Rows.Add(values);
            }
            return table;
        }

        /// <summary>
        /// 获取动态参数
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyNamesToIgnore"></param>
        /// <returns></returns>
        public static DynamicParameters GetParametersFromObject(object obj, string[] propertyNamesToIgnore)
        {
            if (propertyNamesToIgnore == null) propertyNamesToIgnore = new string[] { String.Empty };
            DynamicParameters p = new DynamicParameters();
            PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in properties)
            {
                if (!propertyNamesToIgnore.Contains(prop.Name))
                    p.Add("@" + prop.Name, prop.GetValue(obj, null));
            }
            return p;
        }

        public static void SetIdentity<T>(IDbConnection connection, Action<T> setId)
        {
            dynamic identity = connection.Query("SELECT @@IDENTITY AS Id").Single();
            T newId = (T)identity.Id;
            setId(newId);
        }


        public static object GetPropertyValue(object target, string propertyName)
        {
            PropertyInfo[] properties = target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            object theValue = null;
            foreach (PropertyInfo prop in properties)
            {
                if (String.Compare(prop.Name, propertyName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    theValue = prop.GetValue(target, null);
                }
            }
            return theValue;
        }

        public static void SetPropertyValue(object p, string propName, object value)
        {
            Type t = p.GetType();
            PropertyInfo info = t.GetProperty(propName);
            if (info == null)
                return;
            if (!info.CanWrite)
                return;
            info.SetValue(p, value, null);
        }

        /// <summary>
        /// Stored proc.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procname">The procname.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        public static List<T> StoredProcWithParams<T>(string procname, dynamic parms, string connectionName = null)
        {
            using (var connection = GetOpenConnection(connectionName))
            {
                return connection.Query<T>(procname, (object)parms, commandType: CommandType.StoredProcedure).ToList();
            }

        }


        /// <summary>
        /// Stored proc with params returning dynamic.
        /// </summary>
        /// <param name="procname">The procname.</param>
        /// <param name="parms">The parms.</param>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        public static List<dynamic> StoredProcWithParamsDynamic(string procname, dynamic parms, string connectionName = null)
        {
            using (var connection = GetOpenConnection(connectionName))
            {
                return connection.Query(procname, (object)parms, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        /// <summary>
        /// Stored proc insert with ID.
        /// </summary>
        /// <typeparam name="T">The type of object</typeparam>
        /// <typeparam name="U">The Type of the ID</typeparam>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="parms">instance of DynamicParameters class. Thisshould include a defined output parameter</param>
        /// <returns>U - the @@Identity value from output parameter</returns>
        public static U StoredProcInsertWithID<T, U>(string procName, DynamicParameters parms, string connectionName = null)
        {
            using (var connection = GetOpenConnection(connectionName))
            {
                var x = connection.Execute(procName, (object)parms, commandType: CommandType.StoredProcedure);
                return parms.Get<U>("@ID");
            }
        }


        /// <summary>
        /// SQL with params.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        public static List<T> SqlWithParams<T>(string sql, dynamic parms, string connectionnName = null)
        {
            using (var connection = GetOpenConnection(connectionnName))
            {
                return connection.Query<T>(sql, (object)parms).ToList();
            }
        }

        /// <summary>
        /// SQL with params.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        public static T SqlScalarWithParams<T>(string sql, object parms, string connectionnName = null)
        {
            using (var connection = GetOpenConnection(connectionnName))
            {
                return connection.ExecuteScalar<T>(sql, parms);
            }
        }

        /// <summary>
        /// SQL with params.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        public static T SqlScalarWithParams4Dic<T>(string sql, IDictionary<string, string> parms, string connectionnName = null)
        {
            var dynamicParams = new DynamicParameters();
            if (parms != null && parms.Count > 0)
            {
                foreach (var keyValuePair in parms)
                {
                    dynamicParams.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
            using (var connection = GetOpenConnection(connectionnName))
            {
                return connection.ExecuteScalar<T>(sql, dynamicParams);
            }
        }

        /// <summary>
        /// Insert update or delete SQL.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        public static int InsertUpdateOrDeleteSql(string sql, dynamic parms, string connectionName = null)
        {
            using (var connection = GetOpenConnection(connectionName))
            {
               return connection.Execute(sql, (object)parms);
            }
        }

        /// <summary>
        /// Insert update or delete SQL.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        public static int InsertUpdateOrDeleteSql4Dic(string sql, IDictionary<string, string> parms, string connectionName = null)
        {
            var dynamicParams = new DynamicParameters();
            if (parms != null && parms.Count > 0)
            {
                foreach (var keyValuePair in parms)
                {
                    dynamicParams.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
            using (var connection = GetOpenConnection(connectionName))
            {
                return connection.Execute(sql, dynamicParams);
            }
        }

        /// <summary>
        /// Insert update or delete stored proc.
        /// </summary>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        public static int InsertUpdateOrDeleteStoredProc(string procName, dynamic parms, string connectionName = null)
        {
            using (var connection = GetOpenConnection(connectionName))
            {
                return connection.Execute(procName, (object)parms, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// SQLs the with params single.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="parms">The parms.</param>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        public static T SqlWithParamsSingle<T>(string sql, dynamic parms, string connectionName = null)
        {
            using (var connection = GetOpenConnection(connectionName))
            {
                return connection.Query<T>(sql, (object)parms).FirstOrDefault();
            }
        }

        /// <summary>
        ///  proc with params single returning Dynamic object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="parms">The parms.</param>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        public static System.Dynamic.DynamicObject DynamicProcWithParamsSingle<T>(string sql, dynamic parms, string connectionName = null)
        {
            using (var connection = GetOpenConnection(connectionName))
            {
                return connection.Query(sql, (object)parms, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        /// <summary>
        /// proc with params returning Dynamic.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="parms">The parms.</param>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        public static IEnumerable<dynamic> DynamicProcWithParams<T>(string sql, dynamic parms, string connectionName = null)
        {
            using (var connection = GetOpenConnection(connectionName))
            {
                return connection.Query(sql, (object)parms, commandType: CommandType.StoredProcedure);
            }
        }


        /// <summary>
        /// Stored proc with params returning single.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procname">The procname.</param>
        /// <param name="parms">The parms.</param>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        public static T StoredProcWithParamsSingle<T>(string procname, dynamic parms, string connectionName = null)
        {
            using (var connection = GetOpenConnection(connectionName))
            {
                return connection.Query<T>(procname, (object)parms, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }
        }
    }
}
