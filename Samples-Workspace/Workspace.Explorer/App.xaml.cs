// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Windows;

namespace Workspace.Explorer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            if (LoadComponent(new Uri("Resources/FontAwesome.xaml", UriKind.Relative)) is ResourceDictionary rd)
                Current.Resources.MergedDictionaries.Add(rd);
        }
    }
}