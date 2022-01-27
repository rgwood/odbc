using PostgresWireProtocolServer;

Console.WriteLine("I am a Postgres Server");
var wireServer = new WireServer("127.0.0.1", 9876);
wireServer.StartListener();
