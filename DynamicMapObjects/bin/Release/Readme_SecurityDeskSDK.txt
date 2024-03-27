=============================================================
 USING SECURITY DESK SDK CUSTOM MODULES
=============================================================

1 - General Information
-------------------------------------------------------------
Security Desk SDK Modules allow to add content within Security Desk in the form of task or addon to already existing entities like maps. 
Projects creating a custom module output a Dll. The Security Desk loads it dynamically from the location specified in the registry.
Here is some information on modules and how to add modules into Security Desk.

When adding new task you will find them along with the others included tasks in Security Desk. You could also create a new task group for them like we did in Module Sample. 
If your module does not add any task then you can view the content you added immediately after adding the registry entries (you can check DynamicMapObjects Sample for reference on module with no custom task).

Procedure to add a module:
	A. Compile your assembly: the dll that contains the Security Desk SDK module.
	B. Copy its full path.
	C. Add the following registry keys in HKEY_LOCAL_MACHINE\SOFTWARE\Genetec\Security Center\YOUR_MODULE
		- String value named ClientModule with the path to the dll as value.
		- String value named Enabled with True as value.
		- String value named ServerModule with the path to the dll as value.
	D. Repeat in the 64 bit location: HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Genetec\Security Center\YOUR_MODULE

The samples contain a batch file to facilitate module registration. Run the RegisterModule.bat script as admin in the build output folder to add the required registry keys.
More information on registering modules can be found in the SDK help file in the 'SecurityDesk SDK Overview' topic.

2 - Sample-Specific Instructions 
-------------------------------------------------------------
Here are the instructions on how to observe the modules create by the samples.

Dynamic Maps

	For this sample you have to first configure a map in the Config Tool using a geographical map.  
	Ensure the Map Manager role has a geographical map provider configured. Then create a map by
	adding it to an Area or using the Map designer task.  Define the default view so it displays the 
	Montreal island. Then, open Security Desk and using Maps, select the map you just created. 
	The sample shows officer travelling on the map.  This illustrates how a 3rd party can use the SDK
	to represent temporary custom map objects via a Genetec.Sdk.Workspace.Components.MapObjectProvider.MapObjectProvider subclass.

Module Blank

	For this sample you can search for My task in Security Desk to see the page.  This sample shows 
	a very simple module showing the basics of having a module registered in the Security Desk.
	
Module Sample

	Locate the sample tasks in the task group named 'SDK ModuleSample' in the Security Desk.  This sample 
	also contains example usage of the Genetec.Sdk.Controls library.  Look inside the sample code for more information.

Request Manager Module

	For this sample you need to start both Security Desk and the Request Manager Application. In 
	Security Desk search for Chat page under Maintenance to find the task. In the Request Manager Application 
	you need to login, specifying the Security Desk as the endpoint. Once both are open you can chat between the 
	SDK application and the Security Desk custom task "Chat page", through a Security Desk SDK service
	(implementation of Genetec.Sdk.Workspace.Services.IService), which is a background component loaded by the module 
	inside Security Desk.

=============================================================