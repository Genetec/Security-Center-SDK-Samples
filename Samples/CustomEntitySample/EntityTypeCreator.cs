using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Entities.CustomEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

// ==========================================================================
// Copyright (C) 2017 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace CustomEntitySample
{
   internal class EntityTypeCreator
   {
        private readonly Engine m_engine;
        private readonly SystemConfiguration m_systemConfiguration;

        public EntityTypeCreator(Engine engine)
        {
            m_engine = engine;
            m_systemConfiguration = m_engine.GetEntity<SystemConfiguration>(SystemConfiguration.SystemConfigurationGuid);
        }

        private CustomEvent GetOrCreateCustomEvent(string name)
        {
            if(string.IsNullOrEmpty(name)) throw new ArgumentException("The argument \"name\" is invalid.");
            if (m_systemConfiguration == null) return null;

            var customEventService = m_systemConfiguration.CustomEventService;
            var customEvent = customEventService.CustomEvents.FirstOrDefault(ce => ce.Name == name);
            if(customEvent == null)
            {
                customEvent = customEventService.CreateCustomEventBuilder()
                                                .SetName(name)
                                                .SetEntityType(EntityType.CustomEntity)
                                                .Build();
                var result = customEventService.Add(customEvent);
                if(result.Success)
                {
                    Console.WriteLine("Successfully added the CustomEvent : \"" + name + "\" with id :\"" + result.Id + "\"");
                }
                else
                {
                    Console.WriteLine("Unable to add the CustomEvent : \"" + name + "\".");
                }
            }
            return customEvent;
        }

        /// <summary>
        /// Create the different types
        /// </summary>
        public void CreateTypes()
        {
            var systemConfig = m_engine.GetEntity<SystemConfiguration>(SystemConfiguration.SystemConfigurationGuid);
            if (systemConfig == null) return;

            var customEventRefill = GetOrCreateCustomEvent("Refill");
            var customEventMove = GetOrCreateCustomEvent("Move");

            var typeDescriptors = new List<CustomEntityTypeDescriptor>
            {
                ConstructTrainDescriptor(customEventRefill, customEventMove),
                ConstructTrainCarDescriptor(customEventMove)
            };

            m_engine.TransactionManager.CreateTransaction();
            typeDescriptors.ForEach(type => systemConfig.AddOrUpdateCustomEntityType(type));
            m_engine.TransactionManager.CommitTransaction();
        }

        /// <summary>
        /// Constructs a train type descriptor
        /// </summary>
        private CustomEntityTypeDescriptor ConstructTrainDescriptor(CustomEvent customEventRefill, CustomEvent customEventMove)
        {
            var trainDescriptor = new CustomEntityTypeDescriptor(new Guid("{6D0B6DA5-F152-4490-B38E-5082B6C2B043}"), "Train",
                CustomEntityTypeCapabilities.CreateDelete |
                CustomEntityTypeCapabilities.IsVisible |
                CustomEntityTypeCapabilities.MapSupport,
                new Version("1.0"))
            {
                LargeIcon = new BitmapImage( new Uri("pack://application:,,,/CustomEntitySample;;;component/Resources/train-icon.png", UriKind.RelativeOrAbsolute)),
                SmallIcon = new BitmapImage(new Uri("pack://application:,,,/CustomEntitySample;;;component/Resources/train-icon.png", UriKind.RelativeOrAbsolute))
            };

            trainDescriptor.SetSupportedEvents(new List<EventType> { EventType.DoorOpen, EventType.DoorClose },
                                               new List<CustomEvent> { customEventRefill, customEventMove });

            trainDescriptor.SetAccessPrivileges(new SdkPrivilege(new Guid("{78388FA0-93FE-41F2-B85E-72EA29D11F1B}")), 
                                                new SdkPrivilege(new Guid("BFB972EA-B147-42A4-A88E-CDEAFCED4755")), 
                                                new SdkPrivilege(new Guid("6BE972AA-A1D6-4054-B20A-65EF6373610B")), 
                                                new SdkPrivilege(Guid.Empty));
            trainDescriptor.HierarchicalChildCustomTypes = new[] { new Guid("{66FB3731-85EE-428E-9B91-5A924AE1F9EF}") };

            return trainDescriptor;
        }

        /// <summary>
        /// Constructs a train car type descriptor
        /// </summary>
        private CustomEntityTypeDescriptor ConstructTrainCarDescriptor(CustomEvent customEventMove)
        {
            var trainCarDescriptor = new CustomEntityTypeDescriptor(new Guid("{66FB3731-85EE-428E-9B91-5A924AE1F9EF}"), "Train car",
                CustomEntityTypeCapabilities.CreateDelete |
                CustomEntityTypeCapabilities.IsVisible,
                new Version("1.0"))
            {
                LargeIcon = new BitmapImage( new Uri("pack://application:,,,/CustomEntitySample;;;component/Resources/cartrain-icon.png", UriKind.RelativeOrAbsolute)),
                SmallIcon = new BitmapImage(new Uri("pack://application:,,,/CustomEntitySample;;;component/Resources/train-icon.png", UriKind.RelativeOrAbsolute))
            };

            trainCarDescriptor.SetSupportedEvents(null, new List<CustomEvent> { customEventMove });
            trainCarDescriptor.HierarchicalChildTypes = new[] { EntityType.Camera };

            return trainCarDescriptor;
        }
    }
}
