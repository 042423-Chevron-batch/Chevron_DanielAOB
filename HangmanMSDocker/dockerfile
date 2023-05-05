FROM mcr.microsoft.com/dotnet/nightly/sdk:7.0 as Builds

WORKDIR /app

COPY . /app/

#COPY Program.cs .
#COPY Hangman.csproj .
RUN dotnet publish -o Hangman

FROM mcr.microsoft.com/dotnet/aspnet:7.0 As Run

WORKDIR /app

COPY --from=Builds /app/Hangman/ .

CMD ["dotnet", "Hangman.dll"]
