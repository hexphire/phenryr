using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.Configuration;

namespace Phenryr.Modules
{
    public class ExampleCommands : ModuleBase
    {
        [Command("hello")]
        public async Task HelloCommand()
        {
            var sb = new StringBuilder();

            var user = Context.User;

            sb.AppendLine($"You are -> [{user.Username}]");
            sb.AppendLine("I must now say, World!");

            await ReplyAsync(sb.ToString());
        }
            
        
    }
}
