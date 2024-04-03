// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Windows;
using System.Windows.Input;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Entities.Maps;
using Genetec.Sdk.Workspace.Maps;
using Genetec.Sdk.Workspace.Pages.Contents;

namespace MapsPlayer.Views
{
    /// <summary>
    /// Interaction logic for CameraPlayerView.xaml
    /// </summary>
    public partial class CameraPlayerView : IDisposable
    {

        #region Private Fields

        // Dependency properties
        private static readonly DependencyPropertyKey TitlePropertyKey =
            DependencyProperty.RegisterReadOnly
            ("Title", typeof(string), typeof(CameraPlayerView),
            new PropertyMetadata(string.Empty));

        private bool m_isGeoReferenced;
        private Genetec.Sdk.Workspace.Workspace m_workspace;

        #endregion Private Fields

        #region Public Properties

        public string Title
        {
            get => (string)GetValue(TitlePropertyKey.DependencyProperty);
            private set => SetValue(TitlePropertyKey, value);
        }

        #endregion Public Properties

        #region Public Constructors

        public CameraPlayerView() => InitializeComponent();

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// IDisposable implementation
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Initialize(Genetec.Sdk.Workspace.Workspace workspace, MapObject mapObject)
        {
            m_workspace = workspace;
            Initialize(mapObject);

            var mapId = mapObject.Map;
            if (mapId.HasValue)
            {
                var map = m_workspace.Sdk.GetEntity(mapId.Value) as Map;
                if (map != null)
                {
                    m_isGeoReferenced = map.IsGeoReferenced;

                    if (m_isGeoReferenced)
                    {
                        RelativeSizeMode = RelativeSizeMode.Meters;
                        RelativeHeight = 45;
                        RelativeWidth = 45;
                        MinHeight = 16;
                        MinWidth = 16;
                    }
                    else
                    {
                        RelativeSizeMode = RelativeSizeMode.Pixels;
                        RelativeHeight = 120;
                        RelativeWidth = 120;
                    }
                }
            }

            var camera = m_workspace.Sdk.GetEntity(mapObject.LinkedEntity);
            if (camera != null)
            {
                m_grid.Visibility = Visibility.Visible;
                Title = camera.Name;
                m_mediaPlayer.Initialize(m_workspace.Sdk, mapObject.LinkedEntity);
                m_mediaPlayer.PlayLive();
            }
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Free the resources used by the map item
        /// </summary>
        /// <param name="disposing">Dispose explicitly called flag</param>
        protected virtual void Dispose(bool disposing)
        {
        }

        #endregion Protected Methods

        #region Private Methods

        private void OnBorderMouseDown(object sender, MouseButtonEventArgs e) => IsSelected = !IsSelected;

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            if (MapObject != null)
            {
                var contentGroup = new ContentGroup();
                contentGroup.Initialize(m_workspace);

                var cameraContent = new VideoContent(MapObject.LinkedEntity);
                cameraContent.Initialize(m_workspace);
                contentGroup.Contents.Add(cameraContent);

                DisplayTile(contentGroup);
            }
        }

        #endregion Private Methods

        #region Private Destructors

        /// <summary>
        /// Destructor
        /// </summary>
        ~CameraPlayerView() => Dispose(false);

        #endregion Private Destructors

    }
}