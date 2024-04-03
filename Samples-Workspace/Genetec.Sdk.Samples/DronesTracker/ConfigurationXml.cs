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
using System.Reflection;
using System.Resources;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace DronesTracker
{
    [XmlRoot("Configuration")]
    public sealed class ConfigurationXml
    {

        #region Public Properties

        [XmlElement("ShowHeatMap")]
        public bool ShowHeatMap { get; set; }

        [XmlArray("Simulators")]
        public List<Simulation> Simulators { get; set; }

        [XmlElement("WaitTime")]
        public int WaitTime { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public ConfigurationXml()
        {
            WaitTime = 1500;
            Simulators = new List<Simulation>();
        }

        #endregion Public Constructors

        #region Public Methods

        public static ConfigurationXml CreateDefault()
        {
            var config = new ConfigurationXml();

            var sim1 = new Simulation
            {
                Name = "Car #1",
                Color = "Blue",
                ImageFile = "car.png",
                IsClusterable = false,
                RouteFile = "route1.txt"
            };

            var sim2 = new Simulation
            {
                Name = "Car #2",
                Color = "#80FF0000",
                ImageFile = "car.png",
                IsClusterable = false,
                RouteFile = "route2.txt"
            };

            config.Simulators = new List<Simulation> { sim1, sim2 };
            config.ShowHeatMap = true;
            config.WaitTime = 1500;

            return config;
        }

        public static ConfigurationXml Deserialize(string xml)
        {
            if (!string.IsNullOrEmpty(xml))
            {
                try
                {
                    var xmlSerializer = new XmlSerializer(typeof(ConfigurationXml));
                    using (var sr = new StringReader(xml))
                    {
                        return xmlSerializer.Deserialize(sr) as ConfigurationXml;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Fail($"An error has occurred while deserializing ConfigurationXml ({ex.Message})");
                }
            }

            // Never return null!
            return CreateDefault();
        }

        public static ConfigurationXml FromFile(string filePath)
        {
            ConfigurationXml result = null;

            if (File.Exists(filePath))
            {
                var xml = File.ReadAllText(filePath);
                result = Deserialize(xml);
            }

            return result;
        }
        public static string SerializeDefault()
        {
            string xml = null;
            var value = CreateDefault();

            try
            {
                var xmlSerializer = new XmlSerializer(typeof(ConfigurationXml));
                using (var sw = new StringWriter())
                {
                    xmlSerializer.Serialize(sw, value);
                    xml = sw.ToString();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail($"An error has occurred while serializing ConfigurationXml ({ex.Message})");
            }

            return xml;
        }

        #endregion Public Methods

    }

    [XmlRoot("Simulation")]
    public sealed class Simulation
    {

        #region Private Fields

        private static readonly Dictionary<string, ImageSource> DefaultImages = new Dictionary<string, ImageSource>();

        #endregion Private Fields

        #region Public Properties

        [XmlElement("Color")]
        public string Color { get; set; }

        [XmlElement("ImageFile")]
        public string ImageFile { get; set; }

        /// <summary>
        /// Gets a flag indicating if the object can be clustered
        /// </summary>
        [XmlElement("IsClusterable")]
        public bool IsClusterable { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("RouteFile")]
        public string RouteFile { get; set; }

        #endregion Public Properties

        #region Public Constructors

        static Simulation()
        {
            try
            {
                // Get default images
                var asm = Assembly.GetExecutingAssembly();
                var asmName = asm.GetName().Name;
                var resourceStream = asm.GetManifestResourceStream(asmName + ".g.resources");
                if (resourceStream != null)
                {
                    var resourceReader = new ResourceReader(resourceStream);
                    const string prefix = "resources/";

                    foreach (DictionaryEntry resource in resourceReader)
                    {
                        var key = resource.Key.ToString().ToLowerInvariant();
                        if (key.StartsWith(prefix))
                        {
                            var imageKey = key.Remove(0, prefix.Length);
                            imageKey = Path.GetFileNameWithoutExtension(imageKey);

                            var stream = resource.Value as Stream;
                            if (stream != null)
                            {
                                var bitmap = new BitmapImage();
                                bitmap.BeginInit();
                                bitmap.StreamSource = stream;
                                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                bitmap.EndInit();
                                bitmap.Freeze();

                                DefaultImages.Add(imageKey, bitmap);
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        #endregion Public Constructors

        #region Public Methods

        public Color GetColor()
        {
            var color = Colors.DarkBlue;

            try
            {
                color = (Color)ColorConverter.ConvertFromString(Color);
            }
            catch (Exception ex)
            {
                Debug.Fail($"Unable to parse color ({ex.Message})");
            }

            return color;
        }

        public ImageSource GetImageSource()
        {
            ImageSource result = null;

            try
            {
                var key = ImageFile;
                if (!string.IsNullOrEmpty(key))
                {
                    if (!DefaultImages.TryGetValue(key.ToLowerInvariant(), out result))
                    {
                        Uri uri;
                        if (Uri.TryCreate(key, UriKind.RelativeOrAbsolute, out uri))
                        {
                            try
                            {
                                result = new BitmapImage(new Uri(key));
                            }
                            catch
                            {
                                // If we're not able to build it, try with a filename
                                var dllPath = Assembly.GetExecutingAssembly().Location;
                                if (!string.IsNullOrEmpty(dllPath))
                                {
                                    var directory = Path.GetDirectoryName(dllPath);
                                    if (!string.IsNullOrEmpty(directory))
                                    {
                                        var filePath = Path.Combine(directory, key);
                                        result = new BitmapImage(new Uri(filePath));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Fail($"Unable to load resources ({ex.Message})");
            }

            return result;
        }

        #endregion Public Methods

    }
}