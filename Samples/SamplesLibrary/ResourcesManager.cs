using System.Windows.Forms;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace Genetec.Sdk.Samples.SamplesLibrary
{
    #region Classes

    public static class ResourcesManager
    {
        #region Constants

        public static readonly ImageList EntityImageList = new ImageList();

        #endregion

        #region Constructors

        static ResourcesManager()
        {
            EntityImageList.ImageSize = new System.Drawing.Size(16, 16);
            EntityImageList.ColorDepth = ColorDepth.Depth32Bit;
            EntityImageList.Images.AddStrip(Resources.Entities);
        }

        #endregion
    }

    #endregion
}

