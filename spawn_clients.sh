#!/bin/bash

RESOURCE_GROUP="badgerclan-client-rg-auto"
LOCATION="westus"
IMAGE="tjwalkr3/badgerclan-client"
PORT=5217
REGISTRY_SERVER="index.docker.io"
REGISTRY_USER="tjwalkr3"
REGISTRY_PASS="dckr_pat_JD26NLaK0hy8F624Uj9cIl2KzSs"

# Create resource group
az group create --name $RESOURCE_GROUP --location $LOCATION -o none

# Ask user for the number of clients to generate
read -p "Enter the number of clients to generate: " CLIENT_COUNT

# Generate clients
for ((i=1; i<=CLIENT_COUNT; i++))
do
    CLIENT_NAME="thomas-client-auto$i"
    DNS_LABEL="$CLIENT_NAME"
    echo "Creating client: $CLIENT_NAME"
    
    az container create \
      --resource-group $RESOURCE_GROUP \
      --name $CLIENT_NAME \
      --image $IMAGE \
      --dns-name-label $DNS_LABEL \
      --ports $PORT \
      --os-type Linux \
      --cpu 1 \
      --memory 1.5 \
      --registry-login-server $REGISTRY_SERVER \
      --registry-username $REGISTRY_USER \
      --registry-password $REGISTRY_PASS \
      --output none
    
    echo "Client $CLIENT_NAME created at: http://$DNS_LABEL.$LOCATION.azurecontainer.io:$PORT"
done

# Allow user to delete the resource group
read -p "Press 'q' to delete the resource group, or any other key to exit: " DELETE_CONFIRM
if [ "$DELETE_CONFIRM" == "q" ]; then
    echo "Deleting resource group $RESOURCE_GROUP..."
    az group delete --name $RESOURCE_GROUP --yes --no-wait
fi
