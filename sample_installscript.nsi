
;--------------------------------

; The name of the installer
Name "Zx Spectrum SCR Thumbnail Provider 32bit"

; The file to write
OutFile "ZXSCRThumbs_Install.exe"

; The default installation directory
InstallDir $PROGRAMFILES\ZxThumbnailers\SCR

icon nsis-install-by-arda.ico
uninstallicon nsis-uninstall-by-arda.ico

; Request application privileges for Windows Vista
RequestExecutionLevel admin


LicenseText "You must agree these terms below to proceed." "Next"
LicenseData "license_SCR.txt"
LicenseForceSelection checkbox "I agree"


;--------------------------------

; Pages

Page license
Page Directory
Page instfiles

UninstPage uninstConfirm
UninstPage instfiles


;--------------------------------


; The stuff to install
Section "" ;No components page, name is not important

  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; Put file there
  File ArdaSCRThumbs.dll

  WriteRegStr HKCR ".SCR\shellex\{e357fccd-a995-4576-b01f-234630154e96}" "" "{836d9b4f-9333-4d5e-a1bf-149b3741c163}"

  ExecWait '"$WINDIR\Microsoft.NET\Framework\v2.0.50727\RegAsm.exe" /codebase "$INSTDIR\ArdaSCRThumbs.dll"' $0
  

  !if $0 > 0 
  !error "DLL Registration error!!!"
  !else if  $0 < 0
  !error "!!!Please Check your .net framework install!!! INSTALL FAILED!"
  !else
  DetailPrint "Thumbnailer registration successful."
  !endif



  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\ZXSCRThumbs" "DisplayName" "ZX Spectrum SCR Thumbnail Provider"
 WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\ZXSCRThumbs" "URLInfoAbout" "http://arda.veanewmedia.com/blog"
 WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\ZXSCRThumbs" "DisplayIcon" '"$INSTDIR\ZxSCRThumbs_uninstall.exe"'
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\ZXSCRThumbs" "UninstallString" '"$INSTDIR\ZxSCRThumbs_uninstall.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\ZxSCRThumbs" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\ZXSCRThumbs" "NoRepair" 1
  WriteUninstaller ZxSCRThumbs_uninstall.exe



SectionEnd ; end the section


; Uninstaller

Section "Uninstall"
  
  ; Remove registry keys
  
  ExecWait '"$WINDIR\Microsoft.NET\Framework\v2.0.50727\RegAsm.exe" /unregister "$INSTDIR\ArdaSCRThumbs.dll"' $0
  DetailPrint "DLL registration report: $0"

  DeleteRegKey HKCR ".SCR\shellex\{e357fccd-a995-4576-b01f-234630154e96}"
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\ZXSCRThumbs"


  ; Remove files and uninstaller
  Delete $INSTDIR\ArdaSCRThumbs.dll
  Delete $INSTDIR\ZxSCRThumbs_uninstall.exe

SectionEnd