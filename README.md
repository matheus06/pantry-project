# Pantry Project

Local development for study purposes

* [Microservice PantryManager](/microservices/microservice.pantrymanager)
* [Microservice ProductManager](/microservices/microservice.productmanager)
* [Microservice RecipeManager](/microservices/microservice.recipemanager)
* [Microservice Scheduler](/microservices/microservice.scheduler)
* [UI Pantry](/microservices/ui-pantry)
* [Platform](/microservices/platform)

## Architecture

Local:

![architecture-local](/docs/arch-local.png)

K8s:

![architecture-k8s](/docs/arch-k8s.png)

## How To Run

### Requirements

* Docker
* Tye (for local DEBUG only)
* Dapr
* .Net 7
* SQL

### Run Locally using Tye

* Run SQLServer in container.

```console
   `docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=MyP@ssowrdDocker" -p 1433:1433 --name sqlserver --hostname sql -d mcr.microsoft.com/mssql/server:2022-latest`
```

* `winget install Dapr.CLI` => install dapr locally.
* `dapr init` => Initialize Dapr in your local environment.
* `build` the application.
* `tye run` => run the services using the .NET csproj files in order to DEBUG.

* Urls
  * UI => <http://localhost:4200>
  * Pantry API => <http://localhost:58798/>
  * Pantry API => <http://localhost:58798/swagger>
  * Product API => <http://localhost:40466/>
  * Product API => <http://localhost:40466/swagger>
  * Recipe API => <http://localhost:16681/>
  * Recipe API => <http://localhost:16681/swagger>
  * Scheduler API => <http://localhost:61524/>
  * Scheduler API => <http://localhost:61524/swagger>
  * Hangfire => <http://localhost:61524/hangfire>
  * HealthCheck => <http://localhost:58798/hc-ui>
  * Identity Configuration => <https://localhost:50001/.well-known/openid-configuration>

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

Apply components

```console
.\kubectl-apply-dapr-components.ps1     
```

Install redis

```console
helm repo add bitnami https://charts.bitnami.com/bitnami
helm repo update
helm install redis bitnami/redis --set image.tag=6.2
```

Copy redis secret from default namespace to pantry namespace:
  
`you first need to install yq => <https://github.com/mikefarah/yq/#install>`

```console
kubectl --namespace=default get secret redis -o yaml | yq 'del(.metadata.creationTimestamp, .metadata.uid, .metadata.resourceVersion, .metadata.namespace)' | kubectl apply --namespace=pantry -f -
```

* Using powershell to apply k8s manifest files

Apply Deployments

```console
.\kubectl-apply-all-deployments.ps1     
```

Delete Deployments

```console
 .\kubectl-delete-all-deployments.ps1    
```

* Using helm to install app

Install app

```console
helm upgrade --install pantry-app .  --namespace=pantry    
```

Uninstall app

```console
helm uninstall pantry-app . --namespace=pantry     
```

* k8s Urls ()
  * UI => <https://localdev-tls.me/>
  * Pantry API => <https://localdev-tls.me/api-pantry/>
  * Pantry API Docs => <https://localdev-tls.me/api-pantry/swagger>
  * Product API => <https://localdev-tls.me/api-product/>
  * Product API Docs => <https://localdev-tls.me/api-product/swagger>
  * Recipe API => <https://localdev-tls.me/api-recipe/>
  * Recipe API Docs=> <https://localdev-tls.me/api-recipe/swagger>
  * Scheduler API => <https://localdev-tls.me/api-scheduler/>
  * Scheduler API Docs => <https://localdev-tls.me/api-scheduler/swagger>
  * Hangfire => <https://localdev-tls.me/api-scheduler/hangfire>
  * HealthCheck => <https://localdev-tls.me/api-pantry/hc-ui>
  * Identity Configuration =>  <https://localdev-tls.me/api-identity/.well-known/openid-configuration>
  