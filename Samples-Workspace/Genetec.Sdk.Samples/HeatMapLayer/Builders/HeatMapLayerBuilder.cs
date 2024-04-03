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
using HeatMapLayer.Layers;
using HeatMapLayer.Providers;

namespace HeatMapLayer.Builders
{
    public sealed class HeatMapLayerBuilder : MapLayerBuilder
    {

        #region Private Fields

        private readonly Lazy<Guid> m_uniqueLazyId = new Lazy<Guid>(() => new Guid("{F75D6474-5DEC-4EC2-A348-60895B44DC64}"));
        private CarMapObjectProvider m_provider;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets the name of the component
        /// </summary>
        public override string Name => "Motion Heat Map layer builder";

        /// <summary>
        /// Gets the unique identifier of the component
        /// </summary>
        public override Guid UniqueId => m_uniqueLazyId.Value;

        #endregion Public Properties

        #region Public Methods

        public override IList<MapLayer> CreateLayers(MapContext context) => new List<MapLayer> { new MotionHeatMapLayer(Workspace, m_provider) };

        public void Initialize(Genetec.Sdk.Workspace.Workspace workspace, CarMapObjectProvider provider)
        {
            Initialize(workspace);
            m_provider = provider;
        }
        public override bool IsSupported(MapContext context)
        {
            var map = Workspace.Sdk.GetEntity(context.MapId) as Map;
            return map != null && map.IsGeoReferenced;
        }

        #endregion Public Methods

    }
}