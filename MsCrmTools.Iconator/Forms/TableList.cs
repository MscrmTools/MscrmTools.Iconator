using MsCrmTools.Iconator.AppCode;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace MsCrmTools.Iconator.Forms
{
    public partial class TableList : DockContent
    {
        private Thread searchThread;
        private ListViewItem targetItem;

        public TableList()
        {
            SetStyle(
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.UserPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.DoubleBuffer,
            true);

            InitializeComponent();
        }

        public event EventHandler SelectedItemsChanged;

        public List<VectorImageInfo> Images { get; set; }
        public List<TableInfo> PendingTables => lvTables.Items.Cast<ListViewItem>().Select(i => (TableInfo)i.Tag).Where(t => t.IsPending).ToList();
        public List<TableInfo> SelectedTables => lvTables.SelectedItems.Cast<ListViewItem>().Select(i => (TableInfo)i.Tag).ToList();

        public List<TableInfo> Tables { get; set; }

        public void DisplayTables(object searchTerm = null)
        {
            Thread.Sleep(200);

            Invoke(new Action(() =>
            {
                lvTables.Items.Clear();
                lvTables.Items.AddRange(Tables.Where(t => string.IsNullOrEmpty(searchTerm?.ToString())
                || t.Entity?.DisplayName?.UserLocalizedLabel?.Label?.ToLower()?.IndexOf(searchTerm?.ToString()?.ToLower()) >= 0
                || t.Entity?.SchemaName?.ToLower()?.IndexOf(searchTerm?.ToString()?.ToLower()) >= 0
                ).Select(t => new ListViewItem { Tag = t, Text = t.Entity.DisplayName?.UserLocalizedLabel?.Label, SubItems = { new ListViewItem.ListViewSubItem { Text = t.Entity.LogicalName } } }).ToArray());
            }));
        }

        public void RefreshDisplay()
        {
            lvTables.Invalidate();
        }

        private void lvTables_DragDrop(object sender, DragEventArgs e)
        {
            if (targetItem != null)
            {
                if (e.Data.GetDataPresent(typeof(ListViewItem)))
                {
                    var item = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
                    var vii = (VectorImageInfo)item.Tag;

                    ((TableInfo)targetItem.Tag).SetPendingVectorIcon(vii.Name, vii.Image);

                    lvTables.Invalidate();
                    SelectedItemsChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        private void lvTables_DragOver(object sender, DragEventArgs e)
        {
            Point p = lvTables.PointToClient(new Point(e.X, e.Y));
            var newTargetItem = lvTables.GetItemAt(p.X, p.Y);

            if (e.Data.GetDataPresent(typeof(ListViewItem)) && newTargetItem != null)
            {
                e.Effect = DragDropEffects.Move;

                if (targetItem != newTargetItem)
                {
                    if (targetItem != null)
                    {
                        targetItem.ForeColor = SystemColors.ActiveCaptionText;
                    }

                    targetItem = newTargetItem;
                    targetItem.ForeColor = Color.Green;
                }
            }
        }

        private void lvTables_Resize(object sender, System.EventArgs e)
        {
            lvTables.Columns[0].Width = lvTables.Width - ((lvTables.Items.Count * lvTables.SmallImageList.ImageSize.Height > lvTables.Height) ? 20 : 0) - 4;
        }

        private void lvTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedItemsChanged?.Invoke(this, new EventArgs());
        }

        private void TableList_FormClosing(object sender, FormClosingEventArgs e)
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

        private void txtSearch_TextChanged(object sender, System.EventArgs e)
        {
            searchThread?.Abort();
            searchThread = new Thread(DisplayTables);
            searchThread.Start(txtSearch.Text);
        }
    }
}