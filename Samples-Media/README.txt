=============================================================
 USING GENETEC.SDK.MEDIA
=============================================================

Projects that do not use the SdkAssemblyLoader, 
requiring the usage of the Genetec.Sdk.Media assembly, 
should add the following "Post-Build step: ..." 

xcopy /R /Y "$(GSC_SDK)avcodec*.dll" "$(TargetDir)"
xcopy /R /Y "$(GSC_SDK)avformat*.dll" "$(TargetDir)"
xcopy /R /Y "$(GSC_SDK)avutil*.dll" "$(TargetDir)"
xcopy /R /Y "$(GSC_SDK)Genetec.*MediaComponent*" "$(TargetDir)"

for x86 (32-bit):
xcopy /R /Y "$(GSC_SDK)\x86\Genetec.Nvidia.dll" "$(TargetDir)"
xcopy /R /Y "$(GSC_SDK)\x86\Genetec.QuickSync.dll" "$(TargetDir)"

or for x64
xcopy /R /Y "$(GSC_SDK)\x64\Genetec.Nvidia.dll" "$(TargetDir)"
xcopy /R /Y "$(GSC_SDK)\x64\Genetec.QuickSync.dll" "$(TargetDir)"

xcopy /R /Y "$(GSC_SDK)swscale*.dll" "$(TargetDir)"
xcopy /R /Y "$(GSC_SDK)swresample*.dll" "$(TargetDir)"

This command will copy to the output of the project the EXE
and configuration files required for out-of-process decoding
for native and federated video streams.  Out-of-process 
decoding is a feature that provides:
 - Improved memory usage for video operations by spreading
   the memory usage over several processes
 - Enhanced fault isolation when decoding video streams.  

=============================================================