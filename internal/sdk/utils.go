package sdk

import (
	"strings"
	"unicode"

	"github.com/stoewer/go-strcase"
	"golang.org/x/text/cases"
	"golang.org/x/text/language"
)

func titleCase(s string) string {
	// Create a TitleConverter with the current locale's title case rules
	tc := cases.Title(language.English)

	// Convert the input string to title case
	return tc.String(s)
}

func LowerTitle(s string) string {
	if s == "" {
		return s
	}

	a := []rune(s)
	a[0] = unicode.ToLower(a[0])
	return string(a)
}

func Title(s string) string {
	return titleCase(s)
}

func Stem(s string) string {
	return strings.ReplaceAll(s, ".sql", "")
}

func ToSnakeCase(s string) string {
	return strcase.SnakeCase(s)
}

func ToPascalCase(s string) string {
	return strcase.UpperCamelCase(s)
}

func ToLowerCamelCase(s string) string {
	return strcase.LowerCamelCase(s)
}

func ToCamelCase(s string) string {
	return strcase.UpperCamelCase(s)
}

// Go string literals cannot contain backtick. If a string contains
// a backtick, replace it the following way:
//
// input:
//
//	SELECT `group` FROM foo
//
// output:
//
//	SELECT ` + "`" + `group` + "`" + ` FROM foo
//
// # The escaped string must be rendered inside an existing string literal
//
// A string cannot be escaped twice
func EscapeBacktick(s string) string {
	return strings.Replace(s, "`", "`+\"`\"+`", -1)
}

func DoubleSlashComment(s string) string {
	return "// " + strings.ReplaceAll(s, "\n", "\n// ")
}
