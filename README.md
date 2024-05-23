# Dinner

## Git Repository
git init
curl https://raw.githubusercontent.com/microsoft/dotnet/main/.gitignore -o .gitignore
git add 
git commit -m "first commit"
git branch -M main
git remote add origin https://github.com/Riniga/Dinner.git
git push -u origin main


## Dotnet 
dotnet new sln
dotnet new classlib -o Dinner.Library
func init Dinner.Api --worker-runtime dotnet-isolated --target-framework net8.0
dotnet sln add Dinner.Library/Dinner.Library.csproj
dotnet sln add Dinner.Api/Dinner_Api.csproj

cd Dinner.Library
dotnet add package Newtonsoft.Json
dotnet add package Microsoft.Azure.Cosmos
cd ..



cd Dinner.Api
func new --name DinnerRequestApi --template "HTTP trigger" --authlevel "anonymous"
cd ..


## Build and start
dotnet build
cd Dinner.Api
func start

