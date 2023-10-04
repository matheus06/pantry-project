Begin {
    
}

Process {

    function Invoke-All-Deployments {
        kubectl apply -f ../files/pantry-namespace.yaml
        kubectl apply -f ../files/pantry-ingress-service.yaml
        kubectl apply -f ../files/pantry-manager-deployment.yaml
        kubectl apply -f ../files/product-manager-deployment.yaml
        kubectl apply -f ../files/recipe-manager-deployment.yaml
        kubectl apply -f ../files/scheduler-deployment.yaml
        kubectl apply -f ../files/identityserver-deployment.yaml
        kubectl apply -f ../files/ui-pantry-deployment.yaml
        Write-Output "apply done"
 
        kubectl get pods --namespace=pantry 
        kubectl get services --namespace=pantry
        kubectl get deployments --namespace=pantry
        kubectl get replicaset --namespace=pantry
        kubectl get ingress --namespace=pantry
    }
    
    ######################
    # main execution point
    ######################
    Invoke-All-Deployments 

    Write-Output "work done"
}
End {
    
}