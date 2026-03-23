using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace FluentROI.Core;

public static class RoiImageExporter
{
    public static void ExportToPng(string sourceImagePath, RoiRect imageRect, string outputPath)
    {
        using var source = Image.Load<Rgba32>(sourceImagePath);
        using var cropped = Crop(source, imageRect);
        cropped.Save(outputPath, new PngEncoder());
    }

    public static Image<Rgba32> Crop(Image<Rgba32> source, RoiRect imageRect)
    {
        var rect = imageRect.Normalize();

        var x = (int)MathF.Round(rect.X);
        var y = (int)MathF.Round(rect.Y);
        var width = (int)MathF.Round(rect.Width);
        var height = (int)MathF.Round(rect.Height);

        x = Math.Clamp(x, 0, source.Width - 1);
        y = Math.Clamp(y, 0, source.Height - 1);
        width = Math.Clamp(width, 1, source.Width - x);
        height = Math.Clamp(height, 1, source.Height - y);

        if (width <= 0 || height <= 0)
        {
            throw new ArgumentException("ROI rectangle is outside image bounds.", nameof(imageRect));
        }

        return source.Clone(ctx => ctx.Crop(new Rectangle(x, y, width, height)));
    }
}
