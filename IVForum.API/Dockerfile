FROM microsoft/aspnetcore-build:2.0 AS build-env
ADD . /app
WORKDIR /app
ENTRYPOINT ["dotnet", "IVForum.API.dll"]