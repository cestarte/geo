# How this project was initialized

## Create folder, solution, projects

```
mkdir Geo
cd Geo
dotnet new classlib -n Geo
dotnet new console

dotnet new classlib -n Geo -f net8.0
dotnet new console -n Example -f net8.0

dotnet sln add Geo/Geo.csproj
dotnet sln add Example/Example.csproj

dotnet add Example/Example.csproj reference Geo/Geo.csproj
```

## Create important files

```
touch README.md
touch .gitignore
```

## Make sure it builds and runs

```
dotnet build
dotnet run --project Example/Example.csproj
```

## Initialize git

```
git init
git remote add origin https://github.com/cestarte/geo.git
```

Check status / ensure .gitignore is set up correct.

```
git status
```

Everything look good? Send it up.
```
git add .
git commit -m "Initial commit"
git push -u origin main
```
