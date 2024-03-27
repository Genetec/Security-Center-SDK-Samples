// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace.Components.ContentBuilder;
using Genetec.Sdk.Workspace.Pages.Contents;
using ModuleSample.Events;
using System;
using System.Windows;
using System.Windows.Media;

namespace ModuleSample.Components
{

    public sealed class FaceContent : Content
    {

        #region Public Fields

        // Using a DependencyProperty as the backing store for Face.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FaceProperty =
                            DependencyProperty.Register
                            ("Face", typeof(ImageSource), typeof(FaceContent));

        #endregion Public Fields

        #region Public Properties

        public ImageSource Face
        {
            get => (ImageSource)GetValue(FaceProperty);
            set => SetValue(FaceProperty, value);
        }

        public Guid FaceId { get; set; }

        public byte[] FaceImage { get; set; }

        public string Metadata { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public FaceContent()
        {
        }

        #endregion Public Constructors

    }

    public sealed class FaceContentBuilder : ContentBuilder
    {

        #region Public Properties

        /// <summary>
        /// Gets the name of the component
        /// </summary>
        public override string Name { get; } = "Face ContentBuilder";

        /// <summary>
        /// Gets the priority of the component, lowest is better
        /// </summary>
        public override int Priority { get; } = 100;

        /// <summary>
        /// Gets the unique identifier of the component
        /// </summary>
        public override Guid UniqueId { get; } = new Guid("{1AEACAC4-C455-48CD-BFD5-868744CB8013}");

        #endregion Public Properties

        #region Public Methods

        public override Content BuildContent(ContentBuilderContext context)
        {
            var faceImage = context.Fields.GetValueOrDefault<Byte[]>("Snapshot");
            var metadata = context.Fields.GetValueOrDefault<string>(CustomEventExtender.Metadata);

            if (faceImage != null)
            {
                var content = new FaceContent();
                content.Initialize(Workspace);
                content.FaceImage = faceImage;
                content.Metadata = metadata;
                content.Title = "Detected face";
                return content;
            }

            return base.BuildContent(context);
        }

        /// <summary>
        /// Build a group of contents to be displayed in the tile from the specified list of fields
        /// </summary>
        /// <remarks>If you do not support any of the specified fields, return null</remarks>
        /// <returns>Content group built</returns>
        public override ContentGroup BuildContentGroup(ContentBuilderContext context) => null;
       
        #endregion Public Methods

    }

}