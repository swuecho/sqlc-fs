package camelsnakekebab

import (
	"regexp"
	"strings"

	"golang.org/x/text/cases"
	"golang.org/x/text/language"
)

func SplitWords(str string) []string {
	// A pattern that matches all known word separators
	wordSeparatorPattern := regexp.MustCompile(`\s+|_|-(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z_-])(?=[A-Z])|(?<=[A-Za-z0-9])(?=[^A-Za-z0-9])`)

	// Split the input string using the pattern as separator
	result := wordSeparatorPattern.Split(str, -1)

	// Remove empty strings from the array
	var words []string
	for _, w := range result {
		if strings.TrimSpace(w) != "" {
			words = append(words, w)
		}
	}

	return words
}

func convertCase(convertFirst func(string) string, convertRest func(string) string, sep string, str string) string {
	words := SplitWords(str)
	if len(words) == 0 {
		return ""
	}
	result := make([]string, len(words))
	result[0] = convertFirst(words[0])
	for i := 1; i < len(words); i++ {
		result[i] = convertRest(words[i])
	}
	return strings.Join(result, sep)
}

func titleCase(s string) string {
	// Create a TitleConverter with the current locale's title case rules
	tc := cases.Title(language.English)

	// Convert the input string to title case
	return tc.String(s)
}

func LowerCamelCase(str string) string {
	return convertCase(strings.ToLower, titleCase, "", str)
}

func UpperCamelCase(str string) string {
	return convertCase(titleCase, titleCase, "", str)
}

func LowerSnakeCase(str string) string {
	return convertCase(strings.ToLower, strings.ToLower, "_", str)
}

func UpperSnakeCase(str string) string {
	return convertCase(titleCase, titleCase, "_", str)
}

func ConstantCase(str string) string {
	return convertCase(strings.ToUpper, strings.ToUpper, "_", str)
}

func KebabCase(str string) string {
	return convertCase(strings.ToLower, strings.ToLower, "-", str)
}
