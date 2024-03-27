using System;
using System.Collections.Generic;
using System.Linq;
using Genetec.Sdk.Entities.Maps;
using Genetec.Sdk.Workspace.Components;
using Genetec.Sdk.Workspace.Components.MapObjectViewBuilder;
using Genetec.Sdk.Workspace.Maps;
using VisualMapObject.Maps;
using VisualMapObject.Maps.Layers;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace VisualMapObject.Components
{
    /// <summary>
    /// Represents the factory to create camera visual view.
    /// Those are the visual views.
    /// </summary>
    public sealed class CameraVisualBuilder : MapObjectViewBuilder
    {
        /// <summary>
        /// To avoid creating new Guid's each time the UniqueId.
        /// </summary>
        public static readonly Guid Identifier = new Guid("{F5C33510-818D-4C7F-ACF7-46609E222342}");

        /// <summary>
        /// The name of the Component.
        /// </summary>
        public override string Name {get { return "Camera Visual Builder"; }}

        /// <summary>
        /// Gets the priority of the component, lowest is better.
        /// Lower builder priority will be called before.
        /// </summary>
        public override int Priority {get { return -2; }}

        /// <summary>
        /// Gets the unique identifier of the component.
        /// </summary>
        public override Guid UniqueId {get { return Identifier; }}

        /// <summary>
        /// Create a view for the corresponding map objects.
        /// </summary>
        /// <param name="mapObjects">List of map objects in the current view(visible section of the current map).</param>
        /// <param name="context">Current map context</param>
        /// <returns>List of views representing the map objects</returns>
        public override IEnumerable<IMapObjectView> CreateViews(IEnumerable<MapObject> mapObjects, MapContext context)
        {
            //Here we gets the CameraMapObject to create our own type of object.
            //Visual objects have a high thread affinity thus,
            //we need to create the mapObject using the right thread.
            var result = new List<IMapObjectView>();
            Action pFunc = delegate
            {
                result.AddRange(mapObjects.OfType<CameraMapObject>()
                                          .Select(camMapObject => new CameraVisualView(Workspace, camMapObject)));
            };
            CameraVisualLayer.Invoke(pFunc);
            return result;
        }
    }
}
