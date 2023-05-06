module TestsModel

open Expecto
open Npgsql
open System

[<Tests>]
let tests =
    let DSN = Environment.GetEnvironmentVariable("DATABASE_URL")

    let chatModelParams: ChatModel.CreateChatModelParams =
        { Name = "MyChat"
          Label = "Chat 1"
          IsDefault = true
          Url = "https://example.com"
          ApiAuthHeader = "Authorization"
          ApiAuthKey = "123456"
          UserId = 12345
          EnablePerModeRatelimit = false }

    let createUpdatedParams (Id: int) : ChatModel.UpdateChatModelParams =
        { Id = Id
          Name = "UpdatedChatName"
          Label = "Updated Chat Label"
          IsDefault = false
          Url = "https://updated-example.com"
          ApiAuthHeader = "UpdatedHeader"
          ApiAuthKey = "987654"
          EnablePerModeRatelimit = true
          UserId = 12345 }

    testList
        "test CRUD of ChatModel"
        [ testCase "test create"
          <| fun _ ->
              use conn = new NpgsqlConnection(DSN)
              let model = ChatModel.CreateChatModel conn chatModelParams
              Expect.equal model.Name chatModelParams.Name "name equal"
              Expect.equal model.Label chatModelParams.Label "label equal"
              Expect.equal model.IsDefault chatModelParams.IsDefault "isDefault equal"
              Expect.equal model.Url chatModelParams.Url "url equal"
              Expect.equal model.ApiAuthHeader chatModelParams.ApiAuthHeader "apiAuthHeader equal"
              Expect.equal model.ApiAuthKey chatModelParams.ApiAuthKey "apiAuthKey equal"


              let updatedParams = createUpdatedParams model.Id
              let updateResult = ChatModel.UpdateChatModel conn updatedParams

              Expect.equal updateResult.Name updatedParams.Name "update success"

              let retrieveResult = ChatModel.ChatModelByID conn updateResult.Id

              Expect.equal retrieveResult.Name updatedParams.Name "name updated"
              Expect.equal retrieveResult.Label updatedParams.Label "label updated"
              Expect.equal retrieveResult.IsDefault updatedParams.IsDefault "isDefault updated"
              Expect.equal retrieveResult.Url updatedParams.Url "url updated"
              Expect.equal retrieveResult.ApiAuthHeader updatedParams.ApiAuthHeader "apiAuthHeader updated"
              Expect.equal retrieveResult.ApiAuthKey updatedParams.ApiAuthKey "apiAuthKey updated"

              let allChatModels = ChatModel.ListSystemChatModels conn ()

              Expect.equal allChatModels.Length 1 "there is only one"

              let deletedModelRowCount =
                  ChatModel.DeleteChatModel conn { Id = model.Id; UserId = model.UserId }

              Expect.equal deletedModelRowCount 1 "1 record deleted"

          //let retrieveResult = ChatModel.ChatModelByID conn model.Id

          //Expect.equal retrieveResult {} "delete success" ]
          ]
