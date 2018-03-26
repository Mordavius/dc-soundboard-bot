using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;

namespace MyBot
{
    public class Program
    {
        // Convert our sync main to an async main.
        public static void Main(string[] args) =>
            new Program().Start().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandHandler _commands;

        public async Task Start()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Verbose
            }
            );
            _commands = new CommandHandler();


            await _client.LoginAsync(TokenType.Bot, "insert token here");
            await _client.StartAsync();

            _client.SetGameAsync($"use !Command");

            _client.Log += Log;

            await _commands.Install(_client);

            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}