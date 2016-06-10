using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MsCrmTools.Iconator.AppCode;
using Microsoft.Xrm.Sdk;

namespace MsCrmTools.Iconator.UserControls
{
    public partial class ImageOptimizerControl : UserControl
    {
        Image originalImage;
        Image optimizedImage;
        Entity webResource;

        public ImageOptimizerControl(Entity webResource)
        {
            InitializeComponent();

            this.webResource = webResource;
            originalImage = ImageHelper.ConvertWebResContent(webResource.GetAttributeValue<string>("content"));

            pbOriginalImage.Image = originalImage;

            if (HasTransparentBorder(originalImage)) { Visible = false; }

            var resizedImage = ImageHelper.Resize(originalImage, 28, 28);
            optimizedImage = ImageHelper.GetCenteredImage(resizedImage, 32, 32);

            pbOptimizedImage.Image = optimizedImage;
        }

        private bool HasTransparentBorder(Image image)
        {
            var bmp = new Bitmap(image);
            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    if (i > 1 && i < bmp.Height - 2 && j > 1 && j < bmp.Width - 2)
                    {
                        continue;
                    }

                    var color = bmp.GetPixel(i, j);
                    if (color != Color.FromArgb(0, 0, 0, 0))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool Checked { get { return chkSelected.Checked; } }
        public Image OriginalImage { get { return originalImage; } }
        public Image OptimizedImage { get { return optimizedImage; } }
        public Entity WebResource{ get { return webResource; } }

        public void SetPanelBackgroundImage(Color color)
        {
            BackColor = color;   
        }

        public void SetIconBackgroundImage(Color color)
        {
            pbOriginalImage.BackColor = color;
            pbOptimizedImage.BackColor = color;
        }
    }
}
