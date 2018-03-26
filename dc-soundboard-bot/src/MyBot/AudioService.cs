using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Http;
using Discord.Commands;

public class AudioService
{
    private readonly ConcurrentDictionary<ulong, IAudioClient> ConnectedChannels = new ConcurrentDictionary<ulong, IAudioClient>();

    public MatchCollection namen;
    private static HttpClient client = new HttpClient();

    public bool available;

    public async Task JoinAudio(IGuild guild, IVoiceChannel target)
    {
        IAudioClient client;
        if (ConnectedChannels.TryGetValue(guild.Id, out client))
        {
            return;
        }
        if (target.Guild.Id != guild.Id)
        {
            return;
        }

        var audioClient = await target.ConnectAsync();

        if (ConnectedChannels.TryAdd(guild.Id, audioClient))
        {
            //await Log(LogSeverity.Info, $"Connected to voice on {guild.Name}.");
        }
    }

    public async Task LeaveAudio(IGuild guild)
    {
        IAudioClient client;
        if (ConnectedChannels.TryRemove(guild.Id, out client))
        {
            await client.StopAsync();
            //await Log(LogSeverity.Info, $"Disconnected from voice on {guild.Name}.");
        }
    }

    public async Task FindSound(string soundTag, IMessageChannel channel)
    {
        int aangeroepen = 1;
        string opties = "";
        
        var data = await client.GetStringAsync("https://www.myinstants.com/search/?name=" + soundTag).ConfigureAwait(false);
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
            await channel.SendMessageAsync("Tag did not return any results");
        }
        else
        {
            await channel.SendMessageAsync(opties);
        }
        if (namen.Count > 0)
        {
            available = true;
        }
        else
        {
            available = false;
        }
    }

    public async Task SendAudioAsync(IGuild guild, IMessageChannel channel, string path)
    {
        // Your task: Get a full path to the file if the value of 'path' is only a filename.
        /*if (!File.Exists(path))
        {
            await channel.SendMessageAsync("File does not exist.");
            return;
        }*/
        IAudioClient client;
        if (ConnectedChannels.TryGetValue(guild.Id, out client))
        {
            //await Log(LogSeverity.Debug, $"Starting playback of {path} in {guild.Name}");
            var output = CreateStream(path).StandardOutput.BaseStream;
            var stream = client.CreatePCMStream(AudioApplication.Music, 96000, 10);
            await output.CopyToAsync(stream);
            await stream.FlushAsync().ConfigureAwait(false);
        }
    }

    private Process CreateStream(string path)
    {
        return Process.Start(new ProcessStartInfo
        {
            FileName = "ffmpeg.exe",
            Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
            UseShellExecute = false,
            RedirectStandardOutput = true
        });
    }
}