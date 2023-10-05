#!/usr/bin/env pwsh

$ErrorActionPreference = 'Stop'

$GITHUB_USERNAME_SECRET_NAME = 'GitHubUsername'
$GITHUB_TOKEN_SECRET_NAME = 'GitHubToken'

############ import required PowerShell modules
if ( -not (Get-Module -ListAvailable -Name Microsoft.PowerShell.SecretManagement) ) {
    Write-Output 'Installing module "Microsoft.PowerShell.SecretManagement"...'
    Install-Module Microsoft.PowerShell.SecretManagement -Force
}

Import-Module Microsoft.PowerShell.SecretManagement

if ( -not (Get-Module -ListAvailable -Name Microsoft.PowerShell.SecretStore) ) {
    Write-Output 'Installing module "Microsoft.PowerShell.SecretStore"...'
    Install-Module Microsoft.PowerShell.SecretStore -Force
}

Import-Module Microsoft.PowerShell.SecretStore

############ register default secret store
if ( -not (Get-SecretVault) ) {
    Write-Output 'Registering default secret store...'
    Register-SecretVault -Name SecretStore -ModuleName Microsoft.PowerShell.SecretStore -DefaultVault
}

############ store GitHub credentials in secret store
if ( (-not (Get-SecretInfo -Name $GITHUB_USERNAME_SECRET_NAME)) -or (-not (Get-Secret -Name $GITHUB_USERNAME_SECRET_NAME -AsPlainText -ErrorAction SilentlyContinue))) {
    do {
        $githubUsername = Read-Host -Prompt 'Enter your GitHub username'
    } while ( [string]::IsNullOrWhiteSpace($githubUsername) )
    Set-Secret -Name $GITHUB_USERNAME_SECRET_NAME -Secret $githubUsername
}

if ( (-not (Get-SecretInfo -Name $GITHUB_TOKEN_SECRET_NAME)) -or (-not (Get-Secret -Name $GITHUB_TOKEN_SECRET_NAME -AsPlainText -ErrorAction SilentlyContinue)) ) {
    do {
        $githubToken = Read-Host -Prompt 'Enter your GitHub token used to access GitHub packages'
    } while ( [string]::IsNullOrWhiteSpace($githubToken) )
    Set-Secret -Name $GITHUB_TOKEN_SECRET_NAME -Secret $githubToken
}

############ get GitHub credentials from secret store
$githubUsername = Get-Secret -Name $GITHUB_USERNAME_SECRET_NAME -AsPlainText
$githubToken = Get-Secret -Name $GITHUB_TOKEN_SECRET_NAME -AsPlainText

return @{
    GitHubUsername = $githubUsername
    GitHubToken    = $githubToken
}
