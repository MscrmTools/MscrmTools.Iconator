using MsCrmTools.Iconator.AppCode;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MsCrmTools.Iconator.Forms
{
    public partial class TableList
    {
        internal void Clear()
        {
            lvTables.Items.Clear();
        }

        private void lvTables_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void lvTables_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            var ti = (TableInfo)e.Item.Tag;
            SolidBrush foreBrush = new SolidBrush(Color.Black);
            SolidBrush lineBrush = new SolidBrush(Color.White);
            SolidBrush backBrush = new SolidBrush(Color.White);
            if (((TableInfo)e.Item.Tag).IsPending)
            {
                foreBrush = new SolidBrush(Color.FromArgb(122, 116, 93));
                lineBrush = new SolidBrush(Color.FromArgb(122, 116, 93));
                backBrush = new SolidBrush(Color.FromArgb(253, 244, 207));
            }
            else
            {
                if (e.Item.Selected)
                {
                    foreBrush = new SolidBrush(Color.FromArgb(0, 104, 254));
                    lineBrush = new SolidBrush(Color.FromArgb(0, 104, 254));
                    backBrush = new SolidBrush(Color.FromArgb(170, 230, 255));
                }
            }

            var forePen = new Pen(foreBrush);
            var backPen = new Pen(backBrush);
            var linePen = new Pen(lineBrush);

            e.Graphics.FillRectangle(backBrush, e.Bounds);

            e.Graphics.DrawLine(linePen, e.Bounds.X, e.Bounds.Y, e.Bounds.X + e.Bounds.Width, e.Bounds.Y);
            e.Graphics.DrawLine(linePen, e.Bounds.X + e.Bounds.Width - 1, e.Bounds.Y, e.Bounds.X + e.Bounds.Width - 1, e.Bounds.Y + e.Bounds.Height);
            e.Graphics.DrawLine(linePen, e.Bounds.X, e.Bounds.Y + e.Bounds.Height - 1, e.Bounds.X + e.Bounds.Width, e.Bounds.Y + e.Bounds.Height - 1);
            e.Graphics.DrawLine(linePen, e.Bounds.X, e.Bounds.Y, e.Bounds.X, e.Bounds.Y + e.Bounds.Height);

            Image img = null;
            if (ti.IsPending && ti.PendingWebResourceImage != null)
            {
                img = new Bitmap(ti.PendingWebResourceImage, new Size(40, 40));
            }
            else if (!string.IsNullOrEmpty(ti.WebResourceName))
            {
                var existingImage = Images.FirstOrDefault(i => i.Name == ti.WebResourceName)?.Image;

                img = new Bitmap(existingImage, new Size(40, 40));
            }
            else
            {
                img = new Bitmap(Properties.Resources.puzzle, new Size(40, 40));
            }

            e.Graphics.DrawImage(img, new Point(4, e.Bounds.Y + ((e.Bounds.Height - 40) / 2)));

            var size = e.Graphics.MeasureString(e.Item.Text, e.Item.Font);

            var titleFont = new Font(e.Item.Font, FontStyle.Bold);
            var werFont = new Font(e.Item.Font, FontStyle.Italic);

            var wrStateString = (ti.PendingWebResourceName) ?? ti.WebResourceName ?? "Vector image not set";
            if (wrStateString == ti.PendingWebResourceName) wrStateString += " (pending)";

            e.Graphics.DrawString(e.Item.Text, titleFont, foreBrush, new Point(60, e.Bounds.Y + 4));
            e.Graphics.DrawString(ti.Entity.SchemaName, e.Item.Font, foreBrush, new Point(60, e.Bounds.Y + 20));
            e.Graphics.DrawString(wrStateString, werFont, foreBrush, new Point(60, e.Bounds.Y + 34));
        }
    }
}