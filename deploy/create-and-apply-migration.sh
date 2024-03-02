#!/bin/bash
set -e

if [ -z "$1" ]; then
  echo "WARNING: Migration name is missing."
  echo "Usage: $0 <migration-name>"
  exit 1
fi

echo Creating migration \"$1\"...
cd MoneyFlowTracker.Infrastructure
dotnet ef -s ../MoneyFlowTracker.Api/ migrations add $1 -o ./Data/Migrations

echo Updating DB...
dotnet ef database update

echo All migrations are applied
cd ..