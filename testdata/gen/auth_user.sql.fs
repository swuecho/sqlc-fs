// Code generated by sqlc. DO NOT EDIT.
// source: auth_user.sql


module AuthUser 

open Npgsql
open Npgsql.FSharp
open System










let createAuthUser = """-- name: CreateAuthUser :one
INSERT INTO auth_user (email, "password", first_name, last_name, username, is_staff, is_superuser)
VALUES (@email, @password, @first_name, @last_name, @username, @is_staff, @is_superuser)
RETURNING id, password, last_login, is_superuser, username, first_name, last_name, email, is_staff, is_active, date_joined
"""


type CreateAuthUserParams = {
  Email: string;
  Password: string;
  FirstName: string;
  LastName: string;
  Username: string;
  IsStaff: bool;
  IsSuperuser: bool;
}
type CreateAuthUserRow = {
  ID: int32;
  Password: string;
  LastLogin: DateTime;
  IsSuperuser: bool;
  Username: string;
  FirstName: string;
  LastName: string;
  Email: string;
  IsStaff: bool;
  IsActive: bool;
  DateJoined: DateTime;
}

let CreateAuthUser (db: NpgsqlConnection) (arg: CreateAuthUserParams)  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    Password = read.string "password"
    LastLogin = read.dateTime "last_login"
    IsSuperuser = read.bool "is_superuser"
    Username = read.string "username"
    FirstName = read.string "first_name"
    LastName = read.string "last_name"
    Email = read.string "email"
    IsStaff = read.bool "is_staff"
    IsActive = read.bool "is_active"
    DateJoined = read.dateTime "date_joined"}

  db
  |> Sql.existingConnection
  |> Sql.query createAuthUser
  |> Sql.parameters  [ "@email", Sql.string arg.Email; "@password", Sql.string arg.Password; "@first_name", Sql.string arg.FirstName; "@last_name", Sql.string arg.LastName; "@username", Sql.string arg.Username; "@is_staff", Sql.bool arg.IsStaff; "@is_superuser", Sql.bool arg.IsSuperuser ]
  |> Sql.execute reader
























let deleteAuthUser = """-- name: DeleteAuthUser :exec
DELETE FROM auth_user WHERE email = @email
"""






let DeleteAuthUser (db: NpgsqlConnection) (email: string)  = 
  db 
  |> Sql.existingConnection
  |> Sql.query deleteAuthUser
  |> Sql.parameters  [ "@email", Sql.string email ]
  |> Sql.executeNonQuery




















let getAllAuthUsers = """-- name: GetAllAuthUsers :many
SELECT id, password, last_login, is_superuser, username, first_name, last_name, email, is_staff, is_active, date_joined FROM auth_user ORDER BY id
"""


type GetAllAuthUsersRow = {
  ID: int32;
  Password: string;
  LastLogin: DateTime;
  IsSuperuser: bool;
  Username: string;
  FirstName: string;
  LastName: string;
  Email: string;
  IsStaff: bool;
  IsActive: bool;
  DateJoined: DateTime;
}


let GetAllAuthUsers (db: NpgsqlConnection) ()  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    Password = read.string "password"
    LastLogin = read.dateTime "last_login"
    IsSuperuser = read.bool "is_superuser"
    Username = read.string "username"
    FirstName = read.string "first_name"
    LastName = read.string "last_name"
    Email = read.string "email"
    IsStaff = read.bool "is_staff"
    IsActive = read.bool "is_active"
    DateJoined = read.dateTime "date_joined"}
  db 
  |> Sql.existingConnection
  |> sql.query getAllAuthUsers
  |> getAllAuthUsers
  |> Sql.execute reader













let getAuthUserByEmail = """-- name: GetAuthUserByEmail :one
SELECT id, password, last_login, is_superuser, username, first_name, last_name, email, is_staff, is_active, date_joined FROM auth_user WHERE email = @email
"""


type GetAuthUserByEmailRow = {
  ID: int32;
  Password: string;
  LastLogin: DateTime;
  IsSuperuser: bool;
  Username: string;
  FirstName: string;
  LastName: string;
  Email: string;
  IsStaff: bool;
  IsActive: bool;
  DateJoined: DateTime;
}

let GetAuthUserByEmail (db: NpgsqlConnection) (email: string)  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    Password = read.string "password"
    LastLogin = read.dateTime "last_login"
    IsSuperuser = read.bool "is_superuser"
    Username = read.string "username"
    FirstName = read.string "first_name"
    LastName = read.string "last_name"
    Email = read.string "email"
    IsStaff = read.bool "is_staff"
    IsActive = read.bool "is_active"
    DateJoined = read.dateTime "date_joined"}

  db
  |> Sql.existingConnection
  |> Sql.query getAuthUserByEmail
  |> Sql.parameters  [ "@email", Sql.string email ]
  |> Sql.execute reader












let getAuthUserByID = """-- name: GetAuthUserByID :one
SELECT id, password, last_login, is_superuser, username, first_name, last_name, email, is_staff, is_active, date_joined FROM auth_user WHERE id = @id
"""


