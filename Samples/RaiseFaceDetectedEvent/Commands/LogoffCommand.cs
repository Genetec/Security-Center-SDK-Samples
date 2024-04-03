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
//  1885 – The United Kingdom establishes the Bechuanaland Protectorate.
//  1889 – The Eiffel Tower is officially opened.
//  1931 – An earthquake destroys Managua, Nicaragua, killing 2,000.
// ==========================================================================
namespace RaiseFaceDetectedEvent.Commands
{
    #region Classes

    class LogoffCommand : ICommand
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

        public LogoffCommand(CustomEventMessageViewModel viewModel, Engine engine)
        {
            m_viewModel = viewModel;
            m_engine = engine;
            engine.LoginManager.LoggedOff += OnLoggedOff;
        }

        #endregion

        #region Event Handlers

        private void OnLoggedOff(object sender, LoggedOffEventArgs e)
        {
            m_viewModel.IsLoggedOn = false;
        }

        #endregion

        #region Public Methods

        public bool CanExecute(object parameter)
        {
            return m_viewModel.IsLoggedOn;
        }

        public void Execute(object parameter)
        {
            m_engine?.LoginManager.LogOff();
        }

        #endregion
    }

    #endregion
}

