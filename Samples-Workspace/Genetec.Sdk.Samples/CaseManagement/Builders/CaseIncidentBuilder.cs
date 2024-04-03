// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using CaseManagement.Views;
using Genetec.Sdk.Workspace.Components.Incident;

namespace CaseManagement.Builders
{
    public sealed class CaseIncidentBuilder : IncidentBuilder
    {

        #region Public Fields

        public const string CaseIncidentDataType = "CaseIncident";

        #endregion Public Fields

        #region Private Fields

        private readonly Lazy<Guid> m_uniqueLazyId = new Lazy<Guid>(() => new Guid("{8FB639DF-67C2-440C-BA46-FDB5B47962B5}"));

        #endregion Private Fields

        #region Public Properties

        public override List<IncidentField> Fields
        {
            get
            {
                var fields = new List<IncidentField>
                {
                    new IncidentField {DisplayInReport = false, Name = "Drawing"},
                    new IncidentField {DisplayInReport = true, Name = "Comment1", DisplayName = "Comment 1"},
                    new IncidentField {DisplayInReport = true, Name = "Comment2", DisplayName = "Comment 2"}
                };

                return fields;
            }
        }

        /// <summary>
        /// Gets the incident data type supported by this incident plugin
        /// </summary>
        public override string IncidentDataType => CaseIncidentDataType;

        /// <summary>
        /// Gets the name of the component
        /// </summary>
        public override string Name => "Case incident builder";

        /// <summary>
        /// Gets the unique identifier of the component
        /// </summary>
        public override Guid UniqueId => m_uniqueLazyId.Value;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Create a view that will be inserted in the incident dialog
        /// </summary>
        /// <returns>View to be displayed</returns>
        public override IncidentView CreateView(Genetec.Sdk.Incident incident)
        {
            var view = new CaseIncidentView();
            view.Initialize(Workspace);
            return view;
        }

        /// <summary>
        /// Extract from the incident data the value of the specified field.
        /// </summary>
        /// <param name="incidentData">The incident data</param>
        /// <param name="fieldName">The field name (unique ID)</param>
        /// <returns>The field value</returns>
        public override string GetFieldValue(List<Genetec.Sdk.Incidents.IncidentDataEntry> incidentData, string fieldName)
        {
            if (fieldName == "Drawing")
            {
                Debug.Fail("This field cannot be displayed in report.");
                return string.Empty;
            }

            var entry = incidentData.Find(item => item.Identifier == fieldName);

            return entry != null ? entry.Data : string.Empty;
        }

        #endregion Public Methods

    }
}