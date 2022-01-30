using Npgsql;
using PostgresWireProtocolServer;
using NUnit.Framework;

namespace PostgresWireProtocolServerTests;

public class IntegrationTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }

    public async Task CanSpinUpAndTearDownServer()
    {
        var cts = new CancellationTokenSource();
        Console.WriteLine("I am a Postgres Server");
        var wireServer = new WireServer("127.0.0.1", 5432, cts.Token);
        var listenerTask = wireServer.StartListener();

        await Task.Delay(300);

        cts.Cancel();

        Assert.ThrowsAsync<OperationCanceledException>(async () => await listenerTask);
    }

    [Test]
    public async Task CanConnectToDb()
    {
        var cts = new CancellationTokenSource();

        try
        {
            Console.WriteLine("I am a Postgres Server");
            var wireServer = new WireServer("127.0.0.1", 5432, cts.Token);
            var listenerTask = wireServer.StartListener();

            var connString = "Host=localhost:5432;Username=mylogin;Password=mypass;Database=mydatabase";

            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            cts.Cancel();

            Assert.ThrowsAsync<OperationCanceledException>(async () => await listenerTask);
        }
        finally
        {
            cts.Cancel();
        }

    }
}
