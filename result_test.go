package main

import (
	"strings"
	"testing"

	"github.com/swuecho/sqlc-fs/internal/metadata"
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
	if got := type2readerFunc("jsonb"); got != "string" {
		t.Fatalf("type2readerFunc(jsonb) = %q, want string", got)
	}
	if got := type2readerFuncParam("jsonb"); got != "jsonb" {
		t.Fatalf("type2readerFuncParam(jsonb) = %q, want jsonb", got)
	}
}

func TestUnsupportedCommandsReturnErrors(t *testing.T) {
	req := &plugin.CodeGenRequest{
		Settings: &plugin.Settings{},
		Catalog:  &plugin.Catalog{DefaultSchema: "public"},
		Queries: []*plugin.Query{{
			Name: "InsertAuthorLastID",
			Cmd:  metadata.CmdExecLastID,
		}},
	}

	_, err := buildQueries(req, nil)
	if err == nil {
		t.Fatal("buildQueries() error = nil, want unsupported command error")
	}
	if !strings.Contains(err.Error(), "unsupported sqlc command :execlastid") {
		t.Fatalf("buildQueries() error = %q, want unsupported command message", err)
	}
}

func TestGenerateEmitsBatchExec(t *testing.T) {
	req := &plugin.CodeGenRequest{
		Settings: &plugin.Settings{Codegen: &plugin.Codegen{}},
		Catalog:  &plugin.Catalog{DefaultSchema: "public"},
		Queries: []*plugin.Query{{
			Text:     "DELETE FROM authors WHERE id = $1",
			Name:     "BatchDeleteAuthors",
			Cmd:      metadata.CmdBatchExec,
			Filename: "batch_authors.sql",
			Params:   []*plugin.Parameter{param(1, "id", "pg_catalog.int4")},
		}},
	}
	resp, err := Generate(req)
	if err != nil {
		t.Fatal(err)
	}
	var got string
	for _, f := range resp.Files {
		if f.Name == "batch_authors.sql.fs" {
			got = string(f.Contents)
			break
		}
	}
	if got == "" {
		t.Fatal("missing batch_authors.sql.fs")
	}
	for _, want := range []string{
		`let BatchDeleteAuthors (db: NpgsqlConnection) (args: seq<int32>)`,
		`new NpgsqlBatch(db)`,
		`NpgsqlBatchCommand(batchDeleteAuthors)`,
		`bc.Parameters.AddWithValue("@id", arg)`,
	} {
		if !strings.Contains(got, want) {
			t.Fatalf("missing %q in:\n%s", want, got)
		}
	}
}

func TestGenerateEmitsBatchManyAndBatchOne(t *testing.T) {
	req := &plugin.CodeGenRequest{
		Settings: &plugin.Settings{Codegen: &plugin.Codegen{}},
		Catalog:  &plugin.Catalog{DefaultSchema: "public"},
		Queries: []*plugin.Query{
			{
				Text:     "SELECT id, name, bio FROM authors WHERE name = $1",
				Name:     "BatchAuthorsByName",
				Cmd:      metadata.CmdBatchMany,
				Filename: "batch_authors.sql",
				Params:   []*plugin.Parameter{param(1, "name", "text")},
				Columns: []*plugin.Column{
					{Name: "id", NotNull: true, Type: &plugin.Identifier{Name: "pg_catalog.int4"}},
					{Name: "name", NotNull: true, Type: &plugin.Identifier{Name: "text"}},
					{Name: "bio", NotNull: false, Type: &plugin.Identifier{Name: "text"}},
				},
			},
			{
				Text:     "SELECT id, name, bio FROM authors WHERE id = $1 LIMIT 1",
				Name:     "BatchAuthorByID",
				Cmd:      metadata.CmdBatchOne,
				Filename: "batch_authors.sql",
				Params:   []*plugin.Parameter{param(1, "id", "pg_catalog.int4")},
				Columns: []*plugin.Column{
					{Name: "id", NotNull: true, Type: &plugin.Identifier{Name: "pg_catalog.int4"}},
					{Name: "name", NotNull: true, Type: &plugin.Identifier{Name: "text"}},
					{Name: "bio", NotNull: false, Type: &plugin.Identifier{Name: "text"}},
				},
			},
		},
	}
	resp, err := Generate(req)
	if err != nil {
		t.Fatal(err)
	}
	var got string
	for _, f := range resp.Files {
		if f.Name == "batch_authors.sql.fs" {
			got = string(f.Contents)
			break
		}
	}
	if got == "" {
		t.Fatal("missing batch_authors.sql.fs")
	}
	if !strings.Contains(got, `let BatchAuthorsByName (db: NpgsqlConnection) (args: seq<string>)`) {
		t.Fatalf("batchmany signature missing:\n%s", got)
	}
	if !strings.Contains(got, `List<list<`) || !strings.Contains(got, `readBatchRow`) {
		t.Fatalf("batchmany reader missing:\n%s", got)
	}
	if !strings.Contains(got, `let BatchAuthorByID (db: NpgsqlConnection) (args: seq<int32>)`) {
		t.Fatalf("batchone signature missing:\n%s", got)
	}
	if !strings.Contains(got, `batchone: expected one row`) {
		t.Fatalf("batchone guard missing:\n%s", got)
	}
}

func TestGenerateEmitsCopyFromFunction(t *testing.T) {
	req := &plugin.CodeGenRequest{
		Settings: &plugin.Settings{
			Codegen: &plugin.Codegen{},
		},
		Catalog: &plugin.Catalog{DefaultSchema: "public"},
		Queries: []*plugin.Query{{
			Text:     "INSERT INTO authors (name, bio) VALUES ($1, $2)",
			Name:     "CreateAuthors",
			Cmd:      metadata.CmdCopyFrom,
			Filename: "authors.sql",
			Params: []*plugin.Parameter{
				param(1, "name", "text"),
				{
					Number: 2,
					Column: &plugin.Column{
						Name:    "bio",
						NotNull: false,
						Type:    &plugin.Identifier{Name: "text"},
					},
				},
			},
			InsertIntoTable: &plugin.Identifier{Schema: "public", Name: "authors"},
		}},
	}

	resp, err := Generate(req)
	if err != nil {
		t.Fatal(err)
	}

	var got string
	for _, file := range resp.Files {
		if file.Name == "authors.sql.fs" {
			got = string(file.Contents)
			break
		}
	}
	if got == "" {
		t.Fatal("Generate() did not emit authors.sql.fs")
	}
	for _, want := range []string{
		`type CreateAuthorsParams = {`,
		`Bio: string option`,
		`let CreateAuthors (db: NpgsqlConnection) (args: seq<CreateAuthorsParams>) =`,
		`db.BeginBinaryImport("COPY \"public\".\"authors\" (\"name\", \"bio\") FROM STDIN (FORMAT BINARY)")`,
		`match arg.Bio with`,
		`| None -> writer.WriteNull()`,
	} {
		if !strings.Contains(got, want) {
			t.Fatalf("generated copyfrom code missing %q:\n%s", want, got)
		}
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
