# sqlc-fs

sqlc-fs is a [sqlc plugin](https://docs.sqlc.dev/en/stable/guides/plugins.html) to generate queries that are compatible with [Npgsql.Fsharp](https://github.com/Zaid-Ajaj/Npgsql.FSharp).

This plugin is WIP and not usable yet.

## Installation & Usage

```bash
go install github.com/kyleconroy/sqlc/cmd/@latest
```

```json5
// sqlc.json
{
  "version": "2",
  "plugins": [
    {
      "name": "ts",
      "process": {
        "cmd": "sqlc-ts"
      }
    }
  ],
   "sql": [
    {
      "schema": "schema.sql",
      "queries": "query.sql",
      "engine": "sqlite",
      "codegen": [
        {
          "out": "gen",
          "plugin": "ts"
        }
      ]
    }
  ]
}
```

```build
go build -o bin/sqlc-ts 
```

See `testdata/` for a full example that can be run with:

```bash
sqlc generate -f ./testdata/sqlc.json && cat ./testdata/gen/query.sql.ts | less
```

## CREDIT

the plugin is based on [sqlc-ts](https://github.com/stephen/sqlc-ts), which is a very good starting point to write sqlc plugin.
