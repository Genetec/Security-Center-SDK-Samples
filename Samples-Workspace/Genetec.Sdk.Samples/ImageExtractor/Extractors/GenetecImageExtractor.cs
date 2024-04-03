// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Genetec.Sdk.Workspace.Components.ImageExtractor;
using Microsoft.Win32;

namespace ImageExtractor.Extractors
{
    public sealed class GenetecImageExtractor : Genetec.Sdk.Workspace.Components.ImageExtractor.ImageExtractor
    {

        #region Private Fields

        private static readonly BitmapImage s_watermark;

        private readonly Lazy<Guid> m_uniqueLazyId = new Lazy<Guid>(() => new Guid("{3777D10A-D670-4171-ABC7-1EBE46EA95EC}"));

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets the icon of the component
        /// </summary>
        public override ImageSource Icon =>
            new BitmapImage(new Uri(@"pack://application:,,,/ImageExtractor;Component/Resources/SecurityCenter.png", UriKind.RelativeOrAbsolute));

        /// <summary>
        /// Gets the name of the component
        /// </summary>
        public override string Name => "Genetec ImageExtractor";

        /// <summary>
        /// Gets the priority of the component, lowest is better
        /// </summary>
        public override int Priority => 150;

        /// <summary>
        /// Gets the unique identifier of the component
        /// </summary>
        public override Guid UniqueId => m_uniqueLazyId.Value;

        #endregion Public Properties

        #region Public Constructors

        static GenetecImageExtractor() =>
            s_watermark = new BitmapImage(new Uri(@"pack://application:,,,/ImageExtractor;Component/Resources/Watermark.png", UriKind.RelativeOrAbsolute));

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Extract the image from the component
        /// </summary>
        /// <returns>Extracted image</returns>
        public override ImageSource GetImage()
        {
            ImageSource result = null;

            var dlg = new OpenFileDialog
            {
                Title = "Select picture file",
                Filter =
                    "Image files (*.bmp;*.jpg;*.jpeg;*.gif;*.png)|*.bmp;*.jpg;*.jpeg;*.gif;*.png|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true,
                Multiselect = false
            };

            if (dlg.ShowDialog().GetValueOrDefault())
            {
                var imageFilePath = dlg.FileName;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
                bitmapImage.UriSource = new Uri(imageFilePath);
                bitmapImage.EndInit();

                result = EmbedWatermark(bitmapImage);
            }
            return result;
        }

        /// <summary>
        /// Gets if the image extractor supports a given context
        /// </summary>
        public override bool SupportsContext(ImageExtractorContext context) => context == ImageExtractorContext.CardholderPicture;

        #endregion Public Methods

        #region Private Methods

        private static RenderTargetBitmap EmbedWatermark(BitmapImage source)
        {
            var result = new RenderTargetBitmap(source.PixelWidth, source.PixelHeight, source.DpiX, source.DpiY, PixelFormats.Default);

            var drawVisual = new DrawingVisual();
            using (var dc = drawVisual.RenderOpen())
            {
                dc.DrawImage(source, new Rect(0, 0, result.Width, result.Height));

                var xpos = result.Width - s_watermark.PixelWidth - 8;
                var ypos = result.Height - s_watermark.PixelHeight - 8;
                dc.DrawImage(s_watermark, new Rect(xpos, ypos, s_watermark.Width, s_watermark.Height));
            }
            result.Render(drawVisual);

            return result;
        }

        #endregion Private Methods

    }
}