using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Http;
using Discord.Commands;
using System;

namespace MyBot
{
    public class ImageModule : ModuleBase
    {
        private static HttpClient client = new HttpClient();
        string[] types = new string[3] { "jpg", "gif", "png" };
        public MatchCollection noots;
        string pingu;
        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        [Command("Sendnoots", RunMode = RunMode.Async), Summary("Get a random picture of pingu from the internet")]
        public async Task SendNoots(string type = "random")
        {
            if (type == "random")
            {
                type = types[RandomNumber(0, 2)];
            }


            if (type == "jpg" || type == "gif" || type == "png")
            {
                var data = await client.GetStringAsync($"https://imgur.com/search/score/all?q_type={type}&q_all=pingu").ConfigureAwait(false);
                noots = Regex.Matches(data, @"<div id=""(.*)"" class=""post""");
                await ReplyAsync($"https://i.imgur.com/{noots[RandomNumber(0,noots.Count)].Groups[1].Value}.{type}");
            }
            else
            {
                await ReplyAsync("No correct type given");
            }

        }
    }
}
