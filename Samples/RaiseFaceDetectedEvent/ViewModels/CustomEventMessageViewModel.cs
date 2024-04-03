using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Entities.CustomEvents;
using Genetec.Sdk.Queries;
using RaiseFaceDetectedEvent.Annotations;
using RaiseFaceDetectedEvent.Commands;
using RaiseFaceDetectedEvent.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media.Imaging;

// ==========================================================================
// Copyright (C) 2017 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
//
// Ephemerides for April 11:
//  1241 – Batu Khan defeats Béla IV of Hungary at the Battle of Mohi.
//  1961 – The trial of Adolf Eichmann begins in Jerusalem.
//  2002 – The Ghriba synagogue bombing by al-Qaeda kills 21 in Tunisia.
// ==========================================================================
namespace RaiseFaceDetectedEvent.ViewModels
{
    #region Classes

    public class CustomEventMessageViewModel : INotifyPropertyChanged
    {
        #region Constants

        private readonly List<Camera> m_cameraList;

        #endregion

        #region Fields

        private bool m_engineIsConnected = false;

        private string m_picturePath;

        #endregion

        #region Properties

        public string Age
        {
            get { return FaceDetectedEvent.Age.ToString(); }
            set
            {
                FaceDetectedEvent.Age = int.Parse(value);
                OnPropertyChanged();
            }
        }

        public string ConfidenceRatio
        {
            get { return FaceDetectedEvent.ConfidenceRatio.ToString(CultureInfo.InvariantCulture); }
            set
            {
                FaceDetectedEvent.ConfidenceRatio = Double.Parse(value);
                OnPropertyChanged();
            }
        }

        public Engine Engine { get; } = new Engine();

        public FaceDetectedEvent FaceDetectedEvent { get; set; } = new FaceDetectedEvent();

        public string FaceId
        {
            get { return FaceDetectedEvent.Metadata; }
            set
            {
                FaceDetectedEvent.Metadata = value;
                OnPropertyChanged();
            }
        }

        public BitmapImage Image
        {
            get
            {
                return FaceDetectedEvent.Image;
            }
            set
            {
                FaceDetectedEvent.Image = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoggedOn
        {
            get { return m_engineIsConnected; }
            set
            {
                m_engineIsConnected = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsLoggedOff));
            }
        }

        public string PicturePath
        {
            set
            {
                m_picturePath = value;
                SetPicture(value);
            }
            get { return m_picturePath; }
        }

        public Camera SelectedCamera { get; set; }

        public CustomEvent SelectedCustomEventId { get; set; }

        public ReadOnlyCollection<Camera> Cameras => m_cameraList.AsReadOnly();

        public bool IsLoggedOff => !m_engineIsConnected;

        public ICommand LogoffCommand => new LogoffCommand(this, Engine);

        public ICommand LogonCommand => new LogonCommand(this, Engine);

        public ICommand RaiseFaceDetectedEventCommand => new RaiseFaceDetectedEventCommand(this, Engine);

        public ICommand RaiseObjectDetectedEventCommand => new RaiseObjectDetectedEventCommand(this, Engine);

        #endregion

        #region Events and Delegates

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructors

        public CustomEventMessageViewModel()
        {
            m_cameraList = new List<Camera>();
        }

        #endregion

        #region Event Handlers

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Public Methods

        public void RefreshCameraList()
        {
            m_cameraList.Clear();
            var query =
                (EntityConfigurationQuery)Engine.ReportManager.CreateReportQuery(ReportType.EntityConfiguration);
            query.EntityTypeFilter.Add(EntityType.Camera);

            QueryCompletedEventArgs result = query.Query();
            if (result.Success)
            {
                foreach (DataRow dr in result.Data.Rows)
                {
                    Camera camera = Engine.GetEntity((Guid)dr[0]) as Camera;
                    if ((camera != null) && (camera.IsOnline) && (!camera.IsGhostCamera) && (!camera.IsSequence))
                    {
                        m_cameraList.Add(camera);
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private void SetPicture(string path)
        {
            try
            {
                Image = new BitmapImage(new Uri(path));
            }
            catch (FileNotFoundException ex)
            {
                //Set a default picture in case of an error
                Image = new BitmapImage(new Uri(@"pack://application:,,,/Resources/Face.png"));

                //Properties.Resources.Genetec.ToBitmapImage(ImageFormat.Jpeg);
                Console.WriteLine(@"There was an error setting the picture." + ex.Message);
            }

        }

        #endregion
    }

    #endregion
}

