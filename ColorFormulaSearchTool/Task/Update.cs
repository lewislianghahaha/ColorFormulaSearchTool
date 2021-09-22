using ColorFormulaSearchTool.DB;

namespace ColorFormulaSearchTool.Task
{
    //更新
    public class Update
    {
        ConDb conDb = new ConDb();
        SqlList sqlList = new SqlList();

        private string _sqlscript = string.Empty;

        /// <summary>
        /// 更新色母单价
        /// </summary>
        /// <returns></returns>
        public bool UpdateColorantPrice()
        {
            _sqlscript = sqlList.Update_ColorantPrice("", 0);
            return conDb.UpAndDelInfo(0, _sqlscript);
        }
    }
}
