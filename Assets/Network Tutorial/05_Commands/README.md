# Commands Project
After establishing Connections and Sessions, Clients and Servers must ask/tell each other what to do.
We do this using Commands.

## Entities and Components
No new entites to discuss here. Commands are created using only systems.

## Systems
- **ChatCommandSystem** - Chat *ChatMessage*. This client command sends ChatMessage to all other clients.
- **ConnectCommandSystem** - Connect. This client command is locally run to connect client to the server.
- **DisconnectCommandSystem** - Disconnect. This client command is locally run to disconnect client from the server.
- **ExitCommandSystem** - Exit. This local command stops the game/programming from running.
- **HelpCommandSystem** - Help. This client command asks the server to send helpful information back to the client. Note: It's not that helpful :P
- **RestartCommandSystem** - Restart. This local command restarts the active scene.
- **TestCommandSystem** - Test. This client command is sent to the server which replies back that it received the message.
- **WhisperCommandSystem** - Whisper *WhisperSessionOrNickname* *WhisperMessage*. This client command sends a WhisperMessage to a particular user using their sessionId or nickname.