using Genetec.Sdk;
using System;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace UserPrivileges
{
    #region Classes

    public class PrivilegeChangeRequestEventArgs : EventArgs
    {
        #region Properties

        public PrivilegeAccess Access { get; private set; }

        public bool Cancel { get; set; }

        public Guid PrivilegeId { get; private set; }

        #endregion

        #region Constructors

        public PrivilegeChangeRequestEventArgs(Guid privilegeId, PrivilegeAccess access)
        {
            PrivilegeId = privilegeId;
            Access = access;
        }

        #endregion
    }

    #endregion
}

