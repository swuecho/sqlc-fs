// Code generated by sqlc. DO NOT EDIT.
// source: chat_jwt_secrets.sql

open Npgsql.FSharp

module ChatJwtSecrets =


















let createJwtSecret = """-- name: CreateJwtSecret :one
INSERT INTO jwt_secrets (name, secret, audience)
VALUES (@name, @secret, @audience) RETURNING id, name, secret, audience
"""


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

let CreateJwtSecret (db: NpgsqlConnection) (arg: CreateJwtSecretParams)  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    Name = read.string "name"
    Secret = read.string "secret"
    Audience = read.string "audience"}

  db
  |> Sql.existingConnection
  |> Sql.query createJwtSecret
  |> Sql.parameters  [ "@name", Sql.string arg.name, "@secret", Sql.string arg.secret, "@audience", Sql.string arg.audience ]
  |> Sql.execute reader




















































let getJwtSecret = """-- name: GetJwtSecret :one
SELECT id, name, secret, audience FROM jwt_secrets WHERE name = @name
"""


type GetJwtSecretRow = {
  ID: int32;
  Name: string;
  Secret: string;
  Audience: string;
}

let GetJwtSecret (db: NpgsqlConnection) (name: string)  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    Name = read.string "name"
    Secret = read.string "secret"
    Audience = read.string "audience"}

  db
  |> Sql.existingConnection
  |> Sql.query getJwtSecret
  |> Sql.parameters  [ "@name", Sql.string name ]
  |> Sql.execute reader
























































