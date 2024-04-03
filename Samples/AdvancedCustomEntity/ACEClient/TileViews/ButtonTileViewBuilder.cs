using System;
using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Workspace.Components;
using Genetec.Sdk.Workspace.Components.TileView;
using Genetec.Sdk.Workspace.Pages.Contents;

namespace ACEClient.TileViews
{
    public sealed class ButtonTileViewBuilder : TileViewBuilder
    {
        #region Properties

        public override string Name
        {
            get { return "Button TileView"; }
        }

        public override Guid UniqueId
        {
            get { return new Guid("{E0B61287-FEF3-40A1-937B-7C6CA9F06879}"); }
        }

        #endregion

        #region Public Methods

        public override TileView CreateView()
        {
            return new ButtonTileViewModel(Workspace);
        }

        public override bool IsSupported(TilePluginContext context)
        {
            if (!(context.TileState.Content is EntityContentGroup))
                return false;

            // Get the entity that this tile represents
            Guid tileOwnerGuid = ((EntityContentGroup) context.TileState.Content).EntityId;
            Entity tileOwnerEntity = Workspace.Sdk.GetEntity(tileOwnerGuid);

            // Display this TileView only if it belongs to the "Custom Camera" custom entity 
            if (tileOwnerEntity.EntityType.Equals(EntityType.CustomEntity))
                return ((CustomEntity)tileOwnerEntity).CustomEntityType.Equals(ACECommon.CustomCamera.TypeGuid);

            return false;
        }

        #endregion
    }
}