type GetAuthUserByIDRow = {
  ID: int32;
  Password: string;
  LastLogin: DateTime;
  IsSuperuser: bool;
  Username: string;
  FirstName: string;
  LastName: string;
  Email: string;
  IsStaff: bool;
  IsActive: bool;
  DateJoined: DateTime;
}

let GetAuthUserByID (db: NpgsqlConnection) (id: int32)  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    Password = read.string "password"
    LastLogin = read.dateTime "last_login"
    IsSuperuser = read.bool "is_superuser"
    Username = read.string "username"
    FirstName = read.string "first_name"
    LastName = read.string "last_name"
    Email = read.string "email"
    IsStaff = read.bool "is_staff"
    IsActive = read.bool "is_active"
    DateJoined = read.dateTime "date_joined"}

  db
  |> Sql.existingConnection
  |> Sql.query getAuthUserByID
  |> Sql.parameters  [ "@id", Sql.int id ]
  |> Sql.execute reader


































let getTotalActiveUserCount = """-- name: GetTotalActiveUserCount :one
SELECT COUNT(*) FROM auth_user WHERE is_active = true
"""



let GetTotalActiveUserCount (db: NpgsqlConnection) ()  =
  let reader = fun (read:RowReader) -> {
     = read. ""}

  db
  |> Sql.existingConnection
  |> Sql.query getTotalActiveUserCount
  |> Sql.parameters  [  ]
  |> Sql.execute reader













let getUserByEmail = """-- name: GetUserByEmail :one
SELECT id, password, last_login, is_superuser, username, first_name, last_name, email, is_staff, is_active, date_joined FROM auth_user WHERE email = @email
"""


type GetUserByEmailRow = {
  ID: int32;
  Password: string;
  LastLogin: DateTime;
  IsSuperuser: bool;
  Username: string;
  FirstName: string;
  LastName: string;
  Email: string;
  IsStaff: bool;
  IsActive: bool;
  DateJoined: DateTime;
}

let GetUserByEmail (db: NpgsqlConnection) (email: string)  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    Password = read.string "password"
    LastLogin = read.dateTime "last_login"
    IsSuperuser = read.bool "is_superuser"
    Username = read.string "username"
    FirstName = read.string "first_name"
    LastName = read.string "last_name"
    Email = read.string "email"
    IsStaff = read.bool "is_staff"
    IsActive = read.bool "is_active"
    DateJoined = read.dateTime "date_joined"}

  db
  |> Sql.existingConnection
  |> Sql.query getUserByEmail
  |> Sql.parameters  [ "@email", Sql.string email ]
  |> Sql.execute reader












let getUserStats = """-- name: GetUserStats :many
SELECT 
    auth_user.first_name,
    auth_user.last_name,
    auth_user.email AS user_email,
    COALESCE(user_stats.total_messages, 0) AS total_chat_messages,
    COALESCE(user_stats.total_token_count, 0) AS total_token_count,
    COALESCE(user_stats.total_messages_3_days, 0) AS total_chat_messages_3_days,
    COALESCE(user_stats.total_token_count_3_days, 0) AS total_token_count_3_days,
    COALESCE(auth_user_management.rate_limit, @default_rate_limit::INTEGER) AS rate_limit
FROM auth_user
LEFT JOIN (
    SELECT chat_message_stats.user_id, 
           SUM(total_messages) AS total_messages, 
           SUM(total_token_count) AS total_token_count,
           SUM(CASE WHEN created_at >= NOW() - INTERVAL '3 days' THEN total_messages ELSE 0 END) AS total_messages_3_days,
           SUM(CASE WHEN created_at >= NOW() - INTERVAL '3 days' THEN total_token_count ELSE 0 END) AS total_token_count_3_days
    FROM (
        SELECT user_id, COUNT(*) AS total_messages, SUM(token_count) as total_token_count, MAX(created_at) AS created_at
        FROM chat_message
        GROUP BY user_id, chat_session_uuid
    ) AS chat_message_stats
    GROUP BY chat_message_stats.user_id
) AS user_stats ON auth_user.id = user_stats.user_id
LEFT JOIN auth_user_management ON auth_user.id = auth_user_management.user_id
ORDER BY total_chat_messages DESC, auth_user.id DESC
OFFSET @offset
LIMIT @limit
"""


type GetUserStatsParams = {
  Limit: int32;
  Offset: int32;
  DefaultRateLimit: int32;
}
type GetUserStatsRow = {
  FirstName: string;
  LastName: string;
  UserEmail: string;
  TotalChatMessages: int64;
  TotalTokenCount: int64;
  TotalChatMessages3Days: int64;
  TotalTokenCount3Days: int64;
  RateLimit: int32;
}


let GetUserStats (db: NpgsqlConnection) (arg: GetUserStatsParams)  =
  let reader = fun (read:RowReader) -> {
    FirstName = read.string "first_name"
    LastName = read.string "last_name"
    UserEmail = read.string "user_email"
    TotalChatMessages = read.int64 "total_chat_messages"
    TotalTokenCount = read.int64 "total_token_count"
    TotalChatMessages3Days = read.int64 "total_chat_messages_3_days"
    TotalTokenCount3Days = read.int64 "total_token_count_3_days"
    RateLimit = read.int "rate_limit"}
  db 
  |> Sql.existingConnection
  |> sql.query getUserStats
  |> getUserStats
  |> Sql.execute reader













