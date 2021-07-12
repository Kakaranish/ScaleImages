using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ScaleImages
{
    public class ImageDirectoryResizer : IImageDirectoryResizer
    {
        private static readonly HashSet<string> ValidExtensions = new(new[]
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".bmp",
            ".gif"
        });

        private readonly SemaphoreSlim _semaphore;
        private readonly ImageResizer _imageResizer;

        public ImageDirectoryResizer() : this(int.MaxValue)
        {
        }

        public ImageDirectoryResizer(int degreeOfParallelism)
        {
            if (degreeOfParallelism <= 0) throw new ArgumentOutOfRangeException(nameof(degreeOfParallelism));

            _semaphore = new SemaphoreSlim(degreeOfParallelism);
            _imageResizer = new ImageResizer();
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

        private async Task ProcessImages(string rootDir, Func<Image, Image> resizeAction)
        {
            if (!Directory.Exists(rootDir))
            {
                throw new ArgumentException($"There is no accessible dir {rootDir}");
            }

            var outputDirPath = rootDir + "-" + DateTime.Now.ToString("yy-MM-ddTHH-mm-ss");
            Directory.CreateDirectory(outputDirPath);

            var resizeTasks = await ExtractProcessingTasksFromDirectory(rootDir, rootDir, outputDirPath, resizeAction);
            await Task.WhenAll(resizeTasks);
        }

        private async Task<IEnumerable<Task>> ExtractProcessingTasksFromDirectory(string rootPath, string dirPath,
            string outputDirRootPath, Func<Image, Image> resizeAction)
        {
            var outputDir = Path.GetFullPath(Path.Combine(outputDirRootPath, Path.GetRelativePath(rootPath, dirPath)));
            if (!Directory.Exists(outputDir)) Directory.CreateDirectory(outputDir);

            var tasks = new List<Task>();

            foreach (var filePath in Directory.GetFiles(dirPath))
            {
                tasks.Add(ProcessFile(filePath, outputDir, resizeAction));
            }

            foreach (var subDirectory in Directory.GetDirectories(dirPath))
            {
                tasks.AddRange(await ExtractProcessingTasksFromDirectory(rootPath, subDirectory, outputDirRootPath, resizeAction));
            }

            return tasks;
        }

        private async Task ProcessFile(string filePath, string outputDirPath, Func<Image, Image> resizeAction)
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

                _semaphore.Release();
            });
        }
    }
}
