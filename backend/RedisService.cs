using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace backend;

public class RedisService
{
	private readonly ISubscriber _subscriber;

	public RedisService(IOptions<RedisConfig> config)
	{
		var redis = ConnectionMultiplexer.Connect(config.Value.ConnectionString);
		_subscriber = redis.GetSubscriber();
	}

	public async Task PublishMessageAsync(string channel, RedisValue message)
	{
		Console.WriteLine($"Publishing message to '{channel}': {message}");
		await _subscriber.PublishAsync(new RedisChannel(channel, RedisChannel.PatternMode.Literal), message);
	}

	public async Task SubscribeToChannelAsync(string channel, Action<RedisChannel, RedisValue> handler)
	{
		Console.WriteLine($"Subscribing to channel '{channel}'...");
		await _subscriber.SubscribeAsync(new RedisChannel(channel, RedisChannel.PatternMode.Literal), handler);
	}
}