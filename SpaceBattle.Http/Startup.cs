namespace SpaceBattle.Http;

public class Startup {
    public void ConfigureServices(IServiceCollection services) {
        services.AddServiceModelServices();
        services.AddSingleton<IMessageProcessor, MessageProcessor>();
    }
}
