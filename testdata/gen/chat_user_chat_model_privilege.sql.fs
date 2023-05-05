// Code generated by sqlc. DO NOT EDIT.
// source: chat_user_chat_model_privilege.sql


module ChatUserChatModelPrivilege 

open Npgsql
open Npgsql.FSharp
open System






















let createUserChatModelPrivilege = """-- name: CreateUserChatModelPrivilege :one
INSERT INTO user_chat_model_privilege (user_id, chat_model_id, rate_limit, created_by, updated_by)
VALUES (@user_id, @chat_model_id, @rate_limit, @created_by, @updated_by)
RETURNING id, user_id, chat_model_id, rate_limit, created_at, updated_at, created_by, updated_by
"""


type CreateUserChatModelPrivilegeParams = {
  UserId: int32;
  ChatModelId: int32;
  RateLimit: int32;
  CreatedBy: int32;
  UpdatedBy: int32;
}
type CreateUserChatModelPrivilegeRow = {
  Id: int32;
  UserId: int32;
  ChatModelId: int32;
  RateLimit: int32;
  CreatedAt: DateTime;
  UpdatedAt: DateTime;
  CreatedBy: int32;
  UpdatedBy: int32;
}

let CreateUserChatModelPrivilege (db: NpgsqlConnection) (arg: CreateUserChatModelPrivilegeParams)  =
  
  let reader = fun (read:RowReader) -> {
    Id = read.int "id"
    UserId = read.int "user_id"
    ChatModelId = read.int "chat_model_id"
    RateLimit = read.int "rate_limit"
    CreatedAt = read.dateTime "created_at"
    UpdatedAt = read.dateTime "updated_at"
    CreatedBy = read.int "created_by"
    UpdatedBy = read.int "updated_by"}
  

  db
  |> Sql.existingConnection
  |> Sql.query createUserChatModelPrivilege
  |> Sql.parameters  [ "@user_id", Sql.int arg.UserId; "@chat_model_id", Sql.int arg.ChatModelId; "@rate_limit", Sql.int arg.RateLimit; "@created_by", Sql.int arg.CreatedBy; "@updated_by", Sql.int arg.UpdatedBy ]
  |> Sql.execute reader
























let deleteUserChatModelPrivilege = """-- name: DeleteUserChatModelPrivilege :exec
DELETE FROM user_chat_model_privilege WHERE id = @id
"""






let DeleteUserChatModelPrivilege (db: NpgsqlConnection) (id: int32)  = 
  db 
  |> Sql.existingConnection
  |> Sql.query deleteUserChatModelPrivilege
  |> Sql.parameters  [ "@id", Sql.int id ]
  |> Sql.executeNonQuery

















































let listUserChatModelPrivileges = """-- name: ListUserChatModelPrivileges :many
SELECT id, user_id, chat_model_id, rate_limit, created_at, updated_at, created_by, updated_by FROM user_chat_model_privilege ORDER BY id
"""


type ListUserChatModelPrivilegesRow = {
  Id: int32;
  UserId: int32;
  ChatModelId: int32;
  RateLimit: int32;
  CreatedAt: DateTime;
  UpdatedAt: DateTime;
  CreatedBy: int32;
  UpdatedBy: int32;
}


let ListUserChatModelPrivileges (db: NpgsqlConnection) ()  =
  let reader = fun (read:RowReader) -> {
    Id = read.int "Id"
    UserId = read.int "UserId"
    ChatModelId = read.int "ChatModelId"
    RateLimit = read.int "RateLimit"
    CreatedAt = read.dateTime "CreatedAt"
    UpdatedAt = read.dateTime "UpdatedAt"
    CreatedBy = read.int "CreatedBy"
    UpdatedBy = read.int "UpdatedBy"}
  db 
  |> Sql.existingConnection
  |> Sql.query listUserChatModelPrivileges
  |> Sql.execute reader










let listUserChatModelPrivilegesByUserID = """-- name: ListUserChatModelPrivilegesByUserID :many

SELECT id, user_id, chat_model_id, rate_limit, created_at, updated_at, created_by, updated_by FROM user_chat_model_privilege 
WHERE user_id = @user_id
ORDER BY id
"""


