#Script allows to deploy ARM template from local machine
#Use only if Build definition is not available from VSTS level
Connect-AzureRmAccount
Select-AzureRmSubscription -TenantId 'a935ca34-0dd7-47e7-aee5-878c6cf7b505'
Get-AzureRmResourceGroup -Name 'JobOfferApplication'
New-AzureRmResourceGroupDeployment -Name 'JobOfferDeployment' -ResourceGroupName 'JobOfferApplication' -TemplateFile azureTemplate.json -TemplateParameterFile azureTemplateParameters.json

#Make sure that you are using newest version of the repo with correct branch
#Make sure that paths to files are up to date

#Local deployment from local files
#New-AzureRmResourceGroupDeployment -ResourceGroupName JobOfferApplication  `
# -TemplateFile azureTemplate.json
# -TemplateParameterFile azureTemplateParameters.json

 