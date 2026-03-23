namespace FluentROI.Core;

public sealed class RoiManager
{
    private readonly Dictionary<Guid, RoiItem> _items = new();
    private readonly HashSet<Guid> _selected = new();

    public RoiManager(float minWidth = 8f, float minHeight = 8f)
    {
        if (minWidth <= 0) throw new ArgumentOutOfRangeException(nameof(minWidth));
        if (minHeight <= 0) throw new ArgumentOutOfRangeException(nameof(minHeight));

        MinWidth = minWidth;
        MinHeight = minHeight;
    }

    public float MinWidth { get; }

    public float MinHeight { get; }

    public IReadOnlyList<RoiItem> Items => _items.Values.OrderBy(x => x.Id).ToArray();

    public IReadOnlyCollection<Guid> SelectedIds => _selected.ToArray();

    public RoiItem Add(RoiRect bounds, string? label = null)
    {
        var normalized = EnsureMin(bounds.Normalize());
        var item = new RoiItem(Guid.NewGuid(), normalized, label);
        _items[item.Id] = item;
        return item;
    }

    public bool Remove(Guid id)
    {
        _selected.Remove(id);
        return _items.Remove(id);
    }

    public void ClearSelection() => _selected.Clear();

    public bool Select(Guid id, bool additive = false)
    {
        if (!_items.ContainsKey(id))
        {
            return false;
        }

        if (!additive)
        {
            _selected.Clear();
        }

        _selected.Add(id);
        return true;
    }

    public int SelectMany(IEnumerable<Guid> ids, bool additive = false)
    {
        if (!additive)
        {
            _selected.Clear();
        }

        var count = 0;
        foreach (var id in ids)
        {
            if (_items.ContainsKey(id) && _selected.Add(id))
            {
                count++;
            }
        }

        return count;
    }

    public bool Move(Guid id, float dx, float dy)
    {
        if (!_items.TryGetValue(id, out var item))
        {
            return false;
        }

        _items[id] = item.WithBounds(item.Bounds.MoveBy(dx, dy));
        return true;
    }

    public int MoveSelected(float dx, float dy)
    {
        var moved = 0;
        foreach (var id in _selected.ToArray())
        {
            if (Move(id, dx, dy))
            {
                moved++;
            }
        }

        return moved;
    }

    public bool Resize(Guid id, RoiAnchor anchor, float dx, float dy)
    {
        if (!_items.TryGetValue(id, out var item))
        {
            return false;
        }

        var resized = ResizeRect(item.Bounds, anchor, dx, dy);
        _items[id] = item.WithBounds(EnsureMin(resized));
        return true;
    }

    public bool TryGet(Guid id, out RoiItem item) => _items.TryGetValue(id, out item!);

    private RoiRect EnsureMin(RoiRect rect)
    {
        var normalized = rect.Normalize();
        var width = Math.Max(MinWidth, normalized.Width);
        var height = Math.Max(MinHeight, normalized.Height);
        return new RoiRect(normalized.X, normalized.Y, width, height);
    }

    private static RoiRect ResizeRect(RoiRect rect, RoiAnchor anchor, float dx, float dy)
    {
        rect = rect.Normalize();

        var left = rect.X;
        var top = rect.Y;
        var right = rect.Right;
        var bottom = rect.Bottom;

        switch (anchor)
        {
            case RoiAnchor.TopLeft:
                left += dx;
                top += dy;
                break;
            case RoiAnchor.Top:
                top += dy;
                break;
            case RoiAnchor.TopRight:
                right += dx;
                top += dy;
                break;
            case RoiAnchor.Right:
                right += dx;
                break;
            case RoiAnchor.BottomRight:
                right += dx;
                bottom += dy;
                break;
            case RoiAnchor.Bottom:
                bottom += dy;
                break;
            case RoiAnchor.BottomLeft:
                left += dx;
                bottom += dy;
                break;
            case RoiAnchor.Left:
                left += dx;
                break;
            case RoiAnchor.None:
            default:
                return rect;
        }

        return new RoiRect(left, top, right - left, bottom - top).Normalize();
    }
}
