# Session Project
After connections are established, it is helpful to create a session per player/user.
Sessions can help with disconnections and reconnecting from other locations (ie WIFI to Cell).
Session enable other cool things like multiple players on one connection. [Split screen clients]

## Entities and Components

### Session Entity
Exists on both Client and Server.
Created on Server when RegisterNewSession command is sent to Server.
Created on Client when AddSesssion command is sent to Client.
- **Session** - Stores SessionId, User Info like Build, Nickname, LastReceived Packet, Timeout time

## Systems
- **AddSessionSystem** - AddSession is a command that takes id, build, and nickname arguments. It creates a session on receiver of command (Server or Client).
- **CreateMessageOnClientSessionOnInputFieldSubmit** - Create an OnSendEvent on Session Entity when client submits input field.
- **DestroySessionOnClientDisconnect** - When a client disconnects, all sessions on client are destroyed
- **MonitorUsedIds** - On Server, monitor SessionIds [in case an irresponsible programmer like me forgets to add or delete used session ids :)]
- **ParseOnReceiveFromSessionEvent** - For OnReceiveEvents On Server or Client Entities, if the first argument is a session Id, create OnReceiveEvents on Session Entities
- **RegisterNewSessionOnConnect** - When a client connects, send a RegisterNewSession command
- **RegisterNewSessionSystem** - RegisterNewSession is a command that takes build as an argument. It is a server only command that creates a new session and replies back AddSession to client.
- **RemoveSessionSystem** - RemoveSession is a command with no arguments that is sent from a client session to remove its corresponding server session.
- **SendMessageOnSession** - Monitors OnSendEvents on Session Entities, and if found sends id + message to corresponding remote session.
- **TimeoutSessionAfterNoActivity** - Destroy Session Entity if Time is ever > sesssion.lastReceived + timeoutTime.