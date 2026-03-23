# FluentROI

FluentROI is a **modern desktop image annotation tool** built with **WinUI 3** and **Fluent Design**.  
It allows users to perform **ROI (Region of Interest) annotations**, including multi-selection, anchor-based resizing, moving, and provides a **real-time ROI Preview Panel**.  
The core ROI functionality is also available as a **reusable library/SDK**, enabling other applications to integrate ROI features directly.

---

## ⚡ Features

### Image Operations
- Mouse-centered zooming
- Drag-to-pan
- Supports multiple image formats

### ROI Annotation
- Rectangular ROI
- Single & multi-selection (Ctrl / Shift)
- Move ROI (single or grouped)
- Resize ROI with 8 anchor points
- Minimum size limit
- Real-time preview and save to PNG

### Fluent Design UI
- Acrylic / Mica backgrounds
- Modern buttons (AccentButton)
- Light & shadow effects with smooth animations
- Intuitive and user-friendly interface

### Library / SDK
- Provides core ROI operations independent of GUI
- Add, remove, move, resize, and multi-select ROI
- Convert Canvas coordinates to original image pixels
- Crop and export ROI images
- Can be integrated into other applications

---

## 🧱 Project Structure

```text
FluentROI/
├─ FluentROI.sln
├─ src/
│  └─ FluentROI.Core/
│     ├─ CoordinateMapper.cs
│     ├─ RoiAnchor.cs
│     ├─ RoiImageExporter.cs
│     ├─ RoiItem.cs
│     ├─ RoiManager.cs
│     └─ RoiRect.cs
├─ tests/
│  └─ FluentROI.Core.Tests/
└─ README.md
```

`FluentROI.Core` is a foundational SDK for ROI logic and can be reused by WinUI, WPF, Avalonia, MAUI, or backend services.

---

## 🚀 Quick Start (SDK)

1. Open `FluentROI.sln` in Visual Studio 2022 or newer.
2. Build the `FluentROI.Core` project.
3. Reference `FluentROI.Core` from your UI application.

### Example

```csharp
using FluentROI.Core;

var manager = new RoiManager(minWidth: 12, minHeight: 12);
var roi = manager.Add(new RoiRect(100, 120, 240, 160), "Part-A");

manager.Select(roi.Id);
manager.MoveSelected(dx: 8, dy: -4);
manager.Resize(roi.Id, RoiAnchor.BottomRight, dx: 20, dy: 15);

var imageRect = CoordinateMapper.CanvasToImage(
    canvasRect: roi.Bounds,
    canvasOffsetX: 20,
    canvasOffsetY: 16,
    zoomScale: 1.5f);
```

---

## 📌 Next Steps

- Add `FluentROI.WinUI` front-end project with Fluent Design components.
- Connect pointer/mouse events to `RoiManager`.
- Add undo/redo command stack.
- Add unit tests for resize anchors and coordinate conversion.


---

## ✅ Testing

```bash
dotnet test FluentROI.sln
```

The test project validates ROI move/resize behavior and coordinate mapping round-trip conversions.
