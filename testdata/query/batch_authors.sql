-- name: BatchDeleteAuthors :batchexec
DELETE FROM authors WHERE id = $1;

-- name: BatchAuthorsByName :batchmany
SELECT id, name, bio FROM authors WHERE name = $1;

-- name: BatchAuthorByID :batchone
SELECT id, name, bio FROM authors WHERE id = $1 LIMIT 1;
