FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Ps.Ecomm.ReportService/Ps.Ecomm.ReportService.csproj", "Ps.Ecomm.ReportService/"]
RUN dotnet restore "Ps.Ecomm.ReportService/Ps.Ecomm.ReportService.csproj"
COPY . .
WORKDIR "/src/Ps.Ecomm.ReportService"
RUN dotnet build "Ps.Ecomm.ReportService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ps.Ecomm.ReportService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ps.Ecomm.ReportService.dll"]
