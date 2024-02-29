function Select-LethalCompanyPathGUI {
    Add-Type -AssemblyName System.Windows.Forms
    $form = New-Object System.Windows.Forms.Form
    $form.Text = 'Select Lethal Company Path'
    $form.Size = New-Object System.Drawing.Size(600,400)
    $form.StartPosition = 'CenterScreen'

    $label = New-Object System.Windows.Forms.Label
    $label.Location = New-Object System.Drawing.Point(10,20)
    $label.Size = New-Object System.Drawing.Size(580,20)
    $label.Text = 'Select the Steam library folder where you have installed Lethal Company:'
    $form.Controls.Add($label)

    $listBox = New-Object System.Windows.Forms.ListBox
    $listBox.Location = New-Object System.Drawing.Point(10,50)
    $listBox.Size = New-Object System.Drawing.Size(560,200)
    $listBox.Height = 200

    # Load Steam library paths into the listbox
    $SteamLibraryInfoPath = "C:\Program Files (x86)\Steam\steamapps\libraryfolders.vdf"
    if (-not [System.IO.File]::Exists($SteamLibraryInfoPath)) {
        [System.Windows.Forms.MessageBox]::Show("Fatal error: Cannot find Steam.", "Error", [System.Windows.Forms.MessageBoxButtons]::OK, [System.Windows.Forms.MessageBoxIcon]::Error)
        return $null
    }

    $RawSteamLibsList = Select-String -Path $SteamLibraryInfoPath -Pattern "`"path`""
    $SteamLibsList = $RawSteamLibsList | % { $_.ToString().Split("`t")[4].Trim('"').Replace("\\", "\") }
    $SteamLibsList | ForEach-Object { $listBox.Items.Add($_) }

    $form.Controls.Add($listBox)

    $button = New-Object System.Windows.Forms.Button
    $button.Location = New-Object System.Drawing.Point(10,260)
    $button.Size = New-Object System.Drawing.Size(100,23)
    $button.Text = 'Select'
    $button.DialogResult = [System.Windows.Forms.DialogResult]::OK
    $form.Controls.Add($button)
    $form.AcceptButton = $button

    $form.Topmost = $true

    $result = $form.ShowDialog()

    if ($result -eq [System.Windows.Forms.DialogResult]::OK) {
        $selectedIndex = $listBox.SelectedIndex
        $selectedPath = $SteamLibsList[$selectedIndex]
        if ($selectedIndex -ne -1) {
            $LCPath = "$selectedPath\steamapps\common\Lethal Company"
            Write-Host "Selected $LCPath" -ForegroundColor Green
            return $LCPath
        } else {
            [System.Windows.Forms.MessageBox]::Show("No selection made.", "Warning", [System.Windows.Forms.MessageBoxButtons]::OK, [System.Windows.Forms.MessageBoxIcon]::Warning)
            return $null
        }
    }
}

function FindLethalCompanyPathGUI {
    $ConfigPath = ".\Scripts\LCPath.txt"
    if ([System.IO.File]::Exists($ConfigPath)) {
        $LCPath = Get-Content $ConfigPath
        if (-not [System.IO.File]::Exists("$LCPath\Lethal Company.exe")) {
            Write-Host "Invalid path in config file. Selecting a new installation folder." -ForegroundColor Yellow
            $LCPath = Select-LethalCompanyPathGUI
            Set-Content -Path $ConfigPath -Value $LCPath
        }
    } else {
        $LCPath = Select-LethalCompanyPathGUI
        Set-Content -Path $ConfigPath -Value $LCPath
    }
    return $LCPath
}

