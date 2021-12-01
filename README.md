# PoC using Vault


Use "docker-compose up -d" to start in "datacenter-deploy-auto-config"
Login to http://localhost:8200 create a secret named "consoleApp"

## Policy

```text
path "secret/data/consoleApp" {capabilities = ["read"]}

path "transit/encrypt/myKey" {capabilities = [ "update"]}

path "transit/decrypt/myKey" {capabilities = [ "update"]}
```


https://learn.hashicorp.com/tutorials/consul/docker-compose-auto-config