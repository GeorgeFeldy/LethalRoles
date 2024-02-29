function Select-LethalCompanyPath {

    $SteamLibraryInfoPath = "C:\Program Files (x86)\Steam\steamapps\libraryfolders.vdf"

    if ( -not [System.IO.File]::Exists($SteamLibraryInfoPath) )
    {
        Write-Host "Fatal error: Cannot find Steam." -ForegroundColor Red
        exit
    }

    # Get all of the user's Steam Library folders and make an array of only the paths
    $RawSteamLibsList = Select-String -Path $SteamLibraryInfoPath -Pattern "`"path`""
    $SteamLibsList = $RawSteamLibsList | % { $_.ToString().Split("`t")[4].Trim('"').Replace("\\", "\") }

    # Ask user to select where Lethal Company is installed
    Write-Host "Select the Steam library folder where you have installed Lethal Company from the list below [default: 1]:" -ForegroundColor Blue
    foreach ($idx in (0..($SteamLibsList.Length - 1)))
    {
        Write-Host "$($idx + 1). " -ForegroundColor Yellow -NoNewline
        Write-Host "$($SteamLibsList[$idx])"
    }
    $RawSelection = Read-Host -Prompt "Make a selection"
    Write-Host ""
    $Selection = (0)
    if ( $RawSelection -ne "" )
    {
        $Selection = (([int]$RawSelection) - 1)
    }

    # Validate selection
    if ( ($Selection -lt 0) -or ($Selection -ge $SteamLibsList.Length) )
    {
        Write-Host "Fatal error: Selection invalid." -ForegroundColor Red
        exit
    }

    $LCPath = "$($SteamLibsList[$Selection])\steamapps\common\Lethal Company"
    Write-Host "Selected  $LCPath" -ForegroundColor Green

    return $LCPath
}

function FindLethalCompanyPath {
    $ConfigPath = ".\Scripts\LCPath.txt" 
    # Check if the config file exists and read the game path
    if ([System.IO.File]::Exists($ConfigPath)) {
        $LCPath = Get-Content $ConfigPath
        if (-not [System.IO.File]::Exists("$LCPath\Lethal Company.exe")) {
            Write-Host "Invalid path in config file.\n ($LCPath)\n Selecting a new installation folder." -ForegroundColor Yellow
            $LCPath = Select-LethalCompanyPath
            Set-Content -Path $ConfigPath -Value $LCPath
        }
    } else {
        $LCPath = Select-LethalCompanyPath
        Set-Content -Path $ConfigPath -Value $LCPath
    }
    return $LCPath
}