#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "${BASH_SOURCE[0]}")/.."

dotnet restore practice/Practice.csproj
dotnet run --project practice/Practice.csproj -- init
printf '\nEnvironment ready. Run: dotnet run --project practice -- list\n'
