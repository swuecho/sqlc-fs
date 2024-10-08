-- name: GetAllAuthUsers :many
SELECT * FROM auth_user ORDER BY id;

-- name: ListAuthUsers :many
SELECT * FROM auth_user ORDER BY id LIMIT $1 OFFSET $2;

-- name: ListAuthUserID :many
SELECT id FROM auth_user;

-- name: TotalAuthUsers :one
SELECT COUNT(*) FROM auth_user;

-- name: ListAuthUserIDandEmail :many
SELECT id, email FROM auth_user;

-- name: GetAuthUserByID :one
SELECT * FROM auth_user WHERE id = $1;


-- name: GetAuthUserByEmail :one
SELECT * FROM auth_user WHERE email = $1;

-- name: CreateAuthUser :one
INSERT INTO auth_user (email, "password", first_name, last_name, username, is_staff, is_superuser)
VALUES ($1, $2, $3, $4, $5, $6, $7)
RETURNING *;

-- name: UpdateAuthUser :one
UPDATE auth_user SET first_name = $2, last_name= $3, last_login = now() 
WHERE id = $1
RETURNING first_name, last_name, email;

-- name: UpdateAuthUserByEmail :one
UPDATE auth_user SET first_name = $2, last_name= $3, last_login = now() 
WHERE email = $1
RETURNING first_name, last_name, email;

-- name: DeleteAuthUser :exec
DELETE FROM auth_user WHERE email = $1;

-- name: GetUserByEmail :one
SELECT * FROM auth_user WHERE email = $1;

-- name: UpdateUserPassword :exec
UPDATE auth_user SET "password" = $2 WHERE email = $1;

-- name: GetTotalActiveUserCount :one
SELECT COUNT(*) FROM auth_user WHERE is_active = true;


-- name: UpdateAuthUserRateLimitByEmail :one
INSERT INTO auth_user_management (user_id, rate_limit, created_at, updated_at)
VALUES ((SELECT id FROM auth_user WHERE email = $1), $2, NOW(), NOW())
ON CONFLICT (user_id) DO UPDATE SET rate_limit = $2, updated_at = NOW()
RETURNING rate_limit;

-- name: GetUserStats :many
SELECT 
    auth_user.first_name,
    auth_user.last_name,
    auth_user.email AS user_email,
    COALESCE(user_stats.total_messages, 0) AS total_chat_messages,
    COALESCE(user_stats.total_token_count, 0) AS total_token_count,
    COALESCE(user_stats.total_messages_3_days, 0) AS total_chat_messages_3_days,
    COALESCE(user_stats.total_token_count_3_days, 0) AS total_token_count_3_days,
    COALESCE(auth_user_management.rate_limit, @default_rate_limit::INTEGER) AS rate_limit
FROM auth_user
LEFT JOIN (
    SELECT chat_message_stats.user_id, 
           SUM(total_messages) AS total_messages, 
           SUM(total_token_count) AS total_token_count,
           SUM(CASE WHEN created_at >= NOW() - INTERVAL '3 days' THEN total_messages ELSE 0 END) AS total_messages_3_days,
           SUM(CASE WHEN created_at >= NOW() - INTERVAL '3 days' THEN total_token_count ELSE 0 END) AS total_token_count_3_days
    FROM (
        SELECT user_id, COUNT(*) AS total_messages, SUM(token_count) as total_token_count, MAX(created_at) AS created_at
        FROM chat_message
        GROUP BY user_id, chat_session_uuid
    ) AS chat_message_stats
    GROUP BY chat_message_stats.user_id
) AS user_stats ON auth_user.id = user_stats.user_id
LEFT JOIN auth_user_management ON auth_user.id = auth_user_management.user_id
ORDER BY total_chat_messages DESC, auth_user.id DESC
OFFSET $2
LIMIT $1;