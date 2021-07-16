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
        public async Task rollDice(int dieSize) => await ReplyAsync($"{rand.Next(1, dieSize)}");

        [Command("roll")]
        public async Task lootRoll() => await ReplyAsync($"{rand.Next(1,100)}");

        [Command("say")]
        [RoleRequired("Admin")]
        public async Task Say([Remainder] string msg ) => await ReplyAsync(msg);
        
    }
}
