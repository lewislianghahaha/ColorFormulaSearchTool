using System;
using System.Data;
using System.Data.SqlClient;
using ColorFormulaSearchTool.DB;

namespace ColorFormulaSearchTool.Task
{
    //运算(配方单价运算报表使用)
    public class Generate
    {
        Import import=new Import();
        ConDb conDb=new ConDb();
        SqlList sqlList=new SqlList();

        /// <summary>
        /// 配方单价运算报表
        /// </summary>
        /// <param name="fileAdd">导入EXCEL地址</param>
        /// <param name="colorantPriceList">色母单价列表(初始化值)</param>
        /// <returns></returns>
        public DataTable GenerateColorantPrice(string fileAdd,DataTable colorantPriceList)
        {
            var dt = new DataTable();

            try
            {
                //根据EXCEL地址获取相关EXCEL内容
                var exceltemp = import.ImportExcelToDt(0, fileAdd).Copy();
                //

            }
            catch (Exception)
            {
                dt.Columns.Clear();
                dt.Rows.Clear();
            }
            return dt;
        }

        /// <summary>
        /// 更新‘色母单价’列表
        /// </summary>
        /// <param name="fileAdd">导入EXCEL地址</param>
        /// <param name="colorantPriceList">色母单价列表(初始化值)</param>
        /// <returns></returns>
        public DataTable UpColorantPriceList(string fileAdd, DataTable colorantPriceList)
        {
            var dt = new DataTable();

            try
            {
                //根据EXCEL地址获取相关EXCEL内容
                var exceltemp = import.ImportExcelToDt(1, fileAdd).Copy();
                //

            }
            catch (Exception)
            {
                dt.Columns.Clear();
                dt.Rows.Clear();
            }
            return dt;
        }

        /// <summary>
        /// 针对指定表进行数据插入
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dt"></param>
        private void ImportDtToDb(string tableName, DataTable dt)
        {
            //注:1)要插入的DataTable内的字段数据类型必须要与数据库内的一致;并且要按数据表内的字段顺序 2)SqlBulkCopy类只提供将数据写入到数据库内
            using (var sqlBulkCopy = new SqlBulkCopy(conDb.GetConnString(0)))
            {
                sqlBulkCopy.BatchSize = 1000;                    //表示以1000行 为一个批次进行插入
                sqlBulkCopy.DestinationTableName = tableName;  //数据库中对应的表名
                sqlBulkCopy.NotifyAfter = dt.Rows.Count;      //赋值DataTable的行数
                sqlBulkCopy.WriteToServer(dt);               //数据导入数据库
                sqlBulkCopy.Close();                        //关闭连接 
            }
        }

        /// <summary>
        /// 根据指定条件对指定数据表进行批量更新
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="dt"></param>
        private void UpDbFromDt(string tablename,DataTable dt)
        {
            var sqladpter = new SqlDataAdapter();
            var ds = new DataSet();

            //根据表格名称获取对应的模板表记录
            var searList = sqlList.SearchUpdateTable(tablename);

            using (sqladpter.SelectCommand = new SqlCommand(searList,conDb.GetConn(0)))
            {
                //将查询的记录填充至ds(查询表记录;后面的更新作赋值使用)
                sqladpter.Fill(ds);
                //建立更新模板相关信息(包括更新语句 以及 变量参数)
                sqladpter = GetUpdateAdapter(tablename, conDb.GetConn(0), sqladpter);
                //开始更新(注:通过对DataSet中存在的表进行循环赋值;并进行更新)
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    for (var j = 0; j < dt.Columns.Count; j++)
                    {
                        ds.Tables[0].Rows[0].BeginEdit();
                        ds.Tables[0].Rows[0][j] = dt.Rows[i][j];
                        ds.Tables[0].Rows[0].EndEdit();
                    }
                    sqladpter.Update(ds.Tables[0]);
                }
                //完成更新后将相关内容清空
                ds.Tables[0].Clear();
                sqladpter.Dispose();
                ds.Dispose();
            }
        }

        /// <summary>
        /// 建立更新模板相关信息
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <param name="conn">连接对象</param>
        /// <param name="da">SqlDataAdapter对象</param>
        /// <returns></returns>
        private SqlDataAdapter GetUpdateAdapter(string tablename, SqlConnection conn, SqlDataAdapter da)
        {
            //根据tablename获取对应的更新语句
            var sqlscript = sqlList.UpEntry(tablename);
            da.UpdateCommand = new SqlCommand(sqlscript, conn);

            //定义所需的变量参数
            switch (tablename)
            {
                case "T_BD_ColorantPrice":
                    da.UpdateCommand.Parameters.Add("@PID", SqlDbType.Int, 8, "PID");
                    da.UpdateCommand.Parameters.Add("@ColorCode",SqlDbType.NVarChar,200,"ColorCode");
                    da.UpdateCommand.Parameters.Add("@Price", SqlDbType.Decimal, 4, "Price");
                    da.UpdateCommand.Parameters.Add("@CreateDate", SqlDbType.DateTime, 10, "CreateDate");
                    da.UpdateCommand.Parameters.Add("@ChangeDate", SqlDbType.DateTime, 10, "ChangeDate");
                    break;
            }
            return da;
        }
    }
}
