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

namespace phenryr.source
{
    public class Program
    {

        private readonly DiscordSocketClient _client;
        private readonly IConfiguration _config;

        public static void Main(string[] args)
        { 
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public Program()
        {
            _client = new DiscordSocketClient();

            _client.Log += LogAsync;

            _client.Ready += ReadyAsync;

            _client.MessageReceived += MessageRecievedAsync;

            var _builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(path: "config.json");
            _config = _builder.Build();
        }


        public async Task MainAsync()
        {

            await _client.LoginAsync(TokenType.Bot, _config["Token"]);
            await _client.StartAsync();

            await Task.Delay(-1);

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

        private async Task MessageRecievedAsync(SocketMessage message)
        {

            if(message.Author.Id == _client.CurrentUser.Id)
            {
                return;
            }

            if(message.Content == ".hello")
            {
                await message.Channel.SendMessageAsync("world!");
            }
        }
    }
}
