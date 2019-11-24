// For in browser testing purposes!
var webSocketInstances = [];
var LibraryWebSockets = {
    SocketCreate: function (url) {
        var socket = {
            socket: new WebSocket(url),
            errors: null,
            messages: [],
            url: url
        }

        socket.socket.binaryType = 'arraybuffer';

        socket.socket.onmessage = function (e) {
            if (e.data instanceof ArrayBuffer) {
                var array = new Uint8Array(e.data);
                socket.messages.push(array);
            }
            else if (e.data instanceof Blob) {
                var reader = new FileReader();
                reader.addEventListener("loadend", function () {
                    var array = new Uint8Array(reader.result);
                    socket.messages.push(array);
                });
                reader.readAsArrayBuffer(e.data);
            }
            else if (typeof e.data === "string") {
                var reader = new FileReader();
                reader.addEventListener("loadend", function () {
                    var array = new Uint8Array(reader.result);
                    socket.messages.push(array);
                });
                var blob = new Blob([e.data]);
                reader.readAsArrayBuffer(blob);
            }
        };

        socket.socket.onclose = function (e) {
            if (e.code != 1000) {
                if (e.reason != null && e.reason.length > 0)
                    socket.error = e.reason;
                else {
                    switch (e.code) {
                        case 1001:
                            socket.error = "Endpoint going away.";
                            break;
                        case 1002:
                            socket.error = "Protocol error.";
                            break;
                        case 1003:
                            socket.error = "Unsupported message.";
                            break;
                        case 1005:
                            socket.error = "No status.";
                            break;
                        case 1006:
                            socket.error = "Abnormal disconnection.";
                            break;
                        case 1009:
                            socket.error = "Data frame too large.";
                            break;
                        default:
                            socket.error = "Error " + e.code;
                    }
                }
            }
        };

        var instance = webSocketInstances.push(socket) - 1;
        return instance;
    },

    SocketUrl: function (socketInstance) {
        var socket = webSocketInstances[socketInstance];
        return socket.url;
    },

    SocketState: function (socketInstance) {
        var socket = webSocketInstances[socketInstance];
        return socket.socket.readyState;
    },

    SocketError: function (socketInstance, ptr, bufsize) {
        var socket = webSocketInstances[socketInstance];
        if (socket.error == null)
            return 0;
        var str = socket.error.slice(0, Math.max(0, bufsize - 1));
        writeStringToMemory(str, ptr, false);
        return 1;
    },

    SocketSend: function (socketInstance, message) {
        var socket = webSocketInstances[socketInstance];
        socket.socket.send(message);
    },

    SocketRecvLength: function (socketInstance) {
        var socket = webSocketInstances[socketInstance];
        if (!socket || socket.messages.length == 0)
            return 0;
        return socket.messages[0].length;
    },

    SocketRecv: function (socketInstance) {
        var socket = webSocketInstances[socketInstance];
        if (socket.messages.length == 0)
            return null;
        var message = socket.messages[0];
        socket.messages = socket.messages.slice(1);
        return String.fromCharCode.apply(null, message);
    },

    SocketClose: function (socketInstance) {
        var socket = webSocketInstances[socketInstance];
        socket.socket.close();
    }
};
