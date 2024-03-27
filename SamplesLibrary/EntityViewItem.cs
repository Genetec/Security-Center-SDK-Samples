using Genetec.Sdk.Entities;
using System.Windows.Forms;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace Genetec.Sdk.Samples.SamplesLibrary
{
    #region Classes

    public sealed class EntityViewItem : ListViewItem
    {
        #region Fields

        private Entity m_entity;

        #endregion

        #region Properties

        public Entity Entity
        {
            get { return m_entity; }
        }

        #endregion

        #region Constructors

        public EntityViewItem(Entity entity)
        {
            m_entity = entity;

            Text = m_entity.Name;
            SubItems.Add(m_entity.EntityType.ToString());
            ImageIndex = (int)m_entity.EntityType;
        }

        #endregion
    }

    #endregion
}

