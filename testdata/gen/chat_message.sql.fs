// Code generated by sqlc. DO NOT EDIT.
// source: chat_message.sql


module ChatMessage 

open Npgsql
open Npgsql.FSharp
open System













let createChatMessage = """-- name: CreateChatMessage :one
INSERT INTO chat_message (chat_session_uuid, uuid, role, content, token_count, score, user_id, created_by, updated_by, raw)
VALUES (@chat_session_uuid, @uuid, @role, @content, @token_count, @score, @user_id, @created_by, @updated_by, @chat_session_uuid0)
RETURNING id, uuid, chat_session_uuid, role, content, score, user_id, created_at, updated_at, created_by, updated_by, is_deleted, is_pin, token_count, raw
"""


type CreateChatMessageParams = {
  ChatSessionUuid: string;
  Uuid: string;
  Role: string;
  Content: string;
  TokenCount: int32;
  Score: float;
  UserId: int32;
  CreatedBy: int32;
  UpdatedBy: int32;
  Raw: string;
}

let CreateChatMessage (db: NpgsqlConnection)  (arg: CreateChatMessageParams)  =
  
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
    IsPin = read.bool "is_pin"
    TokenCount = read.int "token_count"
    Raw = read.string "raw"}
  

  db
  |> Sql.existingConnection
  |> Sql.query createChatMessage
  |> Sql.parameters  [ "@chat_session_uuid", Sql.string arg.ChatSessionUuid; "@uuid", Sql.string arg.Uuid; "@role", Sql.string arg.Role; "@content", Sql.string arg.Content; "@token_count", Sql.int arg.TokenCount; "@score", Sql.double arg.Score; "@user_id", Sql.int arg.UserId; "@created_by", Sql.int arg.CreatedBy; "@updated_by", Sql.int arg.UpdatedBy; "@raw", Sql.jsonb arg.Raw ]
  |> Sql.executeRow reader

























let deleteChatMessage = """-- name: DeleteChatMessage :exec
UPDATE chat_message set is_deleted = true, updated_at = now()
WHERE id = @id
"""






let DeleteChatMessage (db: NpgsqlConnection)  (id: int32)  = 
  db 
  |> Sql.existingConnection
  |> Sql.query deleteChatMessage
  |> Sql.parameters  [ "@id", Sql.int id ]
  |> Sql.executeNonQuery








let deleteChatMessageByUUID = """-- name: DeleteChatMessageByUUID :exec
UPDATE chat_message SET is_deleted = true, updated_at = now()
WHERE uuid = @uuid
"""






let DeleteChatMessageByUUID (db: NpgsqlConnection)  (uuid: string)  = 
  db 
  |> Sql.existingConnection
  |> Sql.query deleteChatMessageByUUID
  |> Sql.parameters  [ "@uuid", Sql.string uuid ]
  |> Sql.executeNonQuery








let deleteChatMessagesBySesionUUID = """-- name: DeleteChatMessagesBySesionUUID :exec
UPDATE chat_message 
SET is_deleted = true, updated_at = now()
WHERE is_deleted = false and is_pin = false and chat_session_uuid = @chat_session_uuid
"""






let DeleteChatMessagesBySesionUUID (db: NpgsqlConnection)  (chatSessionUuid: string)  = 
  db 
  |> Sql.existingConnection
  |> Sql.query deleteChatMessagesBySesionUUID
  |> Sql.parameters  [ "@chat_session_uuid", Sql.string chatSessionUuid ]
  |> Sql.executeNonQuery

















let getAllChatMessages = """-- name: GetAllChatMessages :many
SELECT id, uuid, chat_session_uuid, role, content, score, user_id, created_at, updated_at, created_by, updated_by, is_deleted, is_pin, token_count, raw FROM chat_message 
WHERE is_deleted = false
ORDER BY id
"""




let GetAllChatMessages (db: NpgsqlConnection)  =
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
    IsPin = read.bool "is_pin"
    TokenCount = read.int "token_count"
    Raw = read.string "raw"}
  
  db 
  |> Sql.existingConnection
  |> Sql.query getAllChatMessages
  |> Sql.execute reader














let getChatMessageByID = """-- name: GetChatMessageByID :one
SELECT id, uuid, chat_session_uuid, role, content, score, user_id, created_at, updated_at, created_by, updated_by, is_deleted, is_pin, token_count, raw FROM chat_message 
WHERE is_deleted = false and id = @id
"""



