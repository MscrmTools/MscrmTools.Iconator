namespace MsCrmTools.Iconator.Forms
{
    partial class TableList
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
            this.components = new System.ComponentModel.Container();
            this.lvTables = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvTables
            // 
            this.lvTables.AllowDrop = true;
            this.lvTables.CheckBoxes = true;
            this.lvTables.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lvTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvTables.FullRowSelect = true;
            this.lvTables.HideSelection = false;
            this.lvTables.Location = new System.Drawing.Point(0, 36);
            this.lvTables.Name = "lvTables";
            this.lvTables.OwnerDraw = true;
            this.lvTables.Size = new System.Drawing.Size(800, 519);
            this.lvTables.SmallImageList = this.imageList1;
            this.lvTables.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvTables.TabIndex = 2;
            this.lvTables.UseCompatibleStateImageBehavior = false;
            this.lvTables.View = System.Windows.Forms.View.Details;
            this.lvTables.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.lvTables_DrawColumnHeader);
            this.lvTables.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.lvTables_DrawItem);
            this.lvTables.SelectedIndexChanged += new System.EventHandler(this.lvTables_SelectedIndexChanged);
            this.lvTables.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvTables_DragDrop);
            this.lvTables.DragOver += new System.Windows.Forms.DragEventHandler(this.lvTables_DragOver);
            this.lvTables.Resize += new System.EventHandler(this.lvTables_Resize);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 150;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 50);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox1.Location = new System.Drawing.Point(0, 555);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(800, 67);
            this.textBox1.TabIndex = 3;
            this.textBox1.Visible = false;
            // 
            // pnlSearch
            // 
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Controls.Add(this.label1);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.pnlSearch.Size = new System.Drawing.Size(800, 36);
            this.pnlSearch.TabIndex = 4;
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.Location = new System.Drawing.Point(75, 6);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(725, 26);
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
            // TableList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 622);
            this.Controls.Add(this.lvTables);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.textBox1);
            this.Name = "TableList";
            this.Text = "Tables";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TableList_FormClosing);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvTables;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label1;
    }
}