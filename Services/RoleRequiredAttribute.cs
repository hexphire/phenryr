using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace Phenryr.Services
{
    public class RoleRequiredAttribute : PreconditionAttribute
    {

        private readonly string _name;

        public RoleRequiredAttribute(string name) => _name = name;
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            if (context.User is SocketGuildUser gUser)
            {
                if (gUser.Roles.Any(r => r.Name == _name))
                {
                    return Task.FromResult(PreconditionResult.FromSuccess());
                }
                else
                {

                    return Task.FromResult(PreconditionResult.FromError($"Oof, looks like you're missing the {_name} role. You kinda need that for this command."));
                }
            }
            else
            {
                return Task.FromResult(PreconditionResult.FromError($"uh, you aren't on the server. Oh lort baby, what you doin?"));
            }  
        }
    }
}

