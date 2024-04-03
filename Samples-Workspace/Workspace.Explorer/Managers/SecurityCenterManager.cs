// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using Workspace.Explorer.SampleSource;

namespace Workspace.Explorer.Managers
{
    internal interface ISecurityCenterManager
    {

        #region Public Methods

        bool Start(SampleObject selectedSample);

        bool Stop();

        #endregion Public Methods

    }

    public class SecurityCenterManager : ISecurityCenterManager
    {

        #region Private Fields

        private const string InstallationRegistryKey = @"SOFTWARE\Genetec\Security Center\";
        private const string InstallationRegistryKeyX32Bit = @"SOFTWARE\Wow6432Node\Genetec\Security Center\";
        private const string ScInstallPathName = "InstallDir";
        private const string ScPath = @"D:\SecurityCenterRepo\Source\Bin\Debug";
        private const string SdkEnvironmentalVariable = "GSC_SDK";
        private const string SdkInstallPathName = "Installation Path";
        private Process m_process;
        private ArrayList m_processes = new ArrayList();

        #endregion Private Fields

        #region Public Methods

        public bool Start(SampleObject selectedSample)
        {
            var locations = GetScLocation(selectedSample);

            foreach (var location in locations)
            {
                var psi = new ProcessStartInfo(location.ToString()) { WorkingDirectory = ScPath };
                m_process = Process.Start(psi);
                m_processes.Add(m_process?.ProcessName);
            }

            return true;
        }

        public bool Stop()
        {
            var runningProcesses = Process.GetProcesses();

            foreach (var process in runningProcesses)
            {
                if (m_processes.Contains(process.ProcessName))
                {
                    process.Kill();
                }
            }
            return true;
        }

        #endregion Public Methods

        #region Private Methods

        private static ArrayList FormatScLocation(SampleObject selectedSample, string location)
        {
            var secondLocationReplaced = location?.Replace("Debug\\", "Debug");
            Debug.Assert(secondLocationReplaced != null, nameof(secondLocationReplaced) + " != null");

            var locations = new ArrayList();

            var scLocation = Path.Combine(secondLocationReplaced, "SecurityDesk.exe");
            var ctLocation = Path.Combine(secondLocationReplaced, "ConfigTool.exe");

            if (selectedSample.ConfigTool && !selectedSample.SecurityCenter)
            {
                locations.Add(ctLocation);
            }
            else if (selectedSample.SecurityCenter && !selectedSample.ConfigTool)
            {
                locations.Add(scLocation);
            }
            else if (selectedSample.SecurityCenter && selectedSample.ConfigTool)
            {
                locations.Add(scLocation);
                locations.Add(ctLocation);
            }

            return locations;
        }

        private static string GetLatestSdkLocationFromRegistry()
        {
            var sdkLocation = string.Empty;
            var locations = new List<string> { InstallationRegistryKey, InstallationRegistryKeyX32Bit };

            foreach (var registryKeyName in locations)
            {
                using (var registryKey = Registry.LocalMachine.OpenSubKey(registryKeyName))
                {
                    var subKeyName = registryKey?.GetSubKeyNames()
                        .Select(s =>
                        {
                            if (Version.TryParse(s, out var v))
                            {
                                using (var registryKeyValue = registryKey.OpenSubKey(s))
                                {
                                    if (registryKeyValue == null)
                                    {
                                        return null;
                                    }

                                    //try to locate SDK installation path first. If the SDK isn't installed, still try to locate the SC folder
                                    var location = registryKeyValue.GetValue(SdkInstallPathName)?.ToString();
                                    if (string.IsNullOrWhiteSpace(location) || !Directory.Exists(location))
                                    {
                                        location = registryKeyValue.GetValue(ScInstallPathName)?.ToString();
                                        if (string.IsNullOrWhiteSpace(location) || !Directory.Exists(location))
                                        {
                                            return null;
                                        }
                                    }
                                    return new
                                    {
                                        Version = v,
                                        InstallationPath = location
                                    };
                                }
                            }
                            return null;
                        })
                        .Where(v => v != null)
                        .OrderByDescending(v => v?.Version)
                        .FirstOrDefault();

                    if (subKeyName == null)
                    {
                        continue;
                    }

                    sdkLocation = subKeyName.InstallationPath;
                    break;
                }
            }

            return sdkLocation;
        }

        /// <summary>
        /// Gets the Sc location using the following strategies.
        /// 1) Load the Sc folder location using the .exe.config file
        /// 2) Load the Sc folder location using the Environmental Variable GSC_SDK
        /// 3) Load the latest Sc folder location using the Registry.
        /// </summary>
        /// <returns></returns>
        private static ArrayList GetScLocation(SampleObject selectedSample)
        {
            // The Sc location is determined by the .exe.config file first.
            // The location of the SDK installation folder is determined by the environmental variable second; if it is not set, then
            // the location will be obtained from the registry. In the case there is more than one SDK version installed, the location
            // of the latest version will be used to resolve.

            var firstLocation = System.Configuration.ConfigurationManager.AppSettings["SdkLocation"];
            if (!string.IsNullOrEmpty(firstLocation))
            {
                var scFirstLocation = FormatScLocation(selectedSample, firstLocation);
                // The Sc folder location is found using the application's configuration file .exe.config.
                return scFirstLocation;
            }

            var secondLocation = Environment.GetEnvironmentVariable(SdkEnvironmentalVariable);

            if (secondLocation != null)
            {
                var scSecondLocation = FormatScLocation(selectedSample, secondLocation);
                // The Sc folder location was found using the Environmental Variable GSC_SDK.
                return scSecondLocation;
            }

            // The Environmental Variable GSC_SDK could not be found, looking at registry keys to find Security Center installation folder.
            var location = GetLatestSdkLocationFromRegistry();
            if (string.IsNullOrWhiteSpace(location))
            {
                throw new ApplicationException("Could not locate Security Center installation folder.");
            }
            var scThirdLocation = FormatScLocation(selectedSample, location);
            return scThirdLocation;
        }

        #endregion Private Methods

    }
}