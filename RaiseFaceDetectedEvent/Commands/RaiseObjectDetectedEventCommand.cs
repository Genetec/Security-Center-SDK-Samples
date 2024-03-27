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
//  1885 – The United Kingdom establishes the Bechuanaland Protectorate.
//  1909 – Serbia accepts Austrian control over Bosnia and Herzegovina.
//  1921 – The Royal Australian Air Force is formed.
// ==========================================================================
namespace RaiseFaceDetectedEvent.Commands
{
    #region Classes

    class RaiseObjectDetectedEventCommand : ICommand
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

        public RaiseObjectDetectedEventCommand(CustomEventMessageViewModel viewModel, Engine engine)
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
                VideoAnalyticsObjectInFieldEvent objectInFieldEvent =
                    (VideoAnalyticsObjectInFieldEvent)
                        m_engine.ActionManager.BuildEvent(EventType.VideoAnalyticsObjectInField,
                            m_viewModel.SelectedCamera.Guid);

                objectInFieldEvent.Metadata = m_viewModel.FaceDetectedEvent.Metadata;
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                objectInFieldEvent.Image = ImageExtensions.BitmapSourceToBitmap(m_viewModel.FaceDetectedEvent.Image, encoder);

                m_engine.ActionManager.RaiseEvent(objectInFieldEvent);
                m_viewModel.FaceId = Guid.NewGuid().ToString();
            }
        }

        #endregion
    }

    #endregion
}

