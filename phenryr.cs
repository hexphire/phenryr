using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Net;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Phenryr.Services;


namespace Phenryr
{
    public class Program
    {

        private readonly IConfiguration _config;
        private DiscordSocketClient _client;

        public static void Main(string[] args)
        { 
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public Program()
        {
           
            var _builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(path: "config.json");

            _config = _builder.Build();
        }


        public async Task MainAsync()
        {

            using (var services = ConfigureServices())
            {

                var client = services.GetRequiredService<DiscordSocketClient>();
                _client = client;

                client.Log += LogAsync;
                client.Ready += ReadyAsync;
                services.GetRequiredService<CommandService>().Log += LogAsync;

                await client.LoginAsync(TokenType.Bot, _config["Token"]);
                await client.StartAsync();


                await services.GetRequiredService<CommandHandler>().InitializeAsync();

                await Task.Delay(-1);
            }

        }

        private Task LogAsync(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private Task ReadyAsync()
        {
            Console.WriteLine($"Connected as -> [] :)");
            return Task.CompletedTask;
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(_config)
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .BuildServiceProvider();
        }
    }
}
