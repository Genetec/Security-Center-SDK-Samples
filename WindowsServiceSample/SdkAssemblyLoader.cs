using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
//
// Ephemerides for November 23:
//  1174 – Saladin enters Damascus, and adds it to his domain.
//  1943 – World War II: Tarawa and Makin atolls fall to American forces.
//  2001 – The Convention on Cybercrime is signed in Budapest, Hungary.
// ==========================================================================
namespace SdkHelper
{
    #region Classes

    static class SdkAssemblyLoader
    {
        #region Constants

        private const string InstallationRegistryKey = @"SOFTWARE\Genetec\Security Center\";

        private const string InstallationRegistryKeyX32Bit = @"SOFTWARE\Wow6432Node\Genetec\Security Center\";

        private const string SdkEnvironmentalVariable = @"GSC_SDK";

        private const string SdkInstallPathName = @"Installation Path";

        #endregion

        #region Fields

        private static string SdkLocation = string.Empty;

        #endregion

        #region Nested Classes and Structures

        private static class AssemblyLogger
        {
            #region Constants

            private static readonly string ConfigKey = "EnableAssemblyLogger";

            private static readonly string FileName = "SdkAssemblyLoader.log";

            private static readonly object WriteLock = new object();

            #endregion

            #region Fields

            private static readonly bool IsLoggerEnabled;

            #endregion

            #region Constructors

            static AssemblyLogger()
            {
                IsLoggerEnabled = false;
                if (System.Configuration.ConfigurationManager.AppSettings[ConfigKey] != null)
                {
                    IsLoggerEnabled = System.Configuration.ConfigurationManager.AppSettings[ConfigKey].Equals("True", StringComparison.InvariantCultureIgnoreCase);
                }
            }

            #endregion

            #region Public Methods

            public static void Trace(string content)
            {
                if (!IsLoggerEnabled)
                    return;


                lock (WriteLock)
                {
                    using (var fs = File.AppendText(FileName))
                    {
                        fs.WriteLine(string.Format("[{0}] : {1}", DateTime.Now.ToString("O"), content));
                    }
                }
            }

            #endregion
        }

        #endregion

        #region Public Methods

        public static void Start()
        {
            AssemblyLogger.Trace(Environment.NewLine);
            AssemblyLogger.Trace("SdkAssemblyLoader Started");
            SdkLocation = GetSdkLocation();
            ValidateAndTraceSdkLocation(SdkLocation);
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
        }

        public static void Stop()
        {
            AssemblyLogger.Trace("SdkAssemblyLoader Stopped");
            AssemblyLogger.Trace(Environment.NewLine);
            AppDomain.CurrentDomain.AssemblyResolve -= OnAssemblyResolve;
        }

        #endregion

        #region Private Methods

        private static string GetLatestSdkLocationFromRegistery(string registryKeyName)
        {
            string sdklocation = String.Empty;

            using (var registryKey = Registry.LocalMachine.OpenSubKey(registryKeyName))
            {
                if (registryKey == null)
                {
                    return null;
                }
                AssemblyLogger.Trace("Information: Looking inside the registry: " + registryKey.Name);
                var subKeyNames = registryKey.GetSubKeyNames()
                    .Select(s =>
                    {
                        Version v = null;
                        if (Version.TryParse(s, out v))
                        {
                            using (var registryKeyValue = registryKey.OpenSubKey(s))
                            {
                                if (registryKeyValue == null)
                                {
                                    throw new ArgumentNullException("registryKeyValue");
                                }
                                if (registryKeyValue.GetValue(SdkInstallPathName) == null)
                                {
                                    throw new ArgumentNullException("SdkIntallPathName");
                                }
                                var installationPath = registryKeyValue.GetValue(SdkInstallPathName).ToString();
                                if (!string.IsNullOrEmpty(installationPath) && Directory.Exists(installationPath))
                                {
                                    AssemblyLogger.Trace("Information: The InstallationPath is found and the directory exists at: " + installationPath + " version: " + s);
                                    return new
                                    {
                                        Version = v,
                                        InstallationPath = installationPath
                                    };
                                }
                                AssemblyLogger.Trace("Information: The Installation path is invalid at: " + installationPath);
                            }
                        }
                        return null;
                    })
                    .Where(v => v != null)
                    .OrderByDescending(v => v.Version)
                    .FirstOrDefault();

                if (subKeyNames == null)
                {
                    throw new ArgumentNullException("subKeyNames");
                }

                sdklocation = subKeyNames.InstallationPath;
            }

            return sdklocation;
        }

