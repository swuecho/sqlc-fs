// Code generated by sqlc. DO NOT EDIT.
// source: chat_snapshot.sql


module ChatSnapshot 

open Npgsql
open Npgsql.FSharp
open System






let chatSnapshotByID = """-- name: ChatSnapshotByID :one
SELECT id, uuid, user_id, title, summary, model, tags, session, conversation, created_at, text, search_vector FROM chat_snapshot WHERE id = @id
"""


type ChatSnapshotByIDRow = {
  Id: int32;
  Uuid: string;
  UserId: int32;
  Title: string;
  Summary: string;
  Model: string;
  Tags: string;
  Session: string;
  Conversation: string;
  CreatedAt: DateTime;
  Text: string;
  SearchVector: string option;
}

let ChatSnapshotByID (db: NpgsqlConnection) (id: int32)  =
  
  let reader = fun (read:RowReader) -> {
    Id = read.int "id"
    Uuid = read.string "uuid"
    UserId = read.int "user_id"
    Title = read.string "title"
    Summary = read.string "summary"
    Model = read.string "model"
    Tags = read.string "tags"
    Session = read.string "session"
    Conversation = read.string "conversation"
    CreatedAt = read.dateTime "created_at"
    Text = read.string "text"
    SearchVector = read.stringOrNone "search_vector"}
  

  db
  |> Sql.existingConnection
  |> Sql.query chatSnapshotByID
  |> Sql.parameters  [ "@id", Sql.int id ]
  |> Sql.executeRow reader












let chatSnapshotByUUID = """-- name: ChatSnapshotByUUID :one
SELECT id, uuid, user_id, title, summary, model, tags, session, conversation, created_at, text, search_vector FROM chat_snapshot WHERE uuid = @uuid
"""


type ChatSnapshotByUUIDRow = {
  Id: int32;
  Uuid: string;
  UserId: int32;
  Title: string;
  Summary: string;
  Model: string;
  Tags: string;
  Session: string;
  Conversation: string;
  CreatedAt: DateTime;
  Text: string;
  SearchVector: string option;
}

let ChatSnapshotByUUID (db: NpgsqlConnection) (uuid: string)  =
  
  let reader = fun (read:RowReader) -> {
    Id = read.int "id"
    Uuid = read.string "uuid"
    UserId = read.int "user_id"
    Title = read.string "title"
    Summary = read.string "summary"
    Model = read.string "model"
    Tags = read.string "tags"
    Session = read.string "session"
    Conversation = read.string "conversation"
    CreatedAt = read.dateTime "created_at"
    Text = read.string "text"
    SearchVector = read.stringOrNone "search_vector"}
  

  db
  |> Sql.existingConnection
  |> Sql.query chatSnapshotByUUID
  |> Sql.parameters  [ "@uuid", Sql.string uuid ]
  |> Sql.executeRow reader












let chatSnapshotMetaByUserID = """-- name: ChatSnapshotMetaByUserID :many
SELECT uuid, title, summary, tags, created_at
FROM chat_snapshot WHERE user_id = @user_id
order by created_at desc
"""


type ChatSnapshotMetaByUserIDRow = {
  Uuid: string;
  Title: string;
  Summary: string;
  Tags: string;
  CreatedAt: DateTime;
}


let ChatSnapshotMetaByUserID (db: NpgsqlConnection) (userId: int32)  =
  let reader = fun (read:RowReader) -> {
    Uuid = read.string "uuid"
    Title = read.string "title"
    Summary = read.string "summary"
    Tags = read.string "tags"
    CreatedAt = read.dateTime "created_at"}
  db 
  |> Sql.existingConnection
  |> Sql.query chatSnapshotMetaByUserID
  |> Sql.execute reader










let chatSnapshotSearch = """-- name: ChatSnapshotSearch :many
SELECT uuid, title, ts_rank(search_vector, websearch_to_tsquery(@search), 1) as rank
FROM chat_snapshot
WHERE search_vector @@ websearch_to_tsquery(@search) AND user_id = @user_id
ORDER BY rank DESC
LIMIT 20
"""


