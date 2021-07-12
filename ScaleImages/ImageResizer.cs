using System;
using System.Drawing;

namespace ScaleImages
{
    public class ImageResizer
    {
        public Image ResizeImageByWidth(Image image, int width)
        {
            var scaleFactor = image.Width < width ? 1 : (decimal)width / image.Width;
            if (scaleFactor == 1) return image; // Nothing to scale

            var newImageSize = new Size((int)Math.Floor(image.Size.Width * scaleFactor),
                (int)Math.Floor(image.Size.Height * scaleFactor));
            var resizedImage = (Image)new Bitmap(image, newImageSize);

            return resizedImage;
        }

        public Image ResizeImageByHeight(Image image, int height)
        {
            var scaleFactor = image.Height < height ? 1 : (decimal)height / image.Height;
            if (scaleFactor == 1) return image; // Nothing to scale

            var newImageSize = new Size((int)Math.Floor(image.Size.Width * scaleFactor),
                (int)Math.Floor(image.Size.Height * scaleFactor));
            var resizedImage = (Image)new Bitmap(image, newImageSize);

            return resizedImage;
        }

        public Image DownscaleImage(Image image, decimal downscaleTimes)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));
            if (downscaleTimes < 1) throw new ArgumentOutOfRangeException(nameof(downscaleTimes));
            if (downscaleTimes == 1) return image; // Nothing to scale

            var newImageSize = new Size((int)Math.Floor(image.Size.Width / downscaleTimes),
                (int)Math.Floor(image.Size.Height / downscaleTimes));
            var resizedImage = (Image)new Bitmap(image, newImageSize);

            return resizedImage;
        }
    }
}
