package main

import (
	"fmt"
	"strings"

	"github.com/swuecho/sqlc-fs/internal/plugin"
	"github.com/swuecho/sqlc-fs/internal/sdk"
)

func fsType(req *plugin.CodeGenRequest, col *plugin.Column) string {
	typ := postgresType(req, col)
	if !col.NotNull {
		typ = fmt.Sprintf("%s option", typ)
	}

	if col.IsArray {
		typ = fmt.Sprintf("%s[]", typ)
	}

	return typ
}

func parseIdentifierString(name string) (*plugin.Identifier, error) {
	parts := strings.Split(name, ".")
	switch len(parts) {
	case 1:
		return &plugin.Identifier{
			Name: parts[0],
		}, nil
	case 2:
		return &plugin.Identifier{
			Schema: parts[0],
			Name:   parts[1],
		}, nil
	case 3:
		return &plugin.Identifier{
			Catalog: parts[0],
			Schema:  parts[1],
			Name:    parts[2],
		}, nil
	default:
		return nil, fmt.Errorf("invalid name: %s", name)
	}
}

func postgresType(req *plugin.CodeGenRequest, col *plugin.Column) string {
	columnType := sdk.DataType(col.Type)
	notNull := col.NotNull || col.IsArray

	switch columnType {
	case "serial", "serial4", "pg_catalog.serial4":
		return "int32"

	case "bigserial", "serial8", "pg_catalog.serial8":
		return "int64"
	case "smallserial", "serial2", "pg_catalog.serial2":
		return "int16"

	case "integer", "int", "int4", "pg_catalog.int4":
		return "int32"

	case "bigint", "int8", "pg_catalog.int8":
		return "int64"

	case "smallint", "int2", "pg_catalog.int2":
		return "int16"

	case "float", "double precision", "float8", "pg_catalog.float8":
		return "float"

	case "real", "float4", "pg_catalog.float4":
		return "float32"

	case "numeric", "pg_catalog.numeric", "money":
		return "string"

	case "boolean", "bool", "pg_catalog.bool":
		return "bool"
	case "json":
		// TODO
		return "string"

	case "jsonb":
		return "jsonb"

	case "tsvector":
		return "string"

	case "bytea", "blob", "pg_catalog.bytea":
		return "[]byte"

	case "date":
		return "DateTime"

	case "pg_catalog.time":
		return "DateTime"

	case "pg_catalog.timetz":
		return "DateTime"

	case "pg_catalog.timestamp":
		return "DateTime"

	case "pg_catalog.timestamptz", "timestamptz":
		return "DateTime"

	case "text", "pg_catalog.varchar", "pg_catalog.bpchar", "string":
		return "string"

	case "uuid":
		return "uuid.UUID"
	case "inet":
		return "interface{}"

	case "cidr":
		return "interface{}"

	case "macaddr", "macaddr8":
		return "interface{}"

	case "ltree", "lquery", "ltxtquery":
		// This module implements a data type ltree for representing labels
		// of data stored in a hierarchical tree-like structure. Extensive
		// facilities for searching through label trees are provided.
		//
		// https://www.postgresql.org/docs/current/ltree.html
		return "string"

	case "interval", "pg_catalog.interval":
		return "int64"
	case "daterange":
		return "interface{}"

	case "datemultirange":
		return "interface{}"

	case "tsrange":
		return "interface{}"

	case "tsmultirange":
		return "interface{}"

	case "tstzrange":
		return "interface{}"

	case "tstzmultirange":
		return "interface{}"

	case "numrange":
		return "interface{}"

	case "nummultirange":
		return "interface{}"

	case "int4range":
		return "interface{}"

	case "int4multirange":
		return "interface{}"

	case "int8range":
		return "interface{}"
	case "int8multirange":
		return "interface{}"

	case "hstore":
		return "interface{}"

	case "bit", "varbit", "pg_catalog.bit", "pg_catalog.varbit":
		return "string"

	case "box":
		return "string"

	case "cid", "oid":
		return "int32"

	case "tid":
		return "int32"
	case "circle":
		return "pgtype.Circle"

	case "line":
		return "pgtype.Line"

	case "lseg":
		return "pgtype.Lseg"

	case "path":
		return "pgtype.Path"

	case "point":
		return "pgtype.Point"

	case "polygon":
		return "pgtype.Polygon"

	case "void":
		return "interface{}"
	case "any":
		return "interface{}"
	default:
		rel, err := parseIdentifierString(columnType)
		if err != nil {
			// TODO: Should this actually return an error here?
			return "interface{}"
		}
		if rel.Schema == "" {
			rel.Schema = req.Catalog.DefaultSchema
		}

		for _, schema := range req.Catalog.Schemas {
			if schema.Name == "pg_catalog" || schema.Name == "information_schema" {
				continue
			}

			for _, enum := range schema.Enums {
				if rel.Name == enum.Name && rel.Schema == schema.Name {
					if notNull {
						if schema.Name == req.Catalog.DefaultSchema {
							return StructName(enum.Name, req.Settings)
						}
						return StructName(schema.Name+"_"+enum.Name, req.Settings)
					} else {
						if schema.Name == req.Catalog.DefaultSchema {
							return "Null" + StructName(enum.Name, req.Settings)
						}
						return "Null" + StructName(schema.Name+"_"+enum.Name, req.Settings)
					}
				}
			}

			for _, ct := range schema.CompositeTypes {
				if rel.Name == ct.Name && rel.Schema == schema.Name {
					if notNull {
						return "string"
					}
					return "sql.NullString"
				}
			}
		}
	}

	return "string"
}
