using FluentROI.Core;

namespace FluentROI.Core.Tests;

public sealed class CoordinateMapperTests
{
    [Fact]
    public void CanvasToImage_AndBack_RoundTrips()
    {
        var canvas = new RoiRect(140, 90, 300, 180);

        var image = CoordinateMapper.CanvasToImage(canvas, canvasOffsetX: 20, canvasOffsetY: 10, zoomScale: 2);
        var back = CoordinateMapper.ImageToCanvas(image, canvasOffsetX: 20, canvasOffsetY: 10, zoomScale: 2);

        Assert.Equal(canvas.X, back.X, 3);
        Assert.Equal(canvas.Y, back.Y, 3);
        Assert.Equal(canvas.Width, back.Width, 3);
        Assert.Equal(canvas.Height, back.Height, 3);
    }

    [Fact]
    public void CanvasToImage_WithInvalidScale_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => CoordinateMapper.CanvasToImage(new RoiRect(0, 0, 10, 10), 0, 0, 0));
    }
}
