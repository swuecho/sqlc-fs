module TestsSecrets

open Expecto

[<Tests>]
let tests =
    testList
        "surveys"
        [ testCase "can ref code in parent"
          <| fun _ ->
              let subject = ChatJwtSecrets.getJwtSecret

              let expected ="""-- name: GetJwtSecret :one
SELECT id, name, secret, audience FROM jwt_secrets WHERE name = @name
"""

              Expect.equal subject expected "I compute, therefore I am."

        
          testCase "html page"
          <| fun _ ->
              Expect.equal "a" "a" "I compute, therefore I am."
        ]
