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

        [Command("8ball")]
        [Alias("ask")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task AskEightBall([Remainder]string args = null)
        {

            var sb = new StringBuilder();

            var embed = new EmbedBuilder();

            var replies = new List<string>();

            replies.Add("yes");
            replies.Add("no");
            replies.Add("maybe");
            replies.Add("hazzzzzy....");
            replies.Add("How dare you ask that... try again later");
            replies.Add("it's possible... i guess");

            embed.WithColor(new Color(0, 255, 0));
            embed.Title = "Welcome to the 8-ball!";


            sb.AppendLine($",");
            sb.AppendLine();

            if (args == null)
            {
                sb.AppendLine("You'd have to ask a question to get an answer...");
            }
            else
            {
                var answer = replies[new Random().Next(replies.Count - 1)];

                sb.AppendLine($"You asked: [**{args}**]...");
                sb.AppendLine();
                sb.AppendLine($"...my reply is [**{answer}**]");

                switch(answer)
                {

                    case "yes":
                    {
                        embed.WithColor(new Color(0, 255, 0));
                        break;
                    }

                    case "no":
                     {
                        embed.WithColor(new Color(255, 0, 0));
                        break;
                     }

                    case "maybe":
                    {
                        embed.WithColor(new Color(255, 255, 0));
                        break;
                    }

                    case "hazzzzzy....":
                    {
                        embed.WithColor(new Color(255, 0, 255));
                        break;
                    }

                    case "How dare you ask that... try again later":
                    {
                        embed.WithColor(new Color(255, 0, 0));
                        break;
                    }

                    case "it's possible... i guess":
                    {
                        embed.WithColor(new Color(255, 255, 0));
                        break;
                    }
                }
            }

            embed.Description = sb.ToString();

            await ReplyAsync(null, false, embed.Build());
        }
    }
}
