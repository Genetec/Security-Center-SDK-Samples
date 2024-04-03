using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Queries;
using Genetec.Sdk.Queries.LicensePlateManagement;
using Genetec.Sdk.Queries.LicensePlateManagement.Inventory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;

// ==========================================================================
// Copyright (C) 2017 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
//
// Ephemerides for September 7:
//  AD 70 – A Roman army under Titus occupies and plunders Jerusalem.
//  1159 – Pope Alexander III is chosen.
//  1943 – A fire at the Gulf Hotel in Houston kills 55 people.
// ==========================================================================
namespace QuerySample
{
    #region Classes

    public partial class FindByPlate : Form
    {
        #region Constants

        private static readonly int TimeOut = (int)TimeSpan.FromSeconds(20).TotalMilliseconds;

        #endregion

        #region Fields

        /// <summary>
        /// Represent the query lauched to Security Center
        /// </summary>
        private InventoryQuery m_inventoryQuery;

        /// <summary>
        /// Represent the query lauched to Security Center
        /// </summary>
        private ReadQuery m_readQuery;

        /// <summary>
        /// Represent the SDK class used to control Security Center
        /// </summary>
        private Engine m_sdkEngine;

        #endregion

        #region Constructors

        public FindByPlate()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Methods

        public void Initialize(Engine sdkEngine)
        {
            m_sdkEngine = sdkEngine;
            m_inventoryQuery = m_sdkEngine.ReportManager.CreateReportQuery(ReportType.Inventory) as InventoryQuery;
        }

        #endregion

        #region Private Methods

        private void DisplayResults(DataTable dt)
        {
            try
            {
                m_results.BeginUpdate();
                m_results.Items.Clear();

                // Format the list view to receive a read event
                m_results.Columns.Clear();
                m_results.Columns.Add("Plate");
                m_results.Columns.Add("PlateOrigin");
                m_results.Columns.Add("Sector");
                m_results.Columns.Add("Row");
                m_results.Columns.Add("Arrival");
                m_results.Columns.Add("ElapsedTime");
                m_results.Columns.Add("Parking");
                m_results.Columns.Add("Patroller");
                m_results.Columns.Add("ManualCapture");
                m_results.Columns.Add("Inventory");
                m_results.Columns.Add("ContextImage");
                m_results.Columns.Add("ContextThumbnailImage");
                m_results.Columns.Add("Edited");
                m_results.Columns.Add("Timestamp");
                m_results.Columns.Add("ManuallyRemoved");
                m_results.Columns.Add("LprImage");
                m_results.Columns.Add("LprThumbnailImage");
                m_results.Columns.Add("Action");

                if (dt == null) return;
                foreach (DataRow dr in dt.Rows)
                {
                    string plate = (string)dr["PlateRead"];
                    string plateState = (string)dr["PlateOrigin"];
                    string sector = (string)dr["Sector"];
                    string rowName = (string)dr["Row"];
                    DateTime? arrival = (DateTime?)(dr.IsNull("Arrival") ? null : dr["Arrival"]);
                    TimeSpan? elapsedTime = (TimeSpan?)(dr.IsNull("ElapsedTime") ? null : dr["ElapsedTime"]);
                    Guid parking = (Guid)dr["Parking"];
                    Guid patroller = (Guid)dr["Patroller"];
                    bool manualCapture = (bool)dr["ManualCapture"];
                    short inventory = (short)dr["Inventory"];
                    byte[] contextImage = (byte[])dr["ContextImage"];
                    byte[] contextThumb = (byte[])dr["ContextThumbnailImage"];
                    bool edited = (bool)dr["Edited"];
                    DateTime? timestamp = (DateTime?)(dr.IsNull("Timestamp") ? null : dr["Timestamp"]);
                    bool manuallyRemoved = (bool)dr["ManuallyRemoved"];
                    byte[] lprImage = (byte[])dr["LprImage"];
                    byte[] lprThumbnailImage = (byte[])dr["LprThumbnailImage"];
                    string action = (string)dr["Action"];

                    ListViewItem listViewItem = new ListViewItem();
                    listViewItem.Text = plate;
                    listViewItem.SubItems.Add(plateState);
                    listViewItem.SubItems.Add(sector);
                    listViewItem.SubItems.Add(rowName);
                    listViewItem.SubItems.Add(arrival.HasValue ? arrival.Value.ToLocalTime().ToString() : "");
                    listViewItem.SubItems.Add(elapsedTime.HasValue ? elapsedTime.Value.TotalSeconds.ToString() : "");
                    listViewItem.SubItems.Add(parking.ToString());
                    listViewItem.SubItems.Add(patroller.ToString());
                    listViewItem.SubItems.Add(manualCapture.ToString());
                    listViewItem.SubItems.Add(inventory.ToString());
                    listViewItem.SubItems.Add(contextImage != null && contextImage.Length > 0 ? "true" : "false");
                    listViewItem.SubItems.Add(contextThumb != null && contextImage.Length > 0 ? "true" : "false");
                    listViewItem.SubItems.Add(edited.ToString());
                    listViewItem.SubItems.Add(timestamp.HasValue ? timestamp.Value.ToLocalTime().ToString() : "");
                    listViewItem.SubItems.Add(manuallyRemoved.ToString());
                    listViewItem.SubItems.Add(lprImage != null && contextImage.Length > 0 ? "true" : "false");
                    listViewItem.SubItems.Add(lprThumbnailImage != null && contextImage.Length > 0 ? "true" : "false");
                    listViewItem.SubItems.Add(action);
                    m_results.Items.Add(listViewItem);
                }
            }
            finally
            {
                m_results.EndUpdate();
            }
        }

