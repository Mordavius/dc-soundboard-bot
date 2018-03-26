using System.Threading.Tasks;
using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;
using System.Linq;

namespace MyBot
{
    public class CommandHandler
    {
        private IDependencyMap _map;
        private CommandService _cmds;
        private DiscordSocketClient _client;

        public async Task Install(DiscordSocketClient c)
        {
            _client = c;
            _cmds = new CommandService();
            _map = new DependencyMap();
            _map.Add(new AudioService());

            await _cmds.AddModulesAsync(Assembly.GetEntryAssembly());

            _client.MessageReceived += HandleCommand;
        }

        public async Task HandleCommand(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            if (msg == null) return;

            var context = new SocketCommandContext(_client, msg);

            int argPos = 0;
            if (msg.HasStringPrefix("^", ref argPos) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var result = await _cmds.ExecuteAsync(context, argPos,_map);

                if (!result.IsSuccess) await context.Channel.SendMessageAsync(result.ToString());
            }
        }
    }
}