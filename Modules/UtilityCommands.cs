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
using Microsoft.Extensions.DependencyInjection;
using Phenryr.Services;

namespace Phenryr.Modules
{
    public class UtilityCommands : ModuleBase
    {
        private Random rand = new Random();

        [Command("rolldie")]
        public Task rollDiceAsync(int dieSize) => ReplyAsync($"{rand.Next(1, dieSize)}");

        [Command("roll")]
        public Task lootRollAsync() => ReplyAsync($"{rand.Next(1,100)}");

        [Command("say")]
        [RequireOwner]
        public Task SayAsync([Remainder] string msg ) => ReplyAsync(msg);
        
    }
}
