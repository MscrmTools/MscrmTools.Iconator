using System;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace MsCrmTools.Iconator.Forms
{
    public partial class HelpForm : DockContent
    {
        public HelpForm()
        {
            InitializeComponent();
        }

        private void Help_Load(object sender, EventArgs e)
        {
            rtbHelp.SelectionFont = new Font(rtbHelp.SelectionFont, FontStyle.Bold);
            rtbHelp.SelectedText = "Load Dataverse components\n\n";

            rtbHelp.SelectionFont = new Font(rtbHelp.SelectionFont, FontStyle.Regular);
            rtbHelp.SelectedText = "Click on the \"Load\" button to select one or more solutions that contain tables and web resources to use to add images to tables\n\n";

            rtbHelp.SelectionFont = new Font(rtbHelp.SelectionFont, FontStyle.Bold);
            rtbHelp.SelectedText = "Add/Remove images to/from tables\n\n";

            rtbHelp.SelectionFont = new Font(rtbHelp.SelectionFont, FontStyle.Regular);
            rtbHelp.SelectedText = "To add an image to a table, simply drag and drop the image from the right side to a table on the left side. Then click on the \"Apply\" button.\n\n";
            rtbHelp.SelectedText = "You can add images to multiple tables before clicking on the button \"Apply\".\n\n";

            rtbHelp.SelectionFont = new Font(rtbHelp.SelectionFont, FontStyle.Bold);
            rtbHelp.SelectedText = "Need more images?\n\n";

            rtbHelp.SelectionFont = new Font(rtbHelp.SelectionFont, FontStyle.Regular);
            rtbHelp.SelectedText = "You can add images using the button \"Add Images\" or simply by dropping vector images from your computer drive on the list of images.\n\n";

            rtbHelp.SelectionFont = new Font(rtbHelp.SelectionFont, FontStyle.Bold);
            rtbHelp.SelectedText = "Need to update legacy images on tables?\n\n";

            rtbHelp.SelectionFont = new Font(rtbHelp.SelectionFont, FontStyle.Regular);
            rtbHelp.SelectedText = "You can switch back to the old version of Iconator in the settings. You will then need to close the tool and reopen it.\n\n";
        }

        private void HelpForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
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