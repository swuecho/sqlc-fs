// Code generated by sqlc. DO NOT EDIT.
// source: author.sql

open Npgsql.FSharp


let createAuthor = """-- name: CreateAuthor :execresult
INSERT INTO authors (
  name, bio
) VALUES (
  ?, ?
)
""";


type CreateAuthorParams = {
  Name: string;
  Bio: string;
}








let CreateAuthor db: NpgsqlConnection arg: CreateAuthorParams) =
  db 
  |> Sql.existingConnection
  |> Sql.query createAuthor
  |> Sql.parameters  [ "@name", Sql.string name, "@bio", Sql.string bio ]
  |> Sql.execute





let deleteAuthor = """-- name: DeleteAuthor :exec
DELETE FROM authors
WHERE id = ?
""";






let DeleteAuthor db: NpgsqlConnection id: int32  = 
  db 
  |> Sql.existingConnection
  |> Sql.query deleteAuthor
  |> Sql.parameters  [ "@id", Sql.int32 id ]
  |> Sql.executeNonQuery








let getAuthor = """-- name: GetAuthor :one
SELECT id, name, bio FROM authors
WHERE id = ? LIMIT 1
""";


type GetAuthorRow = {
  ID: int32;
  Name: string;
  Bio: string;
}

let GetAuthor db: NpgsqlConnection id: int32 -> GetAuthorRow  =
  let reader = fun read -> {
    ID = read.int32 "id"
    Name = read.string "name"
    Bio = read.string "bio"}

  db
  |> Sql.existingConnection
  |> Sql.query getAuthor
  |> Sql.parameters  [ "@id", Sql.int32 id ]
  |> Sql.execute reader













let listAuthors = """-- name: ListAuthors :many
SELECT id, name, bio FROM authors
ORDER BY name
""";


type ListAuthorsRow = {
  ID: int32;
  Name: string;
  Bio: string;
}


let ListAuthors db: NpgsqlConnection  -> ListAuthorsRow list =
  let reader = fun read -> {
    ID = read.int32 "id"
    Name = read.string "name"
    Bio = read.string "bio"}
  db 
  |> Sql.existingConnection
  |> sql.query listAuthors
  |> listAuthors
  |> Sql.execute reader











