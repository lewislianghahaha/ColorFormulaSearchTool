using System;
using System.Data;
using System.Data.SqlClient;
using ColorFormulaSearchTool.DB;

namespace ColorFormulaSearchTool.Task
{
    //运算(配方单价运算报表使用)
    public class Generate
    {
        ConDb conDb = new ConDb();
        SqlList sqlList = new SqlList();
        TempDt tempDt = new TempDt();
        Search search = new Search();
        Import import = new Import();

        /// <summary>
        /// 配方点击率查询报表-将得出的结果转换类型使用
        /// </summary>
        /// <param name="sourcedt"></param>
        /// <returns></returns>
        public DataTable GenerateColorcodeClickPoint(DataTable sourcedt)
        {
            var a = sourcedt;
            var dt = tempDt.ExportDt(0);
            //批量数据移植
            for (var i = 0; i < sourcedt.Rows.Count; i++)
            {
               
                var newrow = dt.NewRow();
                for (var j = 0; j < sourcedt.Columns.Count; j++)
                {
                    var a1 = sourcedt.Rows[i][j];
                    dt.Rows[i][j] = sourcedt.Rows[i][j];
                }
                dt.Rows.Add(newrow);
            }
            return dt;
        }

        /// <summary>
        /// 配方单价运算报表
        /// </summary>
        /// <param name="fileAdd">导入EXCEL地址</param>
        /// <param name="colorantPriceList">色母单价列表(初始化值)</param>
        /// <returns></returns>
        public DataTable GenerateColorantPrice(string fileAdd,DataTable colorantPriceList)
        {
            var dt = new DataTable();
            //色母编码(中间变量)
            var oldcolorcode = string.Empty;
            var newcolorcode = string.Empty;

            try
            {
                //根据EXCEL地址获取相关EXCEL内容
                var exceltemp = import.ImportExcelToDt(0, fileAdd).Copy();
                //获取‘配方单价运算报表’导出模板(注:用于最后的导出使用)
                dt = tempDt.ExportDt(1);
                //循环EXCEL导入内容,(注:1)内部色号之间用‘空行’分隔 2)配方单价=色母量*色母单价/1000)
                foreach (DataRow rows in exceltemp.Rows)
                {
                    //获取当前行的内部色号
                    newcolorcode = Convert.ToString(rows[1]);
                    //获取当前行的色编码
                    var colorantcode = Convert.ToString(rows[3]);
                    //根据当前行的‘内部色号’获取对应的‘色母单价’
                    var colorantprice = GenerateColorantPrice(colorantPriceList, colorantcode);

                    if (oldcolorcode == "" || newcolorcode == oldcolorcode)
                    {
                        dt.Merge(InsertRecord(dt,rows,colorantprice));
                        //将新变量的‘内部色号’赋值给旧变量
                        oldcolorcode = newcolorcode;
                    }
                    else
                    {
                        //插入空行
                        dt.Merge(InsertEmptyRow(dt));
                        //将新记录进行合拼插入
                        dt.Merge(InsertRecord(dt,rows,colorantprice));                 
                        //将新变量的‘内部色号’赋值给旧变量
                        oldcolorcode = newcolorcode;
                    }
                }
            }
            catch (Exception)
            {
                dt.Columns.Clear();
                dt.Rows.Clear();
            }
            return dt;
        }

        /// <summary>
        /// 插入记录
        /// </summary>
        /// <param name="dt">导出临时表</param>
        /// <param name="rows">插入记录行</param>
        /// <param name="colorantprice">色母单价</param>
        /// <returns></returns>
        private DataTable InsertRecord(DataTable dt, DataRow rows, decimal colorantprice)
        {
            var newrow = dt.NewRow();
            newrow[0] = rows[0];                                            //颜色描述
            newrow[1] = rows[1];                                            //内部色号
            newrow[2] = rows[2];                                            //版本日期
            newrow[3] = rows[3];                                            //色母编码
            newrow[4] = rows[4];                                            //色母名称
            newrow[5] = rows[5];                                            //色母量(G)
            newrow[6] = colorantprice;                                      //色母单价(从‘色母单价列表’获取)
            newrow[7] = Convert.ToDecimal(rows[5]) * colorantprice / 1000;  //配方单价(计算)
            dt.Rows.Add(newrow);
            return dt;
        }

