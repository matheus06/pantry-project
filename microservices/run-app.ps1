#!/usr/bin/env pwsh
[CmdletBinding()]
Param (
    [Parameter(Mandatory = $false, ValueFromPipeline = $false)]
    [string]
    $EnvironmentName = "docker"
)
Begin {
    Push-Location (Join-Path $PSScriptRoot .. -Resolve)
}
Process {

    # clear previous compose
    docker-compose down

    # remove env.list if exists
    $tmp_envListPath = Join-Path $PSScriptRoot ../env.list
    if (Test-Path $tmp_envListPath) {
        Remove-Item $tmp_envListPath -Force
    }

    # initial "env.list" values
    $envVarSettings = ''

    # define hostname
    $envVarSettings += "HOSTNAME=$([System.Environment]::MachineName)"

    if (-not [string]::IsNullOrEmpty($EnvironmentName)) {
        $envVarSettings += "`nASPNETCORE_ENVIRONMENT=$EnvironmentName"
    }

    # save environment variables
    Set-Content -Path $tmp_envListPath -Value $envVarSettings -Encoding UTF8
    # need to convert CRLF to LF line endings
    ((Get-Content $tmp_envListPath) -join "`n") + "`n" | Set-Content -NoNewline $tmp_envListPath

    # run docker compose
    docker-compose up
}
End {
    Pop-Location
}