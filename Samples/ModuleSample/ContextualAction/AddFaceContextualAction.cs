// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace.ContextualAction;
using ModuleSample.Components;
using System;
using System.Windows.Media.Imaging;

namespace ModuleSample.ContextualAction
{

    internal class AddFaceContextualAction : Genetec.Sdk.Workspace.ContextualAction.ContextualAction
    {

        #region Public Properties

        public override Guid Group => new Guid("{0A02E779-1AC2-40B4-BB2B-7F46E8B93ACD}");

        public override Guid Id => new Guid("{DE6A4001-93A3-479A-A909-71DD25446BE7}");

        public override int Priority => 100;

        #endregion Public Properties

        #region Public Constructors

        public AddFaceContextualAction()
        {
            Name = "Add Face";
            Icon =
                new BitmapImage(new Uri(@"pack://application:,,,/ModuleSample;Component/Resources/Faces.png",
                    UriKind.RelativeOrAbsolute));
        }

        #endregion Public Constructors

        #region Public Methods

        public override bool CanExecute(ContextualActionContext context) => context is TileContextualActionContext;
       
        public override bool Execute(ContextualActionContext context)
        {
            Console.WriteLine("Face Added");
            var tile = context as TileContextualActionContext;
            foreach (var content in tile.State.Content.Contents)
            {
                if (content is FaceContent faceContent)
                {
                    var face = faceContent.Face;
                    var metadata = faceContent.Metadata;
                }
            }
            return true;

        }

        #endregion Public Methods

    }

}