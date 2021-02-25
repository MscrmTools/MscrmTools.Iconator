// PROJECT : MsCrmTools.Iconator
// This project was developed by Tanguy Touzard
// CODEPLEX: http://xrmtoolbox.codeplex.com
// BLOG: http://mscrmtools.blogspot.com

using Svg;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace MsCrmTools.Iconator.AppCode
{
    public static class ImageHelper
    {
        /// <summary>
        /// Convert a WebResource to Image displayable into listviews
        /// </summary>
        /// <param name="contentImageList">Base 64 code</param>
        /// <param name="isVector">Is Vector image</param>
        /// <returns>Image</returns>
        public static Image ConvertWebResContent(string contentImageList, bool isVector)
        {
            try
            {
                var imageBytes = Convert.FromBase64String(contentImageList);

                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    if (isVector)
                    {
                        SvgDocument svgDoc = SvgDocument.Open<SvgDocument>(ms);

                        return new Bitmap(svgDoc.Draw(48, 48), new Size(48, 48));
                    }

                    return Image.FromStream(ms, true, true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error on ConvertWebResContent method : {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public static Bitmap GetCenteredImage(Image image, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g2 = Graphics.FromImage(bmp))
            {
                g2.DrawImage(image, 2, 2);
            }

            return bmp;
        }

        /// <summary>
        /// method for resizing an image
        /// </summary>
        /// <param name="img">the image to resize</param>
        /// <param name="width">new width of the image </param>
        /// <param name="height">new height of the image</param>
        /// <returns></returns>
        public static Image Resize(Image originalImage, int w, int h)
        {
            //Original Image attributes
            int originalWidth = originalImage.Width;
            int originalHeight = originalImage.Height;

            // Figure out the ratio
            double ratioX = (double)w / (double)originalWidth;
            double ratioY = (double)h / (double)originalHeight;
            // use whichever multiplier is smaller
            double ratio = ratioX < ratioY ? ratioX : ratioY;

            // now we can get the new height and width
            int newHeight = Convert.ToInt32(originalHeight * ratio);
            int newWidth = Convert.ToInt32(originalWidth * ratio);

            Image thumbnail = new Bitmap(newWidth, newHeight);
            Graphics graphic = Graphics.FromImage(thumbnail);

            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphic.CompositingQuality = CompositingQuality.HighQuality;

            graphic.Clear(Color.Transparent);
            graphic.DrawImage(originalImage, 0, 0, newWidth, newHeight);

            return thumbnail;
        }
    }
}