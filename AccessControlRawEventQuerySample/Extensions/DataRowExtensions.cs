using Genetec.Sdk;
using Genetec.Sdk.Queries;
using System;
using System.Data;

using DrawingHelper = AccessControl.Sample.RawEventQuery.Helpers.Drawing;

// ==========================================================================
// Copyright (C) by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace AccessControl.Sample.RawEventQuery.Extensions
{
    internal static class DataRowExtensions
    {
        public static Guid GetAccessManager(this DataRow row) => (Guid)row[AccessControlRawEventQuery.TableColumns.AccessManagerGuid];
        
        public static DateTime GetEventTimestamp(this DataRow row) => (DateTime)row[AccessControlRawEventQuery.TableColumns.Timestamp];

        public static EventType GetEventType(this DataRow row) => (EventType)row[AccessControlRawEventQuery.TableColumns.EventType];

        public static DateTime GetInsertionTimestamp(this DataRow row) => (DateTime)row[AccessControlRawEventQuery.TableColumns.InsertionTimestamp];

        public static long GetPosition(this DataRow row) => (long)row[AccessControlRawEventQuery.TableColumns.Position];
    }
}
