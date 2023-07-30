using System.Threading.Tasks;
using ScaleImages.ImageDirectoryResizer;

namespace ScaleImages.App;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var extractedConsoleArguments = ConsoleArgumentsExtractor.Extract(args);
        
        var imageResizer = new RecursiveImageDirectoryResizer();
        
        await imageResizer.DownscaleImages(
            extractedConsoleArguments.SourceDirPath, 
            extractedConsoleArguments.DownscaleRatio);
    }
}