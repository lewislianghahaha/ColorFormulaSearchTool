using System;
using System.Data;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace ColorFormulaSearchTool.Task
{
    //导出
    public class Export
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileAddress"></param>
        /// <param name="sourcedt"></param>
        /// <param name="typeid">导入类型;0:配方点击率查询报表 1:配方单价运算报表</param>
        /// <returns></returns>
        public bool ExportDtToExcel(string fileAddress, DataTable sourcedt,int typeid)
        {
            var result = true;
            var sheetcount = 0; //记录所需的sheet页总数
            var rownum = 1;

            try
            {
                //声明一个WorkBook
                var xssfWorkbook = new XSSFWorkbook();

                //执行sheet页(注:1)先列表temp行数判断需拆分多少个sheet表进行填充; 以一个sheet表有100W行记录填充为基准)
                sheetcount = sourcedt.Rows.Count % 1000000 == 0 ? sourcedt.Rows.Count / 1000000 : sourcedt.Rows.Count / 1000000 + 1;

                //i为EXCEL的Sheet页数ID
                for (var i = 0; i <= sheetcount; i++)
                {
                    //创建sheet页
                    var sheet = xssfWorkbook.CreateSheet("Sheet" + i);
                    //创建"标题行"
                    var row = sheet.CreateRow(0);

                    //创建sheet页各列标题
                    for (var j = 0; j < sourcedt.Columns.Count; j++)
                    {
                        //设置列宽度
                        sheet.SetColumnWidth(j, (int)((20 + 0.72) * 256));
                        //设置列名称
                        var colname = sourcedt.Columns[j].ColumnName;
                        row.CreateCell(j).SetCellValue(colname);
                    }

                    //计算进行循环的起始行
                    var startrow = (i - 1) * 1000000;
                    //计算进行循环的结束行
                    var endrow = i == sheetcount ? sourcedt.Rows.Count : i * 1000000;

                    //每一个sheet表显示100000行  
                    for (var r = startrow; r < endrow; r++)
                    {
                        //创建行
                        row = sheet.CreateRow(rownum);
                        //循环获取DT内的列值记录
                        for (var k = 0; k < sourcedt.Columns.Count; k++)
                        {
                            if (Convert.ToString(sourcedt.Rows[r][k]) == "") continue;
                            else
                            {
                                //在前两位设置为字符串类型,后面的都是设置小数类型
                                //(注:要注意值小数位数保留两位;当超出三位小数的时候,会出现OutofMemory异常.)

                                var a=sourcedt.Rows[r][k].GetType();   //todo:检测数据列的数据类型

                                //配方点击率查询报表
                                switch (typeid)
                                {
                                    case 0:
                                        if (k == 8)
                                        {
                                            row.CreateCell(k, CellType.Numeric).SetCellValue(Convert.ToInt32(sourcedt.Rows[r][k]));
                                        }
                                        else
                                        {
                                            row.CreateCell(k, CellType.String).SetCellValue(Convert.ToString(sourcedt.Rows[r][k]));
                                        }
                                        break;
                                    default:
                                        if (k == 5 || k == 6 || k == 7)
                                        {
                                            row.CreateCell(k, CellType.Numeric).SetCellValue(Math.Round(Convert.ToDouble(sourcedt.Rows[r][k]), 2));
                                        }
                                        else
                                        {
                                            row.CreateCell(k, CellType.String).SetCellValue(Convert.ToString(sourcedt.Rows[r][k]));
                                        }
                                        break;
                                }
                            }
                        }
                        rownum++;
                    }
                    //当一个Sheet页填充完毕后,需将变量初始化
                    rownum = 1;
                }

                //写入数据
                var file = new FileStream(fileAddress, FileMode.Create);
                xssfWorkbook.Write(file);
                file.Close();           //关闭文件流
                xssfWorkbook.Close();   //关闭工作簿
                file.Dispose();         //释放文件流
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
    }
}
