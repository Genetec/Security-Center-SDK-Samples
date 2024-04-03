using Genetec.Sdk.Entities.CustomFields;
using System;
using System.Windows.Forms;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace Genetec.Sdk.Samples.SamplesLibrary
{
    #region Classes

    public partial class CustomFieldValueEditorDlg : Form
    {
        #region Fields

        private CustomFieldValue m_objCFValue;

        private Control m_objControl;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or Sets the custom field value
        /// </summary>
        public CustomFieldValue Value
        {
            get { return m_objCFValue; }
            set
            {
                m_objCFValue = value;

                m_fullName.Text = m_objCFValue.CustomField.Name;

                switch (m_objCFValue.CustomField.ValueType)
                {
                    case CustomFieldValueType.Text:
                        {
                            TextBox objControl = new TextBox();
                            objControl.Text = Convert.ToString(m_objCFValue.Value);
                            objControl.KeyPress += OnTextControl_KeyPress;
                            m_objControl = objControl;
                        }
                        break;
                    case CustomFieldValueType.Numeric:
                        {
                            NumericUpDown objControl = new NumericUpDown();
                            objControl.Minimum = int.MinValue;
                            objControl.Maximum = int.MaxValue;
                            objControl.Value = Convert.ToDecimal(m_objCFValue.Value);
                            objControl.ValueChanged += OnNumControl_ValueChanged;
                            m_objControl = objControl;
                        }
                        break;
                    case CustomFieldValueType.Boolean:
                        {
                            CheckBox objControl = new CheckBox();
                            objControl.Checked = Convert.ToBoolean(m_objCFValue.Value);
                            objControl.CheckedChanged += OnCheckControl_CheckedChanged;
                            m_objControl = objControl;
                        }
                        break;
                    case CustomFieldValueType.DateTime:
                        {
                            DateTimePicker objControl = new DateTimePicker();
                            objControl.MinDate = DateTime.MinValue;
                            objControl.MaxDate = DateTime.MaxValue;
                            objControl.Value = Convert.ToDateTime(m_objCFValue.Value);
                            objControl.ValueChanged += OnDateTimeControl_ValueChanged;
                            m_objControl = objControl;
                        }
                        break;
                    case CustomFieldValueType.Decimal:
                        {
                            NumericUpDown objControl = new NumericUpDown();
                            objControl.Minimum = int.MinValue;
                            objControl.Maximum = int.MaxValue;
                            objControl.DecimalPlaces = 8;
                            objControl.Value = Convert.ToDecimal(m_objCFValue.Value);
                            objControl.ValueChanged += OnNumControl_ValueChanged;
                            m_objControl = objControl;
                        }
                        break;
                    case CustomFieldValueType.Image:
                        {
                            PictureBox objControl = new PictureBox();
                            objControl.Image = m_objCFValue.Value as System.Drawing.Image;
                            objControl.DoubleClick += OnPictureBoxDoubleClick;
                            m_objControl = objControl;
                        }
                        break;
                }

                m_name.Controls.Clear();
                m_objControl.Dock = DockStyle.Fill;
                m_objControl.Parent = m_name;
            }
        }

        #endregion

        #region Constructors

        public CustomFieldValueEditorDlg()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        private void OnCheckControl_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox objControl = sender as CheckBox;
            m_objCFValue.Value = objControl.Checked;
        }

        private void OnDateTimeControl_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker objControl = sender as DateTimePicker;
            m_objCFValue.Value = objControl.Value;
        }

        private void OnNumControl_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown objControl = sender as NumericUpDown;
            try
            {
                m_objCFValue.Value = objControl.Value;
            }
            catch (Exception)
            {

            }
        }

        private void OnPictureBoxDoubleClick(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Multiselect = false;
                ofd.Filter = "Bitmaps|*.bmp";
                ofd.FilterIndex = 0;

                // Show file dialog so the user can select a new image
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        System.Drawing.Image bmp = System.Drawing.Image.FromFile(ofd.FileName);

                        m_objCFValue.Value = bmp;

                        Close();
                    }
                    catch
                    {
                        MessageBox.Show("Could not load the image");
                    }
                }
            }
        }

        private void OnTextControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                TextBox objControl = sender as TextBox;
                m_objCFValue.Value = objControl.Text;
            }
        }

        #endregion
    }

    #endregion
}

