using Genetec.Sdk.Entities;
using Genetec.Sdk.Entities.CustomFields;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace Genetec.Sdk.Samples.SamplesLibrary
{
    #region Classes

    public class CustomFieldsListView : ListView
    {
        #region Fields

        private Entity m_entity;

        #endregion

        #region Properties

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Entity Entity
        {
            get { return m_entity; }
            set
            {
                // Unsubscribe from the entity's events
                if (m_entity != null)
                {
                    m_entity.CustomFieldsChanged -= OnEntityCustomFieldsChanged;
                }

                m_entity = value;

                // Subscribe to the entity's events
                if (m_entity != null)
                {
                    m_entity.CustomFieldsChanged += OnEntityCustomFieldsChanged;
                }

                Task.Run(async () => await RefreshCustomFields());
            }
        }

        #endregion

        #region Nested Classes and Structures

        private sealed class CustomFieldValueViewItem : ListViewItem
        {
            #region Fields

            private CustomFieldValue m_objCFValue;

            #endregion

            #region Properties

            public CustomFieldValue Value
            {
                get { return m_objCFValue; }
            }

            #endregion

            #region Constructors

            public CustomFieldValueViewItem(CustomFieldValue objCFValue)
            {
                m_objCFValue = objCFValue;

                Text = objCFValue.CustomField.Name;
                SubItems.Add(objCFValue.CustomField.ValueType.ToString());
                if (objCFValue.Value != null)
                {
                    SubItems.Add(objCFValue.Value.ToString());
                }
                else
                {
                    SubItems.Add("");
                }
            }

            #endregion
        }

        #endregion

        #region Constructors

        public CustomFieldsListView()
        {
            FullRowSelect = true;
            View = View.Details;
            Columns.Add("Name");
            Columns.Add("Type");
            Columns.Add("Value");
        }

        #endregion

        #region Event Handlers

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            if (SelectedItems.Count == 1)
            {
                CustomFieldValueViewItem lvItem = SelectedItems[0] as CustomFieldValueViewItem;
                if (lvItem != null)
                {
                    using (CustomFieldValueEditorDlg dlg = new CustomFieldValueEditorDlg())
                    {
                        dlg.Value = lvItem.Value;
                        dlg.ShowDialog(this);
                    }
                }
            }
        }

        private async void OnEntityCustomFieldsChanged(object sender, EventArgs e)
        {
            await RefreshCustomFields();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Refresh the list of custom fields of the entity
        /// </summary>
        private async Task RefreshCustomFields()
        {
            try
            {
                BeginUpdate();
                Items.Clear();

                if (m_entity == null)
                    return;

                foreach (CustomFieldValue objCFValue in await m_entity.GetCustomFieldsAsync())
                {
                    CustomFieldValueViewItem lvItem = new CustomFieldValueViewItem(objCFValue);
                    Items.Add(lvItem);
                }
            }
            finally
            {
                EndUpdate();
            }
        }

        #endregion
    }

    #endregion
}

