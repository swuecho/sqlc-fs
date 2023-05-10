// Code generated by sqlc. DO NOT EDIT.
// source: chat_model.sql


module ChatModel 

open Npgsql
open Npgsql.FSharp
open System







let chatModelByID = """-- name: ChatModelByID :one
SELECT id, name, label, is_default, url, api_auth_header, api_auth_key, user_id, enable_per_mode_ratelimit FROM chat_model WHERE id = @id
"""



let ChatModelByID (db: NpgsqlConnection)  (id: int32)  =
  
  let reader = fun (read:RowReader) -> {
    Id = read.int "id"
    Name = read.string "name"
    Label = read.string "label"
    IsDefault = read.bool "is_default"
    Url = read.string "url"
    ApiAuthHeader = read.string "api_auth_header"
    ApiAuthKey = read.string "api_auth_key"
    UserId = read.int "user_id"
    EnablePerModeRatelimit = read.bool "enable_per_mode_ratelimit"}
  

  db
  |> Sql.existingConnection
  |> Sql.query chatModelByID
  |> Sql.parameters  [ "@id", Sql.int id ]
  |> Sql.executeRow reader












let chatModelByName = """-- name: ChatModelByName :one
SELECT id, name, label, is_default, url, api_auth_header, api_auth_key, user_id, enable_per_mode_ratelimit FROM chat_model WHERE name = @name
"""



let ChatModelByName (db: NpgsqlConnection)  (name: string)  =
  
  let reader = fun (read:RowReader) -> {
    Id = read.int "id"
    Name = read.string "name"
    Label = read.string "label"
    IsDefault = read.bool "is_default"
    Url = read.string "url"
    ApiAuthHeader = read.string "api_auth_header"
    ApiAuthKey = read.string "api_auth_key"
    UserId = read.int "user_id"
    EnablePerModeRatelimit = read.bool "enable_per_mode_ratelimit"}
  

  db
  |> Sql.existingConnection
  |> Sql.query chatModelByName
  |> Sql.parameters  [ "@name", Sql.string name ]
  |> Sql.executeRow reader



















let createChatModel = """-- name: CreateChatModel :one
INSERT INTO chat_model (name, label, is_default, url, api_auth_header, api_auth_key, user_id, enable_per_mode_ratelimit)
VALUES (@name, @label, @is_default, @url, @api_auth_header, @api_auth_key, @user_id, @enable_per_mode_ratelimit)
RETURNING id, name, label, is_default, url, api_auth_header, api_auth_key, user_id, enable_per_mode_ratelimit
"""


type CreateChatModelParams = {
  Name: string;
  Label: string;
  IsDefault: bool;
  Url: string;
  ApiAuthHeader: string;
  ApiAuthKey: string;
  UserId: int32;
  EnablePerModeRatelimit: bool;
}

let CreateChatModel (db: NpgsqlConnection)  (arg: CreateChatModelParams)  =
  
  let reader = fun (read:RowReader) -> {
    Id = read.int "id"
    Name = read.string "name"
    Label = read.string "label"
    IsDefault = read.bool "is_default"
    Url = read.string "url"
    ApiAuthHeader = read.string "api_auth_header"
    ApiAuthKey = read.string "api_auth_key"
    UserId = read.int "user_id"
    EnablePerModeRatelimit = read.bool "enable_per_mode_ratelimit"}
  

  db
  |> Sql.existingConnection
  |> Sql.query createChatModel
  |> Sql.parameters  [ "@name", Sql.string arg.Name; "@label", Sql.string arg.Label; "@is_default", Sql.bool arg.IsDefault; "@url", Sql.string arg.Url; "@api_auth_header", Sql.string arg.ApiAuthHeader; "@api_auth_key", Sql.string arg.ApiAuthKey; "@user_id", Sql.int arg.UserId; "@enable_per_mode_ratelimit", Sql.bool arg.EnablePerModeRatelimit ]
  |> Sql.executeRow reader



























let deleteChatModel = """-- name: DeleteChatModel :exec
DELETE FROM chat_model WHERE id = @id and user_id = @user_id
"""


type DeleteChatModelParams = {
  Id: int32;
  UserId: int32;
}




let DeleteChatModel (db: NpgsqlConnection)  (arg: DeleteChatModelParams)  = 
  db 
  |> Sql.existingConnection
  |> Sql.query deleteChatModel
  |> Sql.parameters  [ "@id", Sql.int arg.Id; "@user_id", Sql.int arg.UserId ]
  |> Sql.executeNonQuery




































let getDefaultChatModel = """-- name: GetDefaultChatModel :one
SELECT id, name, label, is_default, url, api_auth_header, api_auth_key, user_id, enable_per_mode_ratelimit FROM chat_model WHERE is_default = true
and user_id in (select id from auth_user where is_superuser = true)
"""



let GetDefaultChatModel (db: NpgsqlConnection)   =
  
  let reader = fun (read:RowReader) -> {
    Id = read.int "id"
    Name = read.string "name"
    Label = read.string "label"
    IsDefault = read.bool "is_default"
    Url = read.string "url"
    ApiAuthHeader = read.string "api_auth_header"
    ApiAuthKey = read.string "api_auth_key"
    UserId = read.int "user_id"
    EnablePerModeRatelimit = read.bool "enable_per_mode_ratelimit"}
  

  db
  |> Sql.existingConnection
  |> Sql.query getDefaultChatModel
  |> Sql.parameters  [  ]
  |> Sql.executeRow reader



























