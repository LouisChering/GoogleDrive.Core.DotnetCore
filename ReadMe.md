# Google Drive .net core library

## Description
This is a dotnet core class library that exposes the core classes,services and drivers required for my other GoogleDrive integration projects.  This project is crossplatform as long as the target platform is running at least .net core 2.2.

## Usage
Reference this project's output .dll using the build commands below or alternatively use this project as a git submodule:
```
git submodule add https://github.com/LouisChering/GoogleDrive.Core.DotnetCore.git submodules/GoogleDrive.Core.DotnetCore
```
Then update your .csproj file to include a reference:
```
  <ItemGroup>
    <ProjectReference Include="submodules/drivedotnetcore/DriveDotNet.csproj" />
  </ItemGroup>
```

## Build instructions

Build for your current platform based on your .net core sdk:
```
dotnet build
```

To target another runtime use:
```
dotnet build --runtime ubuntu.18.04-x64=
```

## Testing
This project does not contain tests as it only acts as a class library to wrap Google SDK api's.
