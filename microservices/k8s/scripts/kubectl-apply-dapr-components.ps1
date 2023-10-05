Begin {
    
}

Process {

    function Invoke-All-Deployments {
        # dapr components
        kubectl apply -f ../components/redis-pubsub-default.yaml
        kubectl apply -f ../components/redis-state-default.yaml
        kubectl apply -f ../components/redis-pubsub.yaml
        kubectl apply -f ../components/redis-state.yaml
        Write-Output "apply done"

        kubectl get components --namespace=pantry
        kubectl get components --namespace=default
    }
    
    ######################
    # main execution point
    ######################
    Invoke-All-Deployments 

    Write-Output "work done"
}
End {
    
}