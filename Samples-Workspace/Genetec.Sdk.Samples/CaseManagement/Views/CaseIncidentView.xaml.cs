// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using CaseManagement.Builders;
using Genetec.Sdk.Incidents;
using static System.String;

namespace CaseManagement.Views
{
    /// <summary>
    /// Interaction logic for CaseIncidentView.xaml
    /// </summary>
    public partial class CaseIncidentView
    {

        #region Public Properties

        /// <summary>
        /// Gets if the data can be exported
        /// </summary>
        public override bool CanExport => !IsDataEmpty;

        /// <summary>
        /// Gets or Sets the current incident data
        /// </summary>
        public override List<IncidentDataEntry> IncidentData
        {
            get
            {
                var entries = new List<IncidentDataEntry>();

                try
                {
                    using (var stream = new MemoryStream())
                    {
                        m_inkCanvas.Strokes.Save(stream);
                        entries.Add(new IncidentDataEntry("Drawing", Convert.ToBase64String(stream.ToArray())));
                    }

                    entries.Add(new IncidentDataEntry("Comment1", m_tbComment1.Text));
                    entries.Add(new IncidentDataEntry("Comment2", m_tbComment2.Text));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

                return entries;
            }
            set
            {
                if (value != null)
                {
                    try
                    {
                        foreach (var entry in value)
                        {
                            switch (entry.Identifier)
                            {
                                case "Drawing":
                                    {
                                        using (var stream = new MemoryStream(Convert.FromBase64String(entry.Data)))
                                        {
                                            m_inkCanvas.Strokes = new StrokeCollection(stream);
                                        }
                                        break;
                                    }
                                case "Comment1":
                                    m_tbComment1.Text = entry.Data;
                                    break;

                                case "Comment2":
                                    m_tbComment2.Text = entry.Data;
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or Sets the incident data type supported by this incident plugin
        /// </summary>
        public override string IncidentDataType => CaseIncidentBuilder.CaseIncidentDataType;

        /// <summary>
        /// Gets if the data is empty or not
        /// </summary>
        public override bool IsDataEmpty =>
            !m_inkCanvas.Strokes.Any() &&
            IsNullOrEmpty(m_tbComment1.Text) &&
            IsNullOrEmpty(m_tbComment2.Text);

        #endregion Public Properties

        #region Public Constructors

        public CaseIncidentView() => InitializeComponent();

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Export incident data
        /// </summary>
        /// <returns>False if operation is cancelled</returns>
        public override bool Export()
        {
            var success = false;
            if (CanExport)
            {
                var filePath = Path.Combine("C:\\", "DrawingIncident.txt");

                try
                {
                    using (var writer = new StreamWriter(new FileStream(filePath, FileMode.Create, FileAccess.Write), Encoding.UTF8))
                    {
                        writer.Write(IncidentData);
                    }

                    MessageBox.Show("Incident data saved to " + filePath);

                    success = true;
                }
                catch
                {
                    MessageBox.Show("Error while writing to file " + filePath);
                }
            }

            return success;
        }

        #endregion Public Methods

        #region Private Methods

        private void OnInkCanvasStrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e) => NotifyIncidentDataChanged();

        #endregion Private Methods

    }
}