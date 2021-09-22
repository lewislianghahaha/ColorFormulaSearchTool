using System.Data;
using ColorFormulaSearchTool.DB;

namespace ColorFormulaSearchTool.Task
{
    //查询
    public class Search
    {
        ConDb conDb=new ConDb();
        SqlList sqlList=new SqlList();

        private string _sqlscript = string.Empty;

        /// <summary>
        /// 配方点击率查询报表使用
        /// </summary>
        /// <param name="sdt">开始日期</param>
        /// <param name="edt">结束日期</param>
        /// <param name="brandname">品牌</param>
        /// <param name="colorcode">色母编码</param>
        /// <returns></returns>
        public DataTable SearchColorCodeClick(string sdt, string edt, string brandname, string colorcode)
        {
            _sqlscript = sqlList.Get_SearchColorCodeReport(sdt,edt,brandname,colorcode);
            return conDb.UseSqlSearchInfo(1, _sqlscript);
        }

        /// <summary>
        /// 配方单价运算报表使用-从数据库获取‘色母单价列表’
        /// </summary>
        /// <returns></returns>
        public DataTable SearchColorantPriceList()
        {
            _sqlscript = sqlList.Get_SearchColorantPrice(null,null,null,null,null);
            return conDb.UseSqlSearchInfo(0, _sqlscript);
        }
    }
}
