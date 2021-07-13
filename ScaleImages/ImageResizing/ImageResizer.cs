using System;
using System.Drawing;
using ScaleImages.ImageScaling;

namespace ScaleImages.ImageResizing
{
    public class ImageResizer : IImageResizer
    {
        private readonly IImageScaler _imageScaler;

        internal ImageResizer(IImageScaler imageScaler)
        {
            _imageScaler = imageScaler ?? throw new ArgumentNullException(nameof(imageScaler));
        }

        public ImageResizer() : this(new ImageScaler())
        {
        }

        public Image DownscaleImage(Image image, decimal downscaleTimes)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));
            if (downscaleTimes < 1) throw new ArgumentOutOfRangeException(nameof(downscaleTimes));

            var scaleFactor = 1 / downscaleTimes;
            return _imageScaler.ScaleBy(image, scaleFactor);
        }

        public Image ResizeImageByWidth(Image image, int width)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));
            if (width < 1) throw new ArgumentOutOfRangeException(nameof(width));

            var scaleFactor = image.Width < width ? 1 : (decimal)width / image.Width;

            return _imageScaler.ScaleBy(image, scaleFactor);
        }

        public Image ResizeImageByHeight(Image image, int height)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));
            if (height < 1) throw new ArgumentOutOfRangeException(nameof(height));

            var scaleFactor = image.Height < height ? 1 : (decimal)height / image.Height;

            return _imageScaler.ScaleBy(image, scaleFactor);
        }
    }
}
