module TestsSecrets

open Expecto
open Npgsql
open System

[<Tests>]
let tests =

    let DSN = Environment.GetEnvironmentVariable("DATABASE_URL")

    testList
        "surveys"
        [ testCase "can ref code in parent"
          <| fun _ ->
              let subject = ChatJwtSecrets.getJwtSecret

              let expected =
                  """-- name: GetJwtSecret :one
SELECT id, name, secret, audience FROM jwt_secrets WHERE name = @name
"""

              Expect.equal subject expected "I compute, therefore I am."


          testCase "get secrets"
          <| fun _ ->
              use conn = new NpgsqlConnection(DSN)
              let subject = ChatJwtSecrets.GetJwtSecret conn "my-jwt-secret"

              Expect.equal subject.Head.Audience "my-app" "app name ok" ]
