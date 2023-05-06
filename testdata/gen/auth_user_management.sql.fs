// Code generated by sqlc. DO NOT EDIT.
// source: auth_user_management.sql


module AuthUserManagement 

open Npgsql
open Npgsql.FSharp
open System
































































let getRateLimit = """-- name: GetRateLimit :one
SELECT rate_limit AS rate_limit
FROM auth_user_management
WHERE user_id = @user_id
"""


// GetRateLimit retrieves the rate limit for a user from the auth_user_management table.
// If no rate limit is set for the user, it returns the default rate limit of 100.

let GetRateLimit (db: NpgsqlConnection) (userId: int32)  =
  
  let reader = fun (read:RowReader) -> read.int "rate_limit"

  db
  |> Sql.existingConnection
  |> Sql.query getRateLimit
  |> Sql.parameters  [ "@user_id", Sql.int userId ]
  |> Sql.executeRow reader




















































