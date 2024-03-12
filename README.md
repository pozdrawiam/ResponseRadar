# ResponseRadar

Http requests monitoring for service uptime checks and diagnostics.  
Simple *ASP.NET Core RazorPages* web app.

## Features

- url list to monitor (sqlite db)
- push notifications via [ntfy.sh](https://ntfy.sh)

## Branches

- develop: current changes
- main: stable, tested version

## Dependencies

- .Net 8
- EntityFrameworkCore.Sqlite
- frontend: Bootstrap 5.3
- testing: xUnit, NSubstitute

## Configuration

Setup unique notifications topic in `appsettings.json`:

```json
{
  "Culture": "en",
  "DbConnection": "Data Source=ResponseRadar.db",
  "NtfyUrl": "https://ntfy.sh",
  "NtfyTopic": "your-unique-topic-name",
  "IntervalMinutes": 10,
  "TimeoutSeconds": 60,
  "...": "..."
}
```

## Run from source
```
dotnet run --project Rr.Web
```

## Publish
```
dotnet publish Rr.Web -c Release
```
or use build script from _scripts_ directory.
