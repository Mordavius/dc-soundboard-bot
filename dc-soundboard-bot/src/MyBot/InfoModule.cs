using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace MyBot
{
    public class InfoModule : ModuleBase
    {
        [Command("Bot"), Summary("Explains the purpose of the bot")]
        public async Task Bot()
        {
            await ReplyAsync("This is a SoundBoard bot made by Mordavius");
        }
    }
}
