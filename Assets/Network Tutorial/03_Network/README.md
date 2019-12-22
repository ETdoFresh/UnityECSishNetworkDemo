# Network Client/Server Project
This projects replaces previous Local Client and Server, and replaces it with a Network Client and Server

## Entities and Components

### TCPServer Entity
A server that using TCP protocol over sockets.
It keeps a list of *SocketClientConnnections* that have connected to server.
- **Server** - A tag for indicating this is a server
- **TCPServer** - Stores a list of *SocketClientConnections*. Special MonoBehaviour that launches events listed below [and Receive, Send events]

### TCPClient Entity
A client that connects to a tcp server.
- **Client** - A tag for indicating this is a client
- **TCPClient** - Special MonoBehaviour that launches events listed below [and Receive, Send events]

### ClientConnection Entity
An entity representing a client's connection on the server.
- **SocketClientConnection** - Holds connection data such as socket, host, port, protocol

## Events
- **ClientOnCloseEvent** - Client event when a client disconnects
- **ClientOnOpenEvent** - Client event when a client connects
- **OnErrorEvent** - Event when an error occurs on the network connection
- **ServerOnCloseEvent** - Server event when a client disconnects
- **ServerOnOpenEvent** - Server event when a client connects
- **ServerOnServerCloseEvent** - Server event when a server stops listening
- **ServerOnServerOpenEvent** - Server event when a server starts listening

## Systems
- **OutputClientConnectionEventsToConsole** - Shows Client connects and disconnects on console
- **OutputOnErrorEventToConsole** - Shows error on console
- **OutputServerOnCloseEventToConsole** - Show Client disconnecting from Server on console
- **OutputServerOnOpenEventToConsole** - Show Client connecting to Server on console
- **OutputServerOnServerOpenEventToConsole** - Show Server is listening on console
- **SendFromClientOnSendEventSystem** - For OnSendEvents on Client, TCP Send to Server
- **SendFromTCPServerToAllTCPClients** - For OnSendEvents on Server, TCP Send to all Clients
- **SendFromTCPServerToTCPClientConnection** - For OnSendEvents on Client Connection on Server , TCP Send to Client Connection