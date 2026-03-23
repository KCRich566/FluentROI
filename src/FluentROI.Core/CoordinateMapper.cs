namespace FluentROI.Core;

public static class CoordinateMapper
{
    public static RoiRect CanvasToImage(
        RoiRect canvasRect,
        float canvasOffsetX,
        float canvasOffsetY,
        float zoomScale)
    {
        if (zoomScale <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(zoomScale), "Zoom scale must be greater than 0.");
        }

        var x = (canvasRect.X - canvasOffsetX) / zoomScale;
        var y = (canvasRect.Y - canvasOffsetY) / zoomScale;
        var width = canvasRect.Width / zoomScale;
        var height = canvasRect.Height / zoomScale;

        return new RoiRect(x, y, width, height).Normalize();
    }

    public static RoiRect ImageToCanvas(
        RoiRect imageRect,
        float canvasOffsetX,
        float canvasOffsetY,
        float zoomScale)
    {
        if (zoomScale <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(zoomScale), "Zoom scale must be greater than 0.");
        }

        var x = imageRect.X * zoomScale + canvasOffsetX;
        var y = imageRect.Y * zoomScale + canvasOffsetY;
        var width = imageRect.Width * zoomScale;
        var height = imageRect.Height * zoomScale;

        return new RoiRect(x, y, width, height).Normalize();
    }
}
