package main

import (
	"fmt"
	"strings"
	"github.com/stephen/sqlc-ts/internal/sdk"
)

type Field struct {
	Name              string // CamelCased name for Go
	DBName            string // Name as used in the DB
	Type              string
	TypecheckTemplate string
	Comment           string
}

func (f Field) Typecheck(i int) string {
	return strings.ReplaceAll(f.TypecheckTemplate, "%", fmt.Sprintf("row[%d]", i))
}

func SetCaseStyle(name string, style string) string {
	switch style {
	case "camel":
		return sdk.ToCamelCase(name)
	case "pascal":
		return sdk.ToPascalCase(name)
	case "snake":
		return sdk.ToSnakeCase(name)
	default:
		panic(fmt.Sprintf("unsupported JSON tags case style: '%s'", style))
	}
}
