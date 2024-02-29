# Compress mod metadata and DLL to zip archive
Get-ChildItem -Path ".\manifest.json", ".\icon.png", ".\README.md", ".\CHANGELOG.md", ".\bin\Debug\netstandard2.1\LethalRoles.dll" |

Compress-Archive -DestinationPath ".\bin\Debug\LethalRoles.zip" -Force

. ".\Scripts\FindLethalCompanyPathGUI.ps1"
$LCPath = FindLethalCompanyPathGUI

Copy-Item -Path ".\bin\Debug\netstandard2.1\LethalRoles.dll" -Destination "$LCPath\BepInEx\plugins"
Copy-Item -Path ".\bin\Debug\netstandard2.1\LethalRoles.pdb" -Destination "$LCPath\BepInEx\plugins"

$DLLPath = "$LCPath/BepInEx/plugins/LethalRoles.dll"
Invoke-Expression "pdb2mdb `"$DLLPath`""

