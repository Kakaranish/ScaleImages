using System.Threading.Tasks;
using ScaleImages.ImageDirectoryResizer;

namespace ScaleImages.App
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var imageResizer = new RecursiveImageDirectoryResizer();

            var imgRootDirPath = @"E:\Misc\zdjecia-compressed";
            await imageResizer.DownscaleImages(imgRootDirPath, 4);
        }
    }
}
