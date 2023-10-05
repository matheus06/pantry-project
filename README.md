# Pantry Project

* [Microservice PantryManager](/microservices/microservice.pantrymanager)
* [Microservice ProductManager](/microservices/microservice.productmanager)
* [Microservice RecipeManager](/microservices/microservice.recipemanager)
* [Microservice Scheduler](/microservices/microservice.scheduler)
* [UI Pantry](/microservices/ui-pantry)
* [Platform](/microservices/platform)

## Architeture

Local:

![architeture](/docs/arch.png)

K8s:

![architeture](/docs/arch-k8s.png)

## How To Run

### Requirements

* Docker
* Tye (for DEBUG only)
* Dapr
* .Net 7
* SQL

### Run Locally using Docker-Compose or Tye

* Run SQLServer in container.
  * `docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=MyP@ssowrdDocker" -p 1433:1433 --name sqlserver --hostname sql -d mcr.microsoft.com/mssql/server:2022-latest`
* `docker-compose up` to run all services in Docker.
* `tye run` to run the services using the .NET csproj files in order to DEBUG.

* Urls
  * UI => <http://localhost:4200>

### Run in local k8s cluster

* Requirements for k8s cluster

If you dont have yet please run SQLServer in container.

```console
   `docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=MyP@ssowrdDocker" -p 1433:1433 --name sqlserver --hostname sql -d mcr.microsoft.com/mssql/server:2022-latest`
```

Install ngnix

```console
helm upgrade --install ingress-nginx ingress-nginx  --repo https://kubernetes.github.io/ingress-nginx   --namespace ingress-nginx --create-namespace
```

Install dapr

```console
dapr init -k
```

Install dapr dashboard

```console
helm repo add dapr https://dapr.github.io/helm-charts/
helm repo update
helm install dapr-dashboard dapr/dapr-dashboard

dapr dashboard -k
```

<https://docs.dapr.io/getting-started/tutorials/configure-state-pubsub/>

Copy redis secret from default namespace to pantry namespace (you need first to install yq => <https://github.com/mikefarah/yq/#install>)

```console
kubectl --namespace=default get secret redis -o yaml | yq 'del(.metadata.creationTimestamp, .metadata.uid, .metadata.resourceVersion, .metadata.namespace)' | kubectl apply --namespace=pantry -f -
```

* Run using powershell to apply k8s manifest files

Generate images on a repository

```console
.\docker-build-and-push-all.ps1    
```

Apply Deployments (please update files [k8s/files] using the correct container registry)

```console
.\kubectl-apply-all-deployments.ps1     
```

Delete Deployments (please update files [k8s/files] using the correct container registry)

```console
 .\kubectl-delete-all-deployments.ps1    
```

* Urls
  * UI => <http://pantrymanager.localdev.me/>
  * Pantry API => <http://pantrymanager-pantry-api.localdev.me/>
  * Product API => <http://pantrymanager-product-api.localdev.me/>
  * Recipe API => <http://pantrymanager-recipe-api.localdev.me/>
  * Scheduler API => <http://pantrymanager-scheduler-api.localdev.me>
  * Hangfire => <http://pantrymanager-scheduler-api.localdev.me/hangfire>
  * HealthCheck => <http://pantrymanager-pantry-api.localdev.me/hc-ui>

## Technical Concepts

* [Technical Concepts](/docs/Technical.md)
