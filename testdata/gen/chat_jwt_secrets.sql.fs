// Code generated by sqlc. DO NOT EDIT.
// source: chat_jwt_secrets.sql


module ChatJwtSecrets 

open Npgsql
open Npgsql.FSharp
open System


















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
  Id: int32;
  Name: string;
  Secret: string;
  Audience: string;
}

let CreateJwtSecret (db: NpgsqlConnection) (arg: CreateJwtSecretParams)  =
  let reader = fun (read:RowReader) -> {
    Id = read.int "Id"
    Name = read.string "Name"
    Secret = read.string "Secret"
    Audience = read.string "Audience"}

  db
  |> Sql.existingConnection
  |> Sql.query createJwtSecret
  |> Sql.parameters  [ "@name", Sql.string arg.Name; "@secret", Sql.string arg.Secret; "@audience", Sql.string arg.Audience ]
  |> Sql.execute reader


















































let getJwtSecret = """-- name: GetJwtSecret :one
SELECT id, name, secret, audience FROM jwt_secrets WHERE name = @name
"""


type GetJwtSecretRow = {
  Id: int32;
  Name: string;
  Secret: string;
  Audience: string;
}

let GetJwtSecret (db: NpgsqlConnection) (name: string)  =
  let reader = fun (read:RowReader) -> {
    Id = read.int "Id"
    Name = read.string "Name"
    Secret = read.string "Secret"
    Audience = read.string "Audience"}

  db
  |> Sql.existingConnection
  |> Sql.query getJwtSecret
  |> Sql.parameters  [ "@name", Sql.string name ]
  |> Sql.execute reader




















































