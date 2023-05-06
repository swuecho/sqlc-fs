package main

import (
	"fmt"
	"github.com/swuecho/sqlc-fs/internal/sdk"
)

type Field struct {
	Name              string // CamelCased name for Go
	DBName            string // Name as used in the DB
	Type              string
	Comment           string
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
