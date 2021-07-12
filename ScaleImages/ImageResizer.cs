using System;
using System.Drawing;

namespace ScaleImages
{
    public class ImageResizer
    {
        public Image ResizeImageByWidth(Image image, int width)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));
            var scaleFactor = image.Width < width ? 1 : (decimal)width / image.Width;

            return InternalScaleBy(image, scaleFactor);
        }

        public Image ResizeImageByHeight(Image image, int height)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));
            var scaleFactor = image.Height < height ? 1 : (decimal)height / image.Height;

            return InternalScaleBy(image, scaleFactor);
        }

        public Image DownscaleImage(Image image, decimal downscaleTimes)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));
            if (downscaleTimes < 1) throw new ArgumentOutOfRangeException(nameof(downscaleTimes));

            var scaleFactor = 1 / downscaleTimes;
            return InternalScaleBy(image, scaleFactor);
        }

        private static Image InternalScaleBy(Image image, decimal scaleFactor)
        {
            if (scaleFactor == 1) return image; // Nothing to scale

            var newImageSize = new Size((int)Math.Floor(image.Size.Width * scaleFactor),
                (int)Math.Floor(image.Size.Height * scaleFactor));
            var resizedImage = (Image)new Bitmap(image, newImageSize);

            return resizedImage;
        }
    }
}
