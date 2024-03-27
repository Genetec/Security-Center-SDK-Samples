using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// ==========================================================================
// Copyright (C) by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace AccessControl.Sample.RawEventQuery
{
    internal partial class Sample
    {
        private IEngine CreateEngine() => new Engine();
        
        /// <summary>
        /// Method used to connect the SDK sample to GenetecServer
        /// </summary>
        /// <param name="engine">Object used to communicate with the server</param>
        /// <param name="server">IP or hostname where is located the Directory</param>
        /// <param name="username">Username used to connect to the server</param>
        /// <param name="password">Password used to connect to the server</param>
        /// <param name="token">Cancellation token in order cancel the login</param>
        /// <returns>True if the connection was successful, False otherwise</returns>
        private async Task<bool> LogOnAsync(IEngine engine, string server, string username, string password, CancellationToken token)
        {
            await engine.LogOnAsync (server, username, password, token);

            return (engine.IsConnected);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="engine">Object used to cmmunicate with the server</param>
        /// <param name="insertionStartTimeUtc">Optional lowest bound of insertion timestamp (in UTC)</param>
        /// <param name="insertionEndTimeUtc">Optional highest bound of insertion timestamp (in UTC)</param>
        /// <param name="maximumResultCount">The maximum number of result returned by Access Manager</param>
        /// <param name="eventTypeFilter">Optional filter based on event types</param>
        /// <param name="startAfterIndexes">Optional starting after indexes (position per Access Manager)</param>
        /// <returns>Object containing the results of the query</returns>
        private Task<QueryCompletedEventArgs> QueryRawEventsAsync(
            IEngine engine, 
            DateTime? insertionStartTimeUtc, 
            DateTime? insertionEndTimeUtc, 
            int maximumResultCount, 
            IEnumerable<EventType> eventTypeFilter,
            IEnumerable<RawEventIndex> startAfterIndexes,
            CancellationToken token)
        {
            // Create the query
            var query = engine.ReportManager.CreateReportQuery(ReportType.AccessControlRawEvent) as AccessControlRawEventQuery;

            // Applying insertion start / end time range
            // Note: Insertion start and/or end times can be null which mean there is no restriction on the lowest / highest timestamp.
            query.InsertionStartTimeUtc = insertionStartTimeUtc;
            query.InsertionEndTimeUtc = insertionEndTimeUtc;

            // Maximum result count needs to be in the range [1, 50000]
            query.MaximumResultCount = maximumResultCount;

            // If event types are specified, only events of those types will be returned
            if (eventTypeFilter != null)
            {
                foreach (var eventType in eventTypeFilter)
                {
                    query.EventTypeFilter.Add(eventType);
                }
            }

            // If indexes are specified, only events of the specified access managers will be returned
            // In each index, the specific position will be used as starting point for each acccess manager
            if (startAfterIndexes != null)
            {
                foreach (var index in startAfterIndexes)
                {
                    query.StartingAfterIndexes.Add(index);
                }
            }

            // Sending the query on the server to be executed
            return (Task.Factory.FromAsync(query.BeginQuery(null, null), result => query.EndQuery(result)));
        }

        /// <summary>
        /// Retrieve the access manager roles
        /// </summary>
        /// <param name="engine">Object used to communicate with the server</param>
        /// <returns>List of access manager roles</returns>
        private async Task<List<AccessManagerRole>> RetrieveAccessManagerRolesAsync(IEngine engine)
        {
            // Create the query
            var query = (EntityConfigurationQuery)engine.ReportManager.CreateReportQuery(ReportType.EntityConfiguration);
            query.EntityTypeFilter.Add(EntityType.Role, (byte)RoleType.AccessManager);

            // Sending the query on the server to be executed
            var result = await Task.Factory.FromAsync(query.BeginQuery(null, null), result => query.EndQuery(result));

            // Retrieve the guid of every access manager role
            var accessManagerGuids = result.Data.AsEnumerable().Select(row => row.Field<Guid>("Guid")).ToList();
            
            // Retrieve the complete object for each role
            var roles = engine.GetEntities<AccessManagerRole>(EntityType.Role);

            return (roles.ToList());
        }
    }
}
