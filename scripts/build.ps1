$ErrorActionPreference = "Stop"

$rootDir = "."
$project = "$rootDir/PoloTweaks/PoloTweaks.csproj"

$buildDir = "$rootDir/PoloTweaks/bin/Release/net462"
$outDir = "$rootDir/out"

# Clean up everything
if (Test-Path $buildDir) {
  Remove-Item $buildDir -Recurse -Force
}

if (Test-Path $outDir) {
  Remove-Item $outDir -Recurse -Force
}

New-Item $buildDir -ItemType Directory -Force
New-Item $outDir -ItemType Directory -Force

dotnet clean "$project" --configuration Release
dotnet build "$project" --configuration Release

$version = (Get-Item "$buildDir/PoloTweaks.dll").VersionInfo.ProductVersion
$version = $version -replace '\+.*$'
Compress-Archive -Path "$buildDir/*" -DestinationPath "$outDir/plugin.zip" -Force
