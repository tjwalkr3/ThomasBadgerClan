#!/bin/bash

RESOURCE_GROUP="thomas-badgerclan-grpc"
LOCATION="westus"
IMAGE="tjwalkr3/badgerclan-client"
REGISTRY_SERVER="index.docker.io"
REGISTRY_USER="tjwalkr3"
GRPC_PORT="5000"
HTTP_PORT="5001"
REGISTRY_PASS="dckr_pat_VzQLAIUDYuZM7LKuG-yz6X2Sa_Q"

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
      --os-type Linux \
      --cpu 1 \
      --memory 1.5 \
      --registry-login-server $REGISTRY_SERVER \
      --registry-username $REGISTRY_USER \
      --registry-password $REGISTRY_PASS \
      --ports $GRPC_PORT $HTTP_PORT \
      --environment-variables \
          "KESTREL__ENDPOINTS__GRPC__URL=http://0.0.0.0:$GRPC_PORT" \
          "KESTREL__ENDPOINTS__GRPC__PROTOCOLS=Http2" \
          "KESTREL__ENDPOINTS__WEBAPI__URL=http://0.0.0.0:$HTTP_PORT" \
          "KESTREL__ENDPOINTS__WEBAPI__PROTOCOLS=Http1" \
      --output none
    
    echo "    gRPC= http://$DNS_LABEL.$LOCATION.azurecontainer.io:$GRPC_PORT"
    echo "    HTTP= http://$DNS_LABEL.$LOCATION.azurecontainer.io:$HTTP_PORT"
done

# Allow user to delete the resource group
read -p "Press 'q' to delete the resource group, or any other key to exit: " DELETE_CONFIRM
if [ "$DELETE_CONFIRM" == "q" ]; then
    echo "Deleting resource group $RESOURCE_GROUP..."
    az group delete --name $RESOURCE_GROUP --yes --no-wait
fi
