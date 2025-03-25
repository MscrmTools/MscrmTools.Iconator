using MsCrmTools.Iconator.AppCode;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace MsCrmTools.Iconator.Forms
{
    public partial class VectorImagesList : DockContent
    {
        private Thread searchThread;

        public VectorImagesList()
        {
            InitializeComponent();
        }

        public event EventHandler<VectorImageAddedEventsArgs> ImagesAdded;

        public event EventHandler SelectedItemsChanged;

        public List<VectorImageInfo> Images { get; set; }
        public List<VectorImageInfo> SelectedImages => lvImages.SelectedItems.Cast<ListViewItem>().Select(i => (VectorImageInfo)i.Tag).ToList();

        public void DisplayImages(object searchTerm = null)
        {
            Invoke(new Action(() =>
            {
                lvImages.LargeImageList = new ImageList { ImageSize = new System.Drawing.Size(48, 48) };

                var filteredImages = Images.Where(i => string.IsNullOrEmpty(searchTerm?.ToString())
                    || i.Name.ToLower().IndexOf(searchTerm?.ToString().ToLower()) >= 0).ToList();

                foreach (var vi in filteredImages)
                {
                    lvImages.LargeImageList.Images.Add(vi.Name, vi.Image);
                }

                lvImages.Items.Clear();
                lvImages.Items.AddRange(filteredImages.Select(v => new ListViewItem
                {
                    Tag = v,
                    ImageIndex = lvImages.LargeImageList.Images.IndexOfKey(v.Name),
                    Text = v.Name
                }).ToArray());
            }));
        }

        internal void Clear()
        {
            lvImages.Items.Clear();
        }

        private void lvImages_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                ImagesAdded?.Invoke(this, new VectorImageAddedEventsArgs { Files = files });
            }
        }

        private void lvImages_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void lvImages_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvImages.DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void lvImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedItemsChanged?.Invoke(this, new EventArgs());
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            searchThread?.Abort();
            searchThread = new Thread(DisplayImages);
            searchThread.Start(txtSearch.Text);
        }

        private void VectorImagesList_FormClosing(object sender, FormClosingEventArgs e)
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