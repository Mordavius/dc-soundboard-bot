using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml;
using System.Threading;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;



namespace MyBot
{
   public class AudioModule : ModuleBase
    {
        /*MatchCollection namen;
        private static HttpClient client = new HttpClient();

        bool available;*/

        private readonly AudioService _service;

        public AudioModule(AudioService service)
        {
            _service = service;
        }

        string searchTag;
        [Command("Play"), Summary("Picks a sound from https://www.myinstants.com/index/us/ and plays it on choice of player")]
        public async Task Play([Remainder]string soundTag)
        {
            searchTag = Regex.Replace(soundTag, " ", "+");
            await _service.FindSound(searchTag, Context.Channel);
            /*int aangeroepen = 1;
            string opties = "";
            searchTag = Regex.Replace(soundTag, " ", "+");
            var data = await client.GetStringAsync("https://www.myinstants.com/search/?name=" + searchTag).ConfigureAwait(false);
            namen = Regex.Matches(data, "\\/media\\/sounds.*mp3");
            for (int i = 0; i < 10; i++)
            {
                if (i == namen.Count)
                {
                    break;
                }
                    opties += aangeroepen + " https://www.myinstants.com" + namen[i].Value + "\n";
                    aangeroepen++;
            }
            if (namen.Count == 0)
            {
                await ReplyAsync("Tag did not return any results");
            }
            else
            {
                await ReplyAsync(opties);
            }
            if (namen.Count > 0)
            {
                available = true;
            }
            else
            {
                available = false;
            }
            */
        }

        [Command("Pick", RunMode = RunMode.Async), Summary("Picks a sound from the list")]
        public async Task Pick(int naamIndex)
        {
            if (_service.available)
            {
                //await ReplyAsync($"https://www.myinstants.com{_service.namen[naamIndex - 1].Value}");
                await _service.SendAudioAsync(Context.Guild, Context.Channel, $"https://www.myinstants.com{_service.namen[naamIndex - 1].Value}");
            }
            else
            {
                await ReplyAsync("Er is geen geluid beschikbaar");
            }
        }
    }
}
