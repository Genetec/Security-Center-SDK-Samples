// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Ink;
using Genetec.Sdk.Reports.Fields;
using Genetec.Sdk.Workspace.Components.ContentBuilder;
using Genetec.Sdk.Workspace.Pages.Contents;

namespace CaseManagement.Builders
{
    public sealed class CaseContentBuilder : ContentBuilder
    {

        #region Private Fields

        private const string DRAWING = "Drawing";

        private readonly Lazy<Guid> m_uniqueLazyId = new Lazy<Guid>(() => new Guid("{EE8535F0-B502-4EF3-BDE4-E1CCB0287414}"));

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets the name of the component.
        /// </summary>
        public override string Name => nameof(CaseContentBuilder);

        /// <summary>
        /// Gets the unique identifier of the component.
        /// </summary>
        public override Guid UniqueId => m_uniqueLazyId.Value;

        #endregion Public Properties

        #region Public Methods

        public override Content BuildContent(ContentBuilderContext context)
        {
            CaseContent drawingContent = null;

            if (context.Fields.Contains(DefaultFields.IncidentId))
            {
                var incidentId = context.Fields.GetValueOrDefault<Guid>(DefaultFields.IncidentId);

                var incident = Workspace.Sdk.IncidentManager.GetIncident(incidentId);
                // Is it a drawing content
                var attachedData = incident?.GetAttachedData(CaseIncidentBuilder.CaseIncidentDataType);

                // Display drawing if any
                if (attachedData != null && attachedData.Any())
                {
                    try
                    {
                        var drawing = attachedData.Find(item => item.Identifier == DRAWING);
                        if (drawing?.Data != null)
                        {
                            using (var stream = new MemoryStream(Convert.FromBase64String(drawing.Data)))
                            {
                                var strokes = new StrokeCollection(stream);
                                drawingContent = new CaseContent(strokes);
                                drawingContent.Initialize(Workspace);
                                drawingContent.Title = DRAWING;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            }

            return drawingContent;
        }

        /// <summary>
        /// Builds a group of contents to be displayed in the tile from the specified list of fields.
        /// </summary>
        /// <remarks>If you do not support any of the specified fields, return null</remarks>
        /// <returns>Content group built</returns>
        public override ContentGroup BuildContentGroup(ContentBuilderContext context) => null;

        #endregion Public Methods

    }
}