# Swedish BankID .NET Core
Swedish BankID implementation written for .NET Core. This version does support the new BankID JSON interface.

Currently containing the initial (working) commit, this can be run in production if you just configure the certificates. For now you'll need to reply on my previous blog post (http://www.herlitz.nu/2017/09/13/integrating-with-swedish-bankid-and-.net/) to understand the process.

What you get in the current project is an API that can talk to the BankID API. Any GUI needs to be written by yourself. Swagger is included. The default url is http://localhost:5100/swagger if you run the code as is.

**Todo**
* ~~Publish docker sample~~
* Publish the tests
* Publish the documentation
* Publish some howtos
* Publish some example (standard) GUI's
* Add Azure Key Vault support
* Publish NuGet packages

## Docker and Docker Compose

The default url when using docker is https://localhost:8443/swagger/

**Build and run using docker compose**

`docker-compose up -d`

**Simply build using docker compose**

`docker-compose build`

And run using docker (example for PowerShell)
```
// replace {password}
docker run --name bankid `
	-p 8443:443 `
	-e ASPNETCORE_Kestrel__Certificates__Default__Password="{password}" `
	-e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx `
	-e ASPNETCORE_URLS=https://+:443 `
	-e ASPNETCORE_ENVIRONMENT=Development `
	-v $home\.aspnet\https:/https/:ro `
	-dit herlitz/bankid:latest
```
Please note that to run https in docker for development you need to fix the developer certificate first
```
// This is an example, replace {password}
dotnet dev-certs https --clean
dotnet dev-certs https -ep $home\.aspnet\https\aspnetapp.pfx -p {password}
dotnet dev-certs https --trust
```
For more information https://docs.microsoft.com/en-us/aspnet/core/security/docker-compose-https?view=aspnetcore-3.1

