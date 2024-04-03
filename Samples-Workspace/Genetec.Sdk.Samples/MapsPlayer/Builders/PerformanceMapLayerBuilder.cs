// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Collections.Generic;
using Genetec.Sdk.Workspace.Components;
using Genetec.Sdk.Workspace.Maps;
using MapsPlayer.Layers;

namespace MapsPlayer.Builders
{
    public class PerformanceMapLayerBuilder : MapLayerBuilder
    {

        #region Private Fields

        private readonly Lazy<Guid> m_uniqueLazyId = new Lazy<Guid>(() => new Guid("{880646A5-7703-4D08-9A74-5440D6ADDDB0}"));

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets the name of the component
        /// </summary>
        public override string Name => "Performance layer builder";

        /// <summary>
        /// Gets the unique identifier of the component
        /// </summary>
        public override Guid UniqueId => m_uniqueLazyId.Value;

        #endregion Public Properties

        #region Public Methods

        public override IList<MapLayer> CreateLayers(MapContext context) => new List<MapLayer> { new PerformanceLayer(Workspace) };

        public override bool IsSupported(MapContext context) => true;

        #endregion Public Methods

    }
}