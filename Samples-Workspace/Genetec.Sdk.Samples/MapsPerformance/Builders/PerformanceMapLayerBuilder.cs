// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Collections.Generic;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Workspace.Components;
using Genetec.Sdk.Workspace.Maps;
using MapsPerformance.Layers;

namespace MapsPerformance.Builders
{
    public sealed class PerformanceMapLayerBuilder : MapLayerBuilder
    {

        #region Private Fields

        private readonly Lazy<Guid> m_uniqueLazyId = new Lazy<Guid>(() => new Guid("{7339FF9C-C9C8-4881-A7EE-1165527B4EE7}"));

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

        public override bool IsSupported(MapContext context)
        {
            var supported = false;
            var map = Workspace.Sdk.GetEntity(context.MapId) as Map;
            if (map != null)
            {
                supported = map.IsGeoReferenced;
            }
            return supported;
        }

        #endregion Public Methods

    }
}