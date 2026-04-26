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
	t = normalizeReaderType(t, true)
	return sdk.ToLowerCamelCase(strings.Replace(t, " option", "OrNone", 1))
}

func type2readerFunc(t string) string {
	t = normalizeReaderType(t, false)
	return sdk.ToLowerCamelCase(strings.Replace(t, " option", "OrNone", 1))
}

func isOptionType(t string) bool {
	return strings.HasSuffix(t, " option")
}

func fsharpFieldAccessor(argName string, field Field) string {
	return argName + "." + sdk.ToPascalCase(field.Name)
}

func normalizeReaderType(t string, forParam bool) string {
	isArray := strings.HasSuffix(t, "[]")
	if isArray {
		t = strings.TrimSuffix(t, "[]")
	}

	if t == "int32" {
		t = "int"
	}
	if t == "float64" || t == "float" {
		t = "double"
	}
	if t == "float32" {
		t = "float"
	}

	if forParam && t == "DateTime" {
		t = "date"
	}

	if !forParam && t == "jsonb" {
		t = "string"
	}

	if isArray {
		t += "Array"
	}

	return t
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

func (v QueryValue) CopyFromFields() []Field {
	if v.Struct != nil {
		return v.UniqueFields()
	}
	return []Field{{
		Name:   v.Name,
		DBName: sdk.ToSnakeCase(v.Name),
		Type:   v.Typ,
	}}
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

func validateQueryCommand(q *plugin.Query) error {
	switch q.Cmd {
	case metadata.CmdOne,
		metadata.CmdMany,
		metadata.CmdExec,
		metadata.CmdExecRows,
		metadata.CmdExecResult,
		metadata.CmdCopyFrom,
		metadata.CmdBatchExec,
		metadata.CmdBatchMany,
		metadata.CmdBatchOne:
		return nil
	case metadata.CmdExecLastID:
		return fmt.Errorf("unsupported sqlc command %s for query %s: sqlc-fs does not generate F# for this annotation yet", q.Cmd, q.Name)
	default:
		return fmt.Errorf("unsupported sqlc command %s for query %s", q.Cmd, q.Name)
	}
}

func (q Query) hasRetType() bool {
	scanned := q.Cmd == metadata.CmdOne || q.Cmd == metadata.CmdMany ||
		q.Cmd == metadata.CmdBatchMany || q.Cmd == metadata.CmdBatchOne
	return scanned && !q.Ret.isEmpty()
}

func (q Query) CopyFromSQL() string {
	if q.Table == nil {
		return ""
	}
	tableParts := make([]string, 0, 3)
	for _, part := range []string{q.Table.Catalog, q.Table.Schema, q.Table.Name} {
		if part != "" {
			tableParts = append(tableParts, quotePostgresIdent(part))
		}
	}

	fields := q.Arg.CopyFromFields()
	columnNames := make([]string, 0, len(fields))
	for _, field := range fields {
		name := field.DBName
		if name == "" {
			name = sdk.ToSnakeCase(field.Name)
		}
		columnNames = append(columnNames, quotePostgresIdent(name))
	}

	return fmt.Sprintf("COPY %s (%s) FROM STDIN (FORMAT BINARY)", strings.Join(tableParts, "."), strings.Join(columnNames, ", "))
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

func quotePostgresIdent(s string) string {
	return `"` + strings.ReplaceAll(s, `"`, `""`) + `"`
}

// BatchElemType is the element type of (args: seq<...>) for :batchexec / :batchmany / :batchone.
func (q Query) BatchElemType() string {
	return q.Arg.Type()
}

// BatchResultRowType is the type of one scanned row for :batchmany / :batchone.
func (q Query) BatchResultRowType() string {
	if q.Ret.Struct != nil {
		return q.Ret.Struct.Name
	}
	return q.Ret.Typ
}

// BatchBindParamsFSharp emits NpgsqlBatchCommand parameter binding lines (indented with 4 spaces).
func (q Query) BatchBindParamsFSharp() string {
	return q.Arg.batchBindParamsFSharp("bc", "arg", q.Arg.IsStruct())
}

// CommandBindParamsFSharp binds parameters on an NpgsqlCommand (e.g. :execlastid).
func (q Query) CommandBindParamsFSharp() string {
	if q.Arg.IsStruct() {
		return q.Arg.batchBindParamsFSharp("cmd", "arg", true)
	}
	if q.Arg.isEmpty() {
		return ""
	}
	return q.Arg.batchBindParamsFSharp("cmd", q.Arg.Name, false)
}

// batchBindParamsFSharp binds one statement's parameters. When useStructFields is true, accessors are
// rootArg + "." + PascalCase(field); otherwise rootArg is the full accessor (e.g. batch loop variable "arg"
// or a single scalar parameter name).
func (v QueryValue) batchBindParamsFSharp(cmdVar, rootArg string, useStructFields bool) string {
	fields := v.CopyFromFields()
	var b strings.Builder
	for _, f := range fields {
		col := f.DBName
		if col == "" {
			col = sdk.ToSnakeCase(f.Name)
		}
		param := "@" + col
		var accessor string
		if useStructFields {
			accessor = rootArg + "." + sdk.ToPascalCase(f.Name)
		} else {
			accessor = rootArg
		}
		if isOptionType(f.Type) {
			fmt.Fprintf(&b, "    %s.Parameters.AddWithValue(%q, match %s with | Some v -> box v | None -> box DBNull.Value) |> ignore\n",
				cmdVar, param, accessor)
		} else {
			fmt.Fprintf(&b, "    %s.Parameters.AddWithValue(%q, %s) |> ignore\n", cmdVar, param, accessor)
		}
	}
	return b.String()
}

// ExecLastIDScalarType is the F# type returned for :execlastid (PostgreSQL via lastval()).
func (q Query) ExecLastIDScalarType() string {
	if !q.Ret.isEmpty() && q.Ret.Struct == nil && q.Ret.Typ != "" {
		return q.Ret.Typ
	}
	return "int64"
}

// ExecLastIDScalarExpr converts lastval() ExecuteScalar() to the declared F# type.
func (q Query) ExecLastIDScalarExpr() string {
	switch q.ExecLastIDScalarType() {
	case "int32":
		return "Convert.ToInt32(lv.ExecuteScalar())"
	case "int16":
		return "Convert.ToInt16(lv.ExecuteScalar())"
	default:
		return "Convert.ToInt64(lv.ExecuteScalar())"
	}
}
