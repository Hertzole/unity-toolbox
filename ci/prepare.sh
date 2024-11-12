#!/bin/bash

# Check if nextVersion is provided
if [ -z "$1" ]; then
    echo "Usage: $0 <nextVersion>"
    exit 1
fi

nextVersion=$1

echo "Updating version to $nextVersion"

# Echo all found files
echo "Found .csproj files:"
find . -type f -name "*.csproj"

# Find and replace Version, AssemblyVersion, and ProductVersion in all .csproj files
find . -type f -name "*.csproj" -exec sed -i -e "s/<Version>.*<\/Version>/<Version>${nextVersion}<\/Version>/; s/<AssemblyVersion>.*<\/AssemblyVersion>/<AssemblyVersion>${nextVersion}<\/AssemblyVersion>/; s/<ProductVersion>.*<\/ProductVersion>/<ProductVersion>${nextVersion}<\/ProductVersion>/" {} +

echo "Version updated to $nextVersion in all .csproj files."

# Find all files called AssemblyInfo.cs and update AssemblyVersion and AssemblyFileVersion
find . -type f -name "AssemblyInfo.cs" -exec sed -i -e "s/AssemblyVersion(\".*\")/AssemblyVersion(\"${nextVersion}\")/; s/AssemblyFileVersion(\".*\")/AssemblyFileVersion(\"${nextVersion}\")/" {} +

echo "Version updated to $nextVersion in all AssemblyInfo.cs files."