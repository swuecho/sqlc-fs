// Code generated by sqlc. DO NOT EDIT.
// source: chat_snapshot.sql

open Npgsql.FSharp





let chatSnapshotByID = """-- name: ChatSnapshotByID :one
SELECT id, uuid, user_id, title, summary, model, tags, session, conversation, created_at, text, search_vector FROM chat_snapshot WHERE id = @id
"""


type ChatSnapshotByIDRow = {
  ID: int32;
  Uuid: string;
  UserID: int32;
  Title: string;
  Summary: string;
  Model: string;
  Tags: string;
  Session: string;
  Conversation: string;
  CreatedAt: DateTime;
  Text: string;
  SearchVector: interface{} option;
}

let ChatSnapshotByID (db: NpgsqlConnection) (id: int32)  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    Uuid = read.string "uuid"
    UserID = read.int "user_id"
    Title = read.string "title"
    Summary = read.string "summary"
    Model = read.string "model"
    Tags = read.string "tags"
    Session = read.string "session"
    Conversation = read.string "conversation"
    CreatedAt = read.DateTime "created_at"
    Text = read.string "text"
    SearchVector = read.interface{}OrNone "search_vector"}

  db
  |> Sql.existingConnection
  |> Sql.query chatSnapshotByID
  |> Sql.parameters  [ "@id", Sql.int id ]
  |> Sql.execute reader












let chatSnapshotByUUID = """-- name: ChatSnapshotByUUID :one
SELECT id, uuid, user_id, title, summary, model, tags, session, conversation, created_at, text, search_vector FROM chat_snapshot WHERE uuid = @uuid
"""


type ChatSnapshotByUUIDRow = {
  ID: int32;
  Uuid: string;
  UserID: int32;
  Title: string;
  Summary: string;
  Model: string;
  Tags: string;
  Session: string;
  Conversation: string;
  CreatedAt: DateTime;
  Text: string;
  SearchVector: interface{} option;
}

let ChatSnapshotByUUID (db: NpgsqlConnection) (uuid: string)  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    Uuid = read.string "uuid"
    UserID = read.int "user_id"
    Title = read.string "title"
    Summary = read.string "summary"
    Model = read.string "model"
    Tags = read.string "tags"
    Session = read.string "session"
    Conversation = read.string "conversation"
    CreatedAt = read.DateTime "created_at"
    Text = read.string "text"
    SearchVector = read.interface{}OrNone "search_vector"}

  db
  |> Sql.existingConnection
  |> Sql.query chatSnapshotByUUID
  |> Sql.parameters  [ "@uuid", Sql.string uuid ]
  |> Sql.execute reader












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


let ChatSnapshotMetaByUserID (db: NpgsqlConnection) (userID: int32)  =
  let reader = fun (read:RowReader) -> {
    Uuid = read.string "uuid"
    Title = read.string "title"
    Summary = read.string "summary"
    Tags = read.string "tags"
    CreatedAt = read.DateTime "created_at"}
  db 
  |> Sql.existingConnection
  |> sql.query chatSnapshotMetaByUserID
  |> chatSnapshotMetaByUserID
  |> Sql.execute reader










let chatSnapshotSearch = """-- name: ChatSnapshotSearch :many
SELECT uuid, title, ts_rank(search_vector, websearch_to_tsquery(@search), 1) as rank
FROM chat_snapshot
WHERE search_vector @@ websearch_to_tsquery(@search) AND user_id = @user_id
ORDER BY rank DESC
LIMIT 20
"""


type ChatSnapshotSearchParams = {
  UserID: int32;
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
    Rank = read.float32 "rank"}
  db 
  |> Sql.existingConnection
  |> sql.query chatSnapshotSearch
  |> chatSnapshotSearch
  |> Sql.execute reader

















let createChatSnapshot = """-- name: CreateChatSnapshot :one
INSERT INTO chat_snapshot (uuid, user_id, title, model, summary, tags, conversation ,session, text )
VALUES (@uuid, @user_id, @title, @model, @summary, @tags, @conversation, @session, @text)
RETURNING id, uuid, user_id, title, summary, model, tags, session, conversation, created_at, text, search_vector
"""


type CreateChatSnapshotParams = {
  Uuid: string;
  UserID: int32;
  Title: string;
  Model: string;
  Summary: string;
  Tags: string;
  Conversation: string;
  Session: string;
  Text: string;
}
type CreateChatSnapshotRow = {
  ID: int32;
  Uuid: string;
  UserID: int32;
  Title: string;
  Summary: string;
  Model: string;
  Tags: string;
  Session: string;
  Conversation: string;
  CreatedAt: DateTime;
  Text: string;
  SearchVector: interface{} option;
}

let CreateChatSnapshot (db: NpgsqlConnection) (arg: CreateChatSnapshotParams)  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    Uuid = read.string "uuid"
    UserID = read.int "user_id"
    Title = read.string "title"
    Summary = read.string "summary"
    Model = read.string "model"
    Tags = read.string "tags"
    Session = read.string "session"
    Conversation = read.string "conversation"
    CreatedAt = read.DateTime "created_at"
    Text = read.string "text"
    SearchVector = read.interface{}OrNone "search_vector"}

  db
  |> Sql.existingConnection
  |> Sql.query createChatSnapshot
  |> Sql.parameters  [ "@uuid", Sql.string arg.uuid, "@user_id", Sql.int arg.user_id, "@title", Sql.string arg.title, "@model", Sql.string arg.model, "@summary", Sql.string arg.summary, "@tags", Sql.string arg.tags, "@conversation", Sql.string arg.conversation, "@session", Sql.string arg.session, "@text", Sql.string arg.text ]
  |> Sql.execute reader



























