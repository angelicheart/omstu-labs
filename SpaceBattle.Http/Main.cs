namespace SpaceBattle.Http;

[ExcludeFromCodeCoverage]
public class Program {
    static void Main(string[] args) {
        IWebHost host = CreateWebHostBuilder(args).Build();
        host.Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>();
}
    