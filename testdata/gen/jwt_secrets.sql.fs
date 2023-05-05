// Code generated by sqlc. DO NOT EDIT.
// source: jwt_secrets.sql

open Npgsql.FSharp



let createJwtSecret = """-- name: CreateJwtSecret :one
INSERT INTO jwt_secrets (name, secret, audience)
VALUES ($1, $2, $3) RETURNING id, name, secret, audience
""";


type CreateJwtSecretParams = {
  Name: string;
  Secret: string;
  Audience: string;
}
type CreateJwtSecretRow = {
  ID: int32;
  Name: string;
  Secret: string;
  Audience: string;
}

let CreateJwtSecret db: NpgsqlConnection arg: CreateJwtSecretParams -> CreateJwtSecretRow  =
  let reader = fun (read:RowReader) -> {
    ID = read.int32 "id"
    Name = read.string "name"
    Secret = read.string "secret"
    Audience = read.string "audience"}

  db
  |> Sql.existingConnection
  |> Sql.query createJwtSecret
  |> Sql.parameters  [ "@name", Sql.string name, "@secret", Sql.string secret, "@audience", Sql.string audience ]
  |> Sql.execute reader














let getJwtSecret = """-- name: GetJwtSecret :one
SELECT id, name, secret, audience FROM jwt_secrets WHERE name = $1
""";


type GetJwtSecretRow = {
  ID: int32;
  Name: string;
  Secret: string;
  Audience: string;
}

let GetJwtSecret db: NpgsqlConnection name: string -> GetJwtSecretRow  =
  let reader = fun (read:RowReader) -> {
    ID = read.int32 "id"
    Name = read.string "name"
    Secret = read.string "secret"
    Audience = read.string "audience"}

  db
  |> Sql.existingConnection
  |> Sql.query getJwtSecret
  |> Sql.parameters  [ "@name", Sql.string name ]
  |> Sql.execute reader













