using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Timers;
using Newtonsoft.Json;
using System.IO;


namespace DH_Bot
{
    class Program
    {
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();
        Raid raidBot = new Raid();
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private static Timer aTimer;
        RaidBosses currentraidboss = new RaidBosses();
        public async Task RunBotAsync()
        {
          
            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            string token = "NjcwNDUwMzA0NDk5NzEyMDMy.Xi-P7Q.xsv9H_mVIfF-O-sqzWi-o3lO_Gs";

            _client.Log += _client_Log;

            await RegisterCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, token);

            await _client.StartAsync();

            raidBot.Start();
            RaidTimer();
            await Task.Delay(-1);

            

        }

        private Task _client_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            if (message.Author.IsBot) return;

            int argPos = 0;
            if (message.HasStringPrefix("$", ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
            }
        }

        
        
            
        


        //timed methods
        public void RaidTimer()
        {

            aTimer = new System.Timers.Timer();
            aTimer.Interval = 300000;

            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += RaidTimerCheck;

            // Have the timer fire repeated events (true is the default)
            aTimer.AutoReset = true;

            // Start the timer
            aTimer.Enabled = true;
        }
        private async void RaidTimerCheck(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);

            if (raidBot.Raidenabledreturn() == true)
            {
                Console.WriteLine("raidreset");
                ulong id = 794883940111351808; // 3
                var chnl = _client.GetChannel(id) as IMessageChannel; // 4
                await chnl.SendMessageAsync("Raid has Reset"); // 5
                SetNewRaidBoss();        
            }

        }

        private void SetNewRaidBoss()
        {
            DesRaidBoss(RandomNewRaidBoss());
            DisplayRaidboss();
            SerRaidBoss();
        }

        private string RandomNewRaidBoss()
        {
            var rand = new Random();
            var files = Directory.GetFiles(@"C:\\Users\\Alex\\source\repos\\DH Bot\\DH Bot\\Storage\\Raidbosses", "*.Json");
            return files[rand.Next(files.Length)];

        }

        public async Task DisplayRaidboss()
        {
            ulong id = 794883940111351808; // 3
            var chnl = _client.GetChannel(id) as IMessageChannel; // 4
            
            var eb = new EmbedBuilder();
            eb.WithTitle(currentraidboss.Name + "\n Stats:");           
            eb.WithDescription(currentraidboss.Description+"\n"+DisplayCurrentRaidBossStats() );
            eb.WithThumbnailUrl(currentraidboss.ImageURL);
            eb.Color = Color.Magenta;

            await chnl.SendMessageAsync(MentionUtils.MentionRole(794883416498634763), false, eb.Build());
        }

        private string DisplayCurrentRaidBossStats()
        {
            return "Health: " + currentraidboss.Health + "|" + currentraidboss.MaxHealth + "\nArmor: " + currentraidboss.Armor + "|" + currentraidboss.MaxArmor + "\nDMG: " + currentraidboss.DMG + "\nAttacks: " + currentraidboss.MaxAttacks + "\nServer Debuff: " + currentraidboss.ServerWideDebuf;
        }

        public void DesRaidBoss(string ID)
        {

            string path = ID;
            using (StreamReader file = File.OpenText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                currentraidboss = (RaidBosses)serializer.Deserialize(file, typeof(RaidBosses));

            }

        }
        public void SerRaidBoss()
        {
            string path = @"C:\\Users\\Alex\\source\repos\\DH Bot\\DH Bot\\Storage\\CurrentRaidData.Json";
            using (StreamWriter file = File.CreateText(path))
            {

                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, currentraidboss);

            }
        }


    }
}
