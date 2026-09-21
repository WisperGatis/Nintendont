using Avalonia;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using SkiaSharp;
using Svg.Skia;
using System;
using System.IO;

namespace Ryujinx.Ava.UI.Helpers
{
    /// <summary>
    /// SVG document loaded through Svg.Skia. Avalonia.Svg.Skia has no Avalonia 12 release, so this
    /// provides the small part of its API that the input configuration screens rely on.
    /// </summary>
    public sealed class SvgSource : IDisposable
    {
        private readonly SKSvg _svg;

        private SvgSource(SKSvg svg)
        {
            _svg = svg;
        }

        public SKPicture Picture => _svg.Picture;

        public Size Size => Picture == null ? default : new Size(Picture.CullRect.Width, Picture.CullRect.Height);

        public static SvgSource LoadFromStream(Stream stream)
        {
            SKSvg svg = new();

            svg.Load(stream);

            return new SvgSource(svg);
        }

        public void Dispose()
        {
            _svg.Dispose();
        }
    }

    public sealed class SvgImage : IImage
    {
        public SvgSource Source { get; set; }

        public Size Size => Source?.Size ?? default;

        public void Draw(DrawingContext context, Rect sourceRect, Rect destRect)
        {
            SKPicture picture = Source?.Picture;

            if (picture == null || sourceRect.Width <= 0 || sourceRect.Height <= 0)
            {
                return;
            }

            context.Custom(new SvgDrawOperation(picture, sourceRect, destRect));
        }

        private sealed class SvgDrawOperation : ICustomDrawOperation
        {
            private readonly SKPicture _picture;
            private readonly Rect _sourceRect;
            private readonly Rect _destRect;

            public SvgDrawOperation(SKPicture picture, Rect sourceRect, Rect destRect)
            {
                _picture = picture;
                _sourceRect = sourceRect;
                _destRect = destRect;
            }

            public Rect Bounds => _destRect;

            public bool HitTest(Point p) => _destRect.Contains(p);

            public bool Equals(ICustomDrawOperation other) => ReferenceEquals(this, other);

            public void Dispose()
            {
            }

            public void Render(ImmediateDrawingContext context)
            {
                ISkiaSharpApiLeaseFeature leaseFeature = context.TryGetFeature<ISkiaSharpApiLeaseFeature>();

                if (leaseFeature == null)
                {
                    return;
                }

                using ISkiaSharpApiLease lease = leaseFeature.Lease();

                SKCanvas canvas = lease.SkCanvas;

                canvas.Save();
                canvas.ClipRect(new SKRect((float)_destRect.Left, (float)_destRect.Top, (float)_destRect.Right, (float)_destRect.Bottom));
                canvas.Translate((float)_destRect.X, (float)_destRect.Y);
                canvas.Scale((float)(_destRect.Width / _sourceRect.Width), (float)(_destRect.Height / _sourceRect.Height));
                canvas.Translate((float)-_sourceRect.X, (float)-_sourceRect.Y);
                canvas.DrawPicture(_picture);
                canvas.Restore();
            }
        }
    }
}