type ChatSnapshotSearchParams = {
  UserId: int32;
  Search: string;
}
type ChatSnapshotSearchRow = {
  Uuid: string;
  Title: string;
  Rank: float32;
}


let ChatSnapshotSearch (db: NpgsqlConnection) (arg: ChatSnapshotSearchParams)  =
  let reader = fun (read:RowReader) -> {
    Uuid = read.string "uuid"
    Title = read.string "title"
    Rank = read.float "rank"}
  db 
  |> Sql.existingConnection
  |> Sql.query chatSnapshotSearch
  |> Sql.execute reader

















let createChatSnapshot = """-- name: CreateChatSnapshot :one
INSERT INTO chat_snapshot (uuid, user_id, title, model, summary, tags, conversation ,session, text )
VALUES (@uuid, @user_id, @title, @model, @summary, @tags, @conversation, @session, @text)
RETURNING id, uuid, user_id, title, summary, model, tags, session, conversation, created_at, text, search_vector
"""


type CreateChatSnapshotParams = {
  Uuid: string;
  UserId: int32;
  Title: string;
  Model: string;
  Summary: string;
  Tags: string;
  Conversation: string;
  Session: string;
  Text: string;
}
type CreateChatSnapshotRow = {
  Id: int32;
  Uuid: string;
  UserId: int32;
  Title: string;
  Summary: string;
  Model: string;
  Tags: string;
  Session: string;
  Conversation: string;
  CreatedAt: DateTime;
  Text: string;
  SearchVector: string option;
}

let CreateChatSnapshot (db: NpgsqlConnection) (arg: CreateChatSnapshotParams)  =
  
  let reader = fun (read:RowReader) -> {
    Id = read.int "id"
    Uuid = read.string "uuid"
    UserId = read.int "user_id"
    Title = read.string "title"
    Summary = read.string "summary"
    Model = read.string "model"
    Tags = read.string "tags"
    Session = read.string "session"
    Conversation = read.string "conversation"
    CreatedAt = read.dateTime "created_at"
    Text = read.string "text"
    SearchVector = read.stringOrNone "search_vector"}
  

  db
  |> Sql.existingConnection
  |> Sql.query createChatSnapshot
  |> Sql.parameters  [ "@uuid", Sql.string arg.Uuid; "@user_id", Sql.int arg.UserId; "@title", Sql.string arg.Title; "@model", Sql.string arg.Model; "@summary", Sql.string arg.Summary; "@tags", Sql.string arg.Tags; "@conversation", Sql.string arg.Conversation; "@session", Sql.string arg.Session; "@text", Sql.string arg.Text ]
  |> Sql.executeRow reader




























let deleteChatSnapshot = """-- name: DeleteChatSnapshot :one
DELETE FROM chat_snapshot WHERE uuid = @uuid
and user_id = @user_id
RETURNING id, uuid, user_id, title, summary, model, tags, session, conversation, created_at, text, search_vector
"""


type DeleteChatSnapshotParams = {
  Uuid: string;
  UserId: int32;
}
type DeleteChatSnapshotRow = {
  Id: int32;
  Uuid: string;
  UserId: int32;
  Title: string;
  Summary: string;
  Model: string;
  Tags: string;
  Session: string;
  Conversation: string;
  CreatedAt: DateTime;
  Text: string;
  SearchVector: string option;
}

let DeleteChatSnapshot (db: NpgsqlConnection) (arg: DeleteChatSnapshotParams)  =
  
  let reader = fun (read:RowReader) -> {
    Id = read.int "id"
    Uuid = read.string "uuid"
    UserId = read.int "user_id"
    Title = read.string "title"
    Summary = read.string "summary"
    Model = read.string "model"
    Tags = read.string "tags"
    Session = read.string "session"
    Conversation = read.string "conversation"
    CreatedAt = read.dateTime "created_at"
    Text = read.string "text"
    SearchVector = read.stringOrNone "search_vector"}
  

  db
  |> Sql.existingConnection
  |> Sql.query deleteChatSnapshot
  |> Sql.parameters  [ "@uuid", Sql.string arg.Uuid; "@user_id", Sql.int arg.UserId ]
  |> Sql.executeRow reader




















































