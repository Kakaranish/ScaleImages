using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ScaleImages
{
    public class ImageResizer
    {
        private readonly SemaphoreSlim _semaphore;

        private readonly string[] _validExtensions = {
            ".jpg",
            ".jpeg",
            ".png",
            ".bmp",
            ".gif"
        };

        public ImageResizer() : this(int.MaxValue)
        {
        }

        public ImageResizer(int degreeOfParallelism)
        {
            if (degreeOfParallelism <= 0) throw new ArgumentOutOfRangeException(nameof(degreeOfParallelism));

            _semaphore = new SemaphoreSlim(degreeOfParallelism);
        }

        public async Task ResizeImagesInDirsRecursively(string rootDir, decimal ratio)
        {
            if (!Directory.Exists(rootDir))
            {
                throw new ArgumentException($"There is no accessible dir {rootDir}");
            }

            var outputDirPath = rootDir + "-" + DateTime.Now.ToString("yy-MM-ddTHH-mm-ss");
            Directory.CreateDirectory(outputDirPath);

            var resizeTasks = await ProcessDirectory(rootDir, rootDir, outputDirPath, ratio);
            foreach (var resizeTask in resizeTasks)
            {
                await resizeTask;
            }
        }

        private async Task<IEnumerable<Task>> ProcessDirectory(string rootPath, string dirPath, string outputDirRootPath, decimal ratio)
        {
            var outputDir = Path.GetFullPath(Path.Combine(outputDirRootPath, Path.GetRelativePath(rootPath, dirPath)));
            if (!Directory.Exists(outputDir)) Directory.CreateDirectory(outputDir);

            var tasks = new List<Task>();

            foreach (var filePath in Directory.GetFiles(dirPath))
            {
                tasks.Add(ProcessFile(filePath, outputDir, ratio));
            }

            foreach (var subDirectory in Directory.GetDirectories(dirPath))
            {
                tasks.AddRange(await ProcessDirectory(rootPath, subDirectory, outputDirRootPath, ratio));
            }

            return tasks;
        }

        private async Task ProcessFile(string filePath, string outputDirPath, decimal ratio)
        {
            var fileExtension = Path.GetExtension(filePath);
            if (!_validExtensions.Contains(fileExtension)) return;

            await _semaphore.WaitAsync();

            await Task.Run(() =>
            {
                using var imageToProcess = Image.FromFile(filePath);
                var newImageSize = new Size((int)Math.Floor(imageToProcess.Size.Width * ratio),
                    (int)Math.Floor(imageToProcess.Size.Height * ratio));

                using var processedImage = (Image)new Bitmap(imageToProcess, newImageSize);
                var outputFilePath = Path.Combine(outputDirPath, Path.GetFileName(filePath));
                processedImage.Save(outputFilePath, imageToProcess.RawFormat);
            });

            _semaphore.Release();
        }
    }
}
