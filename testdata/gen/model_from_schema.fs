// Code generated by sqlc. DO NOT EDIT.

[<AutoOpen>]
module ModelFromSchema 
        
open System


type AuthUser = {
     Id: int32
     Password: string
     LastLogin: DateTime
     IsSuperuser: bool
     Username: string
     FirstName: string
     LastName: string
     Email: string
     IsStaff: bool
     IsActive: bool
     DateJoined: DateTime
}

type AuthUserManagement = {
     Id: int32
     UserId: int32
     RateLimit: int32
     CreatedAt: DateTime
     UpdatedAt: DateTime
}

type Author = {
     Id: int32
     Name: string
     Bio: string option
}

type ChatLog = {
     Id: int32
     Session: string
     Question: string
     Answer: string
     CreatedAt: DateTime
}

type ChatMessage = {
     Id: int32
     Uuid: string
     ChatSessionUuid: string
     Role: string
     Content: string
     Score: float
     UserId: int32
     CreatedAt: DateTime
     UpdatedAt: DateTime
     CreatedBy: int32
     UpdatedBy: int32
     IsDeleted: bool
     IsPin: bool
     TokenCount: int32
     Raw: string
}

type ChatModel = {
     Id: int32
     Name: string
     Label: string
     IsDefault: bool
     Url: string
     ApiAuthHeader: string
     ApiAuthKey: string
     UserId: int32
     EnablePerModeRatelimit: bool
}

type ChatPrompt = {
     Id: int32
     Uuid: string
     ChatSessionUuid: string
     Role: string
     Content: string
     Score: float
     UserId: int32
     CreatedAt: DateTime
     UpdatedAt: DateTime
     CreatedBy: int32
     UpdatedBy: int32
     IsDeleted: bool
     TokenCount: int32
}

type ChatSession = {
     Id: int32
     UserId: int32
     Uuid: string
     Topic: string
     CreatedAt: DateTime
     UpdatedAt: DateTime
     Active: bool
     Model: string
     MaxLength: int32
     Temperature: float
     TopP: float
     MaxTokens: int32
     N: int32
     Debug: bool
}

type ChatSnapshot = {
     Id: int32
     Uuid: string
     UserId: int32
     Title: string
     Summary: string
     Model: string
     Tags: string
     Session: string
     Conversation: string
     CreatedAt: DateTime
     Text: string
     SearchVector: string option
}

type JwtSecret = {
     Id: int32
     Name: string
     Secret: string
     Audience: string
}

type UserActiveChatSession = {
     Id: int32
     UserId: int32
     ChatSessionUuid: string
     CreatedAt: DateTime
     UpdatedAt: DateTime
}

type UserChatModelPrivilege = {
     Id: int32
     UserId: int32
     ChatModelId: int32
     RateLimit: int32
     CreatedAt: DateTime
     UpdatedAt: DateTime
     CreatedBy: int32
     UpdatedBy: int32
}


