using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Unity.Core.Interface.Service
{
    public interface IBaseService<T> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="procName"></param>
        /// <returns></returns>
        IEnumerable<T> SqlQuery(string procName);

        /// <summary>
        /// Sql Query With Parameter
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        IEnumerable<T> SqlQuery(string procName, SqlParameter[] sqlParams);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        long ExecuteSql(string procName, SqlParameter[] sqlParams);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        void WriteError(string title, string content);
    }
}