let GetChatMessageByID (db: NpgsqlConnection)  (id: int32)  =
  
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
    IsPin = read.bool "is_pin"
    TokenCount = read.int "token_count"
    Raw = read.string "raw"}
  

  db
  |> Sql.existingConnection
  |> Sql.query getChatMessageByID
  |> Sql.parameters  [ "@id", Sql.int id ]
  |> Sql.executeRow reader












let getChatMessageBySessionUUID = """-- name: GetChatMessageBySessionUUID :one
SELECT cm.id, cm.uuid, cm.chat_session_uuid, cm.role, cm.content, cm.score, cm.user_id, cm.created_at, cm.updated_at, cm.created_by, cm.updated_by, cm.is_deleted, cm.is_pin, cm.token_count, cm.raw
FROM chat_message cm
INNER JOIN chat_session cs ON cm.chat_session_uuid = cs.uuid
WHERE cm.is_deleted = false and cs.active = true and cs.uuid = @limit 
ORDER BY cm.id 
OFFSET @offset
LIMIT @limit
"""


type GetChatMessageBySessionUUIDParams = {
  Limit: int32;
  Offset: int32;
}

let GetChatMessageBySessionUUID (db: NpgsqlConnection)  (arg: GetChatMessageBySessionUUIDParams)  =
  
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
    IsPin = read.bool "is_pin"
    TokenCount = read.int "token_count"
    Raw = read.string "raw"}
  

  db
  |> Sql.existingConnection
  |> Sql.query getChatMessageBySessionUUID
  |> Sql.parameters  [ "@limit", Sql.int arg.Limit; "@offset", Sql.int arg.Offset ]
  |> Sql.executeRow reader












let getChatMessageByUUID = """-- name: GetChatMessageByUUID :one

SELECT id, uuid, chat_session_uuid, role, content, score, user_id, created_at, updated_at, created_by, updated_by, is_deleted, is_pin, token_count, raw FROM chat_message 
WHERE is_deleted = false and uuid = @uuid
"""


//-- UUID ----

let GetChatMessageByUUID (db: NpgsqlConnection)  (uuid: string)  =
  
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
    IsPin = read.bool "is_pin"
    TokenCount = read.int "token_count"
    Raw = read.string "raw"}
  

  db
  |> Sql.existingConnection
  |> Sql.query getChatMessageByUUID
  |> Sql.parameters  [ "@uuid", Sql.string uuid ]
  |> Sql.executeRow reader












let getChatMessagesBySessionUUID = """-- name: GetChatMessagesBySessionUUID :many
SELECT cm.id, cm.uuid, cm.chat_session_uuid, cm.role, cm.content, cm.score, cm.user_id, cm.created_at, cm.updated_at, cm.created_by, cm.updated_by, cm.is_deleted, cm.is_pin, cm.token_count, cm.raw
FROM chat_message cm
INNER JOIN chat_session cs ON cm.chat_session_uuid = cs.uuid
WHERE cm.is_deleted = false and cs.active = true and cs.uuid = @uuid  
ORDER BY cm.id 
OFFSET @offset
LIMIT @limit
"""


type GetChatMessagesBySessionUUIDParams = {
  Uuid: string;
  Offset: int32;
  Limit: int32;
}


let GetChatMessagesBySessionUUID (db: NpgsqlConnection)  (arg: GetChatMessagesBySessionUUIDParams) =
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
    IsPin = read.bool "is_pin"
    TokenCount = read.int "token_count"
    Raw = read.string "raw"}
  
  db 
  |> Sql.existingConnection
  |> Sql.query getChatMessagesBySessionUUID
  |> Sql.parameters  [ "@uuid", Sql.string arg.Uuid; "@offset", Sql.int arg.Offset; "@limit", Sql.int arg.Limit ]
  |> Sql.execute reader










let getChatMessagesCount = """-- name: GetChatMessagesCount :one
SELECT COUNT(*)
FROM chat_message
WHERE user_id = @user_id
AND created_at >= NOW() - INTERVAL '10 minutes'
"""


// Get total chat message count for user in last 10 minutes

let GetChatMessagesCount (db: NpgsqlConnection)  (userId: int32)  =
  
  let reader = fun (read:RowReader) -> read.int64 "count"

  db
  |> Sql.existingConnection
  |> Sql.query getChatMessagesCount
  |> Sql.parameters  [ "@user_id", Sql.int userId ]
  |> Sql.executeRow reader












