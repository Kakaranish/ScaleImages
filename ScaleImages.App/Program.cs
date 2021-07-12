using System.Threading.Tasks;

namespace ScaleImages.App
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var imageResizer = new ImageDirectoryResizer();

            var imgRootDirPath = @"E:\Misc\zdjecia-compressed";
            await imageResizer.DownscaleImages(imgRootDirPath, 4);
        }
    }
}
