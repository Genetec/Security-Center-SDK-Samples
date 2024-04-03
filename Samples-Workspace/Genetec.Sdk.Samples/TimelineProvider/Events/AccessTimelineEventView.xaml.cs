// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Genetec.Sdk.Entities;

namespace TimelineProvider.Events
{
    /// <summary>
    /// Interaction logic for AccessTimelineEventView.xaml
    /// </summary>
    public partial class AccessTimelineEventView
    {

        #region Private Fields

        private const int MaxSize = 100;

        private const string NoImage = "Set a picture for the cardholder.";

        private readonly Guid m_cardholderId;

        private readonly Genetec.Sdk.Workspace.Workspace m_workspace;

        #endregion Private Fields

        #region Public Constructors

        public AccessTimelineEventView(Genetec.Sdk.Workspace.Workspace workspace, Guid cardholderId, bool isGranted)
        {
            m_workspace = workspace;
            m_cardholderId = cardholderId;
            InitializeComponent();
            Background = isGranted
                ? Brushes.LightBlue
                : Brushes.Red;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void OnToolTipOpening(ToolTipEventArgs e)
        {
            base.OnToolTipOpening(e);

            if (ToolTip is Image)
                return;

            if (ToolTip.ToString() == NoImage)
                return;

            ToolTip = null;
            var cardholderPicture = m_workspace.Sdk.GetEntity<Cardholder>(m_cardholderId).Picture;
            if (cardholderPicture != null)
                ToolTip = new Image
                {
                    Source = ConvertToBitmapSource(cardholderPicture),
                    MaxHeight = MaxSize,
                    MaxWidth = MaxSize
                };
            else ToolTip = NoImage;
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Convert a WinForm System.Drawing.Image into a WPF System.Windows.Media.Imaging.BitmapSource
        /// </summary>
        /// <param name="image">input WinForm image</param>
        /// <returns>Converted WPF BitmapSource image</returns>
        private static BitmapSource ConvertToBitmapSource(System.Drawing.Image image)
        {
            if (image == null) 
                return null;

            // Convert the System.Drawing.Image into a BitmapSource
            var bi = new BitmapImage();

            bi.BeginInit();
            var ms = new MemoryStream();

            // Save to a memory stream...
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

            // Rewind the stream...
            ms.Seek(0, SeekOrigin.Begin);

            // Tell the WPF image to use this stream...
            bi.StreamSource = ms;
            bi.EndInit();

            return bi;
        }

        #endregion Private Methods

    }
}