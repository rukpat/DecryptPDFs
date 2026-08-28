#define MyAppName "Decrypt PDFs"
#define MyAppVersion "2.0.2"
#define MyAppPublisher "rukpat"
#define MyAppURL "https://github.com/rukpat/DecryptPDFs"
#define MyAppExeName "DecryptPDFs.exe"

[Setup]
; Unique to this app - do not reuse or regenerate; changing it breaks upgrade detection for anyone
; who already installed a previous version.
AppId={{872D5AB8-0B71-42E7-99EA-CBE1156A5B04}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={localappdata}\Programs\DecryptPDFs
DefaultGroupName=Decrypt PDFs
DisableProgramGroupPage=yes
; Per-user, no admin/UAC prompt - matches the app's own "just this Windows account" design
; (DPAPI encryption, %LocalAppData% storage) and lets the context-menu registry keys below live
; under HKCU rather than needing elevation to write HKEY_CLASSES_ROOT directly.
PrivilegesRequired=lowest
OutputDir=..\DecryptPDFs\publish
OutputBaseFilename=DecryptPDFs-Setup-{#MyAppVersion}
SetupIconFile=..\DecryptPDFs\PDFPasswordRemover.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern
UninstallDisplayIcon={app}\{#MyAppExeName}
LicenseFile=..\LICENSE

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "contextmenu"; Description: "Add a right-click ""Decrypt PDFs"" menu on files and folders"; GroupDescription: "Explorer integration:"
Name: "sendto"; Description: "Add to the ""Send to"" right-click menu"; GroupDescription: "Explorer integration:"

[Files]
Source: "..\DecryptPDFs\publish\DecryptPDFs\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{userappdata}\Microsoft\Windows\SendTo\Decrypt PDFs"; Filename: "{app}\{#MyAppExeName}"; Tasks: sendto

; HKCU (not HKCR) so no admin rights are needed - Windows merges HKCU\Software\Classes into the
; effective context-menu view, taking precedence over HKCR when both exist. %V (not %1) matches
; what the app's own .reg files already use and were tested against, so both files and folders
; invoke it correctly - see Resources/ContextMenu*.reg for the manual-install equivalent.
[Registry]
Root: HKCU; Subkey: "Software\Classes\*\shell\DecryptPDFs"; ValueType: string; ValueName: "MUIVerb"; ValueData: "Decrypt PDFs"; Tasks: contextmenu; Flags: uninsdeletekey
Root: HKCU; Subkey: "Software\Classes\*\shell\DecryptPDFs"; ValueType: string; ValueName: "Icon"; ValueData: """{app}\{#MyAppExeName}"""; Tasks: contextmenu
Root: HKCU; Subkey: "Software\Classes\*\shell\DecryptPDFs"; ValueType: string; ValueName: "Position"; ValueData: "Top"; Tasks: contextmenu
Root: HKCU; Subkey: "Software\Classes\*\shell\DecryptPDFs\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#MyAppExeName}"" ""%V"""; Tasks: contextmenu

Root: HKCU; Subkey: "Software\Classes\Directory\shell\DecryptPDFs"; ValueType: string; ValueName: "MUIVerb"; ValueData: "Decrypt PDFs"; Tasks: contextmenu; Flags: uninsdeletekey
Root: HKCU; Subkey: "Software\Classes\Directory\shell\DecryptPDFs"; ValueType: string; ValueName: "Icon"; ValueData: """{app}\{#MyAppExeName}"""; Tasks: contextmenu
Root: HKCU; Subkey: "Software\Classes\Directory\shell\DecryptPDFs"; ValueType: string; ValueName: "Position"; ValueData: "Top"; Tasks: contextmenu
Root: HKCU; Subkey: "Software\Classes\Directory\shell\DecryptPDFs\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#MyAppExeName}"" ""%V"""; Tasks: contextmenu

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Launch Decrypt PDFs"; Flags: nowait postinstall skipifsilent
