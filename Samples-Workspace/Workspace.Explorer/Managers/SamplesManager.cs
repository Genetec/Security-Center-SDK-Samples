// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using Workspace.Explorer.SampleSource;

namespace Workspace.Explorer.Managers
{
    internal interface ISamplesManager
    {
        #region Public Methods

        ObservableCollection<SampleObject> GetSampleObjects();

        #endregion Public Methods
    }

    internal class SamplesManager : ISamplesManager
    {
        #region Private Fields

        private static readonly Regex DebugReleaseRegex = new Regex(@"(?i)\\bin\\(debug|release)");

        private readonly string m_filePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"Genetec.Sdk.Samples"));

        private readonly IInstallationManager m_installationManager = new InstallationManager();

        private readonly ObservableCollection<SampleObject> m_sampleObjects = new ObservableCollection<SampleObject>();

        #endregion Private Fields

        #region Public Constructors

        public SamplesManager() => CreateSampleObject();

        #endregion Public Constructors

        #region Public Methods

        public ObservableCollection<SampleObject> GetSampleObjects() => m_sampleObjects;

        #endregion Public Methods

        #region Private Methods

        private static SampleInfo LoadSampleInfos(string directory)
        {
            var filePath = Path.Combine(directory, @"Resources\Info.json");
            try
            {
                return JsonConvert.DeserializeObject<SampleInfo>(File.ReadAllText(filePath));
            }
            catch (Exception e)
            {
                Console.Write(e);
                return null;
            }
        }

        private static SampleResources LoadSampleResources(string directory)
        {
            SampleResources resources = null;
            var resourcesPath = Path.Combine(directory, @"Resources");

            var displayFolder = Regex.Replace(directory, DebugReleaseRegex.ToString(), string.Empty);
            if (!Directory.Exists(displayFolder))
                displayFolder = directory;

            try
            {
                resources = new SampleResources
                {
                    ModulePath = Path.GetDirectoryName(resourcesPath),
                    SourceFolder = displayFolder,
                    Thumbnail = new BitmapImage(new Uri(Path.Combine(resourcesPath, "Thumbnail.png")))
                };
            }
            catch(Exception e)
            {
                Trace.WriteLine($"Unable to load a sample: Exception: {e}");
            }

            return resources;
        }

        private void CreateSampleObject()
        {
            var directories = Directory.GetDirectories(m_filePath);
            foreach (var dir in directories.Select((value, i) => new { i, value }))
            {
                var resource = LoadSampleResources(dir.value);
                var info = LoadSampleInfos(dir.value);

                var list = new List<SampleObject.Instruction> { new SampleObject.Instruction(5, " ") };

                if (resource != null && info != null)
                {
                    m_sampleObjects.Add(new SampleObject
                    {
                        Description = info.Description,
                        Requirements = info.Requirements?.ToList() ?? new List<string> { "No requirements are needed for this sample." },
                        Instructions = info.Instructions?.Select(x => new SampleObject.Instruction(info.Instructions.IndexOf(x) + 1, x)).ToList()
                                       ?? new List<SampleObject.Instruction>
                                       {
                                           new SampleObject.Instruction(0, "No instructions are available for this sample.")
                                       },
                        Result = info.Result ?? "No result have been entered for this sample.",
                        Module = info.Module,
                        Name = info.Name,
                        Title = info.Title,
                        Category = info.Category ?? "Miscellaneous",
                        Arguments = info.Arguments,
                        SourceFolder = resource.SourceFolder,
                        ModulePath = resource.ModulePath,
                        Thumbnail = resource.Thumbnail,
                        IsInstalled = m_installationManager.CheckIsInstalled(info.Name),
                        IsEnabled = m_installationManager.CheckIsEnabled(info.Name),
                        SecurityCenter = info.SecurityCenter,
                        ConfigTool = info.ConfigTool
                    });
                }
            }
        }

        #endregion Private Methods
    }
}