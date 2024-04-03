// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Diagnostics.Logging.Core;
using Genetec.Sdk.Events;
using Genetec.Sdk.Events.VideoAnalytics;
using Genetec.Sdk.Workspace.Extenders;
using Genetec.Sdk.Workspace.Fields;
using System;
using System.Collections.Generic;

namespace ModuleSample.Events
{

    public class CustomEventExtender : EventExtender, IDisposable
    {

        #region Public Fields

        public static string Metadata = "Metadata";

        #endregion Public Fields

        #region Private Fields

        private readonly Logger m_logger;

        #endregion Private Fields

        #region Public Properties

        public override IList<Field> Fields { get; }

        #endregion Public Properties

        #region Public Constructors

        public CustomEventExtender()
        {
            m_logger = Logger.CreateInstanceLogger(this);

            Fields = new List<Field>
            {
                new Field(Metadata, typeof(string))
                {
                    Title=Metadata,
                    IsDisplayable = false,
                }
            };
        }

        #endregion Public Constructors

        #region Public Methods

        public void Dispose()
        {
            m_logger.Dispose();
        }

        public override bool Extend(Event @event, FieldsCollection fields)
        {
            var result = false;
            try
            {
                if (@event is VideoAnalyticsFaceDetectedEvent analyticsEvent)
                {
                    fields[Metadata] = analyticsEvent.Metadata;
                    m_logger.TraceDebug("Event Extender added the Metadata field information.");
                    result = true;
                }
            }
            catch
            {
                // it might not be an event for here.
            }

            return result;
        }

        #endregion Public Methods

    }

}