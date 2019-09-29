using System;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace Sara.Common.Ext.ImageHelper
{
    public static class ImageExt
    {
        public static string ToBase64(this Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                var imageBytes = ms.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }
        public static Image ToImage(this string base64String)
        {
            var imageBytes = Convert.FromBase64String(base64String);
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                ms.Write(imageBytes, 0, imageBytes.Length);
                return Image.FromStream(ms, true);
            }
        }
        public static BitmapImage ToBitmapImage(this Image image)
        {
            using (var stream = new MemoryStream())
            {
                image.Save(stream, image.RawFormat);

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream(stream.ToArray());
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

    }
}
