package sdk

import (
	"strings"
	"unicode"
)

func LowerTitle(s string) string {
	if s == "" {
		return s
	}

	a := []rune(s)
	a[0] = unicode.ToLower(a[0])
	return string(a)
}

func Title(s string) string {
	return strings.Title(s)
}


func ToPascalCase(s string) string {
	return ToCamelInitCase(s, true)
}

func ToCamelInitCase(name string, initUpper bool) string {
	out := ""
	for i, p := range strings.Split(name, "_") {
		if !initUpper && i == 0 {
			out += p
			continue
		}
		if p == "id" {
			out += "ID"
		} else {
			out += strings.Title(p)
		}
	}
	return out
}

// Go string literals cannot contain backtick. If a string contains
// a backtick, replace it the following way:
//
// input:
// 	SELECT `group` FROM foo
//
// output:
// 	SELECT ` + "`" + `group` + "`" + ` FROM foo
//
// The escaped string must be rendered inside an existing string literal
//
// A string cannot be escaped twice
func EscapeBacktick(s string) string {
	return strings.Replace(s, "`", "`+\"`\"+`", -1)
}

func DoubleSlashComment(s string) string {
	return "// " + strings.ReplaceAll(s, "\n", "\n// ")
}
