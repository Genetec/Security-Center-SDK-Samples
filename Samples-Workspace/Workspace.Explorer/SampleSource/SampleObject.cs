// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Workspace.Explorer.SampleSource
{
    public class SampleObject : DependencyObject
    {

        #region Public Fields

        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(
            "IsEnabled", typeof(bool), typeof(SampleObject), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty IsInstalledProperty = DependencyProperty.Register(
                    "IsInstalled", typeof(bool), typeof(SampleObject), new PropertyMetadata(default(bool)));

        #endregion Public Fields

        #region Public Properties

        public string Arguments { get; set; }

        public string Category { get; set; }

        public bool ConfigTool { get; set; }
        public string Description { get; set; }

        public List<string> Requirements { get; set; }
        public List<Instruction> Instructions { get; set; }
        public string Result { get; set; }

        public string FullInstructions { get; set; }

        public bool IsEnabled
        {
            get => (bool)GetValue(IsEnabledProperty);
            set => SetValue(IsEnabledProperty, value);
        }

        public bool IsInstalled
        {
            get => (bool)GetValue(IsInstalledProperty);
            set => SetValue(IsInstalledProperty, value);
        }

        public string Module { get; set; }
        public string ModulePath { get; set; }
        public string Name { get; set; }
        public bool SecurityCenter { get; set; }
        public string SourceFolder { get; set; }
        public ImageSource Thumbnail { get; set; }
        public string Title { get; set; }

        #endregion Public Properties

        public class Instruction
        {
            public string Num { get; }

            public string Message { get; }

            public Instruction(int num, string message)
            {
                Num = $"{num}.";
                Message = message;
            }
        }
    }
}