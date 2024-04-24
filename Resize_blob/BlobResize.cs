using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace BlobResizeFuncGr16
{
    public class BlobResize
    {
        [FunctionName("BlobResize")]
        public void Run([BlobTrigger("sybringsstorage/{name}", Connection = "AzureWebJobsStorage")] Stream myBlob,
            [Blob("resizeimages-md/{name}", FileAccess.Write)] Stream imageSmall,
            [Blob("resizeimages-sm/{name}", FileAccess.Write)] Stream imageMedium, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed a blob \n Size: {myBlob.Length} Bytes");

            IImageFormat format;

            using (Image<Rgba32> input = Image.Load<Rgba32>(myBlob, out format))
            {
                ResizeImage(input, imageSmall, ImageSize.Small, format);
            }

            myBlob.Position = 0;
            using (Image<Rgba32> input = Image.Load<Rgba32>(myBlob, out format))
            {
                ResizeImage(input, imageMedium, ImageSize.ExtraSmall, format);
            }

        }

        public static void ResizeImage(Image<Rgba32> input,
            Stream output, ImageSize size, IImageFormat format)
        {
            var dimensions = imageDimensionsTable[size];

            input.Mutate(x => x.Resize(dimensions.Item1, dimensions.Item2));
            input.Save(output, format);
        }

        public enum ImageSize { ExtraSmall, Small, Medium }

        private static Dictionary<ImageSize, (int, int)> imageDimensionsTable = new Dictionary<ImageSize, (int, int)>() {
        { ImageSize.ExtraSmall, (240, 150) },
        { ImageSize.Small,      (640, 400) }
    };
    }
}
