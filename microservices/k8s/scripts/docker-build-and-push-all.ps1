param (
    [Parameter(Mandatory = $false)]
    [string]
    $RegistryName = "matheus06",
    [string]$Context = "../../../microservices"
)
Begin {
    
}
Process {
    function Build-Docker-Images {

        # get GitHub credentials
        $GitHubPackagesCreds = & (Join-Path $PSScriptRoot './github-authentication.ps1' -Resolve)
        
        docker build  -f ../../microservice.pantrymanager/src/microservice.pantrymanager/Dockerfile $Context -t $RegistryName/pantry-manager:latest `
            --build-arg GITHUB_PACKAGES_USERNAME="$($GitHubPackagesCreds.GitHubUsername)" `
            --build-arg GITHUB_PACKAGES_TOKEN="$($GitHubPackagesCreds.GitHubToken)"

        docker build  -f ../../microservice.productmanager/src/microservice.productmanager/Dockerfile $Context -t $RegistryName/product-manager:latest `
            --build-arg GITHUB_PACKAGES_USERNAME="$($GitHubPackagesCreds.GitHubUsername)" `
            --build-arg GITHUB_PACKAGES_TOKEN="$($GitHubPackagesCreds.GitHubToken)"

        docker build  -f ../../microservice.recipemanager/src/microservice.recipemanager/Dockerfile $Context  -t $RegistryName/recipe-manager:latest `
            --build-arg GITHUB_PACKAGES_USERNAME="$($GitHubPackagesCreds.GitHubUsername)" `
            --build-arg GITHUB_PACKAGES_TOKEN="$($GitHubPackagesCreds.GitHubToken)"

        docker build  -f ../../microservice.scheduler/src/microservice.scheduler/Dockerfile $Context -t $RegistryName/scheduler:latest `
            --build-arg GITHUB_PACKAGES_USERNAME="$($GitHubPackagesCreds.GitHubUsername)" `
            --build-arg GITHUB_PACKAGES_TOKEN="$($GitHubPackagesCreds.GitHubToken)"

        docker build  -f ../../microservice.identityserver/src/microservice.identityserver/Dockerfile  $Context -t $RegistryName/identityserver:latest `
            --build-arg GITHUB_PACKAGES_USERNAME="$($GitHubPackagesCreds.GitHubUsername)" `
            --build-arg GITHUB_PACKAGES_TOKEN="$($GitHubPackagesCreds.GitHubToken)"

        docker build  ../../ui-pantry -t $RegistryName/ui-pantry:latest `
        --build-arg ENVIRONMENT="production" 
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