        /// <summary>
        /// Gets the SDK location using the following strategies.
        /// 1) Load the SDK folder location using the .exe.config file
        /// 2) Load the Sdk folder location using the Environmental Variable GSC_SDK
        /// 3) Load the latest Sdk folder location using the Registry.
        /// </summary>
        /// <returns></returns>
        private static string GetSdkLocation()
        {
            // The Sdk location is determined by the .exe.config file first.
            // The location of the SDK installation folder is determined by the environmental variable second; if it is not set, then
            // the location will be obtained from the registry. In the case there is more than one SDK version installed, the location
            // of the latest version will be used to resolve.
            var firstLocation = System.Configuration.ConfigurationManager.AppSettings["SdkLocation"];
            if (!string.IsNullOrEmpty(firstLocation))
            {
                AssemblyLogger.Trace("The Sdk folder location is found using the application's configuration file .exe.config");
                return firstLocation;
            }

            var secondLocation = Environment.GetEnvironmentVariable(SdkEnvironmentalVariable);
            if (secondLocation != null)
            {
                AssemblyLogger.Trace("The Sdk folder location was found using the Environmental Variable GSC_SDK");
                return secondLocation;
            }

            string installationBaseKey = Environment.Is64BitProcess ? InstallationRegistryKey : InstallationRegistryKeyX32Bit;
            AssemblyLogger.Trace("The Sdk folder location was found using the registry");
            AssemblyLogger.Trace("The registry location: " + installationBaseKey);

            return GetLatestSdkLocationFromRegistery(installationBaseKey);
        }

        private static Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly assembly = null;
            AssemblyName assemblyName = new AssemblyName(args.Name);

            if (string.IsNullOrEmpty(SdkLocation) ||
                assemblyName.Name.ToLower().EndsWith(".resources") ||
                assemblyName.Name.ToLower().EndsWith(".xmlserializers"))
            {
                return null;
            }

            string assemblyPath = Path.Combine(SdkLocation, assemblyName.Name + ".dll");
            AssemblyLogger.Trace("Information: The Assembly name: " + assemblyName + " was not found in the folder: " + SdkLocation);
            try
            {
                if (!string.IsNullOrEmpty(assemblyPath))
                {
                    assembly = File.Exists(assemblyPath) ? Assembly.LoadFrom(assemblyPath) : null;
                    if (assembly == null)
                    {
                        string message =
                            string.Format("The assembly resolver was unable to locate the specified assembly: {0}" +
                            " in the location: {1}", assemblyPath, SdkLocation);
                        AssemblyLogger.Trace(message);
                        // Different logic can be used to handle this case, this is just an example.
                        Console.WriteLine(message);
                    }
                }
            }
            catch (Exception e)
            {
                AssemblyLogger.Trace("An error occurred. Detailed Message: " + e.Message);
                // Different logic can be used to handle this case, this is just an example.
                Console.WriteLine("An exception was caught trying to locate a specified assembly: {0}. Description {1}", args.Name, e.Message);
                assembly = null;
            }

            return assembly;
        }

        private static void ValidateAndTraceSdkLocation(string sdkLocation)
        {
            AssemblyLogger.Trace("The SdkLocation is: " + sdkLocation);
            if (string.IsNullOrEmpty(sdkLocation))
            {
                string message =
                    "An Error Occured. The Sdk location found is empty. Ensure the Genetec Security Center SDK is installed.";
                AssemblyLogger.Trace(message);
                throw new DirectoryNotFoundException(message);
            }
            if (!Directory.Exists(sdkLocation))
            {
                string message =
                    "An Error Occured. The Sdk folder on the computer does not exist.  Ensure the Genetec Security Center SDK is installed.";
                AssemblyLogger.Trace(message);
                throw new DirectoryNotFoundException(message);
            }
        }

        #endregion
    }

    #endregion
}

