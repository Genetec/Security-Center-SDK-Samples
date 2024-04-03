// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System.Windows.Ink;

namespace CaseManagement.Views
{
    /// <summary>
    /// Interaction logic for CaseTileViewer.xaml
    /// </summary>
    public partial class CaseTileViewer
    {

        #region Private Fields

        private readonly Genetec.Sdk.Workspace.Workspace m_workspace;
        private CaseContent m_content;

        #endregion Private Fields

        #region Public Constructors

        public CaseTileViewer(Genetec.Sdk.Workspace.Workspace workspace)
        {
            m_workspace = workspace;
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Update(CaseContent content)
        {
            m_content = content;

            // Get our incident attachment
            m_inkCanvas.Strokes = m_content?.Strokes ?? new StrokeCollection();
        }

        #endregion Public Methods

    }
}