internal/plugin/codegen.pb.go: protos/plugin/codegen.proto
	protoc -I ./protos \
		--go_out=. \
		--go_opt=module=github.com/swuecho/sqlc-fs \
		--go-vtproto_out=. \
		--go-vtproto_opt=module=github.com/swuecho/sqlc-fs,features=marshal+unmarshal+size \
		./protos/plugin/codegen.proto

gen: 
	go build -o bin/sqlc-fs   
	sqlc generate -f ./testdata/sqlc.json
