// Code generated by sqlc. DO NOT EDIT.
// source: chat_session.sql


module ChatSession 

open Npgsql
open Npgsql.FSharp
open System















let createChatSession = """-- name: CreateChatSession :one
INSERT INTO chat_session (user_id, topic, max_length, uuid)
VALUES (@user_id, @topic, @max_length, @uuid)
RETURNING id, user_id, uuid, topic, created_at, updated_at, active, model, max_length, temperature, top_p, max_tokens, n, debug
"""


type CreateChatSessionParams = {
  UserID: int32;
  Topic: string;
  MaxLength: int32;
  Uuid: string;
}
type CreateChatSessionRow = {
  ID: int32;
  UserID: int32;
  Uuid: string;
  Topic: string;
  CreatedAt: DateTime;
  UpdatedAt: DateTime;
  Active: bool;
  Model: string;
  MaxLength: int32;
  Temperature: float64;
  TopP: float64;
  MaxTokens: int32;
  N: int32;
  Debug: bool;
}

let CreateChatSession (db: NpgsqlConnection) (arg: CreateChatSessionParams)  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    UserID = read.int "user_id"
    Uuid = read.string "uuid"
    Topic = read.string "topic"
    CreatedAt = read.dateTime "created_at"
    UpdatedAt = read.dateTime "updated_at"
    Active = read.bool "active"
    Model = read.string "model"
    MaxLength = read.int "max_length"
    Temperature = read.float64 "temperature"
    TopP = read.float64 "top_p"
    MaxTokens = read.int "max_tokens"
    N = read.int "n"
    Debug = read.bool "debug"}

  db
  |> Sql.existingConnection
  |> Sql.query createChatSession
  |> Sql.parameters  [ "@user_id", Sql.int arg.UserID; "@topic", Sql.string arg.Topic; "@max_length", Sql.int arg.MaxLength; "@uuid", Sql.string arg.Uuid ]
  |> Sql.execute reader












let createChatSessionByUUID = """-- name: CreateChatSessionByUUID :one
INSERT INTO chat_session (user_id, uuid, topic, created_at, active,  max_length)
VALUES (@user_id, @uuid, @topic, @created_at, @active, @max_length)
RETURNING id, user_id, uuid, topic, created_at, updated_at, active, model, max_length, temperature, top_p, max_tokens, n, debug
"""


type CreateChatSessionByUUIDParams = {
  UserID: int32;
  Uuid: string;
  Topic: string;
  CreatedAt: DateTime;
  Active: bool;
  MaxLength: int32;
}
type CreateChatSessionByUUIDRow = {
  ID: int32;
  UserID: int32;
  Uuid: string;
  Topic: string;
  CreatedAt: DateTime;
  UpdatedAt: DateTime;
  Active: bool;
  Model: string;
  MaxLength: int32;
  Temperature: float64;
  TopP: float64;
  MaxTokens: int32;
  N: int32;
  Debug: bool;
}

let CreateChatSessionByUUID (db: NpgsqlConnection) (arg: CreateChatSessionByUUIDParams)  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    UserID = read.int "user_id"
    Uuid = read.string "uuid"
    Topic = read.string "topic"
    CreatedAt = read.dateTime "created_at"
    UpdatedAt = read.dateTime "updated_at"
    Active = read.bool "active"
    Model = read.string "model"
    MaxLength = read.int "max_length"
    Temperature = read.float64 "temperature"
    TopP = read.float64 "top_p"
    MaxTokens = read.int "max_tokens"
    N = read.int "n"
    Debug = read.bool "debug"}

  db
  |> Sql.existingConnection
  |> Sql.query createChatSessionByUUID
  |> Sql.parameters  [ "@user_id", Sql.int arg.UserID; "@uuid", Sql.string arg.Uuid; "@topic", Sql.string arg.Topic; "@created_at", Sql.dateTime arg.CreatedAt; "@active", Sql.bool arg.Active; "@max_length", Sql.int arg.MaxLength ]
  |> Sql.execute reader














