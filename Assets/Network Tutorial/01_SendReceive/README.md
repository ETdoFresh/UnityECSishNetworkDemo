# Peer-to-Peer Project
Using an input box, peers are able to send and receive messages to other active peers.

## Entities and Components

### PeerToPeer Entity
A peer who is a sender and receiver of messages.
- **LocalPeerClient** - Stores a list of other active peers

### Panel Entity (Ammended)
A Console Panel that allows input via an input field and shows messages in a text box.
- **SplitScreenPanel** - Panel Data storing if isFocused and position/size of the panel
- **InputField.InputFieldEventListener** - Not a component, but creates an event on submit
- **Console.UIConsole** - The UI Console that contains a direct pointer to console text box

## Events
Events are special data components that last exactly one frame.
Event components are created/queued for the next frame.
Event components are added at the beginning of a frame.
Event components are destroyed at the end of a frame.
- **InputFieldSubmitEvent** - When input field is submitted, and contains the text that was submitted
- **OnReceiveEvent** - When an entity receives a message
- **OnSendEvent** - When an entity sends a message

## Systems
- **CreateLocalPeerOnSendEventOnInputField** - Creates an OnSendEvent on all peers in same scene as InputFieldSubmitEvent
- **ConnectLocalPeerToOtherLocalPeers** - Continuously add/remove other peers as they are enabled/disabled
- **OutputOnReceiveEventToConsole** - Display OnReceiveEvents on UIConsole
- **OutputOnSendEventToConsole** - Display OnSendEvents on UIConsole
- **SendToOtherLocalPeers** - For OnSendEvents, Create OnReceiveEvents on other local peers