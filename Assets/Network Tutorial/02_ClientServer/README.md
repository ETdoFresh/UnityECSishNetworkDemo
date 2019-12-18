# Client/Server Project
This projects demonstrates how multiple clients connect to one server.
The communication only occurs between client and server.
There is no direct communication between clients.

## Entities and Components

### LocalServer Entity
A server that is running locally [no sockets, no networking, etc].
It keeps a list of clients that are enabled in any active scnene.
- **Server** - A tag for indicating this is a server
- **LocalServer** - Stores a list of local clients

### LocalClient Entity
A client that is running locally [no sockets, no networking, etc].
It has a link to the server, but not to any other local clients.
- **Client** - A tag for indicating this is a client
- **LocalClient** - Stores a reference to a server

## Events
- **OnLocalClientAccepted** - Server event when a client connects
- **OnLocalClientDisconnected** - Server event when a client disconnects

## Systems
- **ConnectLocalClientsToLocalServer** - Continuously add/remove clients as they are enabled/disabled to the server
- **CreateClientOnSendEventOnInputFieldSubmitEvent** - Creates OnSendEvents on Client when Client Input Field is submitted
- **CreateServerOnSendEventOnInputFieldSubmitEvent** - Creates OnSendEvents on Server when Server Input Field is submitted
- **OutputOnLocalClientConnectionEvents** - Display OnLocalClientAccepted and Disconnected on UIConsole
- **SendFromLocalClientToLocalServer** - For OnSendEvents on Client, Create OnReceiveEvents on local server
- **SendFromLocalServerToLocalClients** - For OnSendEvents on Server, Create OnReceiveEvents on all local clients