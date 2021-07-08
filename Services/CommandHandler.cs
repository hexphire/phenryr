using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;



namespace Phenryr.Services
{
    public class CommandHandler
    {

        private readonly IConfiguration _config;
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;

        public CommandHandler(IServiceProvider services)
        {
            _config = services.GetRequiredService<IConfiguration>();
            _commands = services.GetRequiredService<CommandService>();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _logger = services.GetRequiredService<ILogger<CommandHandler>>();
            _services = services;


            _commands.CommandExecuted += CommandExecutedAsync;

            _client.MessageReceived += MessageRecievedAsync;
        }

        public async Task InitializeAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        public async Task MessageRecievedAsync(SocketMessage rawMessage)
        {

            if(!(rawMessage is SocketUserMessage message))
            {
                return;
            }
            
            if(message.Source != MessageSource.User)
            {
                return;
            }


            var argPos = 0;

            char prefix = Char.Parse(_config["Prefix"]);

            if(!(message.HasMentionPrefix(_client.CurrentUser, ref argPos) || message.HasCharPrefix(prefix, ref argPos)))
            {
                return;
            }

            var context = new SocketCommandContext(_client, message);

            await _commands.ExecuteAsync(context, argPos, _services);
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {

            if (!command.IsSpecified)
            {
                _logger.LogError($"Command failed to execute for [{context.User.Username}] <-> [{result.ErrorReason}]!");
                return;
            }

            if (result.IsSuccess)
            {
                _logger.LogError($"Command [{command.Value.Name}] executed for -> [{context.User.Username}]");
                return;
            }

            await context.Channel.SendMessageAsync($"Sorry, {context.User.Username} ... something went wrong -> [{result}]!");
        }
    }
}
