# cashflow-micro
Microservices for Cashflow Project

Running on Kubernetes
Using Event Driven Architecture

kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.0.0/deploy/static/provider/cloud/deploy.yaml

kubectl describe validatingwebhookconfigurations ingress-nginx-admission

kubectl delete -A ValidatingWebhookConfiguration ingress-nginx-admission

kubectl get svc -n ingress-nginx

kubectl config set-context --current --namespace=ingress-nginx


kubectl create secret generic mssql --from-literal SA_PASSWORD=4h5J3k4h5kJ3h5k3j!
kubectl create secret generic secret-shared --from-file=./shared.secrets.json


Event bus uses - versioned events to determine the order
Order mismatch - moves event back to queue for further processing