let deleteChatSnapshot = """-- name: DeleteChatSnapshot :one
DELETE FROM chat_snapshot WHERE uuid = @uuid
and user_id = @user_id
RETURNING id, uuid, user_id, title, summary, model, tags, session, conversation, created_at, text, search_vector
"""


type DeleteChatSnapshotParams = {
  Uuid: string;
  UserID: int32;
}
type DeleteChatSnapshotRow = {
  ID: int32;
  Uuid: string;
  UserID: int32;
  Title: string;
  Summary: string;
  Model: string;
  Tags: string;
  Session: string;
  Conversation: string;
  CreatedAt: DateTime;
  Text: string;
  SearchVector: interface{} option;
}

let DeleteChatSnapshot (db: NpgsqlConnection) (arg: DeleteChatSnapshotParams)  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    Uuid = read.string "uuid"
    UserID = read.int "user_id"
    Title = read.string "title"
    Summary = read.string "summary"
    Model = read.string "model"
    Tags = read.string "tags"
    Session = read.string "session"
    Conversation = read.string "conversation"
    CreatedAt = read.DateTime "created_at"
    Text = read.string "text"
    SearchVector = read.interface{}OrNone "search_vector"}

  db
  |> Sql.existingConnection
  |> Sql.query deleteChatSnapshot
  |> Sql.parameters  [ "@uuid", Sql.string arg.uuid, "@user_id", Sql.int arg.user_id ]
  |> Sql.execute reader




















































let listChatSnapshots = """-- name: ListChatSnapshots :many
SELECT id, uuid, user_id, title, summary, model, tags, session, conversation, created_at, text, search_vector FROM chat_snapshot ORDER BY id
"""


type ListChatSnapshotsRow = {
  ID: int32;
  Uuid: string;
  UserID: int32;
  Title: string;
  Summary: string;
  Model: string;
  Tags: string;
  Session: string;
  Conversation: string;
  CreatedAt: DateTime;
  Text: string;
  SearchVector: interface{} option;
}


let ListChatSnapshots (db: NpgsqlConnection) ()  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    Uuid = read.string "uuid"
    UserID = read.int "user_id"
    Title = read.string "title"
    Summary = read.string "summary"
    Model = read.string "model"
    Tags = read.string "tags"
    Session = read.string "session"
    Conversation = read.string "conversation"
    CreatedAt = read.DateTime "created_at"
    Text = read.string "text"
    SearchVector = read.interface{}OrNone "search_vector"}
  db 
  |> Sql.existingConnection
  |> sql.query listChatSnapshots
  |> listChatSnapshots
  |> Sql.execute reader






























let updateChatSnapshot = """-- name: UpdateChatSnapshot :one
UPDATE chat_snapshot
SET uuid = @uuid, user_id = @user_id, title = @title, summary = @summary, tags = @tags, conversation = @conversation, created_at = @created_at
WHERE id = @id
RETURNING id, uuid, user_id, title, summary, model, tags, session, conversation, created_at, text, search_vector
"""


type UpdateChatSnapshotParams = {
  ID: int32;
  Uuid: string;
  UserID: int32;
  Title: string;
  Summary: string;
  Tags: string;
  Conversation: string;
  CreatedAt: DateTime;
}
type UpdateChatSnapshotRow = {
  ID: int32;
  Uuid: string;
  UserID: int32;
  Title: string;
  Summary: string;
  Model: string;
  Tags: string;
  Session: string;
  Conversation: string;
  CreatedAt: DateTime;
  Text: string;
  SearchVector: interface{} option;
}

let UpdateChatSnapshot (db: NpgsqlConnection) (arg: UpdateChatSnapshotParams)  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    Uuid = read.string "uuid"
    UserID = read.int "user_id"
    Title = read.string "title"
    Summary = read.string "summary"
    Model = read.string "model"
    Tags = read.string "tags"
    Session = read.string "session"
    Conversation = read.string "conversation"
    CreatedAt = read.DateTime "created_at"
    Text = read.string "text"
    SearchVector = read.interface{}OrNone "search_vector"}

  db
  |> Sql.existingConnection
  |> Sql.query updateChatSnapshot
  |> Sql.parameters  [ "@id", Sql.int arg.id, "@uuid", Sql.string arg.uuid, "@user_id", Sql.int arg.user_id, "@title", Sql.string arg.title, "@summary", Sql.string arg.summary, "@tags", Sql.string arg.tags, "@conversation", Sql.string arg.conversation, "@created_at", Sql.DateTime arg.created_at ]
  |> Sql.execute reader












let updateChatSnapshotMetaByUUID = """-- name: UpdateChatSnapshotMetaByUUID :exec
UPDATE chat_snapshot
SET title = @title, summary = @summary
WHERE uuid = @uuid and user_id = @user_id
"""


type UpdateChatSnapshotMetaByUUIDParams = {
  Uuid: string;
  Title: string;
  Summary: string;
  UserID: int32;
}




let UpdateChatSnapshotMetaByUUID (db: NpgsqlConnection) (arg: UpdateChatSnapshotMetaByUUIDParams)  = 
  db 
  |> Sql.existingConnection
  |> Sql.query updateChatSnapshotMetaByUUID
  |> Sql.parameters  [ "@uuid", Sql.string arg.uuid, "@title", Sql.string arg.title, "@summary", Sql.string arg.summary, "@user_id", Sql.int arg.user_id ]
  |> Sql.executeNonQuery















