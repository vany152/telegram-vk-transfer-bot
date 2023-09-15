FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TelegramVkTransferBot/TelegramVkTransferBot.csproj", "TelegramVkTransferBot/"]
RUN dotnet restore "TelegramVkTransferBot/TelegramVkTransferBot.csproj"
COPY . .
WORKDIR "/src/TelegramVkTransferBot"
RUN dotnet build "TelegramVkTransferBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TelegramVkTransferBot.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TelegramVkTransferBot.dll"]
