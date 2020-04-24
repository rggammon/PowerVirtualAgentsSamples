<template>
    <div>
        <h1>Complete Setup via Cloud Shell</h1>
        <p>
            Application registration into AzureAD must be completed via Azure PowerShell.
        </p>
        <p>
            If you don't have "Azure PowerShell" installed, you can use the Azure Cloud Shell button below.
        </p>
        <p>
            <a href="https://shell.azure.com" target="_blank">
                <img title="Launch Azure Cloud Shell" alt="Embed launch" src="https://shell.azure.com/images/launchcloudshell.png" data-linktype="external">
            </a>
        </p>
        <p>
            Then, run the following commands to register this application in Azure Active Directory:
            <div class="code-snippet">
                cd ~/clouddrive<br />
                Invoke-WebRequest {{ setupScript }} -OutFile Create-AppRegistration.ps1<br />
                .\Create-AppRegistration.ps1
            </div>
        </p>
    </div>
</template>

<script>
    module.exports = Setup;
</script>

<style>
</style>