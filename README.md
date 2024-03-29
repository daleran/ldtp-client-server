# LDTP Client and Server
A student project to build a simple TCP/IP Client and Server. A client written Java can encode and send student data to a server written in C#.

## The Lion Data Transfer Protocol (LDTP)
The Lion Data Transfer Protocol is both a TCP protocol and a key-value serialization format.

### TCP Protocol
The protocol consists of sending a ASCII encoded byte string followed by the EOT `u+004` to complete the tranmission. The reciever will send an ACK `u+006` to the the sender to confirm successful recipt of the message. Since the reciever does not know how large the message is going to be, a single message should not be longer than 1024 bytes. This is the default size of the server buffer. The default port for the server is 7366 (PENN).

### Message Format
The message serialization format has the first line key named "Type" followed by the type of object the rest of the key value pairs represent. Keys are seperated from values by the `\t` tab deliniator and key value pairs are seperated by the `\n` new line feed character.

```
"Type"\t[Object Class Name]\n
[key1]\t[value1]\n
[key2]\t[value2]\n
[key3]\t[value3]\n
...
[keyn]\t[valuen]\n
```
## Getting Started
To get started sending data between the client and server start the server using any CLI passing the localhost IP and port to the program.
```
StudentRegistrationServer.exe [ip] [port]
```
To send data to the server start the client passing the server IP and port to the program.
```
java -jar StudentRosterClient.jar [ip] [port] [studentID]
```

### Client Settings
Set the timeout of the client when waiting for a response with the following line of code after you create the server. Default is 10000ms or 10s.
```
client.setTimeout([time in ms]);
```


### Server Settings
Set the timeout of server when waiting for the end of a tranmission with the following line of code after you create the server. Default is 10000ms or 10s.
```
server.Timeout = [time in ms];
```

Set the pending connection queuesize after you create the server but before you start the server. Once the server has started you cannot change the queue. The connection queue denotes how many pending connections are allowed to queue before the server rejects any further connections. Default is 10.
```
server.PendingConnectionQueueSize = [queue size];
```

Set the request buffer size after you create the server but before you start the server. Once the server has started you cannot change the buffer size. Default is 1024.
```
server.BufferSize = [buffer size in bytes];
```
