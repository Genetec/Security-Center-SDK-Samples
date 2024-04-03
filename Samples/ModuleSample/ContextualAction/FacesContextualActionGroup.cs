// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace.ContextualAction;
using System;
using System.Windows.Media.Imaging;

namespace ModuleSample.ContextualAction
{

    internal class FacesContextualActionGroup : ContextualActionGroup
    {

        #region Public Properties

        public override Guid Id => new Guid("{0A02E779-1AC2-40B4-BB2B-7F46E8B93ACD}");

        #endregion Public Properties

        #region Public Constructors

        public FacesContextualActionGroup()
        {
            Name = "Faces";
            Icon =
                new BitmapImage(new Uri(@"pack://application:,,,/ModuleSample;Component/Resources/Faces.png",
                    UriKind.RelativeOrAbsolute));
        }

        #endregion Public Constructors

    }

}