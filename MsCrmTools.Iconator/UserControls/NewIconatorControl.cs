using McTools.Xrm.Connection;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using MsCrmTools.Iconator.AppCode;
using MsCrmTools.Iconator.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace MsCrmTools.Iconator.UserControls
{
    public partial class NewIconatorControl : PluginControlBase, IPayPalPlugin, IGitHubPlugin
    {
        private readonly List<TableInfo> tables = new List<TableInfo>();
        private readonly List<VectorImageInfo> vectorImages = new List<VectorImageInfo>();
        private HelpForm helpForm;
        private VectorImagesList imageListForm;
        private Settings settings;
        private SettingsForm settingsForm;
        private TableList tableListForm;

        public NewIconatorControl()
        {
            InitializeComponent();

            SetTheme();

            tableListForm = new TableList();
            tableListForm.SelectedItemsChanged += TableListForm_SelectedItemsChanged;
            tableListForm.Show(dpMain, DockState.DockLeft);

            imageListForm = new VectorImagesList();
            imageListForm.ImagesAdded += ImageListForm_ImagesAdded;
            imageListForm.SelectedItemsChanged += ImageListForm_SelectedItemsChanged;
            imageListForm.Show(dpMain, DockState.Document);

            helpForm = new HelpForm();
            helpForm.Show(dpMain, DockState.DockRightAutoHide);

            settingsForm = new SettingsForm();
            settingsForm.Show(dpMain, DockState.DockRightAutoHide);
            SettingsManager.Instance.TryLoad(typeof(Iconator), out settings);
            if (settings == null)
            {
                settings = new Settings();
            }
            settingsForm.Settings = settings;
        }

        public string DonationDescription => "Donation for Iconator";

        public string EmailAccount => "tanguy92@hotmail.com";

        public string RepositoryName => "MsCrmTools.Iconator";

        public string UserName => "MscrmTools";

        public override void ClosingPlugin(PluginCloseInfo info)
        {
            try
            {
                settings.Save();
                base.ClosingPlugin(info);
            }
            catch (Exception error)
            {
                MessageBox.Show(this, $"An error occured when saving settings: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            imageListForm.Clear();
            tableListForm.Clear();

            if (ConnectionDetail != null)
            {
                tsbLoad_Click(tsbLoad, new EventArgs());
            }

            base.UpdateConnection(newService, detail, actionName, parameter);
        }

        private void AddImagesToSolution(string[] fileNames)
        {
            using (var sp = new SolutionPicker(Service, true))
            {
                if (sp.ShowDialog(this) == DialogResult.OK)
                {
                    var solution = sp.SelectedSolutions.First();

                    SetState(true);

                    WorkAsync(new WorkAsyncInfo
                    {
                        Message = "Adding images...",
                        Work = (bw, evt) =>
                        {
                            foreach (var filename in fileNames)
                            {
                                var b64 = Convert.ToBase64String(File.ReadAllBytes(filename));
                                string name = settings.WebresourceNameMask
                                .Replace("{prefix}", solution.GetAttributeValue<AliasedValue>("publisher.customizationprefix").Value?.ToString())
                                .Replace("{filename}", Path.GetFileNameWithoutExtension(filename));

                                bw.ReportProgress(0, $"Creating web resource {name}...");

                                var wr = new Entity("webresource")
                                {
                                    ["content"] = b64,
                                    ["name"] = name,
                                    ["displayname"] = name,
                                    ["webresourcetype"] = new OptionSetValue(11)
                                };

                                wr.Id = Service.Create(wr);

                                vectorImages.Add(new VectorImageInfo(b64) { Name = name });

                                bw.ReportProgress(0, $"Adding web resource {name} to solution {solution.GetAttributeValue<string>("friendlyname")}...");

                                var request = new AddSolutionComponentRequest
                                {
                                    ComponentType = 61,
                                    SolutionUniqueName = solution.GetAttributeValue<string>("uniquename"),
                                    ComponentId = wr.Id
                                };

                                Service.Execute(request);
                            }
                        },
                        ProgressChanged = evt => { SetWorkingMessage(evt.UserState.ToString()); },
                        PostWorkCallBack = evt =>
                        {
                            SetState(false);
                            imageListForm.Images = vectorImages;
                            imageListForm.DisplayImages();
                        }
                    });
                }
            }
        }

        private void DoAction(bool fromSolution)
        {
            var solutionIds = new List<Guid>();

            if (fromSolution)
            {
                var sPicker = new SolutionPicker(Service);
                if (sPicker.ShowDialog(ParentForm) == DialogResult.OK)
                {
                    solutionIds = sPicker.SelectedSolutions.Select(s => s.Id).ToList();
                }
                else
                {
                    return;
                }
            }

            SetState(true);

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading Tables...",
                AsyncArgument = solutionIds,
                Work = (bw, e) =>
                {
                    // Display retrieved entities
                    tables.Clear();
                    tables.AddRange(from emd in MetadataManager.GetEntitiesList(Service, solutionIds, ConnectionDetail.OrganizationMajorVersion, ConnectionDetail.OrganizationMinorVersion)
                                    orderby (emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A")
                                    select new TableInfo(emd));

                    foreach (var table in tables)
                    {
                        table.OnStateChange += Table_OnStateChange;
                    }

                    bw.ReportProgress(0, "Loading Web resources...");

                    var queryWebResources =
                        from webResourceList in WebResourcesManager.GetWebResourcesOnSolution(Service, solutionIds, new[] { 11 }).Entities
                        orderby webResourceList.GetAttributeValue<string>("name")
                        select webResourceList;

                    vectorImages.Clear();
                    foreach (var webResource in queryWebResources)
                    {
                        try
                        {
                            vectorImages.Add(new VectorImageInfo(webResource.GetAttributeValue<string>("content"))
                            {
                                Name = webResource.GetAttributeValue<string>("name")
                            });
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                },
                PostWorkCallBack = e =>
                {
                    SetState(false);

                    if (e.Error != null)
                    {
                        MessageBox.Show(this, "Error while loading components: " + e.Error.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        StringBuilder message = new StringBuilder();

                        if (tables.Count == 0)
                        {
                            message.Append(
                                "No custom tables have been found. ");
                        }

                        if (vectorImages.Count == 0)
                        {
                            message.Append("No images have been found. ");
                        }

                        if (message.Length > 0)
                        {
                            message.Append(
                                "If you selected a solution, add the missing components in it. If not, create custom tables or images");

                            ShowWarningNotification(message.ToString(), null);
                        }
                        tableListForm.Tables = tables;
                        tableListForm.Images = vectorImages;
                        tableListForm.DisplayTables();
                        imageListForm.Images = vectorImages;
                        imageListForm.DisplayImages();
                    }
                },
                ProgressChanged = e => { SetWorkingMessage(e.UserState.ToString()); }
            });
        }

        private void ImageListForm_ImagesAdded(object sender, VectorImageAddedEventsArgs e)
        {
            AddImagesToSolution(e.Files);
        }

        private void ImageListForm_SelectedItemsChanged(object sender, EventArgs e)
        {
            tsbDeleteImages.Enabled = imageListForm.SelectedImages.Count > 0;
        }

        private void SetState(bool isRunning)
        {
            tsbLoad.Enabled = !isRunning;
            tsbAddImages.Enabled = !isRunning;
            tsbApplyImages.Enabled = !isRunning && tableListForm.PendingTables.Count > 0;
            tsbDeleteImages.Enabled = !isRunning && imageListForm.SelectedImages.Count > 0;
            tsbRemoveImage.Enabled = !isRunning && tableListForm.SelectedTables.Any(t => !string.IsNullOrEmpty(t.WebResourceName));
            tsbRemovePendingAssociation.Enabled = !isRunning && tableListForm.PendingTables.Count > 0; ;
        }

        private void SetTheme()
        {
            if (XrmToolBox.Options.Instance.Theme != null)
            {
                switch (XrmToolBox.Options.Instance.Theme)
                {
                    case "Blue theme":
                        {
                            var theme = new VS2015BlueTheme();
                            dpMain.Theme = theme;
                        }
                        break;

                    case "Light theme":
                        {
                            var theme = new VS2015LightTheme();
                            dpMain.Theme = theme;
                        }
                        break;

                    case "Dark theme":
                        {
                            var theme = new VS2015DarkTheme();
                            dpMain.Theme = theme;
                        }
                        break;
                }
            }
        }

        private void Table_OnStateChange(object sender, EntityVectorIconChangeEventsArgs e)
        {
            if (settings.ApplyImageAutomatically && e.Table.IsPending)
            {
                SetState(true);
                WorkAsync(new WorkAsyncInfo
                {
                    Message = "Updating tables...",
                    Work = (bw, evt) =>
                    {
                        bw.ReportProgress(0, $"Updating table {e.Table.Entity.DisplayName?.UserLocalizedLabel?.Label ?? e.Table.Entity.SchemaName}...");
                        e.Table.ApplyPendingWebResource(Service);

                        bw.ReportProgress(0, $"Publishing table...");
                        MetadataManager.PublishEntities(new List<string> { e.Table.Entity.LogicalName }, Service);
                    },
                    ProgressChanged = evt => { SetWorkingMessage(evt.UserState.ToString()); },
                    PostWorkCallBack = evt =>
                    {
                        SetState(false);
                        tableListForm.RefreshDisplay();
                    }
                });
            }
        }

        private void TableListForm_SelectedItemsChanged(object sender, EventArgs e)
        {
            tsbApplyImages.Enabled = tableListForm.PendingTables.Count > 0;
            tsbRemoveImage.Enabled = tableListForm.SelectedTables.Any(t => !string.IsNullOrEmpty(t.WebResourceName));
            tsbRemovePendingAssociation.Enabled = tableListForm.SelectedTables.Any(t => t.IsPending);
        }

        private void tsbAddImages_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog { Filter = "Vector image (*.svg)|*.svg" })
            {
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    AddImagesToSolution(ofd.FileNames);
                }
            }
        }

        private void tsbApplyImages_Click(object sender, EventArgs e)
        {
            var pendingTables = tableListForm.PendingTables;
            if (pendingTables.Count == 0)
            {
                MessageBox.Show(this, "No table selected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SetState(true);

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Updating tables...",
                Work = (bw, evt) =>
                {
                    foreach (var table in pendingTables)
                    {
                        bw.ReportProgress(0, $"Updating table {table.Entity.DisplayName?.UserLocalizedLabel?.Label ?? table.Entity.SchemaName}...");
                        table.ApplyPendingWebResource(Service);
                    }

                    bw.ReportProgress(0, $"Publishing table(s)...");
                    MetadataManager.PublishEntities(pendingTables.Select(t => t.Entity.LogicalName).ToList(), Service);
                },
                ProgressChanged = evt => { SetWorkingMessage(evt.UserState.ToString()); },
                PostWorkCallBack = evt =>
                {
                    SetState(false);
                    tableListForm.RefreshDisplay();
                }
            }

            );
        }

        private void tsbDeleteImages_Click(object sender, EventArgs e)
        {
            var images = imageListForm.SelectedImages;
            if (images.Count == 0)
            {
                MessageBox.Show(this, "No image selected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SetState(true);

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Deleting images...",
                Work = (bw, evt) =>
                {
                    foreach (var image in images)
                    {
                        bw.ReportProgress(0, $"Delete image {image.Name}...");
                        image.Delete(Service);
                        vectorImages.Remove(image);
                    }
                },
                ProgressChanged = evt => { SetWorkingMessage(evt.UserState.ToString()); },
                PostWorkCallBack = evt =>
                {
                    SetState(false);
                    if (evt.Error != null)
                    {
                        MessageBox.Show(this, "Error while deleting images: " + evt.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    imageListForm.Images = vectorImages;
                    imageListForm.DisplayImages();
                }
            });
        }

        private void tsbLoad_Click(object sender, EventArgs e)
        {
            ExecuteMethod(DoAction, true);
        }

        private void tsbRemoveImage_Click(object sender, EventArgs e)
        {
            var pendingTables = tableListForm.SelectedTables.Where(t => !string.IsNullOrEmpty(t.WebResourceName));

            SetState(true);

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Removing image from tables...",
                Work = (bw, evt) =>
                {
                    foreach (var table in pendingTables)
                    {
                        bw.ReportProgress(0, $"Removing image from table {table.Entity.DisplayName?.UserLocalizedLabel?.Label ?? table.Entity.SchemaName}...");
                        table.ApplyWebResourceRemoval(Service);
                    }

                    bw.ReportProgress(0, $"Publishing tables...");
                    MetadataManager.PublishEntities(pendingTables.Select(t => t.Entity.LogicalName).ToList(), Service);
                },
                ProgressChanged = evt => { SetWorkingMessage(evt.UserState.ToString()); },
                PostWorkCallBack = evt =>
                {
                    SetState(false);
                    tableListForm.RefreshDisplay();
                }
            });
        }

        private void tsbRemovePendingAssociation_Click(object sender, EventArgs e)
        {
            var tables = tableListForm.SelectedTables;

            foreach (var table in tables)
            {
                table.CancelPending();
            }

            SetState(false);
            tableListForm.RefreshDisplay();
        }
    }
}