        private void m_search_Click(object sender, EventArgs e)
        {
            Search();
        }

        private Task<DataTable> Query(string plate)
        {
            if (string.IsNullOrEmpty(plate))
                return Task.FromResult<DataTable>(null);

            return Task<DataTable>.Factory.StartNew(() =>
            {
                QueryCompletedEventArgs args = m_readQuery.Query(TimeOut);
                Dictionary<Guid, List<short>> parkingFacilityInventories = new Dictionary<Guid, List<short>>();

                if (!args.Success || args.Data.Rows.Count == 0)
                    return null;

                foreach (DataRow dr in args.Data.Rows)
                {
                    Guid guid = (Guid)dr[29]; //ZoneId guid
                    if (guid != Guid.Empty)
                    {
                        short inventory = (short)dr[34]; //Inventory Index
                        if (inventory == 0) break;
                        if (!parkingFacilityInventories.ContainsKey(guid))
                            parkingFacilityInventories.Add(guid, new List<short>());
                        List<short> inventories = parkingFacilityInventories[guid];
                        if (!inventories.Contains(inventory))
                            inventories.Add(inventory);
                    }
                }

                DataTable dt = null;
                foreach (var parkingFacility in parkingFacilityInventories)
                {
                    m_inventoryQuery.ResetQuery();
                    m_inventoryQuery.Plates.Add(plate);
                    m_inventoryQuery.PartialMatchPlates = true;
                    m_inventoryQuery.SourceCriteria = new InventorySourceCriteria(parkingFacility.Key, InventorySourceType.UntaggedReads);
                    QueryCompletedEventArgs untagged = m_inventoryQuery.Query(TimeOut);
                    if (dt == null) dt = untagged.Data;
                    else dt.Merge(untagged.Data);

                    foreach (var inventory in parkingFacility.Value)
                    {
                        m_inventoryQuery.ResetQuery();
                        m_inventoryQuery.Plates.Add(plate);
                        m_inventoryQuery.PartialMatchPlates = true;
                        m_inventoryQuery.SourceCriteria = new InventorySourceCriteria(parkingFacility.Key, inventory);
                        QueryCompletedEventArgs specific = m_inventoryQuery.Query(TimeOut);
                        if (dt == null) dt = specific.Data;
                        else dt.Merge(specific.Data);
                    }
                }

                return dt;
            });
        }

        /// <summary>
        /// Launch the query
        /// </summary>
        private async void Search()
        {
            string plate = m_plate.Text;
            m_readQuery = m_sdkEngine.ReportManager.CreateReportQuery(ReportType.LprRead) as ReadQuery;
            m_readQuery.Plates.Add(plate);
            m_readQuery.PartialMatchPlates = true;
            m_readQuery.SourceIds.Add(SdkGuids.SystemConfiguration);

            DataTable dt = await Query(plate);
            DisplayResults(dt);
        }

        #endregion
    }

    #endregion
}

