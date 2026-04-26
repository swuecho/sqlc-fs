package main

import (
	"strings"
	"testing"

	"github.com/swuecho/sqlc-fs/internal/plugin"
)

func TestNpgsqlQueryUsesFallbackNamesForUnnamedParams(t *testing.T) {
	q := &plugin.Query{
		Text: "SELECT $1, $10, string_to_array($2, $3) WHERE $2 = ''",
		Params: []*plugin.Parameter{
			param(1, "user_id", "pg_catalog.int4"),
			param(2, "", "any"),
			param(3, "string_to_array", "text"),
			param(10, "", "pg_catalog.int4"),
		},
	}

	got := npgsqlQuery(q)
	want := "SELECT @user_id, @column_10, string_to_array(@column_2, @string_to_array) WHERE @column_2 = ''"
	if got != want {
		t.Fatalf("npgsqlQuery() = %q, want %q", got, want)
	}
}

func TestColumnsToStructUsesParameterNumbersForUnnamedParams(t *testing.T) {
	req := &plugin.CodeGenRequest{
		Settings: &plugin.Settings{},
		Catalog:  &plugin.Catalog{DefaultSchema: "public"},
	}

	got, err := columnsToStruct(req, "SearchParams", []column{
		{id: 1, Column: paramColumn("user_id", "pg_catalog.int4")},
		{id: 4, Column: paramColumn("", "any")},
	}, false)
	if err != nil {
		t.Fatal(err)
	}

	if len(got.Fields) != 2 {
		t.Fatalf("got %d fields, want 2", len(got.Fields))
	}
	if got.Fields[1].Name != "column_4" {
		t.Fatalf("unnamed param field = %q, want column_4", got.Fields[1].Name)
	}
	if got.Fields[1].Type != "string" {
		t.Fatalf("unnamed any param type = %q, want string", got.Fields[1].Type)
	}
}

func TestGeneratedParamsUseFallbackNames(t *testing.T) {
	req := &plugin.CodeGenRequest{
		Settings: &plugin.Settings{},
		Catalog:  &plugin.Catalog{DefaultSchema: "public"},
	}

	got, err := columnsToStruct(req, "SearchParams", []column{
		{id: 1, Column: paramColumn("user_id", "pg_catalog.int4")},
		{id: 2, Column: paramColumn("", "any")},
		{id: 3, Column: paramColumn("string_to_array", "text")},
	}, false)
	if err != nil {
		t.Fatal(err)
	}

	params := QueryValue{Name: "arg", Struct: got}.NpgsqlParams()
	for _, want := range []string{
		`"@user_id", Sql.int arg.UserId`,
		`"@column_2", Sql.string arg.Column2`,
		`"@string_to_array", Sql.string arg.StringToArray`,
	} {
		if !strings.Contains(params, want) {
			t.Fatalf("generated params %q missing %q", params, want)
		}
	}
}

func TestType2ReaderFuncUsesArrayReaders(t *testing.T) {
	if got := type2readerFunc("string[]"); got != "stringArray" {
		t.Fatalf("type2readerFunc(string[]) = %q, want stringArray", got)
	}
	if got := type2readerFunc("int32[]"); got != "intArray" {
		t.Fatalf("type2readerFunc(int32[]) = %q, want intArray", got)
	}
	if got := type2readerFunc("DateTime"); got != "dateTime" {
		t.Fatalf("type2readerFunc(DateTime) = %q, want dateTime", got)
	}
	if got := type2readerFuncParam("DateTime"); got != "date" {
		t.Fatalf("type2readerFuncParam(DateTime) = %q, want date", got)
	}
}

func param(number int32, name string, typ string) *plugin.Parameter {
	return &plugin.Parameter{
		Number: number,
		Column: paramColumn(name, typ),
	}
}

func paramColumn(name string, typ string) *plugin.Column {
	return &plugin.Column{
		Name:    name,
		NotNull: true,
		Type:    &plugin.Identifier{Name: typ},
	}
}
