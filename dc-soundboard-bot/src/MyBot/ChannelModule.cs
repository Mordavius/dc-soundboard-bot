using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using Discord.Audio;

namespace MyBot
{
    public class ChannelModule : ModuleBase
    {
        private readonly AudioService _service;
        public ChannelModule(AudioService service)
        {
            _service = service;
        }
        
        [Command("Join", RunMode = RunMode.Async), Summary("Makes the bot join the voice channel specified")]
        public async Task Join(IVoiceChannel chan) => await _service.JoinAudio(Context.Guild, chan);

        [Command("Leave", RunMode = RunMode.Async), Summary("Makes the bot leave the current voice channel")]
        public async Task Leave()
        {
            await _service.LeaveAudio(Context.Guild);
        }
        /*[Command("Off"), Summary("Turns off the bot")]
        public async Task Off()
        {
            await 
        }*/
    }
}
