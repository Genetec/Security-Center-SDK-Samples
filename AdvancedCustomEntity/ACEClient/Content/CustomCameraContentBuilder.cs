using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Workspace.Components.ContentBuilder;
using Genetec.Sdk.Workspace.Pages.Contents;

namespace ACEClient.Content
{
    public class CustomCameraContentBuilder : ContentBuilder
    {
        private string m_name = "Custom Camera ContentBuilder";
        private Guid m_uniqueId = new Guid("AF527FA0-560A-48AB-9041-9658E6D333E2");

        public override string Name
        {
            get { return m_name; }
        }

        public override Guid UniqueId
        {
            get { return m_uniqueId; }
        }

        public override int Priority
        {
            get { return 1; }
        }

        /// <summary>
        /// Return custom content only if it is the "Custom Camera" custom entity, otherwise returns null.
        /// If null is returned then it will use the default Content for the entity
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override ContentGroup BuildContentGroup(ContentBuilderContext context)
        {
            // Get the entity that we're about to build content for. Depending on the context, the field could be "Id" or "SourceEntityGuid"
            var tileOwnerGuid = context.Fields.Contains("Id") ? context.Fields["Id"] : (
                                context.Fields.Contains("SourceEntityGuid") ? context.Fields["SourceEntityGuid"] :
                                null);
            if (tileOwnerGuid == null)
                return null;

            Entity tileOwnerEntity = Workspace.Sdk.GetEntity((Guid)tileOwnerGuid);
            if (tileOwnerEntity == null)
                return null;

            // Make sure it is the CustomCamera CustomEntity
            if (!tileOwnerEntity.EntityType.Equals(EntityType.CustomEntity) || !((CustomEntity)tileOwnerEntity).CustomEntityType.Equals(ACECommon.CustomCamera.TypeGuid))
                return null;

            // If the custom entity has child entities, can just return null. It will display the children's contents instead.
            if (tileOwnerEntity.HierarchicalChildren != null && tileOwnerEntity.HierarchicalChildren.Count > 0)
                return null;

            
            // Else we need to make content for the custom entity. Use EntityContentGroup for entities
            EntityContentGroup newContentGroup = new EntityContentGroup((Guid)tileOwnerGuid);
            newContentGroup.Initialize(Workspace);
            
            return newContentGroup;
        }
    }
}
