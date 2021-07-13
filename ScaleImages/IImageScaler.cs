using System.Drawing;

namespace ScaleImages
{
    internal interface IImageScaler
    {
        Image ScaleBy(Image image, decimal scaleFactor);
    }
}