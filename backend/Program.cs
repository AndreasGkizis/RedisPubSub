using backend;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = Host.CreateDefaultBuilder(args)
	.ConfigureAppConfiguration((config) =>
	{
		config.SetBasePath(Directory.GetCurrentDirectory());
		config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
		config.AddEnvironmentVariables();
	})
	.ConfigureServices((hostContext, services) =>
	{
		services.AddSingleton(hostContext.Configuration);
		services.Configure<RedisConfig>(hostContext.Configuration.GetSection("Redis"));
		
		//services
		services.AddSingleton<RedisService>();
	});

var app = builder.Build();

var serviceType = Environment.GetEnvironmentVariable(EnvVariables.ServiceTypeKey);
if (serviceType == null)
	throw new ArgumentNullException(nameof(serviceType), $"Missing environment variable '{EnvVariables.ServiceTypeKey}'");

var serviceName = Environment.GetEnvironmentVariable(EnvVariables.ServiceNameKey);

var redisPubSubService = app.Services.GetRequiredService<RedisService>();

if (serviceType.StartsWith("publisher"))
{
	Console.WriteLine("Starting publisher service...");
	int messageCount = 0;
	while (true)
	{
		messageCount++;
		await redisPubSubService.PublishMessageAsync(EnvVariables.ChannelName,
			$"{serviceName} | sent msg to '{EnvVariables.ChannelName}' with ID {messageCount}");
		await Task.Delay(2000);
	}
}
else if (serviceType.StartsWith("consumer"))
{
	int messageCount = 0;

	Console.WriteLine("Starting consumer service...");
	await redisPubSubService.SubscribeToChannelAsync(EnvVariables.ChannelName, (channel, message) =>
	{
		Console.WriteLine($"{serviceName} | Received message on '{channel}': msg :[{message}]");
		messageCount++;
	});
	
	await app.RunAsync();
}