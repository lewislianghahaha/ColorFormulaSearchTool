using System.Data.SqlClient;

namespace ColorFormulaSearchTool.DB
{
    //数据库连接
    public class ConDb
    {
        Conn conn=new Conn();

        /// <summary>
        /// 获取数据连接
        /// </summary>
        /// <param name="id">0:连接K3数据库 1:连接配方数据库</param>
        /// <returns></returns>
        public SqlConnection GetConn(int id)
        {
            var sqlcon=new SqlConnection(conn.GetConnectionString(id));
            return sqlcon;
        }
    }
}
