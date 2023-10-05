param (
    [Parameter(Mandatory = $false)]
    [string]
    $RegistryName = "matheus06"
)
Begin {
    
}
Process {
    function Build-Docker-Images {

        # get GitHub credentials
        $GitHubPackagesCreds = & (Join-Path $PSScriptRoot './github-authentication.ps1' -Resolve)

        docker build  ../../microservice.pantrymanager/src/microservice.pantrymanager -t $RegistryName/pantry-manager:latest `
            --build-arg GITHUB_PACKAGES_USERNAME="$($GitHubPackagesCreds.GitHubUsername)" `
            --build-arg GITHUB_PACKAGES_TOKEN="$($GitHubPackagesCreds.GitHubToken)"
        docker build  ../../microservice.productmanager/src/microservice.productmanager -t $RegistryName/product-manager:latest `
            --build-arg GITHUB_PACKAGES_USERNAME="$($GitHubPackagesCreds.GitHubUsername)" `
            --build-arg GITHUB_PACKAGES_TOKEN="$($GitHubPackagesCreds.GitHubToken)"
        docker build  ../../microservice.recipemanager/src/microservice.recipemanager -t $RegistryName/recipe-manager:latest `
            --build-arg GITHUB_PACKAGES_USERNAME="$($GitHubPackagesCreds.GitHubUsername)" `
            --build-arg GITHUB_PACKAGES_TOKEN="$($GitHubPackagesCreds.GitHubToken)"
        docker build  ../../microservice.scheduler/src/microservice.scheduler -t $RegistryName/scheduler:latest `
            --build-arg GITHUB_PACKAGES_USERNAME="$($GitHubPackagesCreds.GitHubUsername)" `
            --build-arg GITHUB_PACKAGES_TOKEN="$($GitHubPackagesCreds.GitHubToken)"
        docker build  ../../microservice.identityserver/src/microservice.identityserver -t $RegistryName/identityserver:latest `
            --build-arg GITHUB_PACKAGES_USERNAME="$($GitHubPackagesCreds.GitHubUsername)" `
            --build-arg GITHUB_PACKAGES_TOKEN="$($GitHubPackagesCreds.GitHubToken)"
        docker build  ../../ui-pantry -t $RegistryName/ui-pantry:latest
        Write-Output "build done"
    }

    function Push-Docker-Images {
        docker push $RegistryName/pantry-manager:latest
        docker push $RegistryName/product-manager:latest
        docker push $RegistryName/pantry-manager:latest
        docker push $RegistryName/recipe-manager:latest
        docker push $RegistryName/scheduler:latest
        docker push $RegistryName/identityserver:latest
        docker push $RegistryName/ui-pantry:latest
        Write-Output "push done"
    }
    
    ########################
    # main execution point
    ########################
    Build-Docker-Images -Major $Major -Minor $Minor -Patch $Patch
    Push-Docker-Images -Major $Major -Minor $Minor -Patch $Patch

    Write-Output "work done"
}
End {
    
}