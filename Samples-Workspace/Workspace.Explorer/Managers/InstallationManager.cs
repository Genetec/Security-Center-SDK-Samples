// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;
using Workspace.Explorer.SampleSource;

namespace Workspace.Explorer.Managers
{
    public interface IInstallationManager
    {

        #region Public Methods

        bool CheckIsEnabled(SampleObject sampleObject);

        bool CheckIsEnabled(string name);

        bool CheckIsInstalled(SampleObject sampleObject);

        bool CheckIsInstalled(string name);

        bool DisableSample(SampleObject sampleObject);

        bool EnableSample(SampleObject sampleObject);

        bool InstallSample(SampleObject sampleObject);

        bool UnInstallSample(SampleObject sampleObject);

        #endregion Public Methods

    }

    public class InstallationManager : IInstallationManager
    {

        #region Private Fields

        private const string ClientModuleKeyName = @"ClientModule";

        private const string EnabledKeyName = @"Enabled";

        private const string PluginsKey = @"SOFTWARE\Genetec\Security Center\Plugins";

        #endregion Private Fields

        #region Public Methods

        public bool CheckIsEnabled(string sampleName)
                                    => CheckIsEnabledPrivate(GetKeyName(sampleName));

        public bool CheckIsEnabled(SampleObject sampleObject)
            => CheckIsEnabled(sampleObject.Name);

        public bool CheckIsInstalled(string sampleName)
            => CheckIsInstalledPrivate(GetKeyName(sampleName));

        public bool CheckIsInstalled(SampleObject sampleObject)
            => CheckIsInstalled(sampleObject.Name);

        public bool DisableSample(SampleObject sampleObject)
        {
            var registryKeys = GetRegistry();
            try
            {
                var keyName = GetKeyName(sampleObject.Name);

                foreach (var key in registryKeys)
                {
                    RegistryKey pluginKey = null;
                    try
                    {
                        pluginKey = key.OpenSubKey(keyName, RegistryKeyPermissionCheck.ReadWriteSubTree);
                        pluginKey?.SetValue(EnabledKeyName, false);
                    }
                    finally
                    {
                        pluginKey?.Dispose();
                    }
                }
            }
            catch (Exception exception)
            {
                var ex = exception.Message;
                return false;
            }
            finally
            {
                registryKeys.ForEach(x => x?.Dispose());
            }
            return true;
        }

        public bool EnableSample(SampleObject sampleObject)
        {
            var registryKeys = GetRegistry();
            try
            {
                var keyName = GetKeyName(sampleObject.Name);

                foreach (var key in registryKeys)
                {
                    RegistryKey pluginKey = null;
                    try
                    {
                        pluginKey = key.OpenSubKey(keyName, RegistryKeyPermissionCheck.ReadWriteSubTree);
                        pluginKey?.SetValue(EnabledKeyName, true);
                    }
                    finally
                    {
                        pluginKey?.Dispose();
                    }
                }
            }
            catch (Exception exception)
            {
                var ex = exception.Message;
                return false;
            }
            finally
            {
                registryKeys.ForEach(x => x?.Dispose());
            }
            return true;
        }

        public bool InstallSample(SampleObject sampleObject)
        {
            if (CheckIsEnabled(sampleObject))
                EnableSample(sampleObject);
            if (CheckIsInstalled(sampleObject))
                return true;

            var registryKeys = GetRegistry();
            try
            {
                var keyName = GetKeyName(sampleObject.Name);

                foreach (var key in registryKeys)
                {
                    RegistryKey pluginKey = null;
                    try
                    {
                        var filePath = Path.Combine(sampleObject.ModulePath, sampleObject.Module);
                        pluginKey = key.CreateSubKey(keyName, RegistryKeyPermissionCheck.ReadWriteSubTree);
                        pluginKey?.SetValue(ClientModuleKeyName, filePath);
                        pluginKey?.SetValue(EnabledKeyName, true);
                    }
                    finally
                    {
                        pluginKey?.Dispose();
                    }
                }
            }
            catch (Exception exception)
            {
                var ex = exception.Message;
                return false;
            }
            finally
            {
                registryKeys.ForEach(x => x?.Dispose());
            }
            return true;
        }

        public bool UnInstallSample(SampleObject sampleObject)
        {
            if (!CheckIsInstalled(sampleObject))
                return true;

            var registryKeys = GetRegistry();
            try
            {
                var keyName = GetKeyName(sampleObject.Name);

                foreach (var key in registryKeys)
                    key.DeleteSubKey(keyName);
            }
            catch (Exception e)
            {
                Console.Write(e);
                return false;
            }
            finally
            {
                foreach (var key in registryKeys)
                    key.Dispose();
            }
            return true;
        }

        #endregion Public Methods

        #region Private Methods

        private static bool CheckIsEnabledPrivate(string keyName)
        {
            var registryKeys = GetRegistry();

            try
            {
                foreach (var key in registryKeys)
                {
                    RegistryKey pluginKey = null;
                    try
                    {
                        pluginKey = key.OpenSubKey(keyName, RegistryKeyPermissionCheck.ReadWriteSubTree);

                        if (pluginKey?.GetValue(EnabledKeyName).ToString() == "True")
                            return true;
                    }
                    finally
                    {
                        pluginKey?.Dispose();
                    }
                }
            }
            catch (Exception exception)
            {
                var ex = exception.Message;
                return false;
            }
            finally
            {
                registryKeys.ForEach(x => x?.Dispose());
            }
            return false;
        }

        private static bool CheckIsInstalledPrivate(string keyName)
        {
            var registryKeys = GetRegistry();

            try
            {
                foreach (var key in registryKeys)
                {
                    RegistryKey pluginKey = null;
                    try
                    {
                        pluginKey = key.OpenSubKey(keyName, RegistryKeyPermissionCheck.ReadWriteSubTree);
                        if (pluginKey != null)
                            return true;
                    }
                    finally
                    {
                        pluginKey?.Dispose();
                    }
                }
            }
            catch (Exception exception)
            {
                var ex = exception.Message;
                return false;
            }
            finally
            {
                registryKeys.ForEach(x => x?.Dispose());
            }
            return false;
        }

        private static string GetKeyName(string name) => PluginsKey + "\\" + name;

        private static List<RegistryKey> GetRegistry()
            => new List<RegistryKey>
            {
                RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32),
                RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
            };

        #endregion Private Methods
    }
}