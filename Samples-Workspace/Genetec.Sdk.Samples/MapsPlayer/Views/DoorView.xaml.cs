// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using Genetec.Sdk.Entities.Maps;

namespace MapsPlayer.Views
{
    /// <summary>
    /// Interaction logic for DoorView.xaml
    /// </summary>
    public partial class DoorView : IDisposable
    {

        #region Private Fields

        private Genetec.Sdk.Workspace.Workspace m_workspace;

        #endregion Private Fields

        #region Public Constructors

        public DoorView() => InitializeComponent();

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
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Free the resources used by the map item
        /// </summary>
        /// <param name="disposing">Dispose explicitly called flag</param>
        protected virtual void Dispose(bool disposing) { }

        #endregion Protected Methods

        #region Private Destructors

        /// <summary>
        /// Destructor
        /// </summary>
        ~DoorView() => Dispose(false);

        #endregion Private Destructors

    }
}