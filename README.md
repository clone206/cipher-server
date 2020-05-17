# cipher-server

(see also https://github.com/clone206/signup-form)

An api endpoint that encodes imcoming text with a ceasar cipher and returns the result. Works on both upper and lowercase letters, retains all other characters unchanged, and accepts a shift value from -26 to 26.

Depends on the .NET Core 3.1 SDK:

https://dotnet.microsoft.com/download/dotnet-core/3.1

## To run on local machine
From the root of the cloned directory, assuming the dotnet cli was successfully added to your path by installing the above:

`dotnet run` (this should perform a nuget restore automatically)

Example usage:

`curl -v http://localhost:23456/api/encode -d '{"Shift": 20, "Message": "Hey there, friend."}' -H "Content-Type: application/json" -H "Accept: application/json"`

produces:

```
*   Trying ::1...
* TCP_NODELAY set
* Connected to localhost (::1) port 23456 (#0)
> POST /api/encode HTTP/1.1
> Host: localhost:23456
> User-Agent: curl/7.54.0
> Content-Type: application/json
> Accept: application/json
> Content-Length: 46
>
* upload completely sent off: 46 out of 46 bytes
< HTTP/1.1 200 OK
< Date: Sun, 17 May 2020 08:30:21 GMT
< Content-Type: application/json; charset=utf-8
< Server: Kestrel
< Transfer-Encoding: chunked
<
* Connection #0 to host localhost left intact
{"encodedMessage":"Bys nbyly, zlcyhx."}
```

Will also write the output to `wwwroot/encoded.json`

## To create a portable binary
`dotnet publish -c Release -o ./publish/`

Copy the entire contents of the publish folder wherever a .NET Core 3.1 runtime is installed and run there, on port `23456`

eg, `dotnet cipher-server.dll` from a terminal within the publish folder, or via IIS.

## To run as a Docker image
On a machine with Docker running, from the root of the cloned directory:

```
docker build --rm --pull -f "./Dockerfile" -t "cipherserver:latest" "./"
docker run --rm -it  -p 23456:23456/tcp cipherserver:latest
```

## To test a live demo version running on port 80 in the Azure cloud
`curl -v http://cipher-server.azurewebsites.net/api/encode -d '{"Shift": 20, "Message": "Hey buddy. Hows it goin?"}' -H "Content-Type: application/json" -H "Accept: application/json"`

(wait for app service to spin up)
