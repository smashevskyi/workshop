using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace MyWorkshop.Helpers
{
    public class ImageHelper
    {
        public bool SaveToFolder(HttpPostedFileBase image, string pathToSave, out string fileName)
        {

            if(IsImage(image) == false)
            {
                fileName = null;
                return false;
            }

            fileName = string.Format(@"{0}{1}", DateTime.Now.Ticks, System.IO.Path.GetExtension(image.FileName));
            image.SaveAs(pathToSave + "/Original/" + fileName);


            // One of the way to create thumbnail. Low file size. Longer processing.

            //using (MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(pathToSave + "/Original/" + fileName)))
            //{
            //    var thumbnail = new WebImage(stream).Resize(151, 151, false);
            //    thumbnail.Crop(1, 1);
            //    thumbnail.Save(pathToSave + "/Thumbs/" + fileName, "jpg");
            //}

            using (Bitmap a = new Bitmap(image.InputStream))
            {
                //ResizeImage(a, 200, 200, pathToSave + "/Thumbs/" + fileName);
                ScaleAndResize(a, 200, pathToSave + "/Thumbs/" + fileName);
                ScaleAndResize(a, 600, pathToSave + "/Resized/" + fileName);
            }
            return true;
        }


        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public void ResizeImage(Image image, int width, int height, string path)
        {
            int quality = 90;
            System.Drawing.Imaging.EncoderParameters encoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
            encoderParameters.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);

            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            destImage.Save(path, GetImageCodeInfo("image/jpeg"), encoderParameters);
        }

        public void ScaleAndResize(Image image, int maxWidth, string path)
        {
            var ratio = (double)maxWidth / image.Width;
            //var ratioY = (double)maxHeight / image.Height;
            //var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            ResizeImage(image, newWidth, newHeight, path);
        }

        public static ImageCodecInfo GetImageCodeInfo(string mimeType)
        {
            ImageCodecInfo[] info = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in info)
                if (ici.MimeType.Equals(mimeType, StringComparison.OrdinalIgnoreCase))
                    return ici;
            return null;
        }

        private bool IsImage(HttpPostedFileBase file)
        {
            if (file.ContentType.Contains("image"))
            {
                return true;
            }

            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" };

            // linq from Henrik Stenbæk
            return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }

    }
}