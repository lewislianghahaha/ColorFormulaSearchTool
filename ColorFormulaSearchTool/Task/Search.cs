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
            var generate = new Generate();
            _sqlscript = sqlList.Get_SearchColorCodeReport(sdt,edt,brandname,colorcode);
            //将得出的结果放到临时表内进行数据类型转换
            var dt = generate.GenerateColorcodeClickPoint(conDb.UseSqlSearchInfo(1, _sqlscript)).Copy();
            return dt;
        }

        /// <summary>
        /// 获取‘色母单价列表’
        /// 作用:1)获取初始化‘色母单价’列表 2)更新(插入)后刷新使用
        /// </summary>
        /// <returns></returns>
        public DataTable SearchColorantPriceList()
        {
            _sqlscript = sqlList.Get_SearchColorantPrice(null,null,null,2);
            return conDb.UseSqlSearchInfo(0, _sqlscript);
        }

        /// <summary>
        /// 色母单价列表窗体查询使用
        /// </summary>
        /// <param name="brandname">品牌</param>
        /// <param name="creatsedt">开始日期</param>
        /// <param name="createedt">结束日期</param>
        /// <param name="typeid">选择类型(决定是使用‘创建日期’或‘修改日期’进行查询) 0:创建日期 1:修改日期</param>
        /// <returns></returns>
        public DataTable SearchColorantPrice(string brandname, string creatsedt, string createedt, int typeid)
        {
            _sqlscript = sqlList.Get_SearchColorantPrice(brandname, creatsedt, createedt, typeid);
            return conDb.UseSqlSearchInfo(0, _sqlscript);
        }
    }
}
