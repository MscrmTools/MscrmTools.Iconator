// PROJECT : MsCrmTools.Iconator
// This project was developed by Tanguy Touzard
// CODEPLEX: http://xrmtoolbox.codeplex.com
// BLOG: http://mscrmtools.blogspot.com

using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using MsCrmTools.Iconator.AppCode;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace MsCrmTools.Iconator
{
    public partial class Iconator : PluginControlBase, IGitHubPlugin, IHelpPlugin
    {
        #region Variables

        private readonly List<Entity> webResourceRetrivedList;

        private CrmComponents cc;
        private Color defaultColor;

        public string HelpUrl
        {
            get
            {
                return "https://github.com/MscrmTools/MscrmTools.Iconator/wiki";
            }
        }

        public string RepositoryName
        {
            get
            {
                return "MscrmTools.Iconator";
            }
        }

        public string UserName
        {
            get
            {
                return "MscrmTools";
            }
        }

        #endregion Variables

        #region Constructor

        public Iconator()
        {
            InitializeComponent();
            webResourceRetrivedList = new List<Entity>();
        }

        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            SetOrganizationVersionSpecificItems(detail);

            listViewEntities.Items.Clear();
            listViewWebRessources16.Items.Clear();
            listViewWebRessources32.Items.Clear();
            listViewWebRessourcesOther.Items.Clear();
            lvVectorWebresources.Items.Clear();
            webResourceRetrivedList.Clear();

            pictureBox16.Image = imageList1.Images[0];
            pictureBox32.Image = imageList1.Images[1];
            pbVector.Image = imageList1.Images[2];

            base.UpdateConnection(newService, detail, actionName, parameter);
        }

        private void Iconator_Load(object sender, EventArgs e)
        {
            if (ConnectionDetail != null)
            {
                SetOrganizationVersionSpecificItems(ConnectionDetail);
            }
        }

        private void SetOrganizationVersionSpecificItems(ConnectionDetail detail)
        {
            if (!((detail.OrganizationMajorVersion == 7 && detail.OrganizationMinorVersion >= 1)
                || detail.OrganizationMajorVersion > 7))
            {
                btnChangeColor.Enabled = false;
                btnResetColor.Enabled = false;
            }
            else
            {
                btnChangeColor.Enabled = true;
                btnResetColor.Enabled = true;
            }

            if (detail.OrganizationMajorVersion < 9)
            {
                pbVector.Visible = false;
                lblVectorLabel.Visible = false;

                tabControlWebResource.TabPages.Remove(tabPageVector);
            }
            else
            {
                pbVector.Visible = true;
                lblVectorLabel.Visible = true;

                if (tabControlWebResource.TabPages.Count == 3)
                {
                    tabControlWebResource.TabPages.Insert(2, tabPageVector);
                }

                tabControlWebResource.SelectedIndex = 2;
            }

            if (detail.OrganizationMajorVersion >= 6)
            {
                defaultColor = ColorTranslator.FromHtml("#006551");
            }
            else
            {
                defaultColor = Color.FromArgb(0, 0, 0, 0);
            }
        }

        #endregion Constructor

        #region ListViewItems selection

        private void LvEntitiesSelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewEntities.SelectedItems.Count > 0)
            {
                if (listViewEntities.SelectedItems.Count > 1)
                {
                    groupBoxCurrentIcon.Text = "Current icons (first selected entity only)";
                }
                else
                {
                    groupBoxCurrentIcon.Text = "Current icons";
                }

                var entity = (EntityMetadata)listViewEntities.SelectedItems[0].Tag;

                if (!string.IsNullOrEmpty(entity.IconSmallName))
                {
                    var queryWrSmall = from wrList in webResourceRetrivedList
                                       where (string)wrList["name"] == entity.IconSmallName
                                       select wrList;

                    foreach (var entityWrS in queryWrSmall)
                    {
                        pictureBox16.Image = ImageHelper.ConvertWebResContent(entityWrS.Attributes["content"].ToString(), false);
                    }
                }
                else
                {
                    pictureBox16.Image = imageList1.Images[0];
                }

                if (!string.IsNullOrEmpty(entity.IconMediumName))
                {
                    var queryMedium = from wrList in webResourceRetrivedList
                                      where (string)wrList["name"] == entity.IconMediumName
                                      select wrList;

                    foreach (var entityM in queryMedium)
                    {
                        pictureBox32.Image = ImageHelper.ConvertWebResContent(entityM.Attributes["content"].ToString(), false);
                    }
                }
                else
                {
                    pictureBox32.Image = imageList1.Images[1];
                }

                if (!string.IsNullOrEmpty(entity.IconVectorName))
                {
                    var queryMedium = from wrList in webResourceRetrivedList
                                      where (string)wrList["name"] == entity.IconVectorName
                                      select wrList;

                    foreach (var entityM in queryMedium)
                    {
                        pbVector.Image = ImageHelper.ConvertWebResContent(entityM.Attributes["content"].ToString(), true);
                    }
                }
                else
                {
                    pbVector.Image = imageList1.Images[2];
                }

                if (string.IsNullOrEmpty(entity.EntityColor))
                {
                    pictureBox32.BackColor = defaultColor;
                }
                else
                {
                    pictureBox32.BackColor = ColorTranslator.FromHtml(entity.EntityColor);
                }
            }
        }

        private void LvWebRessourcesOtherSelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewWebRessourcesOther.SelectedItems.Count > 0)
            {
                labelSizeWr.Text = "Image size: " +
                                   ((WebResourcesManager.WebResourceAndImage)listViewWebRessourcesOther.FocusedItem.Tag)
                                       .Image.Size.ToString();
            }
        }

        #endregion ListViewItems selection

        #region Main menu actions

        private void DoAction(bool fromSolution)
        {
            listViewEntities.Items.Clear();
            listViewWebRessources16.Items.Clear();
            listViewWebRessources32.Items.Clear();
            listViewWebRessourcesOther.Items.Clear();
            lvVectorWebresources.Items.Clear();

            var solutionIds = new List<Guid>();

            if (fromSolution)
            {
                var sPicker = new SolutionPicker(Service);
                if (sPicker.ShowDialog(ParentForm) == DialogResult.OK)
                {
                    solutionIds = sPicker.SelectedSolutions.Select(s => s.Id).ToList();
                }
            }

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading Entities...",
                AsyncArgument = solutionIds,
                Work = (bw, e) =>
                {
                    cc = new CrmComponents();

                    // Display retrieved entities
                    var queryEntities = from entityList in MetadataManager.GetEntitiesList(Service, solutionIds, ConnectionDetail.OrganizationMajorVersion, ConnectionDetail.OrganizationMinorVersion)
                                        orderby entityList.DisplayName.UserLocalizedLabel.Label
                                        select entityList;

                    foreach (var entity in queryEntities)
                    {
                        var lvi = new ListViewItem(entity.DisplayName.UserLocalizedLabel.Label) { Tag = entity };
                        lvi.SubItems.Add(entity.LogicalName);
                        cc.Entities.Add(lvi);
                    }

                    bw.ReportProgress(0, "Loading Web resources...");

                    var queryWebResources =
                        from webResourceList in WebResourcesManager.GetWebResourcesOnSolution(Service, solutionIds).Entities
                        orderby webResourceList.GetAttributeValue<string>("name")
                        select webResourceList;

                    foreach (var webResource in queryWebResources)
                    {
                        try
                        {
                            // Specific processing for vector images
                            if (webResource.GetAttributeValue<OptionSetValue>("webresourcetype").Value == 11)
                            {
                                var svg =
                                    ImageHelper.ConvertWebResContent(webResource.GetAttributeValue<string>("content"), true);

                                var lvi = new ListViewItem(webResource.GetAttributeValue<string>("name"))
                                {
                                    Name = webResource.GetAttributeValue<string>("name"),
                                    Tag = webResource,
                                    ImageIndex = cc.ImagesVector.Count
                                };
                                cc.IconsVector.Add(lvi);
                                cc.ImagesVector.Add(svg);

                                webResourceRetrivedList.Add(webResource);

                                continue;
                            }

                            var imageConverted =
                                ImageHelper.ConvertWebResContent(webResource.GetAttributeValue<string>("content"), false);

                            if (imageConverted == null)
                                continue;

                            if (imageConverted.Size.Height == 32 && imageConverted.Size.Width == 32)
                            {
                                var lvi = new ListViewItem(webResource.GetAttributeValue<string>("name"))
                                {
                                    Name = webResource.GetAttributeValue<string>("name"),
                                    Tag = webResource,
                                    ImageIndex = cc.Images32.Count
                                };
                                cc.Icons32.Add(lvi);
                                cc.Images32.Add(imageConverted);
                            }
                            else if (imageConverted.Size.Height == 16 && imageConverted.Size.Width == 16)
                            {
                                var lvi = new ListViewItem(webResource.GetAttributeValue<string>("name"))
                                {
                                    Name = webResource.GetAttributeValue<string>("name"),
                                    Tag = webResource,
                                    ImageIndex = cc.Images16.Count
                                };
                                cc.Icons16.Add(lvi);
                                cc.Images16.Add(imageConverted);
                            }
                            else
                            {
                                var listWrImage = new WebResourcesManager.WebResourceAndImage
                                {
                                    Image = imageConverted,
                                    Webresource = webResource
                                };
                                var lvi = new ListViewItem(webResource.GetAttributeValue<string>("name"))
                                {
                                    Name = webResource.GetAttributeValue<string>("name"),
                                    Tag = listWrImage,
                                    ImageIndex = cc.ImagesOthers.Count,
                                };
                                cc.IconsOthers.Add(lvi);
                                cc.ImagesOthers.Add(imageConverted);
                            }

                            webResourceRetrivedList.Add(webResource);
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    e.Result = cc;
                },
                PostWorkCallBack = e =>
                {
                    if (e.Error != null)
                    {
                        MessageBox.Show(this, "Error while loading Crm components: " + e.Error.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        var cc = (CrmComponents)e.Result;

                        var imageList16 = new ImageList
                        {
                            ImageSize = new Size(16, 16),
                            ColorDepth = ColorDepth.Depth32Bit
                        };
                        var imageList32 = new ImageList
                        {
                            ImageSize = new Size(32, 32),
                            ColorDepth = ColorDepth.Depth32Bit
                        };
                        var imageListOther = new ImageList { ImageSize = new Size(32, 32), ColorDepth = ColorDepth.Depth32Bit };
                        var imageListVector = new ImageList { ImageSize = new Size(48, 48), ColorDepth = ColorDepth.Depth32Bit };

                        imageList16.Images.AddRange(cc.Images16.ToArray());
                        imageList32.Images.AddRange(cc.Images32.ToArray());
                        imageListOther.Images.AddRange(cc.ImagesOthers.ToArray());
                        imageListVector.Images.AddRange(cc.ImagesVector.ToArray());

                        listViewWebRessources16.LargeImageList = imageList16;
                        listViewWebRessources32.LargeImageList = imageList32;
                        listViewWebRessourcesOther.LargeImageList = imageListOther;
                        lvVectorWebresources.LargeImageList = imageListVector;

                        listViewEntities.Items.AddRange(cc.Entities.ToArray());
                        listViewWebRessources16.Items.AddRange(cc.Icons16.ToArray());
                        listViewWebRessources32.Items.AddRange(cc.Icons32.ToArray());
                        listViewWebRessourcesOther.Items.AddRange(cc.IconsOthers.ToArray());
                        lvVectorWebresources.Items.AddRange(cc.IconsVector.ToArray());

                        StringBuilder message = new StringBuilder();

                        if (cc.Entities.Count == 0)
                        {
                            message.Append(
                                "No custom entities have been found. ");
                        }

                        if (cc.Icons16.Count == 0 && cc.Icons32.Count == 0 && cc.IconsOthers.Count == 0 && cc.IconsVector.Count == 0)
                        {
                            message.Append(
                                "No images have been found. ");
                        }

                        if (message.Length > 0)
                        {
                            message.Append(
                                "If you selected a solution, add the missing components in it. If not, create custom entities or images");

                            ShowWarningNotification(message.ToString(), null);
                        }
                    }
                    tsbAddIcon.Enabled = true;
                    tsbApply.Enabled = true;
                    tsbCleanOldImages.Enabled = true;

                    SetEnableState(true);
                },
                ProgressChanged = e => { SetWorkingMessage(e.UserState.ToString()); }
            });
        }

        private void TsbAddIconClick(object sender, EventArgs e)
        {
            var icForm = new ImageCreationForm(Service);
            icForm.ShowDialog();

            if (icForm.WebResourcesCreated.Count > 0)
            {
                var imageList16 = listViewWebRessources16.LargeImageList;
                var imageList32 = listViewWebRessources32.LargeImageList;
                var imageListOther = listViewWebRessourcesOther.LargeImageList;
                var imageListVector = lvVectorWebresources.LargeImageList;

                foreach (var webResource in icForm.WebResourcesCreated)
                {
                    // Specific processing for vector images
                    if (webResource.GetAttributeValue<OptionSetValue>("webresourcetype").Value == 11)
                    {
                        Image svg = ImageHelper.ConvertWebResContent(webResource.Attributes["content"].ToString(), true);

                        var lvi = new ListViewItem(webResource.GetAttributeValue<string>("name"))
                        {
                            Name = webResource.GetAttributeValue<string>("name"),
                            Tag = webResource,
                            ImageIndex = imageListVector.Images.Count
                        };
                        lvVectorWebresources.Items.Add(lvi);
                        imageListVector.Images.Add(svg);

                        webResourceRetrivedList.Add(webResource);

                        continue;
                    }

                    var imageConverted = ImageHelper.ConvertWebResContent(webResource.Attributes["content"].ToString(), false);

                    if (imageConverted.Size.Height == 32 && imageConverted.Size.Width == 32)
                    {
                        var lvi = new ListViewItem(webResource.Attributes["name"].ToString())
                        {
                            Name = webResource.GetAttributeValue<string>("name"),
                            Tag = webResource,
                            ImageIndex = imageList32.Images.Count
                        };
                        listViewWebRessources32.Items.Add(lvi);
                        imageList32.Images.Add(imageConverted);
                    }
                    else if (imageConverted.Size.Height == 16 && imageConverted.Size.Width == 16)
                    {
                        var lvi = new ListViewItem(webResource.Attributes["name"].ToString())
                        {
                            Name = webResource.GetAttributeValue<string>("name"),
                            Tag = webResource,
                            ImageIndex = imageList16.Images.Count
                        };
                        listViewWebRessources16.Items.Add(lvi);
                        imageList16.Images.Add(imageConverted);
                    }
                    else
                    {
                        var listWrImage = new WebResourcesManager.WebResourceAndImage
                        {
                            Image = imageConverted,
                            Webresource = webResource
                        };
                        var lvi = new ListViewItem(webResource.Attributes["name"].ToString())
                        {
                            Name = webResource.GetAttributeValue<string>("name"),
                            Tag = listWrImage,
                            ImageIndex = imageListOther.Images.Count,
                        };
                        listViewWebRessourcesOther.Items.Add(lvi);
                        imageListOther.Images.Add(imageConverted);
                    }
                    webResourceRetrivedList.Add(webResource);
                }

                listViewWebRessources32.LargeImageList = imageList32;
                listViewWebRessources16.LargeImageList = imageList16;
                listViewWebRessourcesOther.LargeImageList = imageListOther;
                lvVectorWebresources.LargeImageList = imageListVector;
            }
        }

        #region Apply Images to entities

        private void TsbApplyClick(object sender, EventArgs e)
        {
            if (lvMappings.Items.Count <= 0) return;

            var mappingList = (from ListViewItem item in lvMappings.Items select (EntityImageMap)item.Tag).ToList();
            SetEnableState(false);

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Applying images to entities. Please wait...",
                AsyncArgument = mappingList,
                Work = (bw, evt) =>
                {
                    MetadataManager.ApplyImagesToEntities((List<EntityImageMap>)evt.Argument, Service);
                },
                PostWorkCallBack = evt =>
                {
                    SetEnableState(true);

                    if (evt.Error != null)
                    {
                        MessageBox.Show(this, "Error while applying images to entities: " + evt.Error.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        lvMappings.Items.Clear();
                        LvEntitiesSelectedIndexChanged(null, null);
                    }
                }
            });
        }

        #endregion Apply Images to entities

        #endregion Main menu actions

        #region Map/UnMap

        private void BtnMapClick(object sender, EventArgs e)
        {
            if (listViewEntities.SelectedItems.Count > 0 &&
                (listViewWebRessources16.SelectedItems.Count > 0 || listViewWebRessources32.SelectedItems.Count > 0 ||
                 listViewWebRessourcesOther.SelectedItems.Count > 0 || lvVectorWebresources.SelectedItems.Count > 0))
            {
                foreach (ListViewItem entityItem in listViewEntities.SelectedItems)
                {
                    var selectedEntity = (EntityMetadata)entityItem.Tag;

                    var mapping = new EntityImageMap { Entity = selectedEntity };

                    if (listViewWebRessources16.SelectedItems.Count > 0)
                    {
                        mapping.WebResourceName = ((Entity)listViewWebRessources16.SelectedItems[0].Tag)["name"].ToString();
                        mapping.ImageSize = 16;
                    }
                    else if (listViewWebRessources32.SelectedItems.Count > 0)
                    {
                        mapping.WebResourceName = ((Entity)listViewWebRessources32.SelectedItems[0].Tag)["name"].ToString();
                        mapping.ImageSize = 32;
                    }
                    else if (listViewWebRessourcesOther.SelectedItems.Count > 0)
                    {
                        mapping.WebResourceName =
                            ((WebResourcesManager.WebResourceAndImage)listViewWebRessourcesOther.SelectedItems[0].Tag)
                            .Webresource["name"].ToString();

                        var issDialog = new ImageSizeSelectionDialog { StartPosition = FormStartPosition.CenterParent };
                        if (issDialog.ShowDialog(this) == DialogResult.OK)
                        {
                            mapping.ImageSize = issDialog.ImageSizeSelected;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        mapping.WebResourceName = ((Entity)lvVectorWebresources.SelectedItems[0].Tag)["name"].ToString();
                        mapping.ImageSize = 0;
                    }

                    var item = new ListViewItem(selectedEntity.DisplayName.UserLocalizedLabel.Label)
                    {
                        Tag = mapping
                    };
                    item.SubItems.Add(selectedEntity.LogicalName);
                    item.SubItems.Add(mapping.ImageSize == 0 ? "Vector" : mapping.ImageSize + "x" + mapping.ImageSize);
                    item.SubItems.Add(mapping.WebResourceName);

                    foreach (ListViewItem existingItem in lvMappings.Items)
                    {
                        if (existingItem.SubItems[1].Text == item.SubItems[1].Text
                            && existingItem.SubItems[2].Text == item.SubItems[2].Text)
                        {
                            MessageBox.Show(this, "There is already a mapping for this entity and this size", "Warning",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    lvMappings.Items.Add(item);
                }
            }
            else
            {
                MessageBox.Show(this, "Please select at least one entity and one image", "Warning", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void BtnUnmapClick(object sender, EventArgs e)
        {
            if (lvMappings.SelectedItems.Count > 0)
            {
                lvMappings.Items.Remove(lvMappings.SelectedItems[0]);
            }
        }

        private void LvWebRessourcesMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listViewWebRessources16.SelectedItems.Count > 0
                || listViewWebRessources32.SelectedItems.Count > 0
                || lvVectorWebresources.SelectedItems.Count > 0
                || listViewWebRessourcesOther.SelectedItems.Count > 0)
            {
                BtnMapClick(null, null);
            }
        }

        #endregion Map/UnMap

        #region Reset Icons

        private void btnChangeColor_Click(object sender, EventArgs e)
        {
            if (listViewEntities.SelectedItems.Count == 0)
            {
                return;
            }

            colorDialog.Color = ColorTranslator.FromHtml(((EntityMetadata)listViewEntities.SelectedItems[0].Tag).EntityColor);
            if (colorDialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }
            pictureBox32.BackColor = colorDialog.Color;

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Updating color for entity. Please wait...",
                AsyncArgument = new object[] { pictureBox32.BackColor, listViewEntities.SelectedItems.Cast<ListViewItem>().Select(i => (EntityMetadata)i.Tag).ToList() },
                Work = (bw, evt) =>
                {
                    var parameters = (object[])evt.Argument;
                    var emds = (IEnumerable<EntityMetadata>)parameters[1];
                    var color = (Color)parameters[0];
                    foreach (var emd in emds)
                    {
                        MetadataManager.AddColor(emd, color, Service);
                    }

                    bw.ReportProgress(0, "Publishing entity");

                    MetadataManager.PublishEntities(emds.Select(emd => emd.LogicalName).ToList(), Service);
                },
                PostWorkCallBack = evt =>
                {
                    SetEnableState(true);

                    if (evt.Error != null)
                    {
                        MessageBox.Show(this, "Error while updating color for entity: " + evt.Error.Message,
                            "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        LvEntitiesSelectedIndexChanged(null, null);
                    }
                },
                ProgressChanged = evt =>
                {
                    SetWorkingMessage(evt.UserState.ToString());
                }
            });
        }

        private void btnResetColor_Click(object sender, EventArgs e)
        {
            if (listViewEntities.SelectedItems.Count == 0)
            {
                return;
            }

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Updating color for entity. Please wait...",
                AsyncArgument = listViewEntities.SelectedItems.Cast<ListViewItem>().Select(i => (EntityMetadata)i.Tag).ToList(),
                Work = (bw, evt) =>
                {
                    var emds = (IEnumerable<EntityMetadata>)evt.Argument;
                    foreach (var emd in emds)
                    {
                        MetadataManager.ResetColor(emd, Service);
                    }

                    MetadataManager.PublishEntities(emds.Select(emd => emd.LogicalName).ToList(), Service);
                },
                PostWorkCallBack = evt =>
                {
                    SetEnableState(true);

                    if (evt.Error != null)
                    {
                        MessageBox.Show(this, "Error while updating color for entity: " + evt.Error.Message,
                            "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        LvEntitiesSelectedIndexChanged(null, null);
                    }
                },
            });
        }

        private void BtnResetIconClick(object sender, EventArgs e)
        {
            if (listViewEntities.SelectedItems.Count == 0)
            {
                return;
            }
            if (DialogResult.Yes ==
                MessageBox.Show(this, "Are you sure you want to reset icons for the selected entities?", "Question",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                SetEnableState(false);

                WorkAsync(new WorkAsyncInfo
                {
                    Message = "Reseting icons for entity. Please wait...",
                    AsyncArgument = listViewEntities.SelectedItems.Cast<ListViewItem>().ToList(),
                    Work = (bw, evt) =>
                    {
                        var items = (IEnumerable<ListViewItem>)evt.Argument;
                        foreach (var item in items)
                        {
                            var emd = (EntityMetadata)item.Tag;
                            MetadataManager.ResetIcons(emd, Service);
                        }

                        MetadataManager.PublishEntities(items.Select(i => ((EntityMetadata)i.Tag).LogicalName).ToList(), Service);
                    },
                    PostWorkCallBack = evt =>
                    {
                        SetEnableState(true);

                        if (evt.Error != null)
                        {
                            MessageBox.Show(this, "Error while reseting icons for entity: " + evt.Error.Message,
                                "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            LvEntitiesSelectedIndexChanged(null, null);
                        }
                    },
                });
            }
        }

        #endregion Reset Icons

        #region Others

        private void BtnPreviewClick(object sender, EventArgs e)
        {
            if (listViewWebRessourcesOther.FocusedItem != null)
            {
                var img =
                    ((WebResourcesManager.WebResourceAndImage)listViewWebRessourcesOther.FocusedItem.Tag).Image;

                var preview = new ImagePreview(img) { StartPosition = FormStartPosition.CenterParent };
                preview.ShowDialog();
            }
        }

        private void SetEnableState(bool enabled)
        {
            mainMenu.Enabled = enabled;
            groupBoxCurrentIcon.Enabled = enabled;
            gbEntities.Enabled = enabled;
            splitContainer1.Enabled = enabled;
            tsbOptimizeIcons.Enabled = enabled;
        }

        private void TabControlWebResourceSelectedIndexChanged(object sender, EventArgs e)
        {
            listViewWebRessources16.SelectedItems.Clear();
            listViewWebRessources32.SelectedItems.Clear();
            listViewWebRessourcesOther.SelectedItems.Clear();
        }

        #endregion Others

        private void CleanImagesListviews(List<string> imageList, List<string> imageList2)
        {
            var toRemove = imageList.Except(imageList2);
            foreach (var img in toRemove)
            {
                listViewWebRessources16.Items.RemoveByKey(img);
                listViewWebRessources32.Items.RemoveByKey(img);
            }
        }

        private void listViewEntities_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            var list = (ListView)sender;
            list.Sorting = list.Sorting == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
            list.ListViewItemSorter = new ListViewItemComparer(e.Column, list.Sorting);
        }

        private void llShowAllEntities_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listViewEntities.Items.Clear();
            listViewEntities.Items.AddRange(cc.Entities.ToArray());
        }

        private void llShowEmptyIcon_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listViewEntities.Items.Clear();
            listViewEntities.Items.AddRange(cc.Entities.Where(lvi =>
                ((EntityMetadata)lvi.Tag).IconVectorName == null
                && ((EntityMetadata)lvi.Tag).IconLargeName == null
                && ((EntityMetadata)lvi.Tag).IconSmallName == null
            ).ToArray());
        }

        private void tsbCleanOldImages_Click(object sender, EventArgs e)
        {
            var entities = listViewEntities.CheckedItems.Cast<ListViewItem>().ToList();
            if (entities.Count == 0)
            {
                MessageBox.Show(this, @"Please check at least one entity", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var emds = entities.Select(i => (EntityMetadata)i.Tag).ToList();

            if (DialogResult.No == MessageBox.Show(this, $@"Are you sure you want to clean old images for {emds.Count} entities?", @"Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                return;
            }

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Cleaning entities...",
                Work = (bw, evt) =>
                {
                    var imageList = new List<string>();

                    foreach (var emd in emds)
                    {
                        MetadataManager.CleanOldIcons(emd, imageList, Service);
                    }

                    evt.Result = imageList;
                },
                PostWorkCallBack = evt =>
                {
                    if (evt.Error != null)
                    {
                        MessageBox.Show(this, $@"An error occured when cleaning entities: {evt.Error.Message}", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var imageList = (List<string>)evt.Result;
                    var originalImageList = new string[imageList.Count];
                    imageList.CopyTo(originalImageList);
                    if (imageList.Any())
                    {
                        var result = MessageBox.Show(this, $@"{imageList.Count} image(s) have been cleaned from entities. Would you like to delete these images?", @"Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.No) return;

                        WorkAsync(new WorkAsyncInfo
                        {
                            Message = "Deleting images...",
                            Work = (bw, evt2) =>
                            {
                                var wrs = Service.RetrieveMultiple(new QueryExpression("webresource")
                                {
                                    NoLock = true,
                                    ColumnSet = new ColumnSet("name"),
                                    Criteria = new FilterExpression
                                    {
                                        Conditions =
                                        {
                                            new ConditionExpression("name", ConditionOperator.In, imageList.ToArray())
                                        }
                                    }
                                }).Entities;

                                foreach (var wr in wrs)
                                {
                                    try
                                    {
                                        Service.Delete(wr.LogicalName, wr.Id);
                                        imageList.Remove(wr.GetAttributeValue<string>("name"));
                                    }
                                    catch { }
                                }

                                evt2.Result = imageList;
                            },
                            PostWorkCallBack = evt2 =>
                            {
                                if (evt2.Error != null)
                                {
                                    MessageBox.Show(this, $@"An error occured when deleting images: {evt2.Error.Message}", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                var imageList2 = (List<string>)evt2.Result;
                                if (imageList2.Any())
                                {
                                    MessageBox.Show(this, $"The following images where not deleted:\n{string.Join("\n", imageList2)}", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }

                                CleanImagesListviews(originalImageList.ToList(), imageList2);
                            }
                        });
                    }
                }
            });
        }

        private void TsbCloseThisTabClick(object sender, EventArgs e)
        {
            CloseTool();
        }

        private void tsbLoad_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == loadFromASolutionToolStripMenuItem)
            {
                ExecuteMethod(DoAction, true);
            }
            else
            {
                ExecuteMethod(DoAction, false);
            }
        }

        private void tsbOptimizeIcons_Click(object sender, EventArgs e)
        {
            var webResources = listViewWebRessources32.Items.Cast<ListViewItem>().Select(i => (Entity)i.Tag).ToList();

            var dlg = new IconOptimizer(webResources, Service);
            dlg.ShowDialog();
        }

        private void tsbToggleBackground_Click(object sender, EventArgs e)
        {
            listViewWebRessources16.BackColor = listViewWebRessources16.BackColor == Color.FromName("Window")
                ? Color.Black
                : Color.FromName("Window");

            listViewWebRessources16.ForeColor = listViewWebRessources16.ForeColor == Color.FromName("WindowText")
                ? Color.White
                : Color.FromName("WindowText");

            listViewWebRessources32.BackColor = listViewWebRessources32.BackColor == Color.FromName("Window")
                ? Color.Black
                : Color.FromName("Window");

            listViewWebRessources32.ForeColor = listViewWebRessources32.ForeColor == Color.FromName("WindowText")
                ? Color.White
                : Color.FromName("WindowText");

            listViewWebRessourcesOther.BackColor = listViewWebRessourcesOther.BackColor == Color.FromName("Window")
                ? Color.Black
                : Color.FromName("Window");

            listViewWebRessourcesOther.ForeColor = listViewWebRessourcesOther.ForeColor == Color.FromName("WindowText")
               ? Color.White
               : Color.FromName("WindowText");

            lvVectorWebresources.BackColor = lvVectorWebresources.BackColor == Color.FromName("Window")
                ? Color.Black
                : Color.FromName("Window");

            lvVectorWebresources.ForeColor = lvVectorWebresources.ForeColor == Color.FromName("WindowText")
                ? Color.White
                : Color.FromName("WindowText");
        }
    }
}