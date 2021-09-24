using System;
using System.Data;

namespace ColorFormulaSearchTool.DB
{
    //临时表(注:包含导入模板 及 导出模板)
    public class TempDt
    {
        /// <summary>
        /// 导入模板-配方单价运算报表使用
        /// </summary>
        /// <returns></returns>
        public DataTable ImportTempdt()
        {
            var dt = new DataTable();

            for (var i = 0; i < 6; i++)
            {
                var dc = new DataColumn();

                switch (i)
                {
                    //颜色描述
                    case 0:
                        dc.ColumnName = "颜色描述";
                        dc.DataType=Type.GetType("System.String");
                        break;
                    //内部色号
                    case 1:
                        dc.ColumnName = "内部色号";
                        dc.DataType=Type.GetType("System.String");
                        break;
                    //版本日期
                    case 2:
                        dc.ColumnName = "版本日期";
                        dc.DataType = Type.GetType("System.DateTime"); 
                        break;
                    //色母编码
                    case 3:
                        dc.ColumnName = "色母编码";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    //色母名称
                    case 4:
                        dc.ColumnName = "色母名称";
                        dc.DataType=Type.GetType("System.String");
                        break;
                    //色母量(G)
                    case 5:
                        dc.ColumnName = "色母量(G)";
                        dc.DataType=Type.GetType("System.Decimal"); 
                        break;
                }
                dt.Columns.Add(dc);
            }
            return dt;
        }

        /// <summary>
        /// 导入模板-色母单价导入更新使用
        /// </summary>
        /// <returns></returns>
        public DataTable ImportPriceList()
        {
            var dt = new DataTable();

            for (var i = 0; i < 2; i++)
            {
                var dc = new DataColumn();

                switch (i)
                {
                    //色母编码
                    case 0:
                        dc.ColumnName = "色母编码";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    //色母单价
                    case 1:
                        dc.ColumnName = "色母单价";
                        dc.DataType = Type.GetType("System.Decimal");
                        break;
                }
                dt.Columns.Add(dc);
            }
            return dt;
        }

        /// <summary>
        /// 色母单价临时表(插入使用)
        /// </summary>
        /// <returns></returns>
        public DataTable ColorantPriceInsertTempDt()
        {
            var dt=new DataTable();

            for (var i = 0; i < 4; i++)
            {
                var dc = new DataColumn();

                switch (i)
                {
                    //ColorCode
                    case 0:
                        dc.ColumnName = "ColorCode";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    //Price
                    case 1:
                        dc.ColumnName = "Price";
                        dc.DataType = Type.GetType("System.Decimal");
                        break;
                    //CreateDate
                    case 2:
                        dc.ColumnName = "CreateDate";
                        dc.DataType = Type.GetType("System.DateTime"); 
                        break;
                    //ChangeDate
                    case 3:
                        dc.ColumnName = "ChangeDate";
                        dc.DataType = Type.GetType("System.DateTime");
                        break;
                }
            }
            return dt;
        }

        /// <summary>
        /// 色母单价临时表(更新使用)
        /// </summary>
        /// <returns></returns>
        public DataTable ColorantPriceUpTempDt()
        {
            var dt = new DataTable();

            for (var i = 0; i < 5; i++)
            {
                var dc = new DataColumn();

                switch (i)
                {
                    //Pid
                    case 0:
                        dc.ColumnName = "Pid";
                        dc.DataType = Type.GetType("System.Int32"); 
                        break;
                    //ColorCode
                    case 1:
                        dc.ColumnName = "ColorCode";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    //Price
                    case 2:
                        dc.ColumnName = "Price";
                        dc.DataType = Type.GetType("System.Decimal");
                        break;
                    //CreateDate
                    case 3:
                        dc.ColumnName = "CreateDate";
                        dc.DataType = Type.GetType("System.DateTime");
                        break;
                    //ChangeDate
                    case 4:
                        dc.ColumnName = "ChangeDate";
                        dc.DataType = Type.GetType("System.DateTime");
                        break;
                }
            }
            return dt;
        }

        /// <summary>
        /// 导出模板
        /// </summary>
        /// <param name="id">0:导出“配方点击率查询报表” 1:“配方单价运算报表”</param>
        /// <returns></returns>
        public DataTable ExportDt(int id)
        {
            var dt = new DataTable();

            if (id == 0)
            {
                for (var i = 0; i < 9; i++)
                {
                    var dc = new DataColumn();

                    switch (i)
                    {
                        //品牌
                        case 0:
                            dc.ColumnName = "品牌";
                            dc.DataType = Type.GetType("System.String");
                            break;
                        //产品系列
                        case 1:
                            dc.ColumnName = "产品系列";
                            dc.DataType = Type.GetType("System.String");
                            break;
                        //内部色号
                        case 2:
                            dc.ColumnName = "内部色号";
                            dc.DataType = Type.GetType("System.String");
                            break;
                        //颜色名称
                        case 3:
                            dc.ColumnName = "颜色名称";
                            dc.DataType = Type.GetType("System.String");
                            break;
                        //制造商
                        case 4:
                            dc.ColumnName = "制造商";
                            dc.DataType = Type.GetType("System.String");
                            break;
                        //车型
                        case 5:
                            dc.ColumnName = "车型";
                            dc.DataType = Type.GetType("System.String");
                            break;
                        //色号
                        case 6:
                            dc.ColumnName = "色号";
                            dc.DataType = Type.GetType("System.String");
                            break;
                        //版本日期
                        case 7:
                            dc.ColumnName = "版本日期";
                            dc.DataType = Type.GetType("System.DateTime");
                            break;
                        //点击次数
                        case 8:
                            dc.ColumnName = "点击次数";
                            dc.DataType = Type.GetType("System.Int32");
                            break;
                    }
                    dt.Columns.Add(dc);
                }
            }
            else
            {
                for (var i = 0; i < 8; i++)
                {
                    var dc = new DataColumn();

                    switch (i)
                    {
                        //颜色描述
                        case 0:
                            dc.ColumnName = "颜色描述";
                            dc.DataType = Type.GetType("System.String");
                            break;
                        //内部色号
                        case 1:
                            dc.ColumnName = "内部色号";
                            dc.DataType = Type.GetType("System.String");
                            break;
                        //版本日期
                        case 2:
                            dc.ColumnName = "版本日期";
                            dc.DataType = Type.GetType("System.DateTime");
                            break;
                        //色母编码
                        case 3:
                            dc.ColumnName = "色母编码";
                            dc.DataType = Type.GetType("System.String");
                            break;
                        //色母名称
                        case 4:
                            dc.ColumnName = "色母名称";
                            dc.DataType = Type.GetType("System.String");
                            break;
                        //色母量(G)
                        case 5:
                            dc.ColumnName = "色母量(G)";
                            dc.DataType = Type.GetType("System.Decimal");
                            break;
                        //色母单价
                        case 6:
                            dc.ColumnName = "色母单价";
                            dc.DataType = Type.GetType("System.Decimal");
                            break;
                        //配方单价
                        case 7:
                            dc.ColumnName = "配方单价";
                            dc.DataType = Type.GetType("System.Decimal");
                            break;
                    }
                    dt.Columns.Add(dc);
                }
            }
            return dt;
        }
    }
}
