using System.Drawing;

namespace ScaleImages.ImageResizing
{
    public interface IImageResizer
    {
        Image DownscaleImage(Image image, decimal downscaleTimes);
        Image ResizeImageByWidth(Image image, int width);
        Image ResizeImageByHeight(Image image, int height);
    }
}