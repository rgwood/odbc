using Npgsql;
using PostgresWireProtocolServer;
using Xunit;

namespace PostgresWireProtocolServerTests;

public class IntegrationTests
{
    [Fact]
    public async Task CanSpinUpAndTearDownServer()
    {
        var cts = new CancellationTokenSource();
        Console.WriteLine("I am a Postgres Server");
        var wireServer = new WireServer("127.0.0.1", 9876, cts.Token);
        var listenerTask = wireServer.StartListener();

        await Task.Delay(300);

        cts.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(async () => await listenerTask);
    }

    [Fact]
    public async Task CanConnectToDb()
    {
        var cts = new CancellationTokenSource();

        try
        {
            Console.WriteLine("I am a Postgres Server");
            var wireServer = new WireServer("127.0.0.1", 9876, cts.Token);
            var listenerTask = wireServer.StartListener();

            var connString = "Host=localhost:9876;Username=mylogin;Password=mypass;Database=mydatabase";

            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            cts.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(async () => await listenerTask);
        }
        finally
        {
            cts.Cancel();
        }

    }
}
