-- name: CreateJwtSecret :one
INSERT INTO jwt_secrets (name, secret, audience)
VALUES ($1, $2, $3) RETURNING *;

-- name: CreateJwtSecretLastID :execlastid
INSERT INTO jwt_secrets (name, secret, audience)
VALUES ($1, $2, $3);

-- name: GetJwtSecret :one
SELECT * FROM jwt_secrets WHERE name = $1;

-- name: DeleteAllJwtSecrets :execrows
DELETE FROM jwt_secrets WHERE name = $1;