let createOrUpdateChatSessionByUUID = """-- name: CreateOrUpdateChatSessionByUUID :one
INSERT INTO chat_session(uuid, user_id, topic, max_length, temperature, model, max_tokens, top_p, n, debug)
VALUES (@uuid, @user_id, @topic, @max_length, @temperature, @model, @max_tokens, @top_p, @n, @uuid0)
ON CONFLICT (uuid) 
DO UPDATE SET
max_length = EXCLUDED.max_length, 
debug = EXCLUDED.debug,
max_tokens = EXCLUDED.max_tokens,
temperature = EXCLUDED.temperature, 
top_p = EXCLUDED.top_p,
n= EXCLUDED.n,
model = EXCLUDED.model,
topic = CASE WHEN chat_session.topic IS NULL THEN EXCLUDED.topic ELSE chat_session.topic END,
updated_at = now()
returning id, user_id, uuid, topic, created_at, updated_at, active, model, max_length, temperature, top_p, max_tokens, n, debug
"""


type CreateOrUpdateChatSessionByUUIDParams = {
  Uuid: string;
  UserID: int32;
  Topic: string;
  MaxLength: int32;
  Temperature: float64;
  Model: string;
  MaxTokens: int32;
  TopP: float64;
  N: int32;
  Debug: bool;
}
type CreateOrUpdateChatSessionByUUIDRow = {
  ID: int32;
  UserID: int32;
  Uuid: string;
  Topic: string;
  CreatedAt: DateTime;
  UpdatedAt: DateTime;
  Active: bool;
  Model: string;
  MaxLength: int32;
  Temperature: float64;
  TopP: float64;
  MaxTokens: int32;
  N: int32;
  Debug: bool;
}

let CreateOrUpdateChatSessionByUUID (db: NpgsqlConnection) (arg: CreateOrUpdateChatSessionByUUIDParams)  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    UserID = read.int "user_id"
    Uuid = read.string "uuid"
    Topic = read.string "topic"
    CreatedAt = read.dateTime "created_at"
    UpdatedAt = read.dateTime "updated_at"
    Active = read.bool "active"
    Model = read.string "model"
    MaxLength = read.int "max_length"
    Temperature = read.float64 "temperature"
    TopP = read.float64 "top_p"
    MaxTokens = read.int "max_tokens"
    N = read.int "n"
    Debug = read.bool "debug"}

  db
  |> Sql.existingConnection
  |> Sql.query createOrUpdateChatSessionByUUID
  |> Sql.parameters  [ "@uuid", Sql.string arg.Uuid; "@user_id", Sql.int arg.UserID; "@topic", Sql.string arg.Topic; "@max_length", Sql.int arg.MaxLength; "@temperature", Sql.float64 arg.Temperature; "@model", Sql.string arg.Model; "@max_tokens", Sql.int arg.MaxTokens; "@top_p", Sql.float64 arg.TopP; "@n", Sql.int arg.N; "@debug", Sql.bool arg.Debug ]
  |> Sql.execute reader























let deleteChatSession = """-- name: DeleteChatSession :exec
DELETE FROM chat_session 
WHERE id = @id
"""






let DeleteChatSession (db: NpgsqlConnection) (id: int32)  = 
  db 
  |> Sql.existingConnection
  |> Sql.query deleteChatSession
  |> Sql.parameters  [ "@id", Sql.int id ]
  |> Sql.executeNonQuery








let deleteChatSessionByUUID = """-- name: DeleteChatSessionByUUID :exec
update chat_session set active = false
WHERE uuid = @uuid
returning id, user_id, uuid, topic, created_at, updated_at, active, model, max_length, temperature, top_p, max_tokens, n, debug
"""


type DeleteChatSessionByUUIDRow = {
  ID: int32;
  UserID: int32;
  Uuid: string;
  Topic: string;
  CreatedAt: DateTime;
  UpdatedAt: DateTime;
  Active: bool;
  Model: string;
  MaxLength: int32;
  Temperature: float64;
  TopP: float64;
  MaxTokens: int32;
  N: int32;
  Debug: bool;
}




