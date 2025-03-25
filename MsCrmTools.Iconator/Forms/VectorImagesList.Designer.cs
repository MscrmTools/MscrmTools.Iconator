namespace MsCrmTools.Iconator.Forms
{
    partial class VectorImagesList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lvImages = new System.Windows.Forms.ListView();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvImages
            // 
            this.lvImages.AllowDrop = true;
            this.lvImages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvImages.HideSelection = false;
            this.lvImages.Location = new System.Drawing.Point(0, 36);
            this.lvImages.Name = "lvImages";
            this.lvImages.Size = new System.Drawing.Size(1467, 914);
            this.lvImages.TabIndex = 0;
            this.lvImages.UseCompatibleStateImageBehavior = false;
            this.lvImages.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.lvImages_ItemDrag);
            this.lvImages.SelectedIndexChanged += new System.EventHandler(this.lvImages_SelectedIndexChanged);
            this.lvImages.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvImages_DragDrop);
            this.lvImages.DragOver += new System.Windows.Forms.DragEventHandler(this.lvImages_DragOver);
            // 
            // pnlSearch
            // 
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Controls.Add(this.label1);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.pnlSearch.Size = new System.Drawing.Size(1467, 36);
            this.pnlSearch.TabIndex = 5;
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.Location = new System.Drawing.Point(75, 6);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(1392, 26);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "Search : ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // VectorImagesList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1467, 950);
            this.Controls.Add(this.lvImages);
            this.Controls.Add(this.pnlSearch);
            this.Name = "VectorImagesList";
            this.Text = "Images";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VectorImagesList_FormClosing);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvImages;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label1;
    }
}