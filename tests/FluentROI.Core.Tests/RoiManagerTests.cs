using FluentROI.Core;

namespace FluentROI.Core.Tests;

public sealed class RoiManagerTests
{
    [Fact]
    public void Add_EnforcesMinimumSize()
    {
        var manager = new RoiManager(minWidth: 10, minHeight: 12);

        var roi = manager.Add(new RoiRect(2, 4, 3, 5));

        Assert.Equal(10, roi.Bounds.Width);
        Assert.Equal(12, roi.Bounds.Height);
    }

    [Fact]
    public void MoveSelected_MovesOnlySelectedItems()
    {
        var manager = new RoiManager();
        var first = manager.Add(new RoiRect(0, 0, 20, 20));
        var second = manager.Add(new RoiRect(100, 100, 20, 20));

        manager.Select(first.Id);
        var moved = manager.MoveSelected(5, 6);

        Assert.Equal(1, moved);
        Assert.True(manager.TryGet(first.Id, out var movedFirst));
        Assert.True(manager.TryGet(second.Id, out var unmovedSecond));
        Assert.Equal(5, movedFirst.Bounds.X);
        Assert.Equal(6, movedFirst.Bounds.Y);
        Assert.Equal(100, unmovedSecond.Bounds.X);
        Assert.Equal(100, unmovedSecond.Bounds.Y);
    }

    [Fact]
    public void Resize_FromTopLeft_AdjustsBounds()
    {
        var manager = new RoiManager(minWidth: 8, minHeight: 8);
        var roi = manager.Add(new RoiRect(10, 10, 20, 20));

        var resized = manager.Resize(roi.Id, RoiAnchor.TopLeft, -5, -3);

        Assert.True(resized);
        Assert.True(manager.TryGet(roi.Id, out var after));
        Assert.Equal(5, after.Bounds.X);
        Assert.Equal(7, after.Bounds.Y);
        Assert.Equal(25, after.Bounds.Width);
        Assert.Equal(23, after.Bounds.Height);
    }
}
