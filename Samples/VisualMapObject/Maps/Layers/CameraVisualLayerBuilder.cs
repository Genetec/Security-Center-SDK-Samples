using Genetec.Sdk.Workspace.Components;
using Genetec.Sdk.Workspace.Maps;
using System;
using System.Collections.Generic;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace VisualMapObject.Maps.Layers
{
    #region Classes

    /// <summary>
    /// This class represents the factory to creates the camera visual object. 
    /// </summary>
    public sealed class CameraVisualLayerBuilder : MapLayerBuilder
    {
        #region Constants

        /// <summary>
        /// The unique identifier that represents that builder.
        /// </summary>
        public static readonly Guid Identifier = new Guid("{380DECB3-1465-4810-8785-C89E7436D034}");

        #endregion

        #region Properties

        /// <summary>
        /// Gets the names of the component.
        /// </summary>
        public override string Name {get { return "Accident Visual Layer Builder"; }}

        /// <summary>
        /// Gets the unique identifier of the component.
        /// </summary>
        public override Guid UniqueId { get { return Identifier; } }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the CameraVisualLayer.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override IList<MapLayer> CreateLayers(MapContext context)
        {
            return new List<MapLayer> { new CameraVisualLayer(Workspace) };
        }

        /// <summary>
        /// Returns a value indicating if the component is supported.
        /// </summary>
        /// <param name="context">The context of the map.</param>
        /// <returns>A value indicating if the component is supported.</returns>
        public override bool IsSupported(MapContext context)
        {
            return true;
        }

        #endregion
    }

    #endregion
}

