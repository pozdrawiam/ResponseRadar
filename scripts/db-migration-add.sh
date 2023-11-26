#!/bin/bash

migrationName=$1
if [ -z "$migrationName" ]; then
    echo "Please provide a migration name."
    exit 1
fi

dbProjectPath="../Rr.Core"
startupProjectPath="../Rr.Web"
dbContextName="Db"
migrationsDir="Data/Migrations"

dotnet ef migrations add $migrationName -p $dbProjectPath -s $startupProjectPath -c $dbContextName -o $migrationsDir
