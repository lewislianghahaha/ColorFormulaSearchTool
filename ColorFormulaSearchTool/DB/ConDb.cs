using System;
using System.Data;
using System.Data.SqlClient;

namespace ColorFormulaSearchTool.DB
{
    //数据库连接  查询语句公共方法 及 更新删除公共方法
    public class ConDb
    {
        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <param name="typeid">0:连接K3数据库 1:连接配方数据库</param>
        /// <returns></returns>
        public string GetConnString(int typeid)
        {
            var conn = new Conn();
            return conn.GetConnectionString(typeid);
        }

        /// <summary>
        /// 获取数据连接
        /// </summary>
        /// <param name="id">0:连接K3数据库 1:连接配方数据库</param>
        /// <returns></returns>
        public SqlConnection GetConn(int id)
        {
            var conn = new Conn();
            var sqlcon = new SqlConnection(conn.GetConnectionString(id));
            return sqlcon;
        }

        /// <summary>
        /// 根据SQL语句查询得出对应的DT
        /// </summary>
        /// <param name="type">0:连接K3数据库 1:连接配方数据库</param>
        /// <param name="sqlscript">SQL语句</param>
        /// <returns></returns>
        public DataTable UseSqlSearchInfo(int type,string sqlscript)
        {
            var resultdt = new DataTable();

            try
            {
                var sqlDataAdapter = new SqlDataAdapter(sqlscript,GetConn(type));
                sqlDataAdapter.Fill(resultdt);
            }
            catch (Exception)
            {
                resultdt.Rows.Clear();
                resultdt.Columns.Clear();
            }
            return resultdt;
        }

        /// <summary>
        /// 按照指定的SQL语句执行记录并返回执行结果（true 或 false） 更新 删除时使用
        /// </summary>
        /// <param name="type">0:连接K3数据库 1:连接配方数据库</param>
        /// <param name="sqlscript">SQL语句</param>
        /// <returns></returns>
        public bool UpAndDelInfo(int type,string sqlscript)
        {
            var result = true;

            try
            {
                using (var sql = GetConn(type))
                {
                    sql.Open();
                    var sqlCommand = new SqlCommand(sqlscript,sql);
                    sqlCommand.ExecuteNonQuery();
                    sql.Close();
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
    }
}
