// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace.ContextualAction;
using OfficerMapObject.MapObjects.Officers;
using System;
using System.Windows.Media.Imaging;

namespace OfficerMapObject
{
    public sealed class AddOfficerContextualAction : ContextualAction
    {
        #region Public Properties

        /// <summary>
        /// Gets the action's unique identifier
        /// </summary>
        public override Guid Id => new Guid("{F2AA03A6-6C73-4DEE-8B20-AA05E19A2AAB}");

        /// <summary>
        /// Gets the priority of the component, lowest is better
        /// </summary>
        public override int Priority => 0;

        #endregion Public Properties

        #region Public Constructors

        public AddOfficerContextualAction()
        {
            Name = "Add officer";
            Icon = new BitmapImage(new Uri(@"pack://application:,,,/OfficerMapObject;Component/Resources/Officer.png", UriKind.RelativeOrAbsolute));
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Return true if the contextual action can execute
        /// </summary>
        /// <param name="context">Current action's context</param>
        public override bool CanExecute(ContextualActionContext context) => context is MapContextualActionContext;

        /// <summary>
        /// Executes the contextual action
        /// </summary>
        /// <param name="context">Current action's context</param>
        public override bool Execute(ContextualActionContext context)
        {
            if (!(context is MapContextualActionContext mapContext)) return false;
            var location = mapContext.Location;
            var officer = new MapObjects.Officers.OfficerMapObject(location.Latitude, location.Longitude);
            OfficerMapObjectProvider.AddOfficer(officer);
            return false;
        }

        #endregion Public Methods
    }
}