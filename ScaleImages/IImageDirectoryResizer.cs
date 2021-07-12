using System.Threading.Tasks;

namespace ScaleImages
{
    public interface IImageDirectoryResizer
    {
        Task DownscaleImages(string rootDir, decimal downscaleTimes);

        Task ResizeImagesByWidth(string rootDir, int width);

        Task ResizeImagesByHeight(string rootDir, int height);
    }
}