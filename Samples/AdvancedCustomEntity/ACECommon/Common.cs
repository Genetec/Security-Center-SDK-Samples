using System;
using Genetec.Sdk.Entities;
namespace ACECommon
{
    /// <summary>
    /// Data about the "CustomCamera" CustomEntityTypeDescriptor
    /// </summary>
    public struct CustomCamera
    {
        // Store data that you wish to share between the Server and Client

        /// <summary>
        /// Custom Entity type name
        /// </summary>
        public static readonly string Name = "Custom Camera";
        public static readonly Guid TypeGuid = new Guid("76dc9494-6677-4788-8151-7d23f4cc4132");

        public static Guid AddPrivilege = new Guid("{87B07BF4-E3AD-42C9-8C4F-4276789B5077}");
        public static Guid RemovePrivilege = new Guid("{D4A9619F-55AB-4A7A-AA9C-F52185A6A422}");
        public static Guid ViewPrivilege = new Guid("{9E88FD7B-3364-459B-84BB-FCBE99D823FF}");
        public static Guid ModifyPrivilege = new Guid("{554795C9-6530-4DF8-B67B-91180E5BA8BA}");

        // Give ALL capabilities to this custom entity
        public static readonly CustomEntityTypeCapabilities Capabilities = CustomEntityTypeCapabilities.CreateDelete | CustomEntityTypeCapabilities.IsVisible |
                                                                           CustomEntityTypeCapabilities.HasRunningState | CustomEntityTypeCapabilities.CanBeFederated |
                                                                           CustomEntityTypeCapabilities.MapSupport | CustomEntityTypeCapabilities.MaintenanceMode;

        public struct CustomEventNames
        {
            public static readonly string CustomCameraOn = "CustomCameraOn";
            public static readonly string CustomCameraOff = "CustomCameraOff";
            public static readonly string CustomCameraAlert = "CustomCameraAlert";
        }
    }
}

