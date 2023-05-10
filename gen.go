package main

import (
	"bufio"
	"bytes"
	"encoding/json"
	"fmt"
	"strings"
	"text/template"

	"github.com/pkg/errors"
	"github.com/samber/lo"
	"github.com/swuecho/sqlc-fs/internal/plugin"
	"github.com/swuecho/sqlc-fs/internal/sdk"
)

func Generate(req *plugin.CodeGenRequest) (*plugin.CodeGenResponse, error) {
	structs := buildStructs(req)

	options, err := parseOptions(req)
	if err != nil {
		return nil, errors.Errorf("error parse options: %w", err)
	}

	queries, err := buildQueries(req, structs)
	if err != nil {
		return nil, errors.Errorf("error generating queries: %w", err)
	}
	return generate(req, structs, queries, options)
}

type FSharpOption struct {
	EmitAsyncCode     bool   `json:"emit_async_code,omitempty"`
	EmitModelName     string `json:"emit_model_name,omitempty"`
	EmitModelFileName string `json:"emit_model_file_name,omitempty"`
	EmitAutoOpenModel bool   `json:"emit_auto_open_model,omitempty"`
}

func parseOptions(req *plugin.CodeGenRequest) (*FSharpOption, error) {
	if req.Settings.Codegen != nil {
		if len(req.Settings.Codegen.Options) != 0 {
			var options *FSharpOption
			dec := json.NewDecoder(bytes.NewReader(req.Settings.Codegen.Options))
			dec.DisallowUnknownFields()
			if options.EmitModelFileName == "" {
				options.EmitModelFileName = "model.fs"
			}
			if options.EmitModelName == "" {
				options.EmitModelName = "ModelFromSchema"
			}

			if err := dec.Decode(&options); err != nil {
				return options, fmt.Errorf("unmarshalling options: %s", err)
			}
			return options, nil
		}
	}
	return new(FSharpOption), nil
}

type tmplCtx struct {
	Q       string
	Structs []Struct
	Queries []Query
	Options *FSharpOption

	// XXX: race
	SourceName string
}

func (t *tmplCtx) OutputQuery(sourceName string) bool {
	return t.SourceName == sourceName
}

func generate(req *plugin.CodeGenRequest, structs []Struct, queries []Query, options *FSharpOption) (*plugin.CodeGenResponse, error) {
	funcMap := template.FuncMap{
		"stem":            sdk.Stem,
		"pascalCase":      sdk.ToPascalCase,
		"comment":         sdk.DoubleSlashComment,
		"escape":          sdk.EscapeBacktick,
		"hasPrefix":       strings.HasPrefix,
		"type2readerFunc": type2readerFunc,
	}

	tmpl := template.Must(
		template.New("table").
			Funcs(funcMap).
			ParseFS(
				templates,
				"templates/query.tmpl",
			),
	)

	tctx := tmplCtx{
		Q:       "\"\"\"",
		Queries: queries,
		Structs: structs,
		Options: options,
	}

	output := map[string]string{}

	execute := func(name, templateName string) error {
		var b bytes.Buffer
		w := bufio.NewWriter(&b)
		tctx.SourceName = name
		err := tmpl.ExecuteTemplate(w, templateName, &tctx)
		w.Flush()
		if err != nil {
			return err
		}
		code := b.Bytes()
		if !strings.HasSuffix(name, ".fs") {
			name += ".fs"
		}
		output[name] = string(code)
		return nil
	}

	files := map[string]struct{}{}
	for _, gq := range queries {
		files[gq.SourceName] = struct{}{}
	}

	for source := range files {
		if err := execute(source, "queryFile"); err != nil {
			return nil, err
		}
	}
	resp := plugin.CodeGenResponse{}
	noneSystemStruct := lo.Filter(structs, func(strut Struct, idx int) bool {
		return strut.Table.Schema != "information_schema" && strut.Table.Schema != "pg_catalog"
	})
	model_of_structs, _ := json.Marshal(noneSystemStruct)
	resp.Files = append(resp.Files, &plugin.File{
		Name:     "model.json",
		Contents: model_of_structs,
	})

	// fmt.Printf("%+v", tmpl.Templates())
	tctx.Structs = noneSystemStruct
	renderStructs(funcMap, tctx, output)

	for filename, code := range output {
		resp.Files = append(resp.Files, &plugin.File{
			Name:     filename,
			Contents: []byte(code),
		})
	}

	return &resp, nil
}

func renderStructs(funcMap template.FuncMap, tctx tmplCtx, output map[string]string) {
	var b bytes.Buffer
	w := bufio.NewWriter(&b)

	tmplModel := template.Must(
		template.New("model").
			Funcs(funcMap).
			ParseFS(
				templates,
				"templates/model.tmpl",
			),
	)
	// fmt.Printf("%s\n", tmplModel.Name())
	_ = tmplModel.ExecuteTemplate(w, "structsFile", &tctx)
	w.Flush()
	code := b.Bytes()
	modelFileName := tctx.Options.EmitModelFileName
	output[modelFileName] = string(code)
}
