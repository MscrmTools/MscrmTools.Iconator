using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using MsCrmTools.Iconator.AppCode;
using MsCrmTools.Iconator.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MsCrmTools.Iconator
{
    public partial class IconOptimizer : Form
    {
        List<Entity> webResources;
        IOrganizationService service;
        public IconOptimizer(List<Entity> webResources, IOrganizationService service)
        {
            InitializeComponent();

            this.webResources = webResources;
            this.service = service;
            loadControls();
        }

        private void loadControls()
        {
            foreach (var webResource in webResources) {
                try
                {
                    var ctrl = new ImageOptimizerControl(webResource)
                    {
                        Dock = DockStyle.Top
                    };

                    pnlImages.Controls.Add(ctrl);
                }
                catch(Exception error)
                {

                }
            }
        }

        private void btnChangeBackgroundColor_Click(object sender, EventArgs e)
        {
            if(colorDialog1.ShowDialog(this)== DialogResult.OK)
            {
                foreach(var ctrl in pnlImages.Controls)
                {
                    ((ImageOptimizerControl)(ctrl)).SetPanelBackgroundImage(colorDialog1.Color);
                }
            }
        }

        private void btnChangeIconColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                foreach (var ctrl in pnlImages.Controls)
                {
                    ((ImageOptimizerControl)ctrl).SetIconBackgroundImage(colorDialog1.Color);
                }
            }
        }

        private void pnlImages_Resize(object sender, EventArgs e)
        {
            AutoScrollMinSize = new Size(0, pnlImages.Height);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string backupPath = null;
            var result = MessageBox.Show(this, "Do you want to backup these images before updating them?", "Question", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if(result == DialogResult.Cancel)
            {
                return;
            }
            else if (result == DialogResult.Yes)
            {
                var ofd = new FolderBrowserDialog
                {
                    Description = "Location where to save images",
                    ShowNewFolderButton = true,
                };
                if(ofd.ShowDialog(this) == DialogResult.OK)
                {
                    backupPath = ofd.SelectedPath;
                }
                else
                {
                    return;
                }
            }

            tssLabel.Visible = true;

            var bw = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };
            bw.DoWork += (worker, evt) => {
                var list = new List<Entity>();
                var ctrls = (Control.ControlCollection)evt.Argument;
                int index = 0;
                foreach (var ctrl in ctrls)
                {
                    index++;
                    var ioCtrl = (ImageOptimizerControl)ctrl;
                    var name = ioCtrl.WebResource.GetAttributeValue<string>("name");
                    if (ioCtrl.Checked)
                    {
                        ((BackgroundWorker)worker).ReportProgress(index * 100 / ctrls.Count, "Processing image " + name);

                        if (!string.IsNullOrEmpty(backupPath))
                        {
                            var filePath = Path.Combine(backupPath, name.Replace("/", "\\"));
                            var directory = Path.GetDirectoryName(filePath);
                            if (!Directory.Exists(directory))
                            {
                                Directory.CreateDirectory(directory);
                            }

                            ioCtrl.OriginalImage.Save(filePath, ImageFormat.Png);
                        }

                        using (MemoryStream ms = new MemoryStream())
                        {
                            // Convert Image to byte[]
                            ioCtrl.OptimizedImage.Save(ms, ImageFormat.Png);
                            ioCtrl.WebResource["content"] = Convert.ToBase64String(ms.ToArray());
                            service.Update(ioCtrl.WebResource);
                            list.Add(ioCtrl.WebResource);
                        }
                    }
                }

                ((BackgroundWorker)worker).ReportProgress(100, "Publishing images...");

                PublishWebResources(list);
            };
            bw.RunWorkerCompleted += (worker, evt) => {
                tssLabel.Visible = false;
                if (evt.Error != null)
                {
                    MessageBox.Show(this, "An error occured when updating web resource(s): " + evt.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(this, "Update done!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
            };
            bw.ProgressChanged += (worker, evt) => {
                tssLabel.Text = evt.UserState.ToString();
            };
            bw.RunWorkerAsync(pnlImages.Controls);
        }

        private void PublishWebResources(List<Entity> resources)
        {
            string idsXml = string.Empty;

            foreach (var resource in resources)
            {
                idsXml += string.Format("<webresource>{0}</webresource>", resource.Id.ToString("B"));
            }

            var pxReq1 = new PublishXmlRequest
            {
                ParameterXml = string.Format("<importexportxml><webresources>{0}</webresources></importexportxml>", idsXml)
            };

            service.Execute(pxReq1);
        }
    }
}
