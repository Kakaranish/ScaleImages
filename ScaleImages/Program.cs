using System.Threading.Tasks;

namespace ScaleImages
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var imageUtility = new ImageResizer();
            var imagesDir = @"E:\Misc\zdjecia-compressed";

            await imageUtility.ResizeImagesInDirsRecursively(imagesDir, 0.5m);
        }
    }
}
