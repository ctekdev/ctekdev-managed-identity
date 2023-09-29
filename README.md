# CtekDev-ManagedIdentity
In the event application settings for an Azure app service do not provide enough security, Azure key vault provides an alternative to storing secrets and connection strings. The implementation is easy to set up in .NET. However, testing in a local environment versus production does come with some challenges.  
Inside the KeyVault service, code similar to the line below is all that is needed to connect and return secrets from KeyVault:
var _client = new SecretClient(new Uri(_kvUrl), new DefaultAzureCredential(), options);

From this point on it’s as easy as calling the next line to return a secret.

var secret = await _client.GetSecretAsync(“secret_key”);

The challenge for coders comes with instantiating the DefaultAzureCredential() class. When implemented the DefaultAzureCredential class kicks off a series of credential checks, one of which is looking for a managed identity. Which in this case, running an app service, is as easy as setting up a managed identity inside the Azure portal and giving it permissions to access KeyVault. 

When running locally, a few problems could occur which make it difficult for a developer to access KeyVault. One solution to overcome this is the use of environment variables. In this scenario, all that is needed is for an application to be registered in AAD and given the right permissions to access KeyVault. Then the app registration ids can be stored in the appsettings.json or local user secrets and read into configuration on startup.

Setting up an App Service Managed Identity in the Azure portal.

1)	Inside the portal click on Identity in the Settings section. In the system assigned tab set the Status option to On. Copy the Object (principal) ID. 

2)	Next open the KeyVault service inside Azure and choose Access Policies from the left menu. NOTE: Due to routine updates Azure may change the way Access Policies are configured. Therefore, the various steps to get to the right configuration page may change. In short, you need to choose Access Policy options that allow you to create policies for managed identities, which may differ from RBAC or user permissions. With the correct options selected, the user should be able to choose which permissions (such as get, create, list etc.) will apply. Afterwards search for a service principal by pasting the Object Id given when creating a managed identity in step 1 and select the application that will appear in the search results. Save changes and that is all that is needed. When application is published to this app service, the DefaultAzureCredentials class will give the program access to KeyVault. 

Setting up a development environment to run locally.

1)	Register an application in Azure Active Directory as normal. 
2)	Copy the TenantId and the ClientId associated with the new app registration and paste to appsettings.json or inside user secret manager within Visual Studio. 
3)	On the new app registration overview, choose the option to create a new secret. Copy the secret value and add to configuration inside VS as in previous step. See the example below.

{
  "EnvVariables": {
    "TenantId": "<GUID>",
    "ClientId": "<GUID>",
    "ClientSecret": “your_secret”
  }
}

4)	You’ll follow the same steps in setting up an Access Policy inside KeyVault as in the first section – setting up a managed identity. EXCEPT when assigning permissions to a service principal search by using the Client (Application) Id (or you could type in the name of the app registration just created).
5)	Back in Visual Studio, you can read the credentials recently added to appsettings.json and store them as environment variables. When running locally this should give access to KeyVault just as a managed identity would in production.  


  




