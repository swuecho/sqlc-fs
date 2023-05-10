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

let CreateJwtSecret (db: NpgsqlConnection)  (arg: CreateJwtSecretParams)  =
  
  let reader = fun (read:RowReader) -> {
    Id = read.int "id"
    Name = read.string "name"
    Secret = read.string "secret"
    Audience = read.string "audience"}
  

  db
  |> Sql.existingConnection
  |> Sql.query createJwtSecret
  |> Sql.parameters  [ "@name", Sql.string arg.Name; "@secret", Sql.string arg.Secret; "@audience", Sql.string arg.Audience ]
  |> Sql.executeRow reader
















let deleteAllJwtSecrets = """-- name: DeleteAllJwtSecrets :execrows
DELETE FROM jwt_secrets WHERE name = @name
"""








let DeleteAllJwtSecrets (db: NpgsqlConnection)  (name: string) =
  db 
  |> Sql.existingConnection
  |> Sql.query deleteAllJwtSecrets
  |> Sql.parameters  [ "@name", Sql.string name ]
  |> Sql.executeNonQuery










































let getJwtSecret = """-- name: GetJwtSecret :one
SELECT id, name, secret, audience FROM jwt_secrets WHERE name = @name
"""



let GetJwtSecret (db: NpgsqlConnection)  (name: string)  =
  
  let reader = fun (read:RowReader) -> {
    Id = read.int "id"
    Name = read.string "name"
    Secret = read.string "secret"
    Audience = read.string "audience"}
  

  db
  |> Sql.existingConnection
  |> Sql.query getJwtSecret
  |> Sql.parameters  [ "@name", Sql.string name ]
  |> Sql.executeRow reader
























































