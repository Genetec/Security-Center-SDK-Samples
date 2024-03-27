using System;
using System.Collections.Generic;
using System.Linq;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Entities.Maps;
using Genetec.Sdk.Workspace.Components;
using Genetec.Sdk.Workspace.Components.MapObjectViewBuilder;
using Genetec.Sdk.Workspace.Maps;

namespace ACEClient.Map
{
    public class CustomCameraMapViewBuilder : MapObjectViewBuilder
    {
        public override string Name { get { return "CustomMapViewBuilder"; } }
        public override Guid UniqueId { get { return new Guid("825F9D26-41CE-47AB-8ACC-3100DF339842"); } }

        // Default Priority must be overriden or else CreateViews will not be called
        public override int Priority
        {
            get
            {
                int randomPriority = 42;
                return randomPriority;
            }
        }

        public override IEnumerable<IMapObjectView> CreateViews(IEnumerable<MapObject> mapObjects, MapContext context)
        {
            var result = new List<CustomCameraMapObjectView>();

            foreach (CustomEntityMapObject mapObject in mapObjects.OfType<CustomEntityMapObject>())
            {
                // Only add this map object if the custom entity is a Custom Camera type
                CustomEntity ownerEntity = Workspace.Sdk.GetEntity(mapObject.LinkedEntity) as CustomEntity;
                if (ownerEntity == null)
                    continue;

                if (ownerEntity.CustomEntityType.Equals(ACECommon.CustomCamera.TypeGuid))
                    result.Add(new CustomCameraMapObjectView(Workspace, mapObject));
            }
            return result;
        }
    }
}
