param (
    [Parameter(Mandatory = $false)]
    [string]
    $RegistryName = "matheus06"
)
Begin {
    
}
Process {
    function Build-Docker-Images {
        docker build  ../../microservice.pantrymanager/src/microservice.pantrymanager -t $RegistryName/pantry-manager:latest
        docker build  ../../microservice.productmanager/src/microservice.productmanager -t $RegistryName/product-manager:latest
        docker build  ../../microservice.recipemanager/src/microservice.recipemanager -t $RegistryName/recipe-manager:latest
        docker build  ../../microservice.scheduler/src/microservice.scheduler -t $RegistryName/scheduler:latest
        docker build  ../../microservice.identityserver/src/microservice.identityserver -t $RegistryName/identityserver:latest
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