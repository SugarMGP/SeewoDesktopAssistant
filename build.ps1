[CmdletBinding()]
param (
    [Parameter()]
    [string]
    $Action
)
$ErrorActionPreference = "Stop"
$AllowedActions = "clean", "build", "format", "publish"
$OriginalPublishDir = ".\SeewoDesktopAssistant.Desktop\bin\Release\net5.0-windows\publish"
$DestinationPublishDir = ".\publish"
$Configuration = "Release"

if ($Action -eq "--help") {
    Write-Output("Usage: build [Action]")
    Write-Output("Allowed actions are: " + $AllowedActions)
    exit 0
}

if($Action -eq "") {
    $Action = "build"
}

Write-Output("")
Write-Output("SeewoDesktopAssistant")
Write-Output("")
Write-Output("Action: " + $Action)
Write-Output("Current .NET version:")

dotnet --version

Write-Output("")
Write-Output("Running requested action...")

function Clean {
    Write-Output("Cleaning build outputs...")
    Remove-Item SeewoDesktopAssistant.*\bin -Recurse -Force -Confirm:$false -ErrorAction Ignore
    Remove-Item SeewoDesktopAssistant.*\obj -Recurse -Force -Confirm:$false -ErrorAction Ignore
    Remove-Item publish -Recurse -Force -Confirm:$false -ErrorAction Ignore
    Write-Output("Cleaning finished.")
}

function Build {
    dotnet build -c $Configuration
}

function Format {
    dotnet format -v detailed
}

function Publish {
    Clean
    dotnet publish SeewoDesktopAssistant.Desktop -c $Configuration
    Write-Output("Moving " + $OriginalPublishDir + " into " + $DestinationPublishDir + "...")
    Move-Item $OriginalPublishDir $DestinationPublishDir -Force -Confirm:$false
}

function RunAction {
    param (
        [string]$_Action
    )
    
    if(!($AllowedActions -contains $_Action)) {
        Write-Error("Illegal action: " + $Action + ", allowed actions are: " + $AllowedActions + ".")
    }

    $CurrentAction = $_Action

    Write-Output("Running """ + $CurrentAction + """...");

    if ($CurrentAction -eq "clean") {
        Clean
    }
    if ($CurrentAction -eq "build") {
        Build
    }
    if ($CurrentAction -eq "format") {
        Format
    }
    if ($CurrentAction -eq "publish") {
        Publish
    }
}

RunAction($Action)

Write-Output("")

exit $LASTEXITCODE
