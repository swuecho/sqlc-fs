module Program

open System
open Dapper.FSharp.PostgreSQL
open System.Threading.Tasks
open Npgsql
open Npgsql.FSharp

module Config =
    /// Custom operator for combining paths
    let DSN = Environment.GetEnvironmentVariable("DATABASE_URL")
// postgres://swuecho:VwbE1sYP9OgH@ep-fragrant-sound-219991-pooler.us-west-2.aws.neon.tech/neondb
// Host=192.168.0.135;Port=5432;Username=hwu;Password=using555;Database=stock


let taskToList (t: Task<seq<'a>>) =
    t |> Async.AwaitTask |> Async.RunSynchronously |> Seq.toList



[<EntryPoint>]
let main args =
    OptionTypes.register ()

    printfn "Hello from F#"
    use conn = new NpgsqlConnection(Config.DSN)
    let schema_content = System.IO.File.ReadAllText("schema.sql")

    let _ =
        conn
        |> Sql.existingConnection
        |> Sql.query schema_content
        |> Sql.executeNonQuery


    let createdJwt =
        ChatJwtSecrets.CreateJwtSecret
            conn
            { Name = "my-jwt-secret"
              Secret = "p@ssw0rd"
              Audience = "my-app" }

    let secrets = ChatJwtSecrets.GetJwtSecret conn createdJwt.Name
    printf "%A" secrets

    0
