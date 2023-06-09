package main

import (
	"fmt"
	"strings"

	"github.com/swuecho/sqlc-fs/internal/metadata"
	"github.com/swuecho/sqlc-fs/internal/plugin"
	"github.com/swuecho/sqlc-fs/internal/sdk"
)

type QueryValue struct {
	Emit   bool
	Name   string
	Struct *Struct
	Typ    string
}

func (v QueryValue) EmitStruct() bool {
	return v.Emit
}

func (v QueryValue) IsStruct() bool {
	return v.Struct != nil
}

func (v QueryValue) isEmpty() bool {
	return v.Typ == "" && v.Name == "" && v.Struct == nil
}

func (v QueryValue) Pair() string {
	if v.isEmpty() {
		return ""
	}
	return v.Name + ": " + v.DefineType()
}

func (v QueryValue) SlicePair() string {
	if v.isEmpty() {
		return ""
	}
	return v.Name + " []" + v.DefineType()
}

func (v QueryValue) Type() string {
	if v.Typ != "" {
		return v.Typ
	}
	if v.Struct != nil {
		return v.Struct.Name
	}
	panic("no type for QueryValue: " + v.Name)
}

func (v *QueryValue) DefineType() string {
	t := v.Type()
	return t
}

func (v *QueryValue) ReturnName() string {
	return v.Name
}
func (v QueryValue) UniqueFieldsCountMultiple() bool {
	return len(v.UniqueFields()) > 1
}

func (v QueryValue) UniqueFields() []Field {
	seen := map[string]struct{}{}
	if v.Struct == nil {
		return []Field{{
			Name: v.Name,
			Type: v.Typ,
		}}
	}
	fields := make([]Field, 0, len(v.Struct.Fields))

	for _, field := range v.Struct.Fields {
		if _, found := seen[field.Name]; found {
			continue
		}
		seen[field.Name] = struct{}{}
		fields = append(fields, field)
	}

	return fields
}

func (v QueryValue) Params() string {
	if v.isEmpty() {
		return ""
	}
	var out []string
	if v.Struct == nil {
		out = append(out, v.Name)
	} else {
		for _, f := range v.Struct.Fields {
			out = append(out, v.Name+"."+f.Name)
		}
	}
	if len(out) <= 3 {
		return strings.Join(out, ", ")
	}
	out = append(out, "")
	return "\n" + strings.Join(out, ",\n")
}

func (v QueryValue) NpgsqlParams() string {
	if v.isEmpty() {
		return ""
	}

	// Column names and types
	var columnNames []string
	var columnTypes []string
	var isInStruct []bool

	if v.Struct == nil {
		columnNames = append(columnNames, v.Name)
		columnTypes = append(columnTypes, v.Type())
		isInStruct = append(isInStruct, false)
	} else {
		// For a struct, get all the field names and types
		for _, f := range v.Struct.Fields {
			columnNames = append(columnNames, f.Name)
			columnTypes = append(columnTypes, f.Type)
			isInStruct = append(isInStruct, true)
		}
	}

	// Construct the PostgreSQL params string
	var sqlParamsString string
	for idx, columnName := range columnNames {
		columnNameFull := sdk.ToLowerCamelCase(columnName)
		if isInStruct[idx] {
			columnNameFull = "arg." + sdk.ToPascalCase(columnNameFull)
		}
		sqlParamsString += fmt.Sprintf("\"@%s\", Sql.%s %s", sdk.ToSnakeCase(columnName), type2readerFuncParam(columnTypes[idx]), columnNameFull)
		if idx < len(columnNames)-1 {
			sqlParamsString += "; "
		}
	}
	return sqlParamsString
}

func type2readerFuncParam(t string) string {
	if t == "int32" {
		t = "int"
	}
	if t == "float64" || t == "float" {
		t = "double"
	}
	if t == "float32" {
		t = "float"
	}

	if t == "DateTime" {
		t = "date"
	}

	return sdk.ToLowerCamelCase(strings.Replace(t, " option", "OrNone", 1))
}

func type2readerFunc(t string) string {
	if t == "int32" {
		t = "int"
	}
	if t == "float64" || t == "float" {
		t = "double"
	}

	if t == "float32" {
		t = "float"
	}

	if t == "jsonb" {
		t = "string"
	}

	return sdk.ToLowerCamelCase(strings.Replace(t, " option", "OrNone", 1))
}

func (v QueryValue) ColumnNames() string {
	if v.Struct == nil {
		return fmt.Sprintf("[]string{%q}", v.Name)
	}
	escapedNames := make([]string, len(v.Struct.Fields))
	for i, f := range v.Struct.Fields {
		escapedNames[i] = fmt.Sprintf("%q", f.DBName)
	}
	return "[]string{" + strings.Join(escapedNames, ", ") + "}"
}

func (v QueryValue) Scan() string {
	var out []string
	if v.Struct == nil {
		out = append(out, "&"+v.Name)
	} else {
		for _, f := range v.Struct.Fields {
			out = append(out, "&"+v.Name+"."+f.Name)
		}
	}
	if len(out) <= 3 {
		return strings.Join(out, ",")
	}
	out = append(out, "")
	return "\n" + strings.Join(out, ",\n")
}

// A struct used to generate methods and fields on the Queries struct
type Query struct {
	Cmd          string
	Comments     []string
	MethodName   string
	FieldName    string
	ConstantName string
	SQL          string
	SourceName   string
	Ret          QueryValue
	Arg          QueryValue
	// Used for :copyfrom
	Table *plugin.Identifier
}


func (q Query) hasRetType() bool {
	scanned := q.Cmd == metadata.CmdOne || q.Cmd == metadata.CmdMany ||
		q.Cmd == metadata.CmdBatchMany || q.Cmd == metadata.CmdBatchOne
	return scanned && !q.Ret.isEmpty()
}

func (q Query) TableIdentifier() string {
	escapedNames := make([]string, 0, 3)
	for _, p := range []string{q.Table.Catalog, q.Table.Schema, q.Table.Name} {
		if p != "" {
			escapedNames = append(escapedNames, fmt.Sprintf("%q", p))
		}
	}
	return "[]string{" + strings.Join(escapedNames, ", ") + "}"
}
