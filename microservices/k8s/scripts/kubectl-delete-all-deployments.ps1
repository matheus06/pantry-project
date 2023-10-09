Begin {
    
}

Process {

    function Remove-Deployments {
        kubectl delete -f ../files/pantry-ingress-service.yaml
        kubectl delete -f ../files/pantry-ingress-service-no-rewrite.yaml
        kubectl delete -f ../files/pantry-manager-deployment.yaml
        kubectl delete -f ../files/product-manager-deployment.yaml
        kubectl delete -f ../files/recipe-manager-deployment.yaml
        kubectl delete -f ../files/scheduler-deployment.yaml
        kubectl delete -f ../files/identityserver-deployment.yaml
        kubectl delete -f ../files/ui-pantry-deployment.yaml

        # dapr components
        kubectl delete -f ../components/redis-pubsub-default.yaml
        kubectl delete -f ../components/redis-state-default.yaml
        kubectl delete -f ../components/redis-pubsub.yaml
        kubectl delete -f ../components/redis-state.yaml
        Write-Output "remove done"
 
        kubectl get pods --namespace=pantry 
        kubectl get services --namespace=pantry
        kubectl get deployments --namespace=pantry
        kubectl get replicaset --namespace=pantry
        kubectl get ingress --namespace=pantry
        kubectl get components --namespace=pantry
        kubectl get components --namespace=default
    }
    
    ######################
    # main execution point
    ######################
    Remove-Deployments 

    Write-Output "work done"
}
End {
    
}