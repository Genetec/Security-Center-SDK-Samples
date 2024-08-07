@ECHO OFF
ECHO This file contains commands to register the module in Security Center.
ECHO The Security Desk and Config Tool use these key to locate the module assembly and load it.
ECHO More information in the SDK help file under 'Security Desk SDK Overview topic'
ECHO -----------------------------------------------------------------------------------------------
ECHO These commands are written to work when executed from the folder containing the module (the compiled assembly that uses Genetec.Sdk.Workspace.dll).
ECHO They require administrative privileges to write into the Windows registry.
set /p execute=Execute the commands now? [y/n]?: 
if /i NOT %execute%==y GOTO Canceled

if EXIST "%~dp0ModuleSample.dll" GOTO OUTPUT_FOLDER
if EXIST "%~dp0bin\debug\ModuleSample.dll" GOTO PROJECT_DIR
GOTO WRONG_DIR

:OUTPUT_FOLDER:
SET path_dll="%~dp0ModuleSample.dll"
goto REG_EDIT

:PROJECT_DIR:
SET path_dll="%~dp0bin\debug\ModuleSample.dll"

:REG_EDIT:
ECHO 32 bits
REG ADD "HKEY_LOCAL_MACHINE\SOFTWARE\Genetec\Security Center\Plugins\ModuleSample" /v ClientModule /t REG_SZ /d %path_dll% /f /reg:32
REG ADD "HKEY_LOCAL_MACHINE\SOFTWARE\Genetec\Security Center\Plugins\ModuleSample" /v Enabled /t REG_SZ /d "True" /f /reg:32
REG ADD "HKEY_LOCAL_MACHINE\SOFTWARE\Genetec\Security Center\Plugins\ModuleSample" /v ServerModule /t REG_SZ /d %path_dll% /f /reg:32
ECHO 64 bits
REG ADD "HKEY_LOCAL_MACHINE\SOFTWARE\Genetec\Security Center\Plugins\ModuleSample" /v ClientModule /t REG_SZ /d %path_dll% /f /reg:64
REG ADD "HKEY_LOCAL_MACHINE\SOFTWARE\Genetec\Security Center\Plugins\ModuleSample" /v Enabled /t REG_SZ /d "True" /f /reg:64
REG ADD "HKEY_LOCAL_MACHINE\SOFTWARE\Genetec\Security Center\Plugins\ModuleSample" /v ServerModule /t REG_SZ /d %path_dll% /f /reg:64
goto End

:WRONG_DIR:
ECHO ###############################################################################################
ECHO This batch file has failed to find the compiled Dll.
ECHO Please launch the batch file from your output directory for this project (typicaly bin\debug\).
ECHO ###############################################################################################
goto End

:Canceled:
ECHO Operation was canceled by user.
ECHO -----------------------------------------------------------------------------------------------

:End:
pause