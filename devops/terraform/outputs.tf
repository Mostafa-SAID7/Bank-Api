# Outputs for Bank Management System Infrastructure

output "resource_group_name" {
  description = "Name of the created resource group"
  value       = azurerm_resource_group.bank_rg.name
}

output "aks_cluster_name" {
  description = "Name of the AKS cluster"
  value       = azurerm_kubernetes_cluster.bank_aks.name
}

output "aks_cluster_fqdn" {
  description = "FQDN of the AKS cluster"
  value       = azurerm_kubernetes_cluster.bank_aks.fqdn
}

output "acr_login_server" {
  description = "Login server for the Azure Container Registry"
  value       = azurerm_container_registry.bank_acr.login_server
}

output "sql_server_fqdn" {
  description = "Fully qualified domain name of the SQL server"
  value       = azurerm_mssql_server.bank_sql_server.fully_qualified_domain_name
}

output "key_vault_uri" {
  description = "URI of the Key Vault"
  value       = azurerm_key_vault.bank_kv.vault_uri
}

output "kube_config" {
  description = "Kubernetes configuration"
  value       = azurerm_kubernetes_cluster.bank_aks.kube_config_raw
  sensitive   = true
}