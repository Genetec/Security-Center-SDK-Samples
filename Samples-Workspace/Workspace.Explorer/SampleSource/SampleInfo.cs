// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System.Collections.Generic;

namespace Workspace.Explorer.SampleSource
{
    public class SampleInfo
    {

        #region Public Properties

        public string Arguments { get; set; }
        public string Category { get; set; }
        public bool ConfigTool { get; set; }
        public string Description { get; set; }

        public List<string> Requirements { get; set; }
        public List<string> Instructions { get; set; }
        public string Result { get; set; }
        
        public string Module { get; set; }
        public string Name { get; set; }
        public bool SecurityCenter { get; set; }
        public string Title { get; set; }

        #endregion Public Properties
    }
}