let DeleteChatSessionByUUID (db: NpgsqlConnection) (uuid: string)  = 
  db 
  |> Sql.existingConnection
  |> Sql.query deleteChatSessionByUUID
  |> Sql.parameters  [ "@uuid", Sql.string uuid ]
  |> Sql.executeNonQuery














let getAllChatSessions = """-- name: GetAllChatSessions :many
SELECT id, user_id, uuid, topic, created_at, updated_at, active, model, max_length, temperature, top_p, max_tokens, n, debug FROM chat_session 
where active = true
ORDER BY id
"""


type GetAllChatSessionsRow = {
  ID: int32;
  UserID: int32;
  Uuid: string;
  Topic: string;
  CreatedAt: DateTime;
  UpdatedAt: DateTime;
  Active: bool;
  Model: string;
  MaxLength: int32;
  Temperature: float64;
  TopP: float64;
  MaxTokens: int32;
  N: int32;
  Debug: bool;
}


let GetAllChatSessions (db: NpgsqlConnection) ()  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    UserID = read.int "user_id"
    Uuid = read.string "uuid"
    Topic = read.string "topic"
    CreatedAt = read.dateTime "created_at"
    UpdatedAt = read.dateTime "updated_at"
    Active = read.bool "active"
    Model = read.string "model"
    MaxLength = read.int "max_length"
    Temperature = read.float64 "temperature"
    TopP = read.float64 "top_p"
    MaxTokens = read.int "max_tokens"
    N = read.int "n"
    Debug = read.bool "debug"}
  db 
  |> Sql.existingConnection
  |> Sql.query getAllChatSessions
  |> Sql.execute reader























let getChatSessionByID = """-- name: GetChatSessionByID :one
SELECT id, user_id, uuid, topic, created_at, updated_at, active, model, max_length, temperature, top_p, max_tokens, n, debug FROM chat_session WHERE id = @id
"""


type GetChatSessionByIDRow = {
  ID: int32;
  UserID: int32;
  Uuid: string;
  Topic: string;
  CreatedAt: DateTime;
  UpdatedAt: DateTime;
  Active: bool;
  Model: string;
  MaxLength: int32;
  Temperature: float64;
  TopP: float64;
  MaxTokens: int32;
  N: int32;
  Debug: bool;
}

let GetChatSessionByID (db: NpgsqlConnection) (id: int32)  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    UserID = read.int "user_id"
    Uuid = read.string "uuid"
    Topic = read.string "topic"
    CreatedAt = read.dateTime "created_at"
    UpdatedAt = read.dateTime "updated_at"
    Active = read.bool "active"
    Model = read.string "model"
    MaxLength = read.int "max_length"
    Temperature = read.float64 "temperature"
    TopP = read.float64 "top_p"
    MaxTokens = read.int "max_tokens"
    N = read.int "n"
    Debug = read.bool "debug"}

  db
  |> Sql.existingConnection
  |> Sql.query getChatSessionByID
  |> Sql.parameters  [ "@id", Sql.int id ]
  |> Sql.execute reader












let getChatSessionByUUID = """-- name: GetChatSessionByUUID :one
SELECT id, user_id, uuid, topic, created_at, updated_at, active, model, max_length, temperature, top_p, max_tokens, n, debug FROM chat_session 
WHERE active = true and uuid = @uuid
order by updated_at
"""


type GetChatSessionByUUIDRow = {
  ID: int32;
  UserID: int32;
  Uuid: string;
  Topic: string;
  CreatedAt: DateTime;
  UpdatedAt: DateTime;
  Active: bool;
  Model: string;
  MaxLength: int32;
  Temperature: float64;
  TopP: float64;
  MaxTokens: int32;
  N: int32;
  Debug: bool;
}

