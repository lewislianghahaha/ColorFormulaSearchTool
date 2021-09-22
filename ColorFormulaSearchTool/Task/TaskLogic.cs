using System.Data;

namespace ColorFormulaSearchTool.Task
{
    //任务分布(中转站)
    public class TaskLogic
    {
        Search search = new Search();      //查询
        Import import=new Import();        //导入
        Export export=new Export();        //导出
        Update update=new Update();        //更新
        Generate generate=new Generate();  //生成

        #region 变量定义

        #region 配方点击率查询报表-各参数使用
        private string _sdt;                    //开始日期
        private string _edt;                    //结束日期
        private string _brandname;              //品牌名称
        private string _colorantcode;           //色母编码
        #endregion

        #region Excel导入
        private string _fileAddress;             //文件地址
        #endregion

        #region 色母单价列表(配方单价运算报表使用)
        private DataTable _colorantPriceList;   //色母单价列表
        #endregion

        #region 返回变量
        private DataTable _resultTable;          //返回DT类型
        private bool _resultMark;                //返回是否成功标记
        #endregion

        #region 导出EXCEL
        private DataTable _exportdt;             //获取Dt记录集(用于导出至EXCEL)
        private int _typeid;                     //导入类型;0:配方点击率查询报表 1:配方单价运算报表
        #endregion


        #endregion

        #region Set

        /// <summary>
        /// 开始日期
        /// </summary>
        public string Sdt { set { _sdt = value; } }
        /// <summary>
        /// 结束日期
        /// </summary>
        public string Edt { set { _edt = value; } }
        /// <summary>
        /// 品牌名称
        /// </summary>
        public string BrandName { set { _brandname = value; } }
        /// <summary>
        /// 色母编码
        /// </summary>
        public string ColorantCode { set { _colorantcode = value; } }
        /// <summary>
        /// 文件地址
        /// </summary>
        public string FileAddress { set { _fileAddress = value; } }
        /// <summary>
        /// 色母单价列表
        /// </summary>
        public DataTable ColorantPriceList { set { _colorantPriceList = value; } }
        /// <summary>
        /// 获取Dt记录集(用于导出至EXCEL)
        /// </summary>
        public DataTable Exportdt { set { _exportdt = value; } }
        /// <summary>
        /// 导入类型;0:配方点击率查询报表 1:配方单价运算报表
        /// </summary>
        public int Typeid { set { _typeid = value; } }
        #endregion

        #region Get
        /// <summary>
        ///返回DataTable至主窗体
        /// </summary>
        public DataTable ResultTable => _resultTable;

        /// <summary>
        /// 返回结果标记
        /// </summary>
        public bool ResultMark => _resultMark;
        #endregion

        /// <summary>
        /// 初始化‘色母单价列表’
        /// </summary>
        public void InitializeSearchColorantPrice()
        {
            //若_resultTable有值,即先将其清空,再进行赋值
            if (_resultTable?.Rows.Count > 0)
            {
                _resultTable.Rows.Clear();
                _resultTable.Columns.Clear();
            }
            _resultTable = search.SearchColorantPriceList();
        }

        /// <summary>
        /// 配方点击率查询报表
        /// </summary>
        public void SearchColorCodeClick()
        {
            //若_resultTable有值,即先将其清空,再进行赋值
            if (_resultTable?.Rows.Count > 0)
            {
                _resultTable.Rows.Clear();
                _resultTable.Columns.Clear();
            }
            _resultTable = search.SearchColorCodeClick(_sdt,_edt,_brandname,_colorantcode);
        }

        /// <summary>
        /// 配方单价运算报表
        /// </summary>
        public void GenerateColorantPrice()
        {
            //若_resultTable有值,即先将其清空,再进行赋值
            if (_resultTable?.Rows.Count > 0)
            {
                _resultTable.Rows.Clear();
                _resultTable.Columns.Clear();
            }
            _resultTable = generate.GenerateColorantPrice(_fileAddress,_colorantPriceList);
        }

        /// <summary>
        /// 导出
        /// </summary>
        public void ExportDtToExcel()
        {
            _resultMark = export.ExportDtToExcel(_fileAddress, _exportdt, _typeid);
        }
    }
}
