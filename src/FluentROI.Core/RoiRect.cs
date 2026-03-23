using System.Numerics;

namespace FluentROI.Core;

public readonly record struct RoiRect(float X, float Y, float Width, float Height)
{
    public float Right => X + Width;

    public float Bottom => Y + Height;

    public Vector2 TopLeft => new(X, Y);

    public bool IsValid => Width > 0 && Height > 0;

    public RoiRect Normalize()
    {
        var x = Width >= 0 ? X : X + Width;
        var y = Height >= 0 ? Y : Y + Height;
        var width = MathF.Abs(Width);
        var height = MathF.Abs(Height);
        return new RoiRect(x, y, width, height);
    }

    public RoiRect MoveBy(float dx, float dy)
        => new(X + dx, Y + dy, Width, Height);

    public RoiRect ClampInside(float maxWidth, float maxHeight)
    {
        var normalized = Normalize();
        var x = Math.Clamp(normalized.X, 0, Math.Max(0, maxWidth - normalized.Width));
        var y = Math.Clamp(normalized.Y, 0, Math.Max(0, maxHeight - normalized.Height));
        return new RoiRect(x, y, normalized.Width, normalized.Height);
    }
}