type ListUserChatModelPrivilegesByUserIDRow = {
  Id: int32;
  UserId: int32;
  ChatModelId: int32;
  RateLimit: int32;
  CreatedAt: DateTime;
  UpdatedAt: DateTime;
  CreatedBy: int32;
  UpdatedBy: int32;
}


// TODO add ratelimit
// LIMIT 1000
let ListUserChatModelPrivilegesByUserID (db: NpgsqlConnection) (userId: int32)  =
  let reader = fun (read:RowReader) -> {
    Id = read.int "Id"
    UserId = read.int "UserId"
    ChatModelId = read.int "ChatModelId"
    RateLimit = read.int "RateLimit"
    CreatedAt = read.dateTime "CreatedAt"
    UpdatedAt = read.dateTime "UpdatedAt"
    CreatedBy = read.int "CreatedBy"
    UpdatedBy = read.int "UpdatedBy"}
  db 
  |> Sql.existingConnection
  |> Sql.query listUserChatModelPrivilegesByUserID
  |> Sql.execute reader










let listUserChatModelPrivilegesRateLimit = """-- name: ListUserChatModelPrivilegesRateLimit :many
SELECT ucmp.id, au.email as user_email, CONCAT_WS('',au.last_name, au.first_name) as full_name, cm.name chat_model_name, ucmp.rate_limit  
FROM user_chat_model_privilege ucmp 
INNER JOIN chat_model cm ON cm.id = ucmp.chat_model_id
INNER JOIN auth_user au ON au.id = ucmp.user_id
ORDER by au.last_login DESC
"""


type ListUserChatModelPrivilegesRateLimitRow = {
  Id: int32;
  UserEmail: string;
  FullName: string;
  ChatModelName: string;
  RateLimit: int32;
}


let ListUserChatModelPrivilegesRateLimit (db: NpgsqlConnection) ()  =
  let reader = fun (read:RowReader) -> {
    Id = read.int "Id"
    UserEmail = read.string "UserEmail"
    FullName = read.string "FullName"
    ChatModelName = read.string "ChatModelName"
    RateLimit = read.int "RateLimit"}
  db 
  |> Sql.existingConnection
  |> Sql.query listUserChatModelPrivilegesRateLimit
  |> Sql.execute reader










let rateLimiteByUserAndSessionUUID = """-- name: RateLimiteByUserAndSessionUUID :one
SELECT ucmp.rate_limit, cm.name AS chat_model_name
FROM user_chat_model_privilege ucmp
JOIN chat_session cs ON cs.user_id = ucmp.user_id
JOIN chat_model cm ON (cm.id = ucmp.chat_model_id AND cs.model = cm.name and cm.enable_per_mode_ratelimit = true)
WHERE cs.uuid = @uuid
  AND ucmp.user_id = @user_id
"""


type RateLimiteByUserAndSessionUUIDParams = {
  Uuid: string;
  UserId: int32;
}
type RateLimiteByUserAndSessionUUIDRow = {
  RateLimit: int32;
  ChatModelName: string;
}

let RateLimiteByUserAndSessionUUID (db: NpgsqlConnection) (arg: RateLimiteByUserAndSessionUUIDParams)  =
  
  let reader = fun (read:RowReader) -> {
    RateLimit = read.int "rate_limit"
    ChatModelName = read.string "chat_model_name"}
  

  db
  |> Sql.existingConnection
  |> Sql.query rateLimiteByUserAndSessionUUID
  |> Sql.parameters  [ "@uuid", Sql.string arg.Uuid; "@user_id", Sql.int arg.UserId ]
  |> Sql.execute reader






























let updateUserChatModelPrivilege = """-- name: UpdateUserChatModelPrivilege :one
UPDATE user_chat_model_privilege SET rate_limit = @rate_limit, updated_at = now(), updated_by = @updated_by
WHERE id = @id
RETURNING id, user_id, chat_model_id, rate_limit, created_at, updated_at, created_by, updated_by
"""


