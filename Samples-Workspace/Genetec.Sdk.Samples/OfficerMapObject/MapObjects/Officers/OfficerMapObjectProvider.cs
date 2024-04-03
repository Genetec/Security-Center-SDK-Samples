// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Entities.Maps;
using Genetec.Sdk.Workspace;
using Genetec.Sdk.Workspace.Components.MapObjectProvider;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Map = Genetec.Sdk.Entities.Map;

namespace OfficerMapObject.MapObjects.Officers
{
    public sealed class OfficerMapObjectProvider : MapObjectProvider, IDisposable
    {
        #region Private Fields

        private static readonly ObservableCollection<OfficerMapObject> s_officers = new ObservableCollection<OfficerMapObject>();

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets the name of the component
        /// </summary>
        public override string Name => "Officers map object provider";

        /// <summary>
        /// Gets the unique identifier of the component
        /// </summary>
        public override Guid UniqueId => new Guid("{6E50965C-7DB0-4A1B-91C4-85DF6945C388}");

        #endregion Public Properties

        #region Public Constructors

        public OfficerMapObjectProvider(Workspace workspace)
        {
            Initialize(workspace);
            s_officers.CollectionChanged += OnOfficersCollectionChanged;
        }

        #endregion Public Constructors

        #region Public Methods

        public static void AddOfficer(OfficerMapObject officer)
        {
            lock (s_officers)
            {
                s_officers.Add(officer);
            }
        }

        public static void DeleteEarliestOfficer()
        {
            lock (s_officers)
            {
                s_officers.Remove(s_officers.FirstOrDefault());
            }
        }

        public static void DeleteLatestOfficer()
        {
            lock (s_officers)
            {
                s_officers.Remove(s_officers.LastOrDefault());
            }
        }

        public static void DeleteAllOfficers()
        {
            lock (s_officers)
            {
                foreach (var officerMapObject in s_officers.ToList())
                {
                    s_officers.Remove(officerMapObject);
                }
            }
        }

        public void Dispose()
        {
            if (s_officers != null)
            {
                s_officers.CollectionChanged -= OnOfficersCollectionChanged;
            }
        }

        /// <summary>
        /// Override to return the map objects from our <see cref="MapObjectProvider"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override IList<MapObject> Query(MapObjectProviderContext context)
        {
            var map = Workspace.Sdk.GetEntity(context.MapId) as Map;
            // We only provide officers for geo referenced maps
            if ((map == null) || !map.IsGeoReferenced) return null;
            var result = new List<MapObject>(s_officers);
            return result;
        }

        #endregion Public Methods

        #region Private Methods

        private void OnOfficersCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var addedItems = new List<MapObject>();
            var removedItems = new List<MapObject>();

            if (e.NewItems != null)
            {
                addedItems.AddRange(e.NewItems.Cast<MapObject>());
            }

            if (e.OldItems != null)
            {
                removedItems.AddRange(e.OldItems.Cast<MapObject>());
            }

            //Since our collection has changed, we want to invalidate the map objects on our MapObjectProvider.
            Invalidate(Guid.Empty, addedItems, removedItems, null);
        }

        #endregion Private Methods

        public static ReadOnlyCollection<OfficerMapObject> GetOfficers() 
            => new ReadOnlyObservableCollection<OfficerMapObject>(s_officers);
    }
}