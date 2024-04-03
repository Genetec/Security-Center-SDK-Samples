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
using MapsPlayer.Views;

namespace MapsPlayer.Builders
{
    public sealed class CameraMapObjectViewBuilder : MapObjectViewBuilder
    {

        #region Private Fields

        private readonly Lazy<Guid> m_uniqueLazyId = new Lazy<Guid>(() => new Guid("{05D08CC3-5093-4A59-A77B-E37B29BE8360}"));

        #endregion Private Fields

        #region Public Properties

        public override string Name => "Camera MapObjectView Builder";

        public override int Priority => -10;

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

            foreach (var mapObject in mapObjects)
            {
                if (mapObject is CameraMapObject)
                {
                    var camView = new CameraPlayerView();
                    camView.Initialize(Workspace, mapObject);
                    result.Add(camView);
                }
            }
            return result;
        }

        #endregion Public Methods

    }
}