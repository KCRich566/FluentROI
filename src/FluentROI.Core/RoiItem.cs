namespace FluentROI.Core;

public sealed record RoiItem(Guid Id, RoiRect Bounds, string? Label = null)
{
    public RoiItem WithBounds(RoiRect bounds) => this with { Bounds = bounds.Normalize() };
}
