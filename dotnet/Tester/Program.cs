using System.Diagnostics;
using Npgsql;
using PostgresWireProtocolServer;
using Spectre.Console;

public class Program
{
    public static async Task Main()
    {
        var cts = new CancellationTokenSource();

        var wireServer = new WireServer("127.0.0.1", 5432, cts.Token);
        var listenerTask = wireServer.StartListener();

        var connString = "Host=localhost:5432;Username=mylogin;Password=mypass;Database=mydatabase";

        try
        {
            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex.Demystify());
        }

        cts.Cancel();

        try
        {
            await listenerTask;
        }
        catch (OperationCanceledException) // OK
        {
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex.Demystify());
        }
    }
}
