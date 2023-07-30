using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ScaleImages.ImageResizing;

namespace ScaleImages.ImageDirectoryResizer;

public abstract class ImageDirectoryResizerBase
{
    private readonly SemaphoreSlim _semaphore;
    private readonly IImageResizer _imageResizer;

    private static readonly HashSet<string> ValidExtensions = new(new[]
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".bmp",
        ".gif"
    });

    protected ImageDirectoryResizerBase() : this(int.MaxValue)
    {
    }

    protected ImageDirectoryResizerBase(int degreeOfParallelism) : this(new ImageResizer())
    {
        if (degreeOfParallelism <= 0) throw new ArgumentOutOfRangeException(nameof(degreeOfParallelism));
        _semaphore = new SemaphoreSlim(degreeOfParallelism);
    }

    internal ImageDirectoryResizerBase(IImageResizer imageResizer)
    {
        _imageResizer = imageResizer;
    }

    public async Task DownscaleImages(string rootDir, decimal downscaleTimes)
    {
        Image DownscaleAction(Image image) => _imageResizer.DownscaleImage(image, downscaleTimes);

        await ProcessImages(rootDir, DownscaleAction);
    }

    public async Task ResizeImagesByWidth(string rootDir, int width)
    {
        Image ResizeAction(Image image) => _imageResizer.ResizeImageByWidth(image, width);

        await ProcessImages(rootDir, ResizeAction);
    }
    public async Task ResizeImagesByHeight(string rootDir, int height)
    {
        Image ResizeAction(Image image) => _imageResizer.ResizeImageByHeight(image, height);

        await ProcessImages(rootDir, ResizeAction);
    }

    protected abstract Task<IEnumerable<Task>> CreateImageProcessingTasks(string rootPath, string dirPath,
        string outputDirRootPath, Func<Image, Image> resizeAction);

    protected async Task ProcessImages(string rootDir, Func<Image, Image> resizeAction)
    {
        if (!Directory.Exists(rootDir))
        {
            throw new ArgumentException($"There is no accessible dir {rootDir}");
        }

        var outputDirPath = rootDir + "-" + DateTime.Now.ToString("yy-MM-ddTHH-mm-ss");
        Directory.CreateDirectory(outputDirPath);

        var resizeTasks = (await CreateImageProcessingTasks(rootDir, rootDir, outputDirPath, resizeAction)).ToList();

        Console.WriteLine($"[INFO] {resizeTasks.Count} images to resize\n\n");

        await Task.WhenAll(resizeTasks);
    }

    protected async Task ProcessFile(string filePath, string outputDirPath, Func<Image, Image> resizeAction)
    {
        await _semaphore.WaitAsync();

        var fileExtension = Path.GetExtension(filePath);
        if (!ValidExtensions.Contains(fileExtension)) return;

        await Task.Run(() =>
        {
            using var imageToProcess = Image.FromFile(filePath);
            using var processedImage = resizeAction(imageToProcess);

            var outputFilePath = Path.Combine(outputDirPath, Path.GetFileName(filePath));
            processedImage.Save(outputFilePath, imageToProcess.RawFormat);

            Console.WriteLine($"[INFO] Processed {filePath}");

            _semaphore.Release();
        });
    }
}