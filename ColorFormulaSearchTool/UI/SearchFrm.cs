using System;
using System.Data;
using System.Windows.Forms;

namespace ColorFormulaSearchTool.UI
{
    public partial class SearchFrm : Form
    {
        #region 返回
        private string _sdt;          //开始日期
        private string _edt;          //结束日期
        private string _brandname;    //品牌
        private string _colorantcode; //色母编码
        #endregion

        #region Get
        /// <summary>
        /// 开始日期
        /// </summary>
        public string Sdt=> _sdt;
        /// <summary>
        /// 结束日期
        /// </summary>
        public string Edt => _edt;
        /// <summary>
        /// 品牌
        /// </summary>
        public string Brandname => _brandname;
        /// <summary>
        /// 色母编码
        /// </summary>
        public string ColorantCode => _colorantcode;
        #endregion

        public SearchFrm()
        {
            InitializeComponent();
            OnRegisterEvents();
        }

        private void OnRegisterEvents()
        {
            btnsearch.Click += Btnsearch_Click;
            tmclose.Click += Tmclose_Click;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btnsearch_Click(object sender, EventArgs e)
        {
            _sdt = dtsdt.Value.ToString("yyyy-MM-dd");
            _edt = dtedt.Value.ToString("yyy-MM-dd");
            _brandname = txtbrandname.Text;
            _colorantcode = txtcolorant.Text;
            //关闭窗体
            this.Close();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tmclose_Click(object sender, EventArgs e)
        {
            _sdt = "";
            _edt = "";
            _brandname = "";
            _colorantcode = "";
            this.Close();
        }
    }
}
