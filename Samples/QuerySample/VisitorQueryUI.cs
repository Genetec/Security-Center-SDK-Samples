using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Entities.CustomFields;
using Genetec.Sdk.Queries;
using System;
using System.Linq;
using System.Windows.Forms;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace QuerySample
{
    #region Classes

    public partial class VisitorQueryUI : UserControl
    {
        #region Fields

        private VisitorQuery m_objQuery;

        private Engine m_sdkEngine;

        #endregion

        #region Constructors

        public VisitorQueryUI()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        private void OnButtonQueryClick(object sender, EventArgs e)
        {
            SystemConfiguration objSystemConfig = m_sdkEngine.GetEntity(SystemConfiguration.SystemConfigurationGuid) as SystemConfiguration;
            if (objSystemConfig == null)
            {
                return;
            }

            //Setting the search field to null tell the report engine to ignore them...
            m_objQuery.FirstName = null;
            m_objQuery.LastName = null;

            m_objQuery.CustomFields.Clear();

            if (!string.IsNullOrEmpty(m_firstName.Text))
            {
                m_objQuery.FirstName = m_firstName.Text;
            }
            if (!string.IsNullOrEmpty(m_lastName.Text))
            {
                m_objQuery.LastName = m_lastName.Text;
            }

            //As of version 2.0 company is stored as a cusom field.
            if (m_company.Text.Length > 0)
            {
                //Locate the company field and add a filter for it.
                //Using this method may throw an exception, if it's likely to be null will neeed to do it manually
                //CustomField company = objSystemConfig.CustomFieldService.GetCustomField("Company", EntityType.Visitor);

                CustomField company = objSystemConfig.CustomFieldService
                                                     .CustomFields
                                                     .FirstOrDefault(cf => cf.EntityType==EntityType.Visitor && cf.Name == "Company");
                if (company == null)
                {
                    return;
                }

                CustomFieldFilter filter = new CustomFieldFilter(company, m_company.Text);
                m_objQuery.CustomFields.Add(filter);
            }
            m_objQuery.BeginQuery(null, null);
        }

        #endregion

        #region Public Methods

        public void Initialize(Engine sdkEngine, VisitorQuery objQuery)
        {
            m_sdkEngine = sdkEngine;
            m_objQuery = objQuery;
        }

        #endregion
    }

    #endregion
}

