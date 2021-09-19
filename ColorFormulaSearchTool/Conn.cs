using System.Configuration;

namespace ColorFormulaSearchTool
{
    public class Conn
    {
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="id">0:连接K3数据库 1:连接配方数据库</param>
        /// <returns></returns>
        public string GetConnectionString(int id)
        {
            //读取App.Config配置文件中的Connstring节点
            var pubs = id == 0 ? ConfigurationManager.ConnectionStrings["Connstring"] : ConfigurationManager.ConnectionStrings["ConnstringFormula"];
                        
            var strcon = pubs.ConnectionString;
            return strcon;
        }
    }
}
