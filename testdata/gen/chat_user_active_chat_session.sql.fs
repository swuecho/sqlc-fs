// Code generated by sqlc. DO NOT EDIT.
// source: chat_user_active_chat_session.sql


module ChatUserActiveChatSession 

open Npgsql
open Npgsql.FSharp
open System




















let createOrUpdateUserActiveChatSession = """-- name: CreateOrUpdateUserActiveChatSession :one
INSERT INTO user_active_chat_session(user_id, chat_session_uuid)
VALUES (@user_id, @chat_session_uuid)
ON CONFLICT (user_id) 
DO UPDATE SET
chat_session_uuid = EXCLUDED.chat_session_uuid,
updated_at = now()
returning id, user_id, chat_session_uuid, created_at, updated_at
"""


type CreateOrUpdateUserActiveChatSessionParams = {
  UserID: int32;
  ChatSessionUuid: string;
}
type CreateOrUpdateUserActiveChatSessionRow = {
  ID: int32;
  UserID: int32;
  ChatSessionUuid: string;
  CreatedAt: DateTime;
  UpdatedAt: DateTime;
}

let CreateOrUpdateUserActiveChatSession (db: NpgsqlConnection) (arg: CreateOrUpdateUserActiveChatSessionParams)  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    UserID = read.int "user_id"
    ChatSessionUuid = read.string "chat_session_uuid"
    CreatedAt = read.dateTime "created_at"
    UpdatedAt = read.dateTime "updated_at"}

  db
  |> Sql.existingConnection
  |> Sql.query createOrUpdateUserActiveChatSession
  |> Sql.parameters  [ "@user_id", Sql.int arg.UserID; "@chat_session_uuid", Sql.string arg.ChatSessionUuid ]
  |> Sql.execute reader












let createUserActiveChatSession = """-- name: CreateUserActiveChatSession :one
INSERT INTO user_active_chat_session (user_id, chat_session_uuid)
VALUES (@user_id, @chat_session_uuid)
RETURNING id, user_id, chat_session_uuid, created_at, updated_at
"""


type CreateUserActiveChatSessionParams = {
  UserID: int32;
  ChatSessionUuid: string;
}
type CreateUserActiveChatSessionRow = {
  ID: int32;
  UserID: int32;
  ChatSessionUuid: string;
  CreatedAt: DateTime;
  UpdatedAt: DateTime;
}

let CreateUserActiveChatSession (db: NpgsqlConnection) (arg: CreateUserActiveChatSessionParams)  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    UserID = read.int "user_id"
    ChatSessionUuid = read.string "chat_session_uuid"
    CreatedAt = read.dateTime "created_at"
    UpdatedAt = read.dateTime "updated_at"}

  db
  |> Sql.existingConnection
  |> Sql.query createUserActiveChatSession
  |> Sql.parameters  [ "@user_id", Sql.int arg.UserID; "@chat_session_uuid", Sql.string arg.ChatSessionUuid ]
  |> Sql.execute reader
























let deleteUserActiveChatSession = """-- name: DeleteUserActiveChatSession :exec
DELETE FROM user_active_chat_session WHERE user_id = @user_id
"""






let DeleteUserActiveChatSession (db: NpgsqlConnection) (userID: int32)  = 
  db 
  |> Sql.existingConnection
  |> Sql.query deleteUserActiveChatSession
  |> Sql.parameters  [ "@user_id", Sql.int user_id ]
  |> Sql.executeNonQuery






































let getUserActiveChatSession = """-- name: GetUserActiveChatSession :one
SELECT id, user_id, chat_session_uuid, created_at, updated_at FROM user_active_chat_session WHERE user_id = @user_id
"""


type GetUserActiveChatSessionRow = {
  ID: int32;
  UserID: int32;
  ChatSessionUuid: string;
  CreatedAt: DateTime;
  UpdatedAt: DateTime;
}

let GetUserActiveChatSession (db: NpgsqlConnection) (userID: int32)  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    UserID = read.int "user_id"
    ChatSessionUuid = read.string "chat_session_uuid"
    CreatedAt = read.dateTime "created_at"
    UpdatedAt = read.dateTime "updated_at"}

  db
  |> Sql.existingConnection
  |> Sql.query getUserActiveChatSession
  |> Sql.parameters  [ "@user_id", Sql.int user_id ]
  |> Sql.execute reader






















let listUserActiveChatSessions = """-- name: ListUserActiveChatSessions :many
SELECT id, user_id, chat_session_uuid, created_at, updated_at FROM user_active_chat_session ORDER BY id
"""


type ListUserActiveChatSessionsRow = {
  ID: int32;
  UserID: int32;
  ChatSessionUuid: string;
  CreatedAt: DateTime;
  UpdatedAt: DateTime;
}


let ListUserActiveChatSessions (db: NpgsqlConnection) ()  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    UserID = read.int "user_id"
    ChatSessionUuid = read.string "chat_session_uuid"
    CreatedAt = read.dateTime "created_at"
    UpdatedAt = read.dateTime "updated_at"}
  db 
  |> Sql.existingConnection
  |> sql.query listUserActiveChatSessions
  |> listUserActiveChatSessions
  |> Sql.execute reader































let updateUserActiveChatSession = """-- name: UpdateUserActiveChatSession :one
UPDATE user_active_chat_session SET chat_session_uuid = @chat_session_uuid, updated_at = now()
WHERE user_id = @user_id
RETURNING id, user_id, chat_session_uuid, created_at, updated_at
"""


type UpdateUserActiveChatSessionParams = {
  ChatSessionUuid: string;
  UserID: int32;
}
type UpdateUserActiveChatSessionRow = {
  ID: int32;
  UserID: int32;
  ChatSessionUuid: string;
  CreatedAt: DateTime;
  UpdatedAt: DateTime;
}

let UpdateUserActiveChatSession (db: NpgsqlConnection) (arg: UpdateUserActiveChatSessionParams)  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    UserID = read.int "user_id"
    ChatSessionUuid = read.string "chat_session_uuid"
    CreatedAt = read.dateTime "created_at"
    UpdatedAt = read.dateTime "updated_at"}

  db
  |> Sql.existingConnection
  |> Sql.query updateUserActiveChatSession
  |> Sql.parameters  [ "@chat_session_uuid", Sql.string arg.ChatSessionUuid; "@user_id", Sql.int arg.UserID ]
  |> Sql.execute reader

















