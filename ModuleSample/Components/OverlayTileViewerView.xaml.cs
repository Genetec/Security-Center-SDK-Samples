// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace;
using Genetec.Sdk.Workspace.Pages.Contents;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ModuleSample.Components
{

    /// <summary>
    /// Interaction logic for OverlayTileViewerView.xaml
    /// </summary>
    public partial class OverlayTileViewerView
    {

        #region Private Fields

        private VideoContent m_content;

        private Workspace m_workspace;

        #endregion Private Fields

        #region Public Constructors

        public OverlayTileViewerView(Workspace workspace)
        {
            m_workspace = workspace;
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Update(VideoContent content)
        {
            UnsubscribeContent(m_content);
            m_content = content;
            SubscribeContent(m_content);

            DataContext = m_content;
            UpdateBounds();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnRenderSizeChanged(SizeChangedInfo info)
        {
            base.OnRenderSizeChanged(info);
            UpdateBounds();
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Gets a rectangle that fits in the specified container rectangle with the
        /// same aspect ratio than the specified source rectangle.
        /// </summary>
        /// <param name="source">Source rectangle</param>
        /// <param name="container">Container rectangle</param>
        /// <returns>Result rectangle with the same aspect ratio than the source</returns>
        private static Rect SizeRectWithConstantAspectRatio(Rect source, Rect container)
        {
            Rect destination;

            // Calculate the new size of the image
            var baseHeight = container.Height;
            var baseWidth = container.Width;

            // Determine the ratio of the image
            var aspectRatio = baseWidth / baseHeight;

            // Get the size of the user image
            var sourceHeight = source.Height;
            var sourceWidth = source.Width;

            // Determine the ratio of the image
            var sourceAspectRatio = sourceWidth / sourceHeight;

            // If the aspect ratios are the same then the base rectangle
            // will do, otherwise we need to calculate the new rectangle
            if (sourceAspectRatio > aspectRatio)
            {
                var newHeight = (int)(baseWidth / sourceWidth * sourceHeight);
                var centeringFactor = ((int)baseHeight - newHeight) / 2;

                destination = new Rect(0, centeringFactor, (int)baseWidth, newHeight);
            }
            else if (sourceAspectRatio < aspectRatio)
            {
                var newWidth = (int)(baseHeight / sourceHeight * sourceWidth);
                var centeringFactor = ((int)baseWidth - newWidth) / 2;

                destination = new Rect(centeringFactor, 0, newWidth, (int)baseHeight);
            }
            else
            {
                destination = container;
            }

            return destination;
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            if (m_content != null)
            {
                if (m_content.IsPaused)
                {
                    m_content.Play();
                }
                else
                {
                    m_content.Pause();
                }
            }
        }

        private void OnContentDimensionsChanged(object sender, EventArgs e) => UpdateBounds();
        
        private void SubscribeContent(VideoContent content)
        {
            if (content != null)
            {
                m_content.DimensionsChanged += OnContentDimensionsChanged;
            }
        }

        private void UnsubscribeContent(VideoContent content)
        {
            if (content != null)
            {
                m_content.DimensionsChanged -= OnContentDimensionsChanged;
            }
        }

        private void UpdateBounds()
        {
            var destination = new Rect(0, 0, ActualWidth, ActualHeight);
            var visibility = Visibility.Collapsed;

            if (m_content != null)
            {
                var renderingSize = m_content.RenderingSize;
                if ((renderingSize.Width > 0) && (renderingSize.Height > 0))
                {
                    var source = new Rect(0, 0, renderingSize.Width, renderingSize.Height);
                    var container = new Rect(0, 0, ActualWidth, ActualHeight);

                    destination = SizeRectWithConstantAspectRatio(source, container);

                    Canvas.SetLeft(m_gridVideo, destination.Left);
                    Canvas.SetTop(m_gridVideo, destination.Top);

                    visibility = Visibility.Visible;
                }
            }

            m_border.Visibility = visibility;
            m_gridVideo.Height = destination.Height;
            m_gridVideo.Width = destination.Width;
        }

        #endregion Private Methods

    }

    [ValueConversion(typeof(DateTime), typeof(DateTime))]
    internal sealed class UtcToLocalTimeConverter : IValueConverter
    {

        #region Public Methods

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DateTime time)
            {
                return time.ToLocalTime();
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

    }

    [ValueConversion(typeof(double), typeof(double))]
    internal sealed class WidthToFontSizeConverter : IValueConverter
    {

        #region Public Methods

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double d)
            {
                return d / 20.0;
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

    }

}