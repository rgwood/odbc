using PortForwarderAndPrinter;

var remoteIp = "127.0.0.1";
var remotePort = 5432;
var localPort = 9000;
Console.WriteLine($"Listening on port {localPort} and forwarding to {remoteIp}:{remotePort}");
var server = new Server("127.0.0.1", remotePort, localPort);
await server.StartListener();
