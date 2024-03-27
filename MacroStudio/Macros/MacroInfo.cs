// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk;
using Genetec.Sdk.Scripting;
using System;
using System.Threading.Tasks;

namespace MacroStudio.Macros
{
    /// <summary>
    /// This is a wrapper for something Security Center does by itself. Do not do something similar in production.
    /// </summary>
    public class MacroInfo
    {

        #region Private Fields

        private readonly Engine m_sdkEngine;

        #endregion Private Fields

        #region Public Properties

        public UserMacro ActivatedMacro { get; set; }

        public string Name { get; set; }

        public Type Type { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public MacroInfo(Engine sdkEngine, Type type)
        {
            m_sdkEngine = sdkEngine;
            Name = type.Name;
            Type = type;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Do not ever call Abort like this on production.
        /// </summary>
        public void Abort()
            => Task.Run(() =>
            {
                try { ActivatedMacro?.Abort(); }
                catch { /* Ignored */ }
            });

        /// <summary>
        /// Do not ever call Run like this on production.
        /// </summary>
        public void Run()
        {
            if (ActivatedMacro == null)
                ActivateMacro();
            Task.Run(() => ActivatedMacro.Run());
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Do not ever do something like this is production for macros.
        /// </summary>
        private void ActivateMacro()
        {
            ActivatedMacro = (UserMacro)Activator.CreateInstance(Type);
            ActivatedMacro.SetSynergisSdk(m_sdkEngine);
        }

        #endregion Private Methods

    }
}