# first build production artifact:
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# all subsequent sections would be updated if this section changed
# update only if project config updated 
COPY ./src/MoneyService/*.csproj ./
RUN dotnet restore

# all subsequent sections would be updated if this section changed
# update only if project files
COPY ./src/MoneyService/ ./
RUN dotnet publish -c Release -o out

################################################################################################

# use to run server with artifact from build-env step:
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app

# update only if build-env output changed
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "MoneyService.dll" ]


