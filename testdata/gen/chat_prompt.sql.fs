// Code generated by sqlc. DO NOT EDIT.
// source: chat_prompt.sql


module ChatPrompt 

open Npgsql
open Npgsql.FSharp
open System

















let createChatPrompt = """-- name: CreateChatPrompt :one
INSERT INTO chat_prompt (uuid, chat_session_uuid, role, content, token_count, user_id, created_by, updated_by)
VALUES (@uuid, @chat_session_uuid, @role, @content, @token_count, @user_id, @created_by, @updated_by)
RETURNING id, uuid, chat_session_uuid, role, content, score, user_id, created_at, updated_at, created_by, updated_by, is_deleted, token_count
"""


type CreateChatPromptParams = {
  Uuid: string;
  ChatSessionUuid: string;
  Role: string;
  Content: string;
  TokenCount: int32;
  UserId: int32;
  CreatedBy: int32;
  UpdatedBy: int32;
}

let CreateChatPrompt (db: NpgsqlConnection)  (arg: CreateChatPromptParams)  =
  
  let reader = fun (read:RowReader) -> {
    Id = read.int "id"
    Uuid = read.string "uuid"
    ChatSessionUuid = read.string "chat_session_uuid"
    Role = read.string "role"
    Content = read.string "content"
    Score = read.double "score"
    UserId = read.int "user_id"
    CreatedAt = read.dateTime "created_at"
    UpdatedAt = read.dateTime "updated_at"
    CreatedBy = read.int "created_by"
    UpdatedBy = read.int "updated_by"
    IsDeleted = read.bool "is_deleted"
    TokenCount = read.int "token_count"}
  

  db
  |> Sql.existingConnection
  |> Sql.query createChatPrompt
  |> Sql.parameters  [ "@uuid", Sql.string arg.Uuid; "@chat_session_uuid", Sql.string arg.ChatSessionUuid; "@role", Sql.string arg.Role; "@content", Sql.string arg.Content; "@token_count", Sql.int arg.TokenCount; "@user_id", Sql.int arg.UserId; "@created_by", Sql.int arg.CreatedBy; "@updated_by", Sql.int arg.UpdatedBy ]
  |> Sql.executeRow reader



























let deleteChatPrompt = """-- name: DeleteChatPrompt :exec
UPDATE chat_prompt 
SET is_deleted = true, updated_at = now()
WHERE id = @id
"""






let DeleteChatPrompt (db: NpgsqlConnection)  (id: int32)  = 
  db 
  |> Sql.existingConnection
  |> Sql.query deleteChatPrompt
  |> Sql.parameters  [ "@id", Sql.int id ]
  |> Sql.executeNonQuery








let deleteChatPromptByUUID = """-- name: DeleteChatPromptByUUID :exec
UPDATE chat_prompt
SET is_deleted = true, updated_at = now()
WHERE uuid = @uuid
"""






let DeleteChatPromptByUUID (db: NpgsqlConnection)  (uuid: string)  = 
  db 
  |> Sql.existingConnection
  |> Sql.query deleteChatPromptByUUID
  |> Sql.parameters  [ "@uuid", Sql.string uuid ]
  |> Sql.executeNonQuery















let getAllChatPrompts = """-- name: GetAllChatPrompts :many
SELECT id, uuid, chat_session_uuid, role, content, score, user_id, created_at, updated_at, created_by, updated_by, is_deleted, token_count FROM chat_prompt 
WHERE is_deleted = false
ORDER BY id
"""




let GetAllChatPrompts (db: NpgsqlConnection)  =
  let reader = fun (read:RowReader) -> {
    Id = read.int "id"
    Uuid = read.string "uuid"
    ChatSessionUuid = read.string "chat_session_uuid"
    Role = read.string "role"
    Content = read.string "content"
    Score = read.double "score"
    UserId = read.int "user_id"
    CreatedAt = read.dateTime "created_at"
    UpdatedAt = read.dateTime "updated_at"
    CreatedBy = read.int "created_by"
    UpdatedBy = read.int "updated_by"
    IsDeleted = read.bool "is_deleted"
    TokenCount = read.int "token_count"}
  db 
  |> Sql.existingConnection
  |> Sql.query getAllChatPrompts
  |> Sql.execute reader



















