. "Scripts\FindLethalCompanyPath.ps1"
$LCPath = FindLethalCompanyPath

# Check for Lethal Company
Write-Host "Checking for Lethal Company game files in " -ForegroundColor Blue -NoNewline
Write-Host "$LCPath" -ForegroundColor Yellow -NoNewline
Write-Host "..." -ForegroundColor Blue
if ( -not [System.IO.File]::Exists("$LCPath\Lethal Company.exe") )
{
    Write-Host "Fatal error: Lethal Company not found in selected library. Try again." -ForegroundColor Red
    exit
}
else
{
    Write-Host "Success! (Found Lethal Company game files.)" -ForegroundColor Green
}

Write-Host "Regenerating Managed folder..." -ForegroundColor Blue
Remove-Item ".\Managed" -Recurse -ErrorAction SilentlyContinue
# Create the Managed directory if it does not exist
New-Item -ItemType Directory -Force -Path ".\Managed"
# Enumerate and copy files, excluding those starting with 'System'
Get-ChildItem "$LCPath\Lethal Company_Data\Managed" -Recurse | Where-Object { -not ($_.Name.StartsWith("System")) } | ForEach-Object {
    $destination = $_.FullName.Replace("$LCPath\Lethal Company_Data\Managed", ".\Managed")
    if ($_.PSIsContainer) {
        New-Item -ItemType Directory -Force -Path $destination
    } else {
        Copy-Item $_.FullName -Destination $destination
    }
}
Write-Host "Done!" -ForegroundColor Green

