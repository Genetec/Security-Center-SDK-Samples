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

namespace DronesTracker.Maps.Layers
{
    public sealed class MotionHeatMapLayerBuilder : MapLayerBuilder
    {

        #region Private Fields

        private DemoMapObjectProvider m_provider;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets or Sets a flag indicating if the layer is enabled
        /// </summary>
        public static bool IsEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the name of the component
        /// </summary>
        public override string Name => "Motion Heat Map layer builder";

        /// <summary>
        /// Gets the unique identifier of the component
        /// </summary>
        public override Guid UniqueId => new Guid("{F4F5E725-03F5-460E-A04A-482A9AC4A3AA}");

        #endregion Public Properties

        #region Public Methods

        public override IList<MapLayer> CreateLayers(MapContext context)
        {
            return new List<MapLayer> { new MotionHeatMapLayer(Workspace, m_provider) };
        }

        public void Initialize(Genetec.Sdk.Workspace.Workspace workspace, DemoMapObjectProvider provider)
        {
            Initialize(workspace);
            m_provider = provider;
        }

        public override bool IsSupported(MapContext context)
        {
            if (IsEnabled)
            {
                var map = Workspace.Sdk.GetEntity(context.MapId) as Map;
                if (map != null)
                {
                    return map.IsGeoReferenced;
                }
            }

            return false;
        }

        #endregion Public Methods

    }
}