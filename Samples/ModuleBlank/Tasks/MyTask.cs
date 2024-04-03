using Genetec.Sdk.Workspace.Pages;
using System;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace ModuleBlank.Tasks
{
    #region Classes

    [Page(typeof(MyTaskDescriptor))]
    public class MyTask : Page
    {
        #region Protected Methods

        /// <summary>
        /// Deserializes the data contained by the specified byte array.
        /// </summary>
        /// <param name="data">A byte array that contains the data.</param>
        protected override void Deserialize(byte[] data)
        {
        }

        /// <summary>
        /// Initialize the page.
        /// </summary>
        /// <remarks>At this step, the <see cref="Genetec.Sdk.Workspace.Workspace"/> is available.</remarks>
        protected override void Initialize()
        {
            View = new MyTaskUserControl();
        }

        /// <summary>
        /// Serializes the data to a byte array.
        /// </summary>
        /// <returns>A byte array that contains the data.</returns>
        protected override byte[] Serialize()
        {
            return null;
        }

        #endregion
    }

    /// <summary>
    /// Describes the attributes of MyTask.
    /// </summary>
    public class MyTaskDescriptor : PageDescriptor
    {
        #region Properties

        /// <summary>
        /// Gets the page's default name.
        /// </summary>
        public override string Name
        {
            get
            {
                // This name is the one that will appear in the Home menu. It will also be the default name on creation.
                return "My task";
            }
        }

        /// <summary>
        /// Gets the page's unique ID.
        /// </summary>
        public override Guid Type
        {
            get
            {
                // The Guid has to be unique among all your tasks. It should never be changed afterwards.
                return new Guid("{9F521EA7-AAE1-41CC-A4FF-F6D1424D4A53}");
            }
        }

        #endregion
    }

    #endregion
}