let listChatModels = """-- name: ListChatModels :many
SELECT id, name, label, is_default, url, api_auth_header, api_auth_key, user_id, enable_per_mode_ratelimit FROM chat_model ORDER BY id
"""




let ListChatModels (db: NpgsqlConnection)  =
  let reader = fun (read:RowReader) -> {
    Id = read.int "id"
    Name = read.string "name"
    Label = read.string "label"
    IsDefault = read.bool "is_default"
    Url = read.string "url"
    ApiAuthHeader = read.string "api_auth_header"
    ApiAuthKey = read.string "api_auth_key"
    UserId = read.int "user_id"
    EnablePerModeRatelimit = read.bool "enable_per_mode_ratelimit"}
  db 
  |> Sql.existingConnection
  |> Sql.query listChatModels
  |> Sql.execute reader











let listSystemChatModels = """-- name: ListSystemChatModels :many
SELECT id, name, label, is_default, url, api_auth_header, api_auth_key, user_id, enable_per_mode_ratelimit FROM chat_model
where user_id in (select id from auth_user where is_superuser = true)
ORDER BY id
"""




let ListSystemChatModels (db: NpgsqlConnection)  =
  let reader = fun (read:RowReader) -> {
    Id = read.int "id"
    Name = read.string "name"
    Label = read.string "label"
    IsDefault = read.bool "is_default"
    Url = read.string "url"
    ApiAuthHeader = read.string "api_auth_header"
    ApiAuthKey = read.string "api_auth_key"
    UserId = read.int "user_id"
    EnablePerModeRatelimit = read.bool "enable_per_mode_ratelimit"}
  db 
  |> Sql.existingConnection
  |> Sql.query listSystemChatModels
  |> Sql.execute reader






















let updateChatModel = """-- name: UpdateChatModel :one
UPDATE chat_model SET name = @name, label = @label, is_default = @is_default, url = @url, api_auth_header = @api_auth_header, api_auth_key = @api_auth_key, enable_per_mode_ratelimit = @enable_per_mode_ratelimit
WHERE id = @id and user_id = @user_id
RETURNING id, name, label, is_default, url, api_auth_header, api_auth_key, user_id, enable_per_mode_ratelimit
"""


type UpdateChatModelParams = {
  Id: int32;
  Name: string;
  Label: string;
  IsDefault: bool;
  Url: string;
  ApiAuthHeader: string;
  ApiAuthKey: string;
  UserId: int32;
  EnablePerModeRatelimit: bool;
}

let UpdateChatModel (db: NpgsqlConnection)  (arg: UpdateChatModelParams)  =
  
  let reader = fun (read:RowReader) -> {
    Id = read.int "id"
    Name = read.string "name"
    Label = read.string "label"
    IsDefault = read.bool "is_default"
    Url = read.string "url"
    ApiAuthHeader = read.string "api_auth_header"
    ApiAuthKey = read.string "api_auth_key"
    UserId = read.int "user_id"
    EnablePerModeRatelimit = read.bool "enable_per_mode_ratelimit"}
  

  db
  |> Sql.existingConnection
  |> Sql.query updateChatModel
  |> Sql.parameters  [ "@id", Sql.int arg.Id; "@name", Sql.string arg.Name; "@label", Sql.string arg.Label; "@is_default", Sql.bool arg.IsDefault; "@url", Sql.string arg.Url; "@api_auth_header", Sql.string arg.ApiAuthHeader; "@api_auth_key", Sql.string arg.ApiAuthKey; "@user_id", Sql.int arg.UserId; "@enable_per_mode_ratelimit", Sql.bool arg.EnablePerModeRatelimit ]
  |> Sql.executeRow reader












let updateChatModelKey = """-- name: UpdateChatModelKey :one
UPDATE chat_model SET api_auth_key = @api_auth_key
WHERE id = @id
RETURNING id, name, label, is_default, url, api_auth_header, api_auth_key, user_id, enable_per_mode_ratelimit
"""


type UpdateChatModelKeyParams = {
  Id: int32;
  ApiAuthKey: string;
}

let UpdateChatModelKey (db: NpgsqlConnection)  (arg: UpdateChatModelKeyParams)  =
  
  let reader = fun (read:RowReader) -> {
    Id = read.int "id"
    Name = read.string "name"
    Label = read.string "label"
    IsDefault = read.bool "is_default"
    Url = read.string "url"
    ApiAuthHeader = read.string "api_auth_header"
    ApiAuthKey = read.string "api_auth_key"
    UserId = read.int "user_id"
    EnablePerModeRatelimit = read.bool "enable_per_mode_ratelimit"}
  

  db
  |> Sql.existingConnection
  |> Sql.query updateChatModelKey
  |> Sql.parameters  [ "@id", Sql.int arg.Id; "@api_auth_key", Sql.string arg.ApiAuthKey ]
  |> Sql.executeRow reader


























