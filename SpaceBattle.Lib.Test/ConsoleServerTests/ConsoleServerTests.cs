namespace SpaceBattle.Lib.Test;

public class ConsoleServerTests
{
    int n_threads = 5;

    [Fact]
    public void ConsoleServerTest()
    {
        var server = new ConsoleServer(n_threads);
        server.Execute();
    }
}
