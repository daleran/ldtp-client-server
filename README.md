# LDTP Client and Server
A student project to build a simple TCP/IP Client and Server. A client written Java can encode and send student data to a server written in C#.

## The Lion Data Transfer Protocol (LDTP)
The Lion Data Transfer Protocol is both a TCP protocol and a key-value serialization format.

### TCP Protocol
The protocol consists of sending a ASCII encoded byte string followed by the EOT `u+004` to complete the tranmission. The reciever will send an ACK `u+006` to the the sender to confirm successful recipt of the message. Since the reciever does not know how large the message is going to be, a single message should not be longer than 1024 bytes. This is the default size of the server buffer.

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
java -jar StudentRosterClient.jar [ip] [port]
```
