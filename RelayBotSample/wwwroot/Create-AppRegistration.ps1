#Requires -Version 6

Param(
	[string] $resourceGroupName 
)

$ErrorActionPreference = "Stop"

$webApp = Get-AzWebApp -ResourceGroupName $resourceGroupName
$webApp = Get-AzWebApp -ResourceGroupName $resourceGroupName -Name $webApp.Name

$appUrl = "https://" + $webApp.DefaultHostName

# AppId's can be looked up via:
# $sp = Get-AzureADServicePrincipal -Filter "AppId eq '00000007-0000-0000-c000-000000000000'"
$graphAppId = "00000003-0000-0000-c000-000000000000"
$armAppId = "797f4846-ba00-4fd7-ba43-dac1f8f63013"
$cdsAppId = "00000007-0000-0000-c000-000000000000"

# Permissions then are under $sp.Oauth2Permissions
$graphUserRead = "e1fe6dd8-ba31-4d61-89e7-88639da4683d"
$armUserImpersonation = "41094075-9dad-400e-a0bd-54e686782033"
$cdsUserImpersonation = "78ce3f0f-a1ce-49c2-8cde-64b5c0896db4"

function BuildRequiredResourceAccess ($appId, $scopeId) {
    $resourceAccess = New-Object Microsoft.Open.AzureAD.Model.ResourceAccess
    $resourceAccess.Id = $scopeId
    $resourceAccess.Type = "Scope"

	$requiredResourceAccess = New-Object Microsoft.Open.AzureAD.Model.RequiredResourceAccess
	$requiredResourceAccess.ResourceAppId = $appId
	$requiredResourceAccess.ResourceAccess = @($resourceAccess)

	return $requiredResourceAccess
}

$graphAccess = BuildRequiredResourceAccess $graphAppId $graphUserRead
$armAccess = BuildRequiredResourceAccess $armAppId $armUserImpersonation
$cdsAccess = BuildRequiredResourceAccess $cdsAppId $cdsUserImpersonation

$aadApp = New-AzureADApplication `
	-DisplayName $resourceGroupName `
	-Homepage $appUrl `
	-RequiredResourceAccess @($graphAccess, $armAccess, $cdsAccess) `
	-ReplyUrls @($appUrl)

$password = New-AzureADApplicationPasswordCredential -ObjectId $aadApp.ObjectId

$appSettings = @{
	"MicrosoftAppId" = $aadApp.AppId;
	"MicrosoftAppPassword" = $password.Value;
}

$webApp.SiteConfig.AppSettings | ForEach-Object {
	$appSettings[$_.Name] = $_.Value
}

$webApp = Set-AzWebApp `
	-ResourceGroupName $resourceGroupName `
	-Name $webApp.Name `
	-AppSettings $appSettings