let getChatMessagesCountByUserAndModel = """-- name: GetChatMessagesCountByUserAndModel :one
SELECT COUNT(*)
FROM chat_message cm
JOIN chat_session cs ON (cm.chat_session_uuid = cs.uuid AND cs.user_id = cm.user_id)
WHERE cm.user_id = @user_id
AND cs.model = @model 
AND cm.created_at >= NOW() - INTERVAL '10 minutes'
"""


type GetChatMessagesCountByUserAndModelParams = {
  UserId: int32;
  Model: string;
}
// Get total chat message count for user of model in last 10 minutes

let GetChatMessagesCountByUserAndModel (db: NpgsqlConnection)  (arg: GetChatMessagesCountByUserAndModelParams)  =
  
  let reader = fun (read:RowReader) -> read.int64 "count"

  db
  |> Sql.existingConnection
  |> Sql.query getChatMessagesCountByUserAndModel
  |> Sql.parameters  [ "@user_id", Sql.int arg.UserId; "@model", Sql.string arg.Model ]
  |> Sql.executeRow reader






















let getFirstMessageBySessionUUID = """-- name: GetFirstMessageBySessionUUID :one
SELECT id, uuid, chat_session_uuid, role, content, score, user_id, created_at, updated_at, created_by, updated_by, is_deleted, is_pin, token_count, raw
FROM chat_message
WHERE chat_session_uuid = @chat_session_uuid and is_deleted = false
ORDER BY created_at 
LIMIT 1
"""



let GetFirstMessageBySessionUUID (db: NpgsqlConnection)  (chatSessionUuid: string)  =
  
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
    IsPin = read.bool "is_pin"
    TokenCount = read.int "token_count"
    Raw = read.string "raw"}
  

  db
  |> Sql.existingConnection
  |> Sql.query getFirstMessageBySessionUUID
  |> Sql.parameters  [ "@chat_session_uuid", Sql.string chatSessionUuid ]
  |> Sql.executeRow reader













let getLastNChatMessages = """-- name: GetLastNChatMessages :many
SELECT id, uuid, chat_session_uuid, role, content, score, user_id, created_at, updated_at, created_by, updated_by, is_deleted, is_pin, token_count, raw
FROM chat_message
WHERE chat_message.id in (
    SELECT id
    FROM chat_message cm
    WHERE cm.chat_session_uuid = @chat_session_uuid and cm.is_deleted = false and cm.is_pin = true
    UNION
    (
        SELECT id 
        FROM chat_message cm
        WHERE cm.chat_session_uuid = @chat_session_uuid 
                AND cm.id < (SELECT id FROM chat_message WHERE chat_message.uuid = @uuid)
                AND cm.is_deleted = false -- and cm.is_pin = false
        ORDER BY cm.created_at DESC
        LIMIT @limit
    )
) 
ORDER BY created_at
"""


type GetLastNChatMessagesParams = {
  Uuid: string;
  Limit: int32;
  ChatSessionUuid: string;
}


let GetLastNChatMessages (db: NpgsqlConnection)  (arg: GetLastNChatMessagesParams) =
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
    IsPin = read.bool "is_pin"
    TokenCount = read.int "token_count"
    Raw = read.string "raw"}
  
  db 
  |> Sql.existingConnection
  |> Sql.query getLastNChatMessages
  |> Sql.parameters  [ "@uuid", Sql.string arg.Uuid; "@limit", Sql.int arg.Limit; "@chat_session_uuid", Sql.string arg.ChatSessionUuid ]
  |> Sql.execute reader










let getLatestMessagesBySessionUUID = """-- name: GetLatestMessagesBySessionUUID :many
SELECT id, uuid, chat_session_uuid, role, content, score, user_id, created_at, updated_at, created_by, updated_by, is_deleted, is_pin, token_count, raw
FROM chat_message
Where chat_message.id in 
(
    SELECT chat_message.id
    FROM chat_message
    WHERE chat_message.chat_session_uuid = @chat_session_uuid and chat_message.is_deleted = false and chat_message.is_pin = true
    UNION
    (
        SELECT chat_message.id
        FROM chat_message
        WHERE chat_message.chat_session_uuid = @chat_session_uuid and chat_message.is_deleted = false -- and chat_message.is_pin = false
        ORDER BY created_at DESC
        LIMIT @limit
    )
)
ORDER BY created_at
"""


type GetLatestMessagesBySessionUUIDParams = {
  ChatSessionUuid: string;
  Limit: int32;
}


