using Genetec.Sdk;
using Genetec.Sdk.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ArchiveTransferManagerSample.Services
{
    /// <summary>
    /// Simple class to remove the querying from the MainWindow page.
    /// </summary>
    public class QueryService
    {
        private readonly Engine m_sdkEngine;

        public QueryService(Engine engine)
            => m_sdkEngine = engine ?? throw new ArgumentNullException(nameof(engine));

        public event EventHandler OnQueryCompleted;

        public void AddEntitiesToCache(IEnumerable<EntityType> entityTypes)
        {
            void _OnQueryCompleted(object sender, EventArgs e)
            {
                if (sender is AddEntitiesToCacheQuery querySender)
                {
                    querySender.OnQueryCompleted -= _OnQueryCompleted;
                    OnQueryCompleted?.Invoke(this, e);
                }
            }

            var q = new AddEntitiesToCacheQuery(m_sdkEngine, entityTypes);
            q.OnQueryCompleted += _OnQueryCompleted;
            q.StartEntityConfigurationQuery();
        }

        /// <summary>
        /// Queries
        /// </summary>
        class AddEntitiesToCacheQuery
        {
            private readonly Engine m_sdkEngine;
            private readonly IEnumerable<EntityType> m_entityTypes;

            public event EventHandler OnQueryCompleted;

            public int PageSize { get; set; } = 1000;

            public AddEntitiesToCacheQuery(Engine engine, IEnumerable<EntityType> entityTypes)
            {
                m_sdkEngine = engine ?? throw new ArgumentNullException(nameof(engine));
                m_entityTypes = entityTypes ?? throw new ArgumentNullException(nameof(entityTypes));
            }

            public void StartEntityConfigurationQuery(int page = 1)
            {
                var entityConfigurationQuery = (EntityConfigurationQuery)m_sdkEngine.ReportManager.CreateReportQuery(ReportType.EntityConfiguration);
                entityConfigurationQuery.Page = page;                              //The 1 based page index that will be received in the callback
                entityConfigurationQuery.PageSize = PageSize;                      //It's usually the size of page that SecurityCenter likes.
                entityConfigurationQuery.EntityTypeFilter.AddRange(m_entityTypes); //The type of entities for which we want to have data.
                entityConfigurationQuery.DownloadAllRelatedData = true;           //Be careful with this it downloads a lot of data, depending on the entities.
                entityConfigurationQuery.BeginQuery(OnEntityQueryResultsReceived, entityConfigurationQuery);
            }

            private void OnEntityQueryResultsReceived(IAsyncResult ar)
            {
                var query = ar.AsyncState as EntityConfigurationQuery;
                var results = query.EndQuery(ar);
                var entities = results.Data.Rows
                                      .Cast<DataRow>()
                                      .Select(row => (Guid)row[0])                 // First row of the query is the guid
                                      .Select(guid => m_sdkEngine.GetEntity(guid)) // Get the entities into the engine cache, now they are synchronized with the server
                                      .Where(entity => entity != null)             // Filter out the potential nulls
                                      .ToArray();

                // If there is more to query re-fetch the entities with an higher page number
                if (results.Data.Rows.Count >= PageSize)
                    StartEntityConfigurationQuery(query.Page + 1);
                else
                    OnQueryCompleted?.Invoke(this, EventArgs.Empty);
            }
        }        
    }
}
