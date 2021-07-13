using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using ScaleImages.ImageResizing;

namespace ScaleImages.ImageDirectoryResizer
{
    public class RecursiveImageDirectoryResizer : ImageDirectoryResizerBase
    {
        #region Ctors

        public RecursiveImageDirectoryResizer()
        {
        }

        public RecursiveImageDirectoryResizer(int degreeOfParallelism) : base(degreeOfParallelism)
        {
        }

        internal RecursiveImageDirectoryResizer(IImageResizer imageResizer) : base(imageResizer)
        {
        }

        #endregion

        protected override async Task<IEnumerable<Task>> CreateImageProcessingTasks(string rootPath, string dirPath,
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
                tasks.AddRange(await CreateImageProcessingTasks(rootPath, subDirectory, outputDirRootPath, resizeAction));
            }

            return tasks;
        }
    }
}
