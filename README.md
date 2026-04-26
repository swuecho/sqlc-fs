# sqlc-fs

sqlc-fs is a [sqlc plugin](https://docs.sqlc.dev/en/stable/guides/plugins.html) to generate queries that are compatible with [Npgsql.Fsharp](https://github.com/Zaid-Ajaj/Npgsql.FSharp).

This plugin is in alpha stage.

check <https://github.com/swuecho/sqlc-fs/tree/main/testdata> for demo.

## Supported sqlc features

sqlc-fs currently targets PostgreSQL and emits F# modules that use Npgsql and Npgsql.FSharp.

Supported query annotations:

- `:one`
- `:many`
- `:exec`
- `:execrows`
- `:execresult`
- `:copyfrom`
- `:batchexec`, `:batchmany`, and `:batchone` (via `NpgsqlBatch` / `NpgsqlBatchCommand`; row reading uses `RowReader` from Npgsql.FSharp)

Unsupported query annotations fail generation with a clear error instead of producing an empty function. This currently includes `:execlastid`.

Batch methods are synchronous only (`emit_async_code` does not yet emit async batch variants).

The plugin supports sqlc `rename` settings and the plugin options shown below. Other sqlc generator features, such as Go type overrides, enum helper generation, exact table name settings, and MySQL/SQLite-specific generation are not implemented for the F# generator.

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
          "plugin": "fs",
          "options": {
            // options with default value
            // "emit_async_code": false,
            // "emit_auto_open_model": true,
            // "emit_model_file_name": "model_from_schema.fs",
            // "emit_model_name": "ModelFromSchema",
          }
        }
      ]
    }
  ]
```

## Dev & Contribution

```build
go build -o bin/sqlc-fs
```

See `testdata/` for a full example that can be run with:

```bash
sqlc generate -f ./testdata/sqlc.json
```

## CREDIT

the plugin is based on [sqlc-ts](https://github.com/stephen/sqlc-ts), which is a very good starting point to write sqlc plugin.
