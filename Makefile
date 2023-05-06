gen: 
	go build -o bin/sqlc-fs   
	sqlc generate -f ./testdata/sqlc.json
