using MsCrmTools.Iconator.AppCode;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace MsCrmTools.Iconator.Forms
{
    public partial class SettingsForm : DockContent
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        public Settings Settings
        {
            get
            {
                return (Settings)pgSettngs?.SelectedObject;
            }
            set
            {
                pgSettngs.SelectedObject = value;
            }
        }

        private void SettingsForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.ApplicationExitCall
            || e.CloseReason == CloseReason.FormOwnerClosing
            || e.CloseReason == CloseReason.MdiFormClosing
            || e.CloseReason == CloseReason.TaskManagerClosing
            || e.CloseReason == CloseReason.WindowsShutDown)
            {
                return;
            }

            e.Cancel = true;
        }
    }
}