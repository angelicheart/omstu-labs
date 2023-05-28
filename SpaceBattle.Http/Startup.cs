namespace SpaceBattle.Http;

public class Startup {
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddServiceModelServices()
            .AddServiceModelMetadata();
            services.AddScoped<IMessageProcessor, MessageProcessor>();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseServiceModel(builder =>
        {
            builder.AddService<MessageProcessor>()
            .AddServiceEndpoint<MessageProcessor, IMessageProcessor>(new BasicHttpBinding(), "/MessageProcessor/basicHttp");

            var serviceMetadataBehavior = app.ApplicationServices.GetRequiredService<CoreWCF.Description.ServiceMetadataBehavior>();
            serviceMetadataBehavior.HttpGetEnabled = true;
        });
    }
}
