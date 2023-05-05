module Program

open System
open Dapper.FSharp.PostgreSQL
open System.Threading.Tasks
open Npgsql.FSharp
open Npgsql

module Config =
    /// Custom operator for combining paths
    let neonDSN = Environment.GetEnvironmentVariable("NEON_DATABASE_URL")
    // postgres://swuecho:VwbE1sYP9OgH@ep-fragrant-sound-219991-pooler.us-west-2.aws.neon.tech/neondb
    // Host=192.168.0.135;Port=5432;Username=hwu;Password=using555;Database=stock


let taskToList (t: Task<seq<'a>>) =
    t |> Async.AwaitTask |> Async.RunSynchronously |> Seq.toList


let getJwtSecret = """-- name: GetJwtSecret :one
SELECT id, name, secret, audience FROM jwt_secrets WHERE name = @name
"""


type GetJwtSecretRow = {
  ID: int32;
  Name: string;
  Secret: string;
  Audience: string;
}

let GetJwtSecret (db: NpgsqlConnection) (name: string)  =
  let reader = fun (read:RowReader) -> {
    ID = read.int "id"
    Name = read.string "name"
    Secret = read.string "secret"
    Audience = read.string "audience"}

  db
  |> Sql.existingConnection
  |> Sql.query getJwtSecret
  |> Sql.parameters  [ "@name", Sql.string name ]
  |> Sql.execute reader

[<EntryPoint>]
let main args =
    OptionTypes.register ()

    printfn "Hello from F#"

    use conn = new NpgsqlConnection("Host=192.168.0.135;Port=5432;Username=hwu;Password=using555;Database=hwu")
    let secrets = GetJwtSecret conn "chat"
    printf "%A" secrets

    0
