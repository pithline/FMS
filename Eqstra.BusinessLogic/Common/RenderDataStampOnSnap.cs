using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.DXGI;
using SharpDX.WIC;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Bitmap = SharpDX.WIC.Bitmap;
using D2DPixelFormat = SharpDX.Direct2D1.PixelFormat;
using WicPixelFormat = SharpDX.WIC.PixelFormat;

namespace Eqstra.BusinessLogic
{
    public class RenderDataStampOnSnap
    {
        public async static Task<MemoryStream> RenderStaticTextToBitmap(StorageFile ImageFile)
        {
            var bitmap = new BitmapImage();
            using (var strm = await ImageFile.OpenAsync(FileAccessMode.Read))
            {
                bitmap.SetSource(strm);
            }
            var width = bitmap.PixelWidth;
            var height = bitmap.PixelHeight;
            var pixelFormat = WicPixelFormat.Format32bppBGR;

            var wicFactory = new ImagingFactory2();
            var dddFactory = new SharpDX.Direct2D1.Factory();
            var dwFactory = new SharpDX.DirectWrite.Factory();
            WicRenderTarget renderTarget;
            Bitmap wicBitmap;

            using (var bitmapSource = LoadBitmap(wicFactory, ImageFile.Path))
            {
                wicBitmap = new Bitmap(wicFactory, bitmapSource, BitmapCreateCacheOption.CacheOnLoad);

                int pixelWidth = (int)(wicBitmap.Size.Width * DisplayProperties.LogicalDpi / 96.0);
                int pixelHeight = (int)(wicBitmap.Size.Height * DisplayProperties.LogicalDpi / 96.0);

                var renderTargetProperties = new RenderTargetProperties(RenderTargetType.Default,
                    new D2DPixelFormat(Format.Unknown, AlphaMode.Unknown), 0, 0, RenderTargetUsage.None,
                    FeatureLevel.Level_DEFAULT);

                renderTarget = new WicRenderTarget(
                dddFactory,
                wicBitmap,
                renderTargetProperties)
                {
                    TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Cleartype
                };
            }

            renderTarget.BeginDraw();

            var textFormat = new TextFormat(dwFactory, "Segoe UI Light", 25)
            {
                TextAlignment = SharpDX.DirectWrite.TextAlignment.Leading,
                ParagraphAlignment = ParagraphAlignment.Far
            };
            var textBrush = new SharpDX.Direct2D1.SolidColorBrush(
                renderTarget,
                SharpDX.Colors.DarkBlue);

            StringBuilder sb = new StringBuilder();
            var dstamp = StampPersistData.Instance.DataStamp;
            sb.Append(dstamp.KMReading).Append("\n");
            sb.Append(dstamp.DateOfFirstReg).Append("\n");
          
            sb.Append(dstamp.Gps).Append("\n");
            sb.Append(dstamp.VehRegNo).Append("\n");
            sb.Append(dstamp.Make).Append("\n");
           
            sb.Append(dstamp.CusName).Append("\n");
            sb.Append(dstamp.InspectorName).Append("\n");
            sb.Append(dstamp.CaseNo);

            renderTarget.DrawText(
                sb.ToString(),
                textFormat,
                new RectangleF(1, 1, width + 50, height+25),
                textBrush);

            //new RectangleF(width - 150, 0, width, height + 25),

            renderTarget.EndDraw();

            var ms = new MemoryStream();

            var stream = new WICStream(
                wicFactory,
                ms);

            BitmapEncoder encoder = null;
            if (ImageFile.FileType == ".png")
                encoder = new PngBitmapEncoder(wicFactory);
            else if (ImageFile.FileType == ".jpg")
                encoder = new JpegBitmapEncoder(wicFactory);

            encoder.Initialize(stream);

            var frameEncoder = new BitmapFrameEncode(encoder);
            frameEncoder.Initialize();
            frameEncoder.SetSize(width, height);
            frameEncoder.PixelFormat = WicPixelFormat.FormatDontCare;
            frameEncoder.WriteSource(wicBitmap);
            frameEncoder.Commit();

            encoder.Commit();

            frameEncoder.Dispose();
            encoder.Dispose();
            stream.Dispose();

            ms.Position = 0;
            return ms;
        }


        public static SharpDX.WIC.BitmapSource LoadBitmap(ImagingFactory2 factory, string ImageFilePath)
        {
            var bitmapDecoder = new BitmapDecoder(
                factory,
                ImageFilePath,
                DecodeOptions.CacheOnDemand
                );

            var formatConverter = new FormatConverter(factory);

            formatConverter.Initialize(
                bitmapDecoder.GetFrame(0),
                SharpDX.WIC.PixelFormat.Format32bppPRGBA,
                BitmapDitherType.None,
                null,
                0.0,
                BitmapPaletteType.Custom);

            return formatConverter;
        }

    }
}
