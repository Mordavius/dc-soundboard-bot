using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace MyBot.Modules.Public
{

    public class PublicModule : ModuleBase
    {
        [Command("Ping")]
        public async Task ping()
        {
            await ReplyAsync(Context.User.Mention + ", Pong!");
        }
    }
}