let listAuthUsers = """-- name: ListAuthUsers :many
SELECT id, password, last_login, is_superuser, username, first_name, last_name, email, is_staff, is_active, date_joined FROM auth_user ORDER BY id LIMIT @limit OFFSET @offset
"""


type ListAuthUsersParams = {
  Limit: int32;
  Offset: int32;
}
type ListAuthUsersRow = {
  ID: int32;
  Password: string;
  LastLogin: DateTime;
  IsSuperuser: bool;
  Username: string;
  FirstName: string;
  LastName: string;
  Email: string;
  IsStaff: bool;
  IsActive: bool;
  DateJoined: DateTime;
}


let ListAuthUsers (db: NpgsqlConnection) (arg: ListAuthUsersParams)  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    Password = read.string "password"
    LastLogin = read.dateTime "last_login"
    IsSuperuser = read.bool "is_superuser"
    Username = read.string "username"
    FirstName = read.string "first_name"
    LastName = read.string "last_name"
    Email = read.string "email"
    IsStaff = read.bool "is_staff"
    IsActive = read.bool "is_active"
    DateJoined = read.dateTime "date_joined"}
  db 
  |> Sql.existingConnection
  |> sql.query listAuthUsers
  |> listAuthUsers
  |> Sql.execute reader



















let updateAuthUser = """-- name: UpdateAuthUser :one
UPDATE auth_user SET first_name = @first_name, last_name= @last_name, last_login = now() 
WHERE id = @id
RETURNING first_name, last_name, email
"""


type UpdateAuthUserParams = {
  ID: int32;
  FirstName: string;
  LastName: string;
}
type UpdateAuthUserRow = {
  FirstName: string;
  LastName: string;
  Email: string;
}

let UpdateAuthUser (db: NpgsqlConnection) (arg: UpdateAuthUserParams)  =
  let reader = fun (read:RowReader) -> {
    FirstName = read.string "first_name"
    LastName = read.string "last_name"
    Email = read.string "email"}

  db
  |> Sql.existingConnection
  |> Sql.query updateAuthUser
  |> Sql.parameters  [ "@id", Sql.int arg.ID; "@first_name", Sql.string arg.FirstName; "@last_name", Sql.string arg.LastName ]
  |> Sql.execute reader












let updateAuthUserByEmail = """-- name: UpdateAuthUserByEmail :one
UPDATE auth_user SET first_name = @first_name, last_name= @last_name, last_login = now() 
WHERE email = @email
RETURNING first_name, last_name, email
"""


type UpdateAuthUserByEmailParams = {
  Email: string;
  FirstName: string;
  LastName: string;
}
type UpdateAuthUserByEmailRow = {
  FirstName: string;
  LastName: string;
  Email: string;
}

let UpdateAuthUserByEmail (db: NpgsqlConnection) (arg: UpdateAuthUserByEmailParams)  =
  let reader = fun (read:RowReader) -> {
    FirstName = read.string "first_name"
    LastName = read.string "last_name"
    Email = read.string "email"}

  db
  |> Sql.existingConnection
  |> Sql.query updateAuthUserByEmail
  |> Sql.parameters  [ "@email", Sql.string arg.Email; "@first_name", Sql.string arg.FirstName; "@last_name", Sql.string arg.LastName ]
  |> Sql.execute reader












let updateAuthUserRateLimitByEmail = """-- name: UpdateAuthUserRateLimitByEmail :one
INSERT INTO auth_user_management (user_id, rate_limit, created_at, updated_at)
VALUES ((SELECT id FROM auth_user WHERE email = @email), @rate_limit, NOW(), NOW())
ON CONFLICT (user_id) DO UPDATE SET rate_limit = @rate_limit, updated_at = NOW()
RETURNING rate_limit
"""


type UpdateAuthUserRateLimitByEmailParams = {
  Email: string;
  RateLimit: int32;
}

let UpdateAuthUserRateLimitByEmail (db: NpgsqlConnection) (arg: UpdateAuthUserRateLimitByEmailParams)  =
  let reader = fun (read:RowReader) -> {
     = read. ""}

  db
  |> Sql.existingConnection
  |> Sql.query updateAuthUserRateLimitByEmail
  |> Sql.parameters  [ "@email", Sql.string arg.Email; "@rate_limit", Sql.int arg.RateLimit ]
  |> Sql.execute reader




























let updateUserPassword = """-- name: UpdateUserPassword :exec
UPDATE auth_user SET "password" = @password WHERE email = @email
"""


type UpdateUserPasswordParams = {
  Email: string;
  Password: string;
}




let UpdateUserPassword (db: NpgsqlConnection) (arg: UpdateUserPasswordParams)  = 
  db 
  |> Sql.existingConnection
  |> Sql.query updateUserPassword
  |> Sql.parameters  [ "@email", Sql.string arg.Email; "@password", Sql.string arg.Password ]
  |> Sql.executeNonQuery











