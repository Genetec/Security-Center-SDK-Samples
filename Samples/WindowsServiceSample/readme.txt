How to install:

1. Open a visual studio command prompt as an ADMINISTRATOR
2. Go to the bin/debug of the WindowsServiceSample
3. Enter the following command: installutil.exe WindowsServiceSample.exe

How to start and run the service:

1. Open the Services application
		You should see Genetec Windows Service Sample listed in the Services window
2. Click on the service. A menu should appear on the left. Click start. To stop your service, click stop.

How to view even logs;

1. Open Event Viewer
2. In the section "Applications and Services Logs", click on "WindowsServiceSampleLogs"

How to uninstall:

1. Open a visual studio command prompt as an ADMINISTRATOR
2. Go to the bin/debug of the WindowsServiceSample
3. Enter the following command: installutil.exe /u WindowsServiceSample.exe