let listChatSnapshots = """-- name: ListChatSnapshots :many
SELECT id, uuid, user_id, title, summary, model, tags, session, conversation, created_at, text, search_vector FROM chat_snapshot ORDER BY id
"""


type ListChatSnapshotsRow = {
  Id: int32;
  Uuid: string;
  UserId: int32;
  Title: string;
  Summary: string;
  Model: string;
  Tags: string;
  Session: string;
  Conversation: string;
  CreatedAt: DateTime;
  Text: string;
  SearchVector: string option;
}


let ListChatSnapshots (db: NpgsqlConnection) ()  =
  let reader = fun (read:RowReader) -> {
    Id = read.int "id"
    Uuid = read.string "uuid"
    UserId = read.int "user_id"
    Title = read.string "title"
    Summary = read.string "summary"
    Model = read.string "model"
    Tags = read.string "tags"
    Session = read.string "session"
    Conversation = read.string "conversation"
    CreatedAt = read.dateTime "created_at"
    Text = read.string "text"
    SearchVector = read.stringOrNone "search_vector"}
  db 
  |> Sql.existingConnection
  |> Sql.query listChatSnapshots
  |> Sql.execute reader






























let updateChatSnapshot = """-- name: UpdateChatSnapshot :one
UPDATE chat_snapshot
SET uuid = @uuid, user_id = @user_id, title = @title, summary = @summary, tags = @tags, conversation = @conversation, created_at = @created_at
WHERE id = @id
RETURNING id, uuid, user_id, title, summary, model, tags, session, conversation, created_at, text, search_vector
"""


type UpdateChatSnapshotParams = {
  Id: int32;
  Uuid: string;
  UserId: int32;
  Title: string;
  Summary: string;
  Tags: string;
  Conversation: string;
  CreatedAt: DateTime;
}
type UpdateChatSnapshotRow = {
  Id: int32;
  Uuid: string;
  UserId: int32;
  Title: string;
  Summary: string;
  Model: string;
  Tags: string;
  Session: string;
  Conversation: string;
  CreatedAt: DateTime;
  Text: string;
  SearchVector: string option;
}

let UpdateChatSnapshot (db: NpgsqlConnection) (arg: UpdateChatSnapshotParams)  =
  
  let reader = fun (read:RowReader) -> {
    Id = read.int "id"
    Uuid = read.string "uuid"
    UserId = read.int "user_id"
    Title = read.string "title"
    Summary = read.string "summary"
    Model = read.string "model"
    Tags = read.string "tags"
    Session = read.string "session"
    Conversation = read.string "conversation"
    CreatedAt = read.dateTime "created_at"
    Text = read.string "text"
    SearchVector = read.stringOrNone "search_vector"}
  

  db
  |> Sql.existingConnection
  |> Sql.query updateChatSnapshot
  |> Sql.parameters  [ "@id", Sql.int arg.Id; "@uuid", Sql.string arg.Uuid; "@user_id", Sql.int arg.UserId; "@title", Sql.string arg.Title; "@summary", Sql.string arg.Summary; "@tags", Sql.string arg.Tags; "@conversation", Sql.string arg.Conversation; "@created_at", Sql.date arg.CreatedAt ]
  |> Sql.executeRow reader












let updateChatSnapshotMetaByUUID = """-- name: UpdateChatSnapshotMetaByUUID :exec
UPDATE chat_snapshot
SET title = @title, summary = @summary
WHERE uuid = @uuid and user_id = @user_id
"""


type UpdateChatSnapshotMetaByUUIDParams = {
  Uuid: string;
  Title: string;
  Summary: string;
  UserId: int32;
}




let UpdateChatSnapshotMetaByUUID (db: NpgsqlConnection) (arg: UpdateChatSnapshotMetaByUUIDParams)  = 
  db 
  |> Sql.existingConnection
  |> Sql.query updateChatSnapshotMetaByUUID
  |> Sql.parameters  [ "@uuid", Sql.string arg.Uuid; "@title", Sql.string arg.Title; "@summary", Sql.string arg.Summary; "@user_id", Sql.int arg.UserId ]
  |> Sql.executeNonQuery















