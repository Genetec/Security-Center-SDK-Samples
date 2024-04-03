// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Collections.Generic;
using Genetec.Sdk.Entities.Maps;
using Genetec.Sdk.Workspace.Components;
using Genetec.Sdk.Workspace.Components.MapObjectViewBuilder;
using Genetec.Sdk.Workspace.Maps;
using MapsPerformance.Layers;
using MapsPerformance.Visuals;

namespace MapsPerformance.Builders
{
    public sealed class PerformanceMapObjectViewBuilder : MapObjectViewBuilder
    {

        #region Private Fields

        private readonly Lazy<Guid> m_uniqueLazyId = new Lazy<Guid>(() => new Guid("{79BDC41A-E584-4CF1-AF75-AF3D8F417A79}"));

        #endregion Private Fields

        #region Public Properties

        public override string Name => "Performance MapObjectView Builder";

        public override int Priority => -2;

        public override Guid UniqueId => m_uniqueLazyId.Value;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Create a view for the corresponding map object
        /// </summary>
        /// <param name="mapObjects">List of map objects to build</param>
        /// <param name="context">Current map context</param>
        /// <returns>List of views representing the map objects</returns>
        public override IEnumerable<IMapObjectView> CreateViews(IEnumerable<MapObject> mapObjects, MapContext context)
        {
            var result = new List<IMapObjectView>();

            // Build the visuals on the different thread
            Action pFunc = delegate
            {
                foreach (var mapObject in mapObjects)
                {
                    if (mapObject is CameraMapObject)
                    {
                        var camView = new CameraVisual();

                        camView.Initialize(Workspace, mapObject);
                        result.Add(camView);
                    }
                }
            };
            PerformanceLayer.LocalDispatcher.Invoke(pFunc);

            return result;
        }

        #endregion Public Methods

    }
}