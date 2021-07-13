using System;
using System.Drawing;

namespace ScaleImages
{
    internal class ImageScaler : IImageScaler
    {
        public Image ScaleBy(Image image, decimal scaleFactor)
        {
            if (scaleFactor <= 0) throw new ArgumentOutOfRangeException(nameof(scaleFactor));
            if (scaleFactor == 1) return image; // Nothing to scale

            var newImageSize = new Size((int)Math.Floor(image.Size.Width * scaleFactor),
                (int)Math.Floor(image.Size.Height * scaleFactor));
            var resizedImage = (Image)new Bitmap(image, newImageSize);

            return resizedImage;
        }
    }
}