let GetChatSessionByUUID (db: NpgsqlConnection) (uuid: string)  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    UserID = read.int "user_id"
    Uuid = read.string "uuid"
    Topic = read.string "topic"
    CreatedAt = read.dateTime "created_at"
    UpdatedAt = read.dateTime "updated_at"
    Active = read.bool "active"
    Model = read.string "model"
    MaxLength = read.int "max_length"
    Temperature = read.float64 "temperature"
    TopP = read.float64 "top_p"
    MaxTokens = read.int "max_tokens"
    N = read.int "n"
    Debug = read.bool "debug"}

  db
  |> Sql.existingConnection
  |> Sql.query getChatSessionByUUID
  |> Sql.parameters  [ "@uuid", Sql.string uuid ]
  |> Sql.execute reader












let getChatSessionByUUIDWithInActive = """-- name: GetChatSessionByUUIDWithInActive :one
SELECT id, user_id, uuid, topic, created_at, updated_at, active, model, max_length, temperature, top_p, max_tokens, n, debug FROM chat_session 
WHERE uuid = @uuid
order by updated_at
"""


type GetChatSessionByUUIDWithInActiveRow = {
  ID: int32;
  UserID: int32;
  Uuid: string;
  Topic: string;
  CreatedAt: DateTime;
  UpdatedAt: DateTime;
  Active: bool;
  Model: string;
  MaxLength: int32;
  Temperature: float64;
  TopP: float64;
  MaxTokens: int32;
  N: int32;
  Debug: bool;
}

let GetChatSessionByUUIDWithInActive (db: NpgsqlConnection) (uuid: string)  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    UserID = read.int "user_id"
    Uuid = read.string "uuid"
    Topic = read.string "topic"
    CreatedAt = read.dateTime "created_at"
    UpdatedAt = read.dateTime "updated_at"
    Active = read.bool "active"
    Model = read.string "model"
    MaxLength = read.int "max_length"
    Temperature = read.float64 "temperature"
    TopP = read.float64 "top_p"
    MaxTokens = read.int "max_tokens"
    N = read.int "n"
    Debug = read.bool "debug"}

  db
  |> Sql.existingConnection
  |> Sql.query getChatSessionByUUIDWithInActive
  |> Sql.parameters  [ "@uuid", Sql.string uuid ]
  |> Sql.execute reader












let getChatSessionsByUserID = """-- name: GetChatSessionsByUserID :many
SELECT cs.id, cs.user_id, cs.uuid, cs.topic, cs.created_at, cs.updated_at, cs.active, cs.model, cs.max_length, cs.temperature, cs.top_p, cs.max_tokens, cs.n, cs.debug
FROM chat_session cs
WHERE cs.user_id = @user_id and cs.active = true
ORDER BY cs.id
"""


type GetChatSessionsByUserIDRow = {
  ID: int32;
  UserID: int32;
  Uuid: string;
  Topic: string;
  CreatedAt: DateTime;
  UpdatedAt: DateTime;
  Active: bool;
  Model: string;
  MaxLength: int32;
  Temperature: float64;
  TopP: float64;
  MaxTokens: int32;
  N: int32;
  Debug: bool;
}


let GetChatSessionsByUserID (db: NpgsqlConnection) (userID: int32)  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    UserID = read.int "user_id"
    Uuid = read.string "uuid"
    Topic = read.string "topic"
    CreatedAt = read.dateTime "created_at"
    UpdatedAt = read.dateTime "updated_at"
    Active = read.bool "active"
    Model = read.string "model"
    MaxLength = read.int "max_length"
    Temperature = read.float64 "temperature"
    TopP = read.float64 "top_p"
    MaxTokens = read.int "max_tokens"
    N = read.int "n"
    Debug = read.bool "debug"}
  db 
  |> Sql.existingConnection
  |> Sql.query getChatSessionsByUserID
  |> Sql.execute reader























