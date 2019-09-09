FROM mcr.microsoft.com/dotnet/core/runtime:2.1-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["AuthorizeTransaction/AuthorizeTransaction.csproj", "AuthorizeTransaction/"]
RUN dotnet restore "AuthorizeTransaction/AuthorizeTransaction.csproj"
COPY . .
WORKDIR "/src/AuthorizeTransaction"
RUN dotnet build "AuthorizeTransaction.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "AuthorizeTransaction.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AuthorizeTransaction.dll"]