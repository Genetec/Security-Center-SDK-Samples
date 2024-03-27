using Genetec.Sdk;
using System;
using System.Windows;
using System.Windows.Controls;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace UserPrivileges
{
    #region Classes

    /// <summary>
    /// Interaction logic for UserControlPrivilege.xaml
    /// </summary>
    public partial class UserControlPrivilege : UserControl
    {
        #region Fields

        /// <summary>
        /// The 3 radio box group number
        /// This will let only 1 of the 3 radioboxes to be selected at once
        /// Every 3 radioboxes need a different group number
        /// </summary>
        private static int s_mGroupNumber;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the control unique identifier.
        /// </summary>
        /// <value>
        /// The Guid. It represents the Guid of the privilege.
        /// </value>
        public Guid ControlGuid { get; set; }

        /// <summary>
        /// The privilege. Either Granted, Deny or undefined.
        /// </summary>
        public PrivilegeAccess Privilege { get; private set; }

        #endregion

        #region Events and Delegates

        public event EventHandler<PrivilegeChangeRequestEventArgs> PrivilegeChangeRequested;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserControlPrivilege"/> class.
        /// </summary>
        /// <param name="privilege">The privilege.</param>
        /// <param name="name">The name.</param>
        /// <param name="guid">The unique identifier.</param>
        public UserControlPrivilege(PrivilegeAccess privilege, string name, Guid guid)
        {
            InitializeComponent();
            ControlGuid = guid;

            //We increment the group number so that the next 3 radioboxes work together so that only 1 is selected at a time.
            s_mGroupNumber++;
            Privilege = privilege;

            //This will group all 3 radio buttons to show that only 1 checked at a time.
            radioButtonGranted.GroupName += "group" + s_mGroupNumber;
            radioButtonDenied.GroupName += "group" + s_mGroupNumber;
            radioButtonUndefined.GroupName += "group" + s_mGroupNumber;

            UpdateRadioButtons(privilege, name);
        }

        #endregion

        #region Public Methods

        public void ForceSetPrivilegeAccess(PrivilegeAccess access)
        {
            Privilege = access;
            switch (Privilege)
            {
                case PrivilegeAccess.Denied:
                    radioButtonDenied.IsChecked = true;
                    break;
                case PrivilegeAccess.Granted:
                    radioButtonGranted.IsChecked = true;
                    break;
                case PrivilegeAccess.Undefined:
                    radioButtonUndefined.IsChecked = true;
                    break;
            }
        }

        /// <summary>
        /// Updates the radio buttons so that the correct one is selected depending on the privilege.
        /// </summary>
        /// <param name="privilege">The privilege.</param>
        /// <param name="name">The name.</param>
        public void UpdateRadioButtons(PrivilegeAccess privilege, string name)
        {
            if (name != null && name.Equals(string.Empty))
            {
                labelName.Content = ControlGuid.ToString();
            }
            else
            {
                labelName.Content = name;
            }

            if (privilege == PrivilegeAccess.Granted)
            {
                radioButtonGranted.IsChecked = true;
                radioButtonUndefined.IsEnabled = false;
            }
            if (privilege == PrivilegeAccess.Denied)
            {
                radioButtonDenied.IsChecked = true;
                radioButtonUndefined.IsEnabled = false;
            }
            if (privilege == PrivilegeAccess.Undefined)
            {
                radioButtonUndefined.IsChecked = true;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// RadioButtons the denied checked.
        /// If this radiobutton is selected, then we update the PrivilegeAccess also
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void RadioButtonDeniedChecked(object sender, RoutedEventArgs e)
        {
            RequestSetPrivilegeAccess(PrivilegeAccess.Denied);
        }

        /// <summary>
        /// RadioButtons the granted checked. 
        /// If this radiobutton is selected, then we update the PrivilegeAccess also
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void RadioButtonGrantedChecked(object sender, RoutedEventArgs e)
        {
            RequestSetPrivilegeAccess(PrivilegeAccess.Granted);
        }

        /// <summary>
        /// RadioButtons the undefined checked.
        /// If this radiobutton is selected, then we update the PrivilegeAccess also
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void RadioButtonUndefinedChecked(object sender, RoutedEventArgs e)
        {
            RequestSetPrivilegeAccess(PrivilegeAccess.Undefined);
        }

        private void RequestSetPrivilegeAccess(PrivilegeAccess requestedPrivilegeAccess)
        {
            if (Privilege == requestedPrivilegeAccess)
            {
                return;
            }
            var args = new PrivilegeChangeRequestEventArgs(ControlGuid, requestedPrivilegeAccess);
            var handler = PrivilegeChangeRequested;
            if (handler != null)
            {
                handler(this, args);
            }
            if (args.Cancel)
            {
                ForceSetPrivilegeAccess(Privilege);
            }
            else
            {
                Privilege = requestedPrivilegeAccess;
            }
        }

        #endregion
    }

    #endregion
}