let hasChatSessionPermission = """-- name: HasChatSessionPermission :one
SELECT COUNT(*) > 0 as has_permission
FROM chat_session cs
INNER JOIN auth_user au ON cs.user_id = au.id
WHERE cs.id = @id AND (cs.user_id = @user_id OR au.is_superuser)
"""


type HasChatSessionPermissionParams = {
  ID: int32;
  UserID: int32;
}

let HasChatSessionPermission (db: NpgsqlConnection) (arg: HasChatSessionPermissionParams)  =
  let reader = fun (read:RowReader) -> {
     = read. ""}

  db
  |> Sql.existingConnection
  |> Sql.query hasChatSessionPermission
  |> Sql.parameters  [ "@id", Sql.int arg.ID; "@user_id", Sql.int arg.UserID ]
  |> Sql.execute reader

































let updateChatSession = """-- name: UpdateChatSession :one
UPDATE chat_session SET user_id = @user_id, topic = @topic, updated_at = now(), active = @active
WHERE id = @id
RETURNING id, user_id, uuid, topic, created_at, updated_at, active, model, max_length, temperature, top_p, max_tokens, n, debug
"""


type UpdateChatSessionParams = {
  ID: int32;
  UserID: int32;
  Topic: string;
  Active: bool;
}
type UpdateChatSessionRow = {
  ID: int32;
  UserID: int32;
  Uuid: string;
  Topic: string;
  CreatedAt: DateTime;
  UpdatedAt: DateTime;
  Active: bool;
  Model: string;
  MaxLength: int32;
  Temperature: float64;
  TopP: float64;
  MaxTokens: int32;
  N: int32;
  Debug: bool;
}

let UpdateChatSession (db: NpgsqlConnection) (arg: UpdateChatSessionParams)  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    UserID = read.int "user_id"
    Uuid = read.string "uuid"
    Topic = read.string "topic"
    CreatedAt = read.dateTime "created_at"
    UpdatedAt = read.dateTime "updated_at"
    Active = read.bool "active"
    Model = read.string "model"
    MaxLength = read.int "max_length"
    Temperature = read.float64 "temperature"
    TopP = read.float64 "top_p"
    MaxTokens = read.int "max_tokens"
    N = read.int "n"
    Debug = read.bool "debug"}

  db
  |> Sql.existingConnection
  |> Sql.query updateChatSession
  |> Sql.parameters  [ "@id", Sql.int arg.ID; "@user_id", Sql.int arg.UserID; "@topic", Sql.string arg.Topic; "@active", Sql.bool arg.Active ]
  |> Sql.execute reader












let updateChatSessionByUUID = """-- name: UpdateChatSessionByUUID :one
UPDATE chat_session SET user_id = @user_id, topic = @topic, updated_at = now()
WHERE uuid = @uuid
RETURNING id, user_id, uuid, topic, created_at, updated_at, active, model, max_length, temperature, top_p, max_tokens, n, debug
"""


type UpdateChatSessionByUUIDParams = {
  Uuid: string;
  UserID: int32;
  Topic: string;
}
type UpdateChatSessionByUUIDRow = {
  ID: int32;
  UserID: int32;
  Uuid: string;
  Topic: string;
  CreatedAt: DateTime;
  UpdatedAt: DateTime;
  Active: bool;
  Model: string;
  MaxLength: int32;
  Temperature: float64;
  TopP: float64;
  MaxTokens: int32;
  N: int32;
  Debug: bool;
}

let UpdateChatSessionByUUID (db: NpgsqlConnection) (arg: UpdateChatSessionByUUIDParams)  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    UserID = read.int "user_id"
    Uuid = read.string "uuid"
    Topic = read.string "topic"
    CreatedAt = read.dateTime "created_at"
    UpdatedAt = read.dateTime "updated_at"
    Active = read.bool "active"
    Model = read.string "model"
    MaxLength = read.int "max_length"
    Temperature = read.float64 "temperature"
    TopP = read.float64 "top_p"
    MaxTokens = read.int "max_tokens"
    N = read.int "n"
    Debug = read.bool "debug"}

  db
  |> Sql.existingConnection
  |> Sql.query updateChatSessionByUUID
  |> Sql.parameters  [ "@uuid", Sql.string arg.Uuid; "@user_id", Sql.int arg.UserID; "@topic", Sql.string arg.Topic ]
  |> Sql.execute reader