let GetLatestMessagesBySessionUUID (db: NpgsqlConnection)  (arg: GetLatestMessagesBySessionUUIDParams) =
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
    IsPin = read.bool "is_pin"
    TokenCount = read.int "token_count"
    Raw = read.string "raw"}
  
  db 
  |> Sql.existingConnection
  |> Sql.query getLatestMessagesBySessionUUID
  |> Sql.parameters  [ "@chat_session_uuid", Sql.string arg.ChatSessionUuid; "@limit", Sql.int arg.Limit ]
  |> Sql.execute reader
















let hasChatMessagePermission = """-- name: HasChatMessagePermission :one
SELECT COUNT(*) > 0 as has_permission
FROM chat_message cm
INNER JOIN chat_session cs ON cm.chat_session_uuid = cs.uuid
INNER JOIN auth_user au ON cs.user_id = au.id
WHERE cm.is_deleted = false and  cm.id = @id AND (cs.user_id = @user_id OR au.is_superuser) and cs.active = true
"""


type HasChatMessagePermissionParams = {
  Id: int32;
  UserId: int32;
}

let HasChatMessagePermission (db: NpgsqlConnection)  (arg: HasChatMessagePermissionParams)  =
  
  let reader = fun (read:RowReader) -> read.bool "has_permission"

  db
  |> Sql.existingConnection
  |> Sql.query hasChatMessagePermission
  |> Sql.parameters  [ "@id", Sql.int arg.Id; "@user_id", Sql.int arg.UserId ]
  |> Sql.executeRow reader































let updateChatMessage = """-- name: UpdateChatMessage :one
UPDATE chat_message SET role = @role, content = @content, score = @score, user_id = @user_id, updated_by = @updated_by, updated_at = now()
WHERE id = @id
RETURNING id, uuid, chat_session_uuid, role, content, score, user_id, created_at, updated_at, created_by, updated_by, is_deleted, is_pin, token_count, raw
"""


type UpdateChatMessageParams = {
  Id: int32;
  Role: string;
  Content: string;
  Score: float;
  UserId: int32;
  UpdatedBy: int32;
}

let UpdateChatMessage (db: NpgsqlConnection)  (arg: UpdateChatMessageParams)  =
  
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
    IsPin = read.bool "is_pin"
    TokenCount = read.int "token_count"
    Raw = read.string "raw"}
  

  db
  |> Sql.existingConnection
  |> Sql.query updateChatMessage
  |> Sql.parameters  [ "@id", Sql.int arg.Id; "@role", Sql.string arg.Role; "@content", Sql.string arg.Content; "@score", Sql.double arg.Score; "@user_id", Sql.int arg.UserId; "@updated_by", Sql.int arg.UpdatedBy ]
  |> Sql.executeRow reader












let updateChatMessageByUUID = """-- name: UpdateChatMessageByUUID :one
UPDATE chat_message SET content = @content, is_pin = @is_pin, token_count = @token_count,  updated_at = now() 
WHERE uuid = @uuid
RETURNING id, uuid, chat_session_uuid, role, content, score, user_id, created_at, updated_at, created_by, updated_by, is_deleted, is_pin, token_count, raw
"""


type UpdateChatMessageByUUIDParams = {
  Uuid: string;
  Content: string;
  IsPin: bool;
  TokenCount: int32;
}

let UpdateChatMessageByUUID (db: NpgsqlConnection)  (arg: UpdateChatMessageByUUIDParams)  =
  
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
    IsPin = read.bool "is_pin"
    TokenCount = read.int "token_count"
    Raw = read.string "raw"}
  

  db
  |> Sql.existingConnection
  |> Sql.query updateChatMessageByUUID
  |> Sql.parameters  [ "@uuid", Sql.string arg.Uuid; "@content", Sql.string arg.Content; "@is_pin", Sql.bool arg.IsPin; "@token_count", Sql.int arg.TokenCount ]
  |> Sql.executeRow reader












let updateChatMessageContent = """-- name: UpdateChatMessageContent :exec
UPDATE chat_message
SET content = @content, updated_at = now(), token_count = @token_count
WHERE uuid = @uuid
"""


type UpdateChatMessageContentParams = {
  Uuid: string;
  Content: string;
  TokenCount: int32;
}




let UpdateChatMessageContent (db: NpgsqlConnection)  (arg: UpdateChatMessageContentParams)  = 
  db 
  |> Sql.existingConnection
  |> Sql.query updateChatMessageContent
  |> Sql.parameters  [ "@uuid", Sql.string arg.Uuid; "@content", Sql.string arg.Content; "@token_count", Sql.int arg.TokenCount ]
  |> Sql.executeNonQuery
























