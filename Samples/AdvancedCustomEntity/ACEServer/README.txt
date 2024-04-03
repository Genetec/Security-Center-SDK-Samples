=============================================================
 USING CUSTOM PRIVILEGES
=============================================================
Projects that uses custom privileges require privileges.xml file to be copied in their local folder. 
You should add the following "Post-Build step" to avoid the exception at runtime.

xcopy /R /Y "$(GSC_SDK)privileges.xml" "$(TargetDir)"

=============================================================