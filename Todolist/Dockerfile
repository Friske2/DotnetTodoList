FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
# Copy the project file and restore dependencies
COPY ["Todolist.csproj", "./"]
RUN dotnet restore "Todolist.csproj"

# Copy the rest of the source code
COPY . .
WORKDIR "/src"
RUN dotnet build "Todolist.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Todolist.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Todolist.dll"]
