using System;
using System.IO;
using System.Numerics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace OcrConsoleAppFx
{
    public static class ImageExtensions
    {
        private static readonly Vector4 White = new Vector4(1, 1, 1, 1);
        private static readonly Vector4 Black = new Vector4(0, 0, 0, 1);

        public static Size ResizeKeepAspect(this Size sourceSize, int maxWidth, int maxHeight, bool enlarge = false)
        {
            maxWidth = enlarge ? maxWidth : Math.Min(maxWidth, sourceSize.Width);
            maxHeight = enlarge ? maxHeight : Math.Min(maxHeight, sourceSize.Height);

            var rnd = Math.Min(maxWidth / (decimal)sourceSize.Width, maxHeight / (decimal)sourceSize.Height);
            return new Size((int)Math.Round(sourceSize.Width * rnd), (int)Math.Round(sourceSize.Height * rnd));
        }
        
        public static byte[] ToArray<TPixel>(this Image<TPixel> image, IImageFormat imageFormat) where TPixel : unmanaged, IPixel<TPixel>
        {
            using (var memoryStream = new MemoryStream())
            {
                var imageEncoder = image.GetConfiguration().ImageFormatsManager.FindEncoder(imageFormat);
                image.Save(memoryStream, imageEncoder);
                return memoryStream.ToArray();
            }
        }
       
        public static IImageProcessingContext ProcessOnBackground(this IImageProcessingContext context, int percentageWhite, Size size)
        {
            if (percentageWhite > 55)
                context
                    .Saturate(2.5f)
                    .Invert()
                    .BlackWhite()
                    .Invert()
                    .Dilate(1)
                    ;
            else if (percentageWhite <= 7)
                context
                    .BlackWhite()
                    .WhiteFilter(0.8f)
                    ;
            else
                context
                    .Saturate(2.5f)
                    .HistogramEqualization()
                    .WhiteFilter(0.87f)
                    .Dilate(1)
                    ;

            return context;
        }
        
        public static void GetBackgroundColor(this IImageProcessingContext context, float threshold, out int percentageWhite)
        {
            var whiteCount = 0;
            var blackCount = 0;
            
            context.ProcessPixelRowsAsVector4(r =>
            {
                for (int x = 0; x < r.Length; x++)
                {
                    if (r[x].X < threshold)
                        blackCount++;
                    else
                        whiteCount++;
                }
            });
            
            percentageWhite = Convert.ToInt32(decimal.Divide(whiteCount, whiteCount + blackCount) * 100);
        }

        private static IImageProcessingContext WhiteFilter(this IImageProcessingContext context, float threshold)
            => context.ProcessPixelRowsAsVector4(r =>
            {
                for (int x = 0; x < r.Length; x++)
                {
                    r[x] = r[x].X < threshold ? White : Black;
                }
            });

        private static IImageProcessingContext Dilate(this IImageProcessingContext context, int radius)
            => context
                .BoxBlur(radius)
                .ProcessPixelRowsAsVector4(r =>
                {
                    for (int x = 0; x < r.Length; x++)
                    {
                        Vector4 c = r[x];
                        r[x] = c.X == 1 && c.Y == 1 && c.Z == 1 ? White : Black;
                    }
                });
    }
}