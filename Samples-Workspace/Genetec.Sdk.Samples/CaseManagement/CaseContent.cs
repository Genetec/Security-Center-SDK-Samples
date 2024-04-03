// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System.Windows.Ink;
using Genetec.Sdk.Workspace.Pages.Contents;

namespace CaseManagement
{
    public sealed class CaseContent : Content
    {

        #region Public Properties

        public StrokeCollection Strokes { get; }

        #endregion Public Properties

        #region Internal Constructors

        internal CaseContent(StrokeCollection strokes) => Strokes = strokes;

        #endregion Internal Constructors

    }
}