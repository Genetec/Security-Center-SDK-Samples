using Genetec.Sdk;
using RaiseFaceDetectedEvent.ViewModels;
using System;
using System.Windows.Input;

// ==========================================================================
// Copyright (C) 2017 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
//
// Ephemerides for March 31:
//  1889 – The Eiffel Tower is officially opened.
//  1921 – The Royal Australian Air Force is formed.
//  1931 – An earthquake destroys Managua, Nicaragua, killing 2,000.
// ==========================================================================
namespace RaiseFaceDetectedEvent.Commands
{
    #region Classes

    class LogonCommand : ICommand
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

        public LogonCommand(CustomEventMessageViewModel viewModel, Engine engine)
        {
            m_viewModel = viewModel;
            m_engine = engine;
            engine.LoginManager.LoggedOn += OnLoggedOn;
            engine.LoginManager.LogonFailed += OnLoggonFailed;
        }

        #endregion

        #region Event Handlers

        private void OnLoggedOn(object sender, LoggedOnEventArgs e)
        {
            Console.WriteLine(e.ServerName);

            m_viewModel.IsLoggedOn = true;
            m_viewModel.RefreshCameraList();
            m_viewModel.FaceId = Guid.NewGuid().ToString();
        }

        private void OnLoggonFailed(object sender, LogonFailedEventArgs e)
        {
            Console.WriteLine(e.FailureCode);
        }

        #endregion

        #region Public Methods

        public bool CanExecute(object parameter)
        {
            return !m_engine.LoginManager.IsConnected;
        }

        public void Execute(object parameter)
        {
            m_engine.LoginManager.LogOn("", "admin", "");
        }

        #endregion
    }

    #endregion
}

