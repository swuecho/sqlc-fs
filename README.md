# sqlc-fs

sqlc-fs is a [sqlc plugin](https://docs.sqlc.dev/en/stable/guides/plugins.html) to generate queries that are compatible with [Npgsql.Fsharp](https://github.com/Zaid-Ajaj/Npgsql.FSharp).

This plugin is in alpha stage.

check https://github.com/swuecho/sqlc-fs/tree/main/testdata for demo.

## Installation & Usage

```bash
go install github.com/swuecho/sqlc-fs@latest
```

```json5
// sqlc.json
{
  "version": "2",
  "plugins": [
    {
      "name": "fs",
      "process": {
        "cmd": "sqlc-fs"
      }
    }
  ],
   "sql": [
    {
      "schema": "schema.sql",
      "queries": "query/",
      "engine": "postgresql",
      "codegen": [
        {
          "out": "gen",
          "plugin": "fs"
        }
      ]
    }
  ]
```

## Dev & Contribution

```build
go build -o bin/sqlc-ts 
```

See `testdata/` for a full example that can be run with:

```bash
sqlc generate -f ./testdata/sqlc.json
```

## CREDIT

the plugin is based on [sqlc-ts](https://github.com/stephen/sqlc-ts), which is a very good starting point to write sqlc plugin.
