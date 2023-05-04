package main

import (
	"bufio"
	"fmt"
	"io"
	"io/ioutil"
	"os"

	"github.com/golang/protobuf/jsonpb"
	"github.com/golang/protobuf/proto"
	"github.com/stephen/sqlc-ts/internal/plugin"
)

func ProtoToJSON(msg proto.Message) (string, error) {
	marshaler := &jsonpb.Marshaler{OrigName: true}
	jsonstr, err := marshaler.MarshalToString(msg)

	if err != nil {
		return "", err
	}

	return jsonstr, nil
}

func main() {
	if err := run(); err != nil {
		fmt.Fprintf(os.Stderr, "error generating typescript: %s", err)
		os.Exit(2)
	}
}

func run() error {
	var req plugin.CodeGenRequest
	reqBlob, err := io.ReadAll(os.Stdin)
	if err != nil {
		return err
	}
	if err := req.UnmarshalVT(reqBlob); err != nil {
		return err
	}

	// convert protobuf message to JSON
	jsonStr, _:= ProtoToJSON(&req)
	_ = ioutil.WriteFile("output.json", []byte(jsonStr), 0644)
	resp, err := Generate(&req)
	if err != nil {
		return err
	}
	respBlob, err := resp.MarshalVT()
	if err != nil {
		return err
	}
	w := bufio.NewWriter(os.Stdout)
	if _, err := w.Write(respBlob); err != nil {
		return err
	}
	if err := w.Flush(); err != nil {
		return err
	}
	return nil
}
