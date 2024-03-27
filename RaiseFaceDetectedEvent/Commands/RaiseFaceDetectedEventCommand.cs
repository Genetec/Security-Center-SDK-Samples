using Genetec.Sdk;
using Genetec.Sdk.Events.VideoAnalytics;
using RaiseFaceDetectedEvent.ViewModels;
using SdkHelpers.Common;
using System;
using System.Windows.Input;
using System.Windows.Media.Imaging;

// ==========================================================================
// Copyright (C) 2017 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
//
// Ephemerides for March 31:
//  1561 – The city of San Cristóbal, Táchira is founded.
//  1909 – Serbia accepts Austrian control over Bosnia and Herzegovina.
//  1931 – An earthquake destroys Managua, Nicaragua, killing 2,000.
// ==========================================================================
namespace RaiseFaceDetectedEvent.Commands
{
    #region Classes

    class RaiseFaceDetectedEventCommand : ICommand
    {
        #region Constants

        private readonly Engine m_engine;

        private readonly CustomEventMessageViewModel m_viewModel;

        #endregion

        #region Events and Delegates

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion

        #region Constructors

        public RaiseFaceDetectedEventCommand(CustomEventMessageViewModel viewModel, Engine engine)
        {
            m_viewModel = viewModel;
            m_engine = engine;
        }

        #endregion

        #region Public Methods

        public bool CanExecute(object parameter)
        {
            return m_engine.LoginManager.IsConnected;
        }

        public void Execute(object parameter)
        {
            if (m_viewModel.SelectedCamera != null)
            {
                VideoAnalyticsFaceDetectedEvent faceDetectedEvent =
                    (VideoAnalyticsFaceDetectedEvent)
                        m_engine.ActionManager.BuildEvent(EventType.VideoAnalyticsFaceDetected,
                            m_viewModel.SelectedCamera.Guid);

                faceDetectedEvent.Age = m_viewModel.FaceDetectedEvent.Age;
                faceDetectedEvent.Confidence = new Ratio(m_viewModel.FaceDetectedEvent.ConfidenceRatio);
                faceDetectedEvent.Metadata = m_viewModel.FaceDetectedEvent.Metadata;
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                faceDetectedEvent.Image = ImageExtensions.BitmapSourceToBitmap(m_viewModel.FaceDetectedEvent.Image, encoder);

                m_engine.ActionManager.RaiseEvent(faceDetectedEvent);
                m_viewModel.FaceId = Guid.NewGuid().ToString();
            }
        }

        #endregion
    }

    #endregion
}