let getChatPromptByID = """-- name: GetChatPromptByID :one
SELECT id, uuid, chat_session_uuid, role, content, score, user_id, created_at, updated_at, created_by, updated_by, is_deleted, token_count FROM chat_prompt
WHERE is_deleted = false and  id = @id
"""



let GetChatPromptByID (db: NpgsqlConnection)  (id: int32)  =
  
  let reader = fun (read:RowReader) -> {
    Id = read.int "id"
    Uuid = read.string "uuid"
    ChatSessionUuid = read.string "chat_session_uuid"
    Role = read.string "role"
    Content = read.string "content"
    Score = read.double "score"
    UserId = read.int "user_id"
    CreatedAt = read.dateTime "created_at"
    UpdatedAt = read.dateTime "updated_at"
    CreatedBy = read.int "created_by"
    UpdatedBy = read.int "updated_by"
    IsDeleted = read.bool "is_deleted"
    TokenCount = read.int "token_count"}
  

  db
  |> Sql.existingConnection
  |> Sql.query getChatPromptByID
  |> Sql.parameters  [ "@id", Sql.int id ]
  |> Sql.executeRow reader












let getChatPromptByUUID = """-- name: GetChatPromptByUUID :one
SELECT id, uuid, chat_session_uuid, role, content, score, user_id, created_at, updated_at, created_by, updated_by, is_deleted, token_count FROM chat_prompt
WHERE uuid = @uuid
"""



let GetChatPromptByUUID (db: NpgsqlConnection)  (uuid: string)  =
  
  let reader = fun (read:RowReader) -> {
    Id = read.int "id"
    Uuid = read.string "uuid"
    ChatSessionUuid = read.string "chat_session_uuid"
    Role = read.string "role"
    Content = read.string "content"
    Score = read.double "score"
    UserId = read.int "user_id"
    CreatedAt = read.dateTime "created_at"
    UpdatedAt = read.dateTime "updated_at"
    CreatedBy = read.int "created_by"
    UpdatedBy = read.int "updated_by"
    IsDeleted = read.bool "is_deleted"
    TokenCount = read.int "token_count"}
  

  db
  |> Sql.existingConnection
  |> Sql.query getChatPromptByUUID
  |> Sql.parameters  [ "@uuid", Sql.string uuid ]
  |> Sql.executeRow reader












let getChatPromptsBySessionUUID = """-- name: GetChatPromptsBySessionUUID :many
SELECT id, uuid, chat_session_uuid, role, content, score, user_id, created_at, updated_at, created_by, updated_by, is_deleted, token_count
FROM chat_prompt 
WHERE chat_session_uuid = @chat_session_uuid and is_deleted = false
ORDER BY id
"""




let GetChatPromptsBySessionUUID (db: NpgsqlConnection)  (chatSessionUuid: string) =
  let reader = fun (read:RowReader) -> {
    Id = read.int "id"
    Uuid = read.string "uuid"
    ChatSessionUuid = read.string "chat_session_uuid"
    Role = read.string "role"
    Content = read.string "content"
    Score = read.double "score"
    UserId = read.int "user_id"
    CreatedAt = read.dateTime "created_at"
    UpdatedAt = read.dateTime "updated_at"
    CreatedBy = read.int "created_by"
    UpdatedBy = read.int "updated_by"
    IsDeleted = read.bool "is_deleted"
    TokenCount = read.int "token_count"}
  db 
  |> Sql.existingConnection
  |> Sql.query getChatPromptsBySessionUUID
  |> Sql.execute reader