        /// <summary>
        /// 插入空白行
        /// </summary>
        /// <param name="sourcedt"></param>
        /// <returns></returns>
        private DataTable InsertEmptyRow(DataTable sourcedt)
        {
            var newrow = sourcedt.NewRow();
            for (var i = 0; i < sourcedt.Columns.Count; i++)
            {
                newrow[i] = DBNull.Value;
            }
            sourcedt.Rows.Add(newrow);
            return sourcedt;
        }

        /// <summary>
        /// 获取色母单价
        /// </summary>
        /// <param name="colorantpricelist">色母单价列表</param>
        /// <param name="colorantcode">色母编码</param>
        /// <returns></returns>
        private decimal GenerateColorantPrice(DataTable colorantpricelist,string colorantcode)
        {
            decimal result = 0;
            //以colorcode为条件,找出对应的‘色母单价’
            var dtlrows = colorantpricelist.Select("ColorantCode='" + colorantcode + "'");
            if (dtlrows.Length > 0)
            {
                for (var i = 0; i < dtlrows.Length; i++)
                {
                    result += Convert.ToDecimal(dtlrows[i][2]);
                }
            }
            else
            {
                result = 0;
            }
            return result;
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
                //获取‘色母单价’模板(插入及更新使用)
                var inserttemp = tempDt.ColorantPriceInsertTempDt();
                var uptemp = tempDt.ColorantPriceUpTempDt();

                //使用colorantPriceList数据表进行查找,若有记录,就进行更新,没有就进行插入
                foreach (DataRow rows in exceltemp.Rows)
                {
                    var dtrows = colorantPriceList.Select("ColorantCode='" + rows[0]+"'");
                    //若dtrows有值,就执行更新,反之,执行插入
                    if (dtrows.Length > 0)
                    {
                        for (var i = 0; i < dtrows.Length; i++)
                        {
                            var newrow = uptemp.NewRow();
                            newrow[0] = dtrows[i][0];      //Pid
                            newrow[1] = dtrows[i][1];      //ColorCode
                            newrow[2] = rows[1];           //Price
                            newrow[3] = dtrows[i][3];      //CreateDate
                            newrow[4] = DateTime.Now;      //ChangeDate
                            uptemp.Rows.Add(newrow);
                        }
                    }
                    else
                    {
                        var newrow = inserttemp.NewRow();
                        newrow[0] = rows[0];                           //ColorCode
                        newrow[1] = rows[1];                           //Price
                        newrow[2] = DateTime.Now;                      //CreateDate
                        newrow[3] = Convert.ToDateTime(DBNull.Value);  //ChangeDate
                        inserttemp.Rows.Add(newrow);
                    }
                }
                //最后将得出的结果进行插入或更新
                if (inserttemp.Rows.Count > 0)
                    ImportDtToDb("T_BD_ColorantPrice", inserttemp);
                if(uptemp.Rows.Count>0)
                    UpDbFromDt("T_BD_ColorantPrice", uptemp);
                //最后重新读取T_BD_ColorantPrice获取最新的记录集,并赋值至dt内
                dt = search.SearchColorantPriceList().Copy();
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
                    da.UpdateCommand.Parameters.Add("@ColorantCode", SqlDbType.NVarChar,200, "ColorantCode");
                    da.UpdateCommand.Parameters.Add("@Price", SqlDbType.Decimal, 4, "Price");
                    da.UpdateCommand.Parameters.Add("@CreateDate", SqlDbType.DateTime, 10, "CreateDate");
                    da.UpdateCommand.Parameters.Add("@ChangeDate", SqlDbType.DateTime, 10, "ChangeDate");
                    break;
            }
            return da;
        }
    }
}
