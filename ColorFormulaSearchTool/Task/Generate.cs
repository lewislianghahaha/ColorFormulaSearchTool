using System;
using System.Data;

namespace ColorFormulaSearchTool.Task
{
    //运算(配方单价运算报表使用)
    public class Generate
    {
        Import import=new Import();

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
    }
}
