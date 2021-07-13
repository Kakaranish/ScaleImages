using System.Drawing;

namespace ScaleImages.ImageScaling
{
    internal interface IImageScaler
    {
        Image ScaleBy(Image image, decimal scaleFactor);
    }
}