let getChatPromptsByUserID = """-- name: GetChatPromptsByUserID :many
SELECT id, uuid, chat_session_uuid, role, content, score, user_id, created_at, updated_at, created_by, updated_by, is_deleted, token_count
FROM chat_prompt 
WHERE user_id = @user_id and is_deleted = false
ORDER BY id
"""




let GetChatPromptsByUserID (db: NpgsqlConnection)  (userId: int32) =
  let reader = fun (read:RowReader) -> {
    Id = read.int "id"
    Uuid = read.string "uuid"
    ChatSessionUuid = read.string "chat_session_uuid"
    Role = read.string "role"
    Content = read.string "content"
    Score = read.double "score"
    UserId = read.int "user_id"
    CreatedAt = read.dateTime "created_at"
    UpdatedAt = read.dateTime "updated_at"
    CreatedBy = read.int "created_by"
    UpdatedBy = read.int "updated_by"
    IsDeleted = read.bool "is_deleted"
    TokenCount = read.int "token_count"}
  db 
  |> Sql.existingConnection
  |> Sql.query getChatPromptsByUserID
  |> Sql.execute reader










let getChatPromptsBysession_uuid = """-- name: GetChatPromptsBysession_uuid :many
SELECT id, uuid, chat_session_uuid, role, content, score, user_id, created_at, updated_at, created_by, updated_by, is_deleted, token_count
FROM chat_prompt 
WHERE chat_session_uuid = @chat_session_uuid and is_deleted = false
ORDER BY id
"""




let GetChatPromptsBysession_uuid (db: NpgsqlConnection)  (chatSessionUuid: string) =
  let reader = fun (read:RowReader) -> {
    Id = read.int "id"
    Uuid = read.string "uuid"
    ChatSessionUuid = read.string "chat_session_uuid"
    Role = read.string "role"
    Content = read.string "content"
    Score = read.double "score"
    UserId = read.int "user_id"
    CreatedAt = read.dateTime "created_at"
    UpdatedAt = read.dateTime "updated_at"
    CreatedBy = read.int "created_by"
    UpdatedBy = read.int "updated_by"
    IsDeleted = read.bool "is_deleted"
    TokenCount = read.int "token_count"}
  db 
  |> Sql.existingConnection
  |> Sql.query getChatPromptsBysession_uuid
  |> Sql.execute reader



















let getOneChatPromptBySessionUUID = """-- name: GetOneChatPromptBySessionUUID :one
SELECT id, uuid, chat_session_uuid, role, content, score, user_id, created_at, updated_at, created_by, updated_by, is_deleted, token_count
FROM chat_prompt 
WHERE chat_session_uuid = @chat_session_uuid and is_deleted = false
ORDER BY id
LIMIT 1
"""



let GetOneChatPromptBySessionUUID (db: NpgsqlConnection)  (chatSessionUuid: string)  =
  
  let reader = fun (read:RowReader) -> {
    Id = read.int "id"
    Uuid = read.string "uuid"
    ChatSessionUuid = read.string "chat_session_uuid"
    Role = read.string "role"
    Content = read.string "content"
    Score = read.double "score"
    UserId = read.int "user_id"
    CreatedAt = read.dateTime "created_at"
    UpdatedAt = read.dateTime "updated_at"
    CreatedBy = read.int "created_by"
    UpdatedBy = read.int "updated_by"
    IsDeleted = read.bool "is_deleted"
    TokenCount = read.int "token_count"}
  

  db
  |> Sql.existingConnection
  |> Sql.query getOneChatPromptBySessionUUID
  |> Sql.parameters  [ "@chat_session_uuid", Sql.string chatSessionUuid ]
  |> Sql.executeRow reader


















let hasChatPromptPermission = """-- name: HasChatPromptPermission :one
SELECT COUNT(*) > 0 as has_permission
FROM chat_prompt cp
INNER JOIN auth_user au ON cp.user_id = au.id
WHERE cp.id = @id AND (cp.user_id = @user_id OR au.is_superuser) AND cp.is_deleted = false
"""


