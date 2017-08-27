using System.Windows.Forms;

namespace PullAndBuildAll
{
    public partial class LogForm : Form
    {

        public string Log
        {
            get => logTextBox.Text;
            set => logTextBox.Text = value;
        }

        public LogForm()
        {
            InitializeComponent();
        }
    }
}
