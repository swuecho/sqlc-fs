// Code generated by sqlc. DO NOT EDIT.
// source: chat_logs.sql


module ChatLogs 

open Npgsql
open Npgsql.FSharp
open System



let chatLogByID = """-- name: ChatLogByID :one
SELECT id, session, question, answer, created_at FROM chat_logs WHERE id = @id
"""


type ChatLogByIDRow = {
  ID: int32;
  Session: string;
  Question: string;
  Answer: string;
  CreatedAt: DateTime;
}

let ChatLogByID (db: NpgsqlConnection) (id: int32)  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    Session = read.string "session"
    Question = read.string "question"
    Answer = read.string "answer"
    CreatedAt = read.dateTime "created_at"}

  db
  |> Sql.existingConnection
  |> Sql.query chatLogByID
  |> Sql.parameters  [ "@id", Sql.int id ]
  |> Sql.execute reader



















let createChatLog = """-- name: CreateChatLog :one
INSERT INTO chat_logs (session, question, answer)
VALUES (@session, @question, @answer)
RETURNING id, session, question, answer, created_at
"""


type CreateChatLogParams = {
  Session: string;
  Question: string;
  Answer: string;
}
type CreateChatLogRow = {
  ID: int32;
  Session: string;
  Question: string;
  Answer: string;
  CreatedAt: DateTime;
}

let CreateChatLog (db: NpgsqlConnection) (arg: CreateChatLogParams)  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    Session = read.string "session"
    Question = read.string "question"
    Answer = read.string "answer"
    CreatedAt = read.dateTime "created_at"}

  db
  |> Sql.existingConnection
  |> Sql.query createChatLog
  |> Sql.parameters  [ "@session", Sql.string arg.Session; "@question", Sql.string arg.Question; "@answer", Sql.string arg.Answer ]
  |> Sql.execute reader
























let deleteChatLog = """-- name: DeleteChatLog :exec
DELETE FROM chat_logs WHERE id = @id
"""






let DeleteChatLog (db: NpgsqlConnection) (id: int32)  = 
  db 
  |> Sql.existingConnection
  |> Sql.query deleteChatLog
  |> Sql.parameters  [ "@id", Sql.int id ]
  |> Sql.executeNonQuery























































let listChatLogs = """-- name: ListChatLogs :many
SELECT id, session, question, answer, created_at FROM chat_logs ORDER BY id
"""


type ListChatLogsRow = {
  ID: int32;
  Session: string;
  Question: string;
  Answer: string;
  CreatedAt: DateTime;
}


let ListChatLogs (db: NpgsqlConnection) ()  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    Session = read.string "session"
    Question = read.string "question"
    Answer = read.string "answer"
    CreatedAt = read.dateTime "created_at"}
  db 
  |> Sql.existingConnection
  |> Sql.query listChatLogs
  |> Sql.execute reader





















let updateChatLog = """-- name: UpdateChatLog :one
UPDATE chat_logs SET session = @session, question = @question, answer = @answer
WHERE id = @id
RETURNING id, session, question, answer, created_at
"""


type UpdateChatLogParams = {
  ID: int32;
  Session: string;
  Question: string;
  Answer: string;
}
type UpdateChatLogRow = {
  ID: int32;
  Session: string;
  Question: string;
  Answer: string;
  CreatedAt: DateTime;
}

let UpdateChatLog (db: NpgsqlConnection) (arg: UpdateChatLogParams)  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    Session = read.string "session"
    Question = read.string "question"
    Answer = read.string "answer"
    CreatedAt = read.dateTime "created_at"}

  db
  |> Sql.existingConnection
  |> Sql.query updateChatLog
  |> Sql.parameters  [ "@id", Sql.int arg.ID; "@session", Sql.string arg.Session; "@question", Sql.string arg.Question; "@answer", Sql.string arg.Answer ]
  |> Sql.execute reader































