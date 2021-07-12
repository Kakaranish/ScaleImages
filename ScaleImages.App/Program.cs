using System.Threading.Tasks;

namespace ScaleImages.App
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var imageResizer = new ImageResizer();

            var imgRootDirPath = @"E:\Misc\zdjecia-compressed";
            await imageResizer.ResizeImagesInDirsRecursively(imgRootDirPath, 0.5m);
        }
    }
}