type UpdateUserChatModelPrivilegeParams = {
  Id: int32;
  RateLimit: int32;
  UpdatedBy: int32;
}
type UpdateUserChatModelPrivilegeRow = {
  Id: int32;
  UserId: int32;
  ChatModelId: int32;
  RateLimit: int32;
  CreatedAt: DateTime;
  UpdatedAt: DateTime;
  CreatedBy: int32;
  UpdatedBy: int32;
}

let UpdateUserChatModelPrivilege (db: NpgsqlConnection) (arg: UpdateUserChatModelPrivilegeParams)  =
  
  let reader = fun (read:RowReader) -> {
    Id = read.int "id"
    UserId = read.int "user_id"
    ChatModelId = read.int "chat_model_id"
    RateLimit = read.int "rate_limit"
    CreatedAt = read.dateTime "created_at"
    UpdatedAt = read.dateTime "updated_at"
    CreatedBy = read.int "created_by"
    UpdatedBy = read.int "updated_by"}
  

  db
  |> Sql.existingConnection
  |> Sql.query updateUserChatModelPrivilege
  |> Sql.parameters  [ "@id", Sql.int arg.Id; "@rate_limit", Sql.int arg.RateLimit; "@updated_by", Sql.int arg.UpdatedBy ]
  |> Sql.execute reader













let userChatModelPrivilegeByID = """-- name: UserChatModelPrivilegeByID :one
SELECT id, user_id, chat_model_id, rate_limit, created_at, updated_at, created_by, updated_by FROM user_chat_model_privilege WHERE id = @id
"""


type UserChatModelPrivilegeByIDRow = {
  Id: int32;
  UserId: int32;
  ChatModelId: int32;
  RateLimit: int32;
  CreatedAt: DateTime;
  UpdatedAt: DateTime;
  CreatedBy: int32;
  UpdatedBy: int32;
}

let UserChatModelPrivilegeByID (db: NpgsqlConnection) (id: int32)  =
  
  let reader = fun (read:RowReader) -> {
    Id = read.int "id"
    UserId = read.int "user_id"
    ChatModelId = read.int "chat_model_id"
    RateLimit = read.int "rate_limit"
    CreatedAt = read.dateTime "created_at"
    UpdatedAt = read.dateTime "updated_at"
    CreatedBy = read.int "created_by"
    UpdatedBy = read.int "updated_by"}
  

  db
  |> Sql.existingConnection
  |> Sql.query userChatModelPrivilegeByID
  |> Sql.parameters  [ "@id", Sql.int id ]
  |> Sql.execute reader












let userChatModelPrivilegeByUserAndModelID = """-- name: UserChatModelPrivilegeByUserAndModelID :one
SELECT id, user_id, chat_model_id, rate_limit, created_at, updated_at, created_by, updated_by FROM user_chat_model_privilege WHERE user_id = @user_id AND chat_model_id = @chat_model_id
"""


type UserChatModelPrivilegeByUserAndModelIDParams = {
  UserId: int32;
  ChatModelId: int32;
}
type UserChatModelPrivilegeByUserAndModelIDRow = {
  Id: int32;
  UserId: int32;
  ChatModelId: int32;
  RateLimit: int32;
  CreatedAt: DateTime;
  UpdatedAt: DateTime;
  CreatedBy: int32;
  UpdatedBy: int32;
}

let UserChatModelPrivilegeByUserAndModelID (db: NpgsqlConnection) (arg: UserChatModelPrivilegeByUserAndModelIDParams)  =
  
  let reader = fun (read:RowReader) -> {
    Id = read.int "id"
    UserId = read.int "user_id"
    ChatModelId = read.int "chat_model_id"
    RateLimit = read.int "rate_limit"
    CreatedAt = read.dateTime "created_at"
    UpdatedAt = read.dateTime "updated_at"
    CreatedBy = read.int "created_by"
    UpdatedBy = read.int "updated_by"}
  

  db
  |> Sql.existingConnection
  |> Sql.query userChatModelPrivilegeByUserAndModelID
  |> Sql.parameters  [ "@user_id", Sql.int arg.UserId; "@chat_model_id", Sql.int arg.ChatModelId ]
  |> Sql.execute reader













