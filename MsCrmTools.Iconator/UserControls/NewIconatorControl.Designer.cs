namespace MsCrmTools.Iconator.UserControls
{
    partial class NewIconatorControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.tsbLoad = new System.Windows.Forms.ToolStripButton();
            this.tssTables = new System.Windows.Forms.ToolStripSeparator();
            this.tsbApplyImages = new System.Windows.Forms.ToolStripButton();
            this.tsbRemovePendingAssociation = new System.Windows.Forms.ToolStripButton();
            this.tsbRemoveImage = new System.Windows.Forms.ToolStripButton();
            this.tssImages = new System.Windows.Forms.ToolStripSeparator();
            this.tsbAddImages = new System.Windows.Forms.ToolStripButton();
            this.tsbDeleteImages = new System.Windows.Forms.ToolStripButton();
            this.dpMain = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.tsMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsMain
            // 
            this.tsMain.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbLoad,
            this.tssTables,
            this.tsbApplyImages,
            this.tsbRemovePendingAssociation,
            this.tsbRemoveImage,
            this.tssImages,
            this.tsbAddImages,
            this.tsbDeleteImages});
            this.tsMain.Location = new System.Drawing.Point(0, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(1812, 41);
            this.tsMain.TabIndex = 0;
            this.tsMain.Text = "toolStrip1";
            // 
            // tsbLoad
            // 
            this.tsbLoad.Image = global::MsCrmTools.Iconator.Properties.Resources.Dataverse_32x32;
            this.tsbLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLoad.Name = "tsbLoad";
            this.tsbLoad.Size = new System.Drawing.Size(87, 36);
            this.tsbLoad.Text = "Load";
            this.tsbLoad.ToolTipText = "Load tables and vector images from your Dataverse environment";
            this.tsbLoad.Click += new System.EventHandler(this.tsbLoad_Click);
            // 
            // tssTables
            // 
            this.tssTables.Name = "tssTables";
            this.tssTables.Size = new System.Drawing.Size(6, 41);
            // 
            // tsbApplyImages
            // 
            this.tsbApplyImages.Enabled = false;
            this.tsbApplyImages.Image = global::MsCrmTools.Iconator.Properties.Resources.Startup32;
            this.tsbApplyImages.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbApplyImages.Name = "tsbApplyImages";
            this.tsbApplyImages.Size = new System.Drawing.Size(95, 36);
            this.tsbApplyImages.Text = "Apply";
            this.tsbApplyImages.ToolTipText = "Apply pending changes to your Dataverse environment";
            this.tsbApplyImages.Click += new System.EventHandler(this.tsbApplyImages_Click);
            // 
            // tsbRemovePendingAssociation
            // 
            this.tsbRemovePendingAssociation.Enabled = false;
            this.tsbRemovePendingAssociation.Image = global::MsCrmTools.Iconator.Properties.Resources.Eraser32;
            this.tsbRemovePendingAssociation.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRemovePendingAssociation.Name = "tsbRemovePendingAssociation";
            this.tsbRemovePendingAssociation.Size = new System.Drawing.Size(237, 36);
            this.tsbRemovePendingAssociation.Text = "Remove pending image";
            this.tsbRemovePendingAssociation.ToolTipText = "Remove pending image for the selected table(s)";
            this.tsbRemovePendingAssociation.Click += new System.EventHandler(this.tsbRemovePendingAssociation_Click);
            // 
            // tsbRemoveImage
            // 
            this.tsbRemoveImage.Enabled = false;
            this.tsbRemoveImage.Image = global::MsCrmTools.Iconator.Properties.Resources.No32;
            this.tsbRemoveImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRemoveImage.Name = "tsbRemoveImage";
            this.tsbRemoveImage.Size = new System.Drawing.Size(166, 36);
            this.tsbRemoveImage.Text = "Remove image";
            this.tsbRemoveImage.ToolTipText = "Remove image for the selected table in your Dataverse environment";
            this.tsbRemoveImage.Click += new System.EventHandler(this.tsbRemoveImage_Click);
            // 
            // tssImages
            // 
            this.tssImages.Name = "tssImages";
            this.tssImages.Size = new System.Drawing.Size(6, 41);
            // 
            // tsbAddImages
            // 
            this.tsbAddImages.Enabled = false;
            this.tsbAddImages.Image = global::MsCrmTools.Iconator.Properties.Resources.NewToInstall;
            this.tsbAddImages.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddImages.Name = "tsbAddImages";
            this.tsbAddImages.Size = new System.Drawing.Size(155, 36);
            this.tsbAddImages.Text = "Add Image(s)";
            this.tsbAddImages.ToolTipText = "Add Image(s) to your Dataverse environment.\r\n\r\nYou can also directly drag and dro" +
    "p files from your drive to the list of images";
            this.tsbAddImages.Click += new System.EventHandler(this.tsbAddImages_Click);
            // 
            // tsbDeleteImages
            // 
            this.tsbDeleteImages.Enabled = false;
            this.tsbDeleteImages.Image = global::MsCrmTools.Iconator.Properties.Resources.delete;
            this.tsbDeleteImages.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDeleteImages.Name = "tsbDeleteImages";
            this.tsbDeleteImages.Size = new System.Drawing.Size(171, 36);
            this.tsbDeleteImages.Text = "Delete Image(s)";
            this.tsbDeleteImages.ToolTipText = "Delete Image(s) from your Dataverse environment.\r\n\r\nIf the image still has depend" +
    "encies, the deletion will fail";
            this.tsbDeleteImages.Click += new System.EventHandler(this.tsbDeleteImages_Click);
            // 
            // dpMain
            // 
            this.dpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpMain.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
            this.dpMain.Location = new System.Drawing.Point(0, 41);
            this.dpMain.Name = "dpMain";
            this.dpMain.Size = new System.Drawing.Size(1812, 1169);
            this.dpMain.TabIndex = 1;
            // 
            // NewIconatorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dpMain);
            this.Controls.Add(this.tsMain);
            this.Name = "NewIconatorControl";
            this.Size = new System.Drawing.Size(1812, 1210);
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsMain;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dpMain;
        private System.Windows.Forms.ToolStripButton tsbLoad;
        private System.Windows.Forms.ToolStripSeparator tssTables;
        private System.Windows.Forms.ToolStripButton tsbApplyImages;
        private System.Windows.Forms.ToolStripButton tsbRemoveImage;
        private System.Windows.Forms.ToolStripSeparator tssImages;
        private System.Windows.Forms.ToolStripButton tsbAddImages;
        private System.Windows.Forms.ToolStripButton tsbRemovePendingAssociation;
        private System.Windows.Forms.ToolStripButton tsbDeleteImages;
    }
}
