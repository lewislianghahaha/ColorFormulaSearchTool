using System.Windows.Forms;
using ColorFormulaSearchTool.Task;

namespace ColorFormulaSearchTool.UI
{
    public partial class Main : Form
    {
        TaskLogic taskLogic=new TaskLogic();
        Load load=new Load();

        public Main()
        {
            InitializeComponent();
        }


    }
}
