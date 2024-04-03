using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Entities.Maps;
using Genetec.Sdk.Workspace.Components.MapObjectProvider;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using VisualMapObject.Maps;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace VisualMapObject.Components
{
    #region Classes

    /// <summary>
    /// A provider class holds the related map object elements.
    /// Provider are mostly usefull for objects that don't already exists.
    /// </summary>
    public sealed class AccidentMapObjectProvider : MapObjectProvider, IDisposable
    {
        #region Constants

        /// <summary>
        /// The guid of this provider.
        /// This is use to register the provider (of course this should be unique).
        /// </summary>
        public static readonly Guid ProviderGuid = new Guid("{F2D3A624-DAAF-47EE-BE2F-6EFB0361B3B9}");

        /// <summary>
        /// Lazy initialization of the collection.
        /// </summary>
        private static readonly Lazy<RangeObservableCollection<AccidentMapObject>> s_accidents =
                            new Lazy<RangeObservableCollection<AccidentMapObject>>(() => new RangeObservableCollection<AccidentMapObject>());

        /// <summary>
        /// Classic lock for synchronization.
        /// </summary>
        private static readonly Lazy<object> s_lock = new Lazy<object>(() => new object());

        #endregion

        #region Properties

        /// <summary>
        /// The collection that hold our map objects.
        /// </summary>
        private static RangeObservableCollection<AccidentMapObject> Accidents {get{return s_accidents.Value;}}

        /// <summary>
        /// The name of the Component.
        /// </summary>
        public override string Name {get{return "Accident map object provider";}}

        /// <summary>
        /// Gets the unique identifier of the component.
        /// </summary>
        public override Guid UniqueId { get { return ProviderGuid; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Provider are only build one time (they should only be build one time or at least register one time only in the module).
        /// </summary>
        public AccidentMapObjectProvider()
        {
            lock (s_lock.Value)
            {
                //Our hardcoded accidents list.
                Accidents.AddRange(new[]
                {
                    new AccidentMapObject(45.469069, -73.523598, "Accident on Champlain bridge.\r\nReported via Youbert app."),
                    new AccidentMapObject(45.521400, -73.525474, "Accident on Jacques-Cartier bridge.\r\nReported via Waze app."),
                    new AccidentMapObject(45.511280, -73.764424, "Accident on Autoroute Chomedey.\r\nReported via Waze app."),
                    new AccidentMapObject(43.679429, -79.617244, "Toronto #1"),
                    new AccidentMapObject(43.676422, -79.611782, "Toronto #2"),
                    new AccidentMapObject(43.681317, -79.609050, "Toronto #3")
                });

                Accidents.CollectionChanged += OnCollectionChanged;
            }
        }

        #endregion

        #region Destructors and Dispose Methods

        /// <summary>
        /// Nothing much to do except removing the reference to events.
        /// </summary>
        public void Dispose()
        {
            lock (s_lock.Value)
            {
                Accidents.CollectionChanged -= OnCollectionChanged;
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Informs the map that the objects are invalidated.
        /// </summary>
        /// <param name="sender">The sender of the event(Accidents collection).</param>
        /// <param name="e">The event.</param>
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Replace:

                    List<MapObject> removed = null;
                    var data = ((AccidentMapObject[])e.OldItems.Cast<object>().FirstOrDefault());
                    if (data != null)
                    {
                        removed = data.Cast<MapObject>().ToList();
                    }

                    Invalidate(
                        Guid.Empty,
                        null,
                        removed,
                        null
                    );
                    break;
                default:
                    Invalidate(
                        Guid.Empty,
                        e.NewItems != null ? e.NewItems.Cast<MapObject>().ToList() : null,
                        e.OldItems != null ? e.OldItems.Cast<MapObject>().ToList() : null,
                        null
                    );
                    break;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds accidents to the collection.
        /// </summary>
        /// <param name="accidents">The accident(s) to add.</param>
        public static void AddAccident(params AccidentMapObject[] accidents)
        {
            lock (s_lock.Value)
            {
                Accidents.AddRange(accidents);
            }
        }

        /// <summary>
        /// Clears accidents to the collection.
        /// </summary>
        public static void ClearAccident()
        {
            lock (s_lock.Value)
            {
                Accidents.Clear();
            }
        }

        /// <summary>
        /// Removes accidents to the collection.
        /// </summary>
        /// <param name="accidents">The accident(s) to remove.</param>
        public static void RemoveAcccident(params AccidentMapObject[] accidents)
        {
            lock (s_lock.Value)
            {
                Accidents.RemoveRange(accidents);
            }
        }

        /// <summary>
        /// This is called by the workspace when the view changed. It queries the map objects.
        /// </summary>
        /// <param name="mapId">The id of the current map that request the query.</param>
        /// <param name="viewArea">The area view of the map.</param>
        /// <returns></returns>
        public override IList<MapObject> Query(MapObjectProviderContext context)
        {
            lock (s_lock.Value)
            {
                var map = Workspace.Sdk.GetEntity(context.MapId) as Map;
                return map != null && map.IsGeoReferenced
                    ? Accidents.Cast<MapObject>().ToList()
                    : null;
            }
        }

        #endregion
    }

    #endregion
}