let updateChatSessionTopicByUUID = """-- name: UpdateChatSessionTopicByUUID :one
INSERT INTO chat_session(uuid, user_id, topic)
VALUES (@uuid, @user_id, @topic)
ON CONFLICT (uuid) 
DO UPDATE SET
topic = EXCLUDED.topic, 
updated_at = now()
returning id, user_id, uuid, topic, created_at, updated_at, active, model, max_length, temperature, top_p, max_tokens, n, debug
"""


type UpdateChatSessionTopicByUUIDParams = {
  Uuid: string;
  UserID: int32;
  Topic: string;
}
type UpdateChatSessionTopicByUUIDRow = {
  ID: int32;
  UserID: int32;
  Uuid: string;
  Topic: string;
  CreatedAt: DateTime;
  UpdatedAt: DateTime;
  Active: bool;
  Model: string;
  MaxLength: int32;
  Temperature: float64;
  TopP: float64;
  MaxTokens: int32;
  N: int32;
  Debug: bool;
}

let UpdateChatSessionTopicByUUID (db: NpgsqlConnection) (arg: UpdateChatSessionTopicByUUIDParams)  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    UserID = read.int "user_id"
    Uuid = read.string "uuid"
    Topic = read.string "topic"
    CreatedAt = read.dateTime "created_at"
    UpdatedAt = read.dateTime "updated_at"
    Active = read.bool "active"
    Model = read.string "model"
    MaxLength = read.int "max_length"
    Temperature = read.float64 "temperature"
    TopP = read.float64 "top_p"
    MaxTokens = read.int "max_tokens"
    N = read.int "n"
    Debug = read.bool "debug"}

  db
  |> Sql.existingConnection
  |> Sql.query updateChatSessionTopicByUUID
  |> Sql.parameters  [ "@uuid", Sql.string arg.Uuid; "@user_id", Sql.int arg.UserID; "@topic", Sql.string arg.Topic ]
  |> Sql.execute reader














let updateSessionMaxLength = """-- name: UpdateSessionMaxLength :one
UPDATE chat_session
SET max_length = @max_length,
    updated_at = now()
WHERE uuid = @uuid
RETURNING id, user_id, uuid, topic, created_at, updated_at, active, model, max_length, temperature, top_p, max_tokens, n, debug
"""


type UpdateSessionMaxLengthParams = {
  Uuid: string;
  MaxLength: int32;
}
type UpdateSessionMaxLengthRow = {
  ID: int32;
  UserID: int32;
  Uuid: string;
  Topic: string;
  CreatedAt: DateTime;
  UpdatedAt: DateTime;
  Active: bool;
  Model: string;
  MaxLength: int32;
  Temperature: float64;
  TopP: float64;
  MaxTokens: int32;
  N: int32;
  Debug: bool;
}

let UpdateSessionMaxLength (db: NpgsqlConnection) (arg: UpdateSessionMaxLengthParams)  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    UserID = read.int "user_id"
    Uuid = read.string "uuid"
    Topic = read.string "topic"
    CreatedAt = read.dateTime "created_at"
    UpdatedAt = read.dateTime "updated_at"
    Active = read.bool "active"
    Model = read.string "model"
    MaxLength = read.int "max_length"
    Temperature = read.float64 "temperature"
    TopP = read.float64 "top_p"
    MaxTokens = read.int "max_tokens"
    N = read.int "n"
    Debug = read.bool "debug"}

  db
  |> Sql.existingConnection
  |> Sql.query updateSessionMaxLength
  |> Sql.parameters  [ "@uuid", Sql.string arg.Uuid; "@max_length", Sql.int arg.MaxLength ]
  |> Sql.execute reader


