type HasChatPromptPermissionParams = {
  Id: int32;
  UserId: int32;
}

let HasChatPromptPermission (db: NpgsqlConnection)  (arg: HasChatPromptPermissionParams)  =
  
  let reader = fun (read:RowReader) -> read.bool "has_permission"

  db
  |> Sql.existingConnection
  |> Sql.query hasChatPromptPermission
  |> Sql.parameters  [ "@id", Sql.int arg.Id; "@user_id", Sql.int arg.UserId ]
  |> Sql.executeRow reader
































let updateChatPrompt = """-- name: UpdateChatPrompt :one
UPDATE chat_prompt SET chat_session_uuid = @chat_session_uuid, role = @role, content = @content, score = @score, user_id = @user_id, updated_at = now(), updated_by = @updated_by
WHERE id = @id
RETURNING id, uuid, chat_session_uuid, role, content, score, user_id, created_at, updated_at, created_by, updated_by, is_deleted, token_count
"""


type UpdateChatPromptParams = {
  Id: int32;
  ChatSessionUuid: string;
  Role: string;
  Content: string;
  Score: float;
  UserId: int32;
  UpdatedBy: int32;
}

let UpdateChatPrompt (db: NpgsqlConnection)  (arg: UpdateChatPromptParams)  =
  
  let reader = fun (read:RowReader) -> {
    Id = read.int "id"
    Uuid = read.string "uuid"
    ChatSessionUuid = read.string "chat_session_uuid"
    Role = read.string "role"
    Content = read.string "content"
    Score = read.double "score"
    UserId = read.int "user_id"
    CreatedAt = read.dateTime "created_at"
    UpdatedAt = read.dateTime "updated_at"
    CreatedBy = read.int "created_by"
    UpdatedBy = read.int "updated_by"
    IsDeleted = read.bool "is_deleted"
    TokenCount = read.int "token_count"}
  

  db
  |> Sql.existingConnection
  |> Sql.query updateChatPrompt
  |> Sql.parameters  [ "@id", Sql.int arg.Id; "@chat_session_uuid", Sql.string arg.ChatSessionUuid; "@role", Sql.string arg.Role; "@content", Sql.string arg.Content; "@score", Sql.double arg.Score; "@user_id", Sql.int arg.UserId; "@updated_by", Sql.int arg.UpdatedBy ]
  |> Sql.executeRow reader












let updateChatPromptByUUID = """-- name: UpdateChatPromptByUUID :one
UPDATE chat_prompt SET content = @content, token_count = @token_count, updated_at = now()
WHERE uuid = @uuid and is_deleted = false
RETURNING id, uuid, chat_session_uuid, role, content, score, user_id, created_at, updated_at, created_by, updated_by, is_deleted, token_count
"""


type UpdateChatPromptByUUIDParams = {
  Uuid: string;
  Content: string;
  TokenCount: int32;
}

let UpdateChatPromptByUUID (db: NpgsqlConnection)  (arg: UpdateChatPromptByUUIDParams)  =
  
  let reader = fun (read:RowReader) -> {
    Id = read.int "id"
    Uuid = read.string "uuid"
    ChatSessionUuid = read.string "chat_session_uuid"
    Role = read.string "role"
    Content = read.string "content"
    Score = read.double "score"
    UserId = read.int "user_id"
    CreatedAt = read.dateTime "created_at"
    UpdatedAt = read.dateTime "updated_at"
    CreatedBy = read.int "created_by"
    UpdatedBy = read.int "updated_by"
    IsDeleted = read.bool "is_deleted"
    TokenCount = read.int "token_count"}
  

  db
  |> Sql.existingConnection
  |> Sql.query updateChatPromptByUUID
  |> Sql.parameters  [ "@uuid", Sql.string arg.Uuid; "@content", Sql.string arg.Content; "@token_count", Sql.int arg.TokenCount ]
  |> Sql.executeRow reader
























