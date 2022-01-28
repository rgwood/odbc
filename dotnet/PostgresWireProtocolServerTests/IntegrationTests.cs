using PostgresWireProtocolServer;
using Xunit;

namespace PostgresWireProtocolServerTests;

public class UnitTest1
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
}