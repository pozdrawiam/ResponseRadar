@echo off

SET migrationName=%1

IF "%migrationName%"=="" (
    echo Please provide a migration name.
    exit /b
)

SET dbProjectPath=../Rr.Core
SET startupProjectPath=../Rr.Web
SET dbContextName=Db
SET migrationsDir=Data/Migrations

dotnet ef migrations add %migrationName% -p %dbProjectPath% -s %startupProjectPath% -c %dbContextName% -o %migrationsDir%
