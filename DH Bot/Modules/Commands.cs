using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Timers;

namespace DH_Bot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        
       




        //Global Varibles
        string userID;
        public Player player = new Player();
        Items item = new Items();
         Monsters m = new Monsters();
        Spells SP = new Spells();
        RaidBosses currentraidboss = new RaidBosses();


        //commands
        [Command("JoinHunters")]
        public async Task Joining()
        {           
            userID = Context.User.Id.ToString();
            string path = @"C:\\Users\\Alex\\source\repos\\DH Bot\\DH Bot\\Storage\\Players\\" + userID + ".Json";
            if (!File.Exists(path))
            {
                player.Lvl = 1;
                player.Health = 50;
                player.MaxHealth = 50;
                player.Armor = 20;
                player.MaxArmor = 20;
                player.Spd = 2;
                player.Str = 5;
                player.Wisdom = 10;                
                player.Stam = 5;
                player.Dungeonstam = 3;
                player.ExploreStam = 1;
                player.DailyCheckin = DateTime.Now;
                player.Mainhand = 0;
                player.Offhand = 0;
                player.Helm = 0;
                player.Chest = 0;
                player.Legs = 0;
                player.Boots = 0;
               
                player.Etherium = player.Etherium + CalcEth();
                Ser();
                await Context.Channel.SendMessageAsync(Context.User.Mention + "\tYou Have Joined The Hunter Guild\nThanks with checking in with me, Ive added " + CalcEth() + " 💎 into your account\nThere are three classes you can become proficient at, these are:\n Warrior \t Assassin \t Mage \nPlease chose one of the following commands to select your class:\n $SetClassWarrior \t$SetClassAssassin \t$SetClassMage");
            //    await ReplyAsync("You Have Joined The Hunter Guild");
            //    await ReplyAsync("Thanks with checking in with me, Ive added " + CalcEth() + " 💎 into your account");
            //    await ReplyAsync("There are three classes you can become proficient at these are:\n Warrior \t Assassin \t Mage \nPlease chose one of the following commands to select your class:\n $SetClassWarrior \t$SetClassAssassin \t$SetClassMage ");
            }
            else
            {
                await Context.Channel.SendMessageAsync(Context.User.Mention + "Your already Part of the hunters Guild you cannot join twice");
            }
          
        }

        [Command("Stats")]
        public async Task DisplaystatsT()
        {
            userID = Context.User.Id.ToString();
            await ReplyAsync("Getting Your Stats");
            Des();            
            var eb = new EmbedBuilder();
            eb.WithTitle("("+player.Lvl+") "+Context.User.Username+" Stats:");
            if (player.WeaponClass == "Warrior")
            {
                eb.WithThumbnailUrl("https://cdn.discordapp.com/attachments/794883940111351808/795579590642237440/5050sw.png");               
            }
            else if (player.WeaponClass == "Assasin")
            {
                eb.WithThumbnailUrl("https://cdn.discordapp.com/attachments/794883940111351808/795584107845386271/hiclipart.com.png");
            }
            else if (player.WeaponClass == "Mage")
            {
                eb.WithThumbnailUrl("https://cdn.discordapp.com/attachments/794883940111351808/795583596713738240/5050mag.png");
            }
            

            eb.WithDescription(DisplayStats()+"\n💎: " + player.Etherium);
            eb.Color = Color.Green;
            
            await Context.Channel.SendMessageAsync(Context.User.Mention, false, eb.Build());
        }

        [Command("Checkin")]
        public async Task Checkin()
        {
            userID = Context.User.Id.ToString();
            Des();
            if (DateTime.Compare(DateTime.Now, player.DailyCheckin.AddDays(1))==-1)
            {
                await ReplyAsync(Context.User.Mention + " Sorry you can only check in once a day come back tomorrow ");
            }
            else
            {
                await ReplyAsync(Context.User.Mention + " Thanks with checking in with me, Ive added " + CalcEth() + " 💎 into your account");
                player.Etherium = player.Etherium + CalcEth();
                player.DailyCheckin = DateTime.Now;
                player.Stam = 5;
                player.Dungeonstam = 3;
                player.ExploreStam = 1;
                Ser();
            }
            
        }


        [Command("TestItemGen")]
        public async Task Testitem(
        [Summary("The slot to check")]
       int amount)        
        {
            for (int i = 0; i<= amount; i++)
            {
                userID = Context.User.Id.ToString();
                item = new Items();
                item.GenerateItem();
                SerItem();
            }
            
        }

        [Command("TestEquip")]
        public async Task TestEquip()
        {
            userID = Context.User.Id.ToString();
            SaveItem("MainHand", 20787746,false);
            
        }

        //these set the class type of the player when the execute the command making them have diff stats and abilities
        [Command("SetClassWarrior")]
        public async Task ClassWarrior()
        {
            userID = Context.User.Id.ToString();
            Des();
            player.WeaponClass = "Warrior";
            Ser();
            await Context.Channel.SendMessageAsync(Context.User.Mention + " You Have joined as the Warrior Class\n You will now get bonuses with Swords and with Shields");
        }

        [Command("SetClassAssassin")]
        public async Task ClassAssassin()
        {
            userID = Context.User.Id.ToString();
            Des();
            player.WeaponClass = "Assasin";
            Ser();
            await Context.Channel.SendMessageAsync(Context.User.Mention + " You Have joined as the Assassin Class\n You will now get bonuses with Daggers and Bows");
        }
        [Command("SetClassMage")]
        public async Task ClassMage()
        {
            userID = Context.User.Id.ToString();
            Des();
            player.WeaponClass = "Mage";
            Ser();
            await Context.Channel.SendMessageAsync(Context.User.Mention+" You Have joined as the Mage Class\nYou will now get bonuses with Wands, Griamores and with Staffs");
            
        }

        //a command to make base json file for monsters
        [Command("Makemonster")]
        public async Task monsterbase()
        {

            m = new Monsters();
            m.Name = "basejson";
            SerM();
            await ReplyAsync("created monster base file");
        }
        //testing making spells
        //[Command("Makespelljson")]
        //public async Task Spell()
        //{
        //    SP = new Spells();
        //    SP.SpellDictionary.Add(1,"basejson");
        //    SP.SpellDictionary.Add(2, "basejsonspell1");
        //    SP.SpellRatio.Add(1, 0.7);
        //    SP.SpellRatio.Add(2, 0.8);
        //    SerSP();
        //    await ReplyAsync("created spells base file");
        //}

        //check item command checks the players buffer item so they can check multiple times if needed to see if they waant to replace the item on the same slot
        [Command("CheckItem")]
        public async Task CheckItem()
        {
            userID = Context.User.Id.ToString();
            Des();
            if (player.ItemBuffer != 0)
            {
                DesItem(player.ItemBuffer);
                var eb = new EmbedBuilder();
                eb.WithTitle(DisplayITitle());
                eb.WithDescription(DisplayIStats());
                eb.Color = Color.DarkBlue;
                eb.WithFooter(Context.User.Username + " Do you Wish to Replace Your Current item with this one\nPlease Use the commands $ReplaceItem $TrashItem");
                await Context.Channel.SendMessageAsync(Context.User.Mention, false, eb.Build());

            }
            else
            {
                await Context.Channel.SendMessageAsync(Context.User.Mention + " You have no item to check");
            }
        }

        // displays the item in slot such as "$Item offhand" will display name and stats of the weapon to the dcord server
        [Command("Item")]
        [Summary("displays item stats")]
        public async Task Item(
        [Summary("The slot to check")]
        string slot)
        {
            slot = slot.ToLower();
            userID = Context.User.Id.ToString();
            Des();
            if (slot == "offhand")
            {
                if (player.Offhand != 0)
                {

                DesItem(player.Offhand);
                var eb = new EmbedBuilder();
                eb.WithTitle(DisplayITitle());
                eb.WithDescription(DisplayIStats());
                eb.Color = Color.DarkBlue;
                await Context.Channel.SendMessageAsync(Context.User.Mention, false, eb.Build());
                }
                else
                {
                    await Context.Channel.SendMessageAsync(Context.User.Mention + " Sorry $Item " + slot + " Has nothing equiped");
                }
            }
            else if (slot == "mainhand")
            {
                if (player.Mainhand != 0)
                {
                DesItem(player.Mainhand);
                var eb = new EmbedBuilder();
                eb.WithTitle(DisplayITitle());
                eb.WithDescription(DisplayIStats());
                eb.Color = Color.DarkBlue;
                await Context.Channel.SendMessageAsync(Context.User.Mention, false, eb.Build());
                }
                else
                {
                    await Context.Channel.SendMessageAsync(Context.User.Mention + " Sorry $Item " + slot + " Has nothing equiped");
                }
            }
            else if (slot == "helm")
            {
                if (player.Helm != 0)
                {
                DesItem(player.Helm);
                var eb = new EmbedBuilder();
                eb.WithTitle(DisplayITitle());
                eb.WithDescription(DisplayIStats());
                eb.Color = Color.DarkBlue;
                await Context.Channel.SendMessageAsync(Context.User.Mention, false, eb.Build());
                }
                else
                {
                    await Context.Channel.SendMessageAsync(Context.User.Mention + " Sorry $Item " + slot + " Has nothing equiped");
                }
            }
            else if (slot == "chest")
            {
                if (player.Chest != 0)
                {
                    DesItem(player.Chest);
                    var eb = new EmbedBuilder();
                    eb.WithTitle(DisplayITitle());
                    eb.WithDescription(DisplayIStats());
                    eb.Color = Color.DarkBlue;
                    await Context.Channel.SendMessageAsync(Context.User.Mention, false, eb.Build());
                }
                else
                {
                    await Context.Channel.SendMessageAsync(Context.User.Mention + " Sorry $Item " + slot + " Has nothing equiped");
                }

            }
            else if (slot == "legs")
            {
                if (player.Legs != 0)
                {
                    DesItem(player.Legs);
                    var eb = new EmbedBuilder();
                    eb.WithTitle(DisplayITitle());
                    eb.WithDescription(DisplayIStats());
                    eb.Color = Color.DarkBlue;
                    await Context.Channel.SendMessageAsync(Context.User.Mention, false, eb.Build());
                }
                else
                {
                    await Context.Channel.SendMessageAsync(Context.User.Mention + " Sorry $Item " + slot + " Has nothing equiped");
                }
            }
            else if (slot == "boots")
            {
                if (player.Boots !=0)
                {
                    DesItem(player.Boots);
                    var eb = new EmbedBuilder();
                    eb.WithTitle(DisplayITitle());
                    eb.WithDescription(DisplayIStats());
                    eb.Color = Color.DarkBlue;
                    await Context.Channel.SendMessageAsync(Context.User.Mention, false, eb.Build());
                }
                else
                {
                    await Context.Channel.SendMessageAsync(Context.User.Mention + " Sorry $Item " + slot + " Has nothing equiped");
                }
               

            }
            else
            {
                await Context.Channel.SendMessageAsync(Context.User.Mention+ " Sorry $Item "+slot+" is not a command");

            }
        }
        //the replace item and trash item commands are for the buffer item 
        [Command("ReplaceItem")]
        public async Task ReplaceItem()
        {
            userID = Context.User.Id.ToString();
            Des();
            if (player.ItemBuffer != 0)
            {
                DesItem(player.ItemBuffer);
                SaveItem(item.Slot, item.ID, item.is2h);
                player.ItemBuffer = 0;
                await Context.Channel.SendMessageAsync(Context.User.Mention +" "+ item.Slot + " Has Been replaced by " +DisplayITitle());
            }
            else
            {
                await Context.Channel.SendMessageAsync(Context.User.Mention + " There is no item to replace");
            }
           
            Ser();

       

        }
        [Command("TrashItem")]
        public async Task TrashItem()
        {
            userID = Context.User.Id.ToString();
            Des();
            player.ItemBuffer = 0;
            Ser();
            await Context.Channel.SendMessageAsync(Context.User.Mention + " Available item has been trashed");

        }

        //this replies to the user on dcord and displays the current raid boss status
        [Command("RaidBoss status")]
        public async Task Raidboss()
        {

            DesRaidBoss();
            DisplayRaidboss();
            SerRaidBoss();
            
        }
        //this is the same command but displays dmg done and taken at the same time
        public async Task Raidboss(int rdmg, int pdmg)
        {

            DesRaidBoss();
            DisplayRaidboss(rdmg,pdmg);
            SerRaidBoss();

        }

        //this is the current fight command example "$fight raid" will select the raid boss to fight and check player varibles if they are dead ect
        [Command("Fight")]
        [Summary("Attacks the raid boss")]
        public async Task AttackRB(
        [Summary("Attacks the boss")]
        string r)
        {

            r = r.ToLower();
            userID = Context.User.Id.ToString();
            Des();
            DesRaidBoss();
            if (r == "raid")
            {
                if (currentraidboss.Health <= 0)
                {
                    await Context.Channel.SendMessageAsync(Context.User.Mention + "Sorry but the raid boss is dead please wait till reset");
                }
                else if (!(player.Health > 0))
                {
                    await Context.Channel.SendMessageAsync(Context.User.Mention + "Sorry but Your incapacitated and cannot fight until you have healed");

                }
                else if (!player.InDungeon && !player.InExplore && !player.InRegion)
                {

                    if (player.Stam == 0)
                    {
                        await Context.Channel.SendMessageAsync(Context.User.Mention + "Sorry your Character Has done too many raids\nMake sure you dont forget to check in to refresh your stam");
                    }
                    else
                    {

                        int Pdmg = AttackRB();
                        int Rbdmg = RBAttackBack(currentraidboss.DMG, currentraidboss.MaxAttacks);
                        player.Stam -= 1;
                        Ser();
                       
                        SerRaidBoss();
                        Raidboss(Rbdmg,Pdmg);

                    }

                }
                else
                {
                    await Context.Channel.SendMessageAsync(Context.User.Mention + "Sorry your Character is doing somthing else");
                }
            }
           
        }













        //methods
        //displays current player stats (string format)
        public string DisplayStats()
        {            
            return "HP: " + player.Health.ToString()+"/"+player.MaxHealth + " | Armor: " + player.Armor.ToString()+" / "+player.MaxArmor + " | SPD: " + player.Spd.ToString() + " |  STR:  " + player.Str.ToString() + " | Wisdom: "+ player.Wisdom.ToString()+" | "+"Stam: "+player.Stam+"\nExp Needed For LVL: "+((player.Lvl*100)-player.Exp);
        }
        // displays current item stats (string format)
        public string DisplayIStats()
        {
             return "HP: "+ item.Health+"\n" + "Armor: " + item.Armor+"\n" + "STR: " + item.Str+"\n" + "SPD: " + item.Spd+"\n" + "WIS: " +item.Wis+"\n2H: "+item.is2h;
        }
        //displays current item title (string format)
        public string DisplayITitle()
        {
            return item.Teir + " " + item.Prefix + " " + item.Name + " ";
        }
        //calc the currency in the game based on stats (want to change it to consecutive checkins instead)
        public int CalcEth()
        {

            double n = (0.05 * (player.Wisdom * player.Wisdom));
            return (int)n;
            
        }
        // serializes current player to json
        public void Ser()
        {
            string path = @"C:\\Users\\Alex\\source\repos\\DH Bot\\DH Bot\\Storage\\Players\\" + userID + ".Json";
            using (StreamWriter file = File.CreateText(path))
            {

                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, player);

            }
        }
        // serializes current monster to json
        public void SerM()
        {
            string path = @"C:\\Users\\Alex\\source\repos\\DH Bot\\DH Bot\\Storage\\Monsters\\" + m.Name + ".Json";
            using (StreamWriter file = File.CreateText(path))
            {

                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, m);

            }
        }
        // serializes current spells to json
        public void SerSP()
        {
            string path = @"C:\\Users\\Alex\\source\repos\\DH Bot\\DH Bot\\Storage\\Spells\\Spelllist.Json";
            using (StreamWriter file = File.CreateText(path))
            {

                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, SP);

            }
        }
        // serializes current item to json
        public void SerItem()
        {
            string path = @"C:\\Users\\Alex\\source\repos\\DH Bot\\DH Bot\\Storage\\Items\\" + item.ID + ".Json";
            using (StreamWriter file = File.CreateText(path))
            {

                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, item);

            }
        }
        // deserializes current player from json to class file
        public void Des()
        {
            string path = @"C:\\Users\\Alex\\source\repos\\DH Bot\\DH Bot\\Storage\\Players\\" + userID + ".Json";
            using (StreamReader file = File.OpenText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                player = (Player)serializer.Deserialize(file, typeof(Player));

            }

        }
        // deserializes current item from json to class file
        public void DesItem(int ID)
        {
            string path = @"C:\\Users\\Alex\\source\repos\\DH Bot\\DH Bot\\Storage\\Items\\" + ID + ".Json";
            using (StreamReader file = File.OpenText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                item = (Items)serializer.Deserialize(file, typeof(Items));

            }

        }
        //calculates dmg acording to str stats this is for basic attackss
        public int CalcStrDmg()
        {
            Des();


            return (int)((player.Str*1.6) +1);
        }
        //calculates spell dmg acording to spell used, this is for random spell casting
        public int CalcSpellDmg(double ratio)
        {
            Des();


            return (int)(player.Wisdom * ratio);
        }
        //this replaces removes the stats from current slot item and replaces the item and applies the new stats (may break still not enought testing done)
        public void SaveItem(string slot, int itemID, bool Is2h)
        {
            
            if (slot == "MainHand")
            {
                if (player.Mainhand != 0)
                {
                    //removing stats of old item
                    DesItem(player.Mainhand);
                    player.Str -= item.Str;
                    player.Spd -= item.Spd;
                    player.Wisdom -= item.Wis;
                    if ((player.MaxHealth -= item.Health) <= player.Health)
                    {
                        player.MaxHealth -= item.Health;
                        player.Health = player.MaxHealth;
                    }
                    if ((player.MaxArmor -= item.Armor) <= player.Armor)
                    {
                        player.MaxArmor -= item.Armor;
                        player.Armor = player.MaxArmor;
                    }
                    SerItem();
                }

                if (itemID != player.Mainhand)
                {
                    //replacing item and adding stats to char sheet
                    player.Mainhand = itemID;
                    DesItem(itemID);
                    player.Str += item.Str;
                    player.Spd += item.Spd;
                    player.Wisdom += item.Wis;
                    player.MaxHealth += item.Health;
                    player.MaxArmor += item.Armor;
                    SerItem();
                }
              

            }
            else if (slot == "OffHand")
            {
                if (player.Offhand != 0)
                {
                    //removing stats of old item
                    DesItem(player.Offhand);
                    player.Str -= item.Str;
                    player.Spd -= item.Spd;
                    player.Wisdom -= item.Wis;
                    if ((player.MaxHealth -= item.Health) <= player.Health)
                    {
                        player.MaxHealth -= item.Health;
                        player.Health = player.MaxHealth;
                    }
                    if ((player.MaxArmor -= item.Armor) <= player.Armor)
                    {
                        player.MaxArmor -= item.Armor;
                        player.Armor = player.MaxArmor;
                    }
                    SerItem();
                }
                if (itemID != player.Offhand)
                {
                    //replacing item and adding stats to char sheet
                    player.Offhand = itemID;
                    DesItem(itemID);
                    player.Str += item.Str;
                    player.Spd += item.Spd;
                    player.Wisdom += item.Wis;
                    player.MaxHealth += item.Health;
                    player.MaxArmor += item.Armor;
                    SerItem();
                }
            }
            else if (slot == "Helm")
            {
                if (player.Helm != 0)
                {
                    //removing stats of old item
                    DesItem(player.Helm);
                    player.Str -= item.Str;
                    player.Spd -= item.Spd;
                    player.Wisdom -= item.Wis;
                    if ((player.MaxHealth -= item.Health) <= player.Health)
                    {
                        player.MaxHealth -= item.Health;
                        player.Health = player.MaxHealth;
                    }
                    if ((player.MaxArmor -= item.Armor) <= player.Armor)
                    {
                        player.MaxArmor -= item.Armor;
                        player.Armor = player.MaxArmor;
                    }
                    SerItem();
                }
                if (itemID != player.Helm)
                {
                    //replacing item and adding stats to char sheet
                    player.Helm = itemID;
                    DesItem(itemID);
                    player.Str += item.Str;
                    player.Spd += item.Spd;
                    player.Wisdom += item.Wis;
                    player.MaxHealth += item.Health;
                    player.MaxArmor += item.Armor;
                    SerItem();
                }
            }
            else if (slot == "Chest")
            {
                if (player.Chest != 0)
                {
                    //removing stats of old item
                    DesItem(player.Chest);
                    player.Str -= item.Str;
                    player.Spd -= item.Spd;
                    player.Wisdom -= item.Wis;
                    if ((player.MaxHealth -= item.Health) <= player.Health)
                    {
                        player.MaxHealth -= item.Health;
                        player.Health = player.MaxHealth;
                    }
                    if ((player.MaxArmor -= item.Armor) <= player.Armor)
                    {
                        player.MaxArmor -= item.Armor;
                        player.Armor = player.MaxArmor;
                    }
                    SerItem();
                }
                if (itemID != player.Chest)
                {
                    //replacing item and adding stats to char sheet
                    player.Chest = itemID;
                    DesItem(itemID);
                    player.Str += item.Str;
                    player.Spd += item.Spd;
                    player.Wisdom += item.Wis;
                    player.MaxHealth += item.Health;
                    player.MaxArmor += item.Armor;
                    SerItem();
                }
            }
            else if (slot == "Legs")
            {
                if (player.Legs != 0)
                {
                    //removing stats of old item
                    DesItem(player.Legs);
                    player.Str -= item.Str;
                    player.Spd -= item.Spd;
                    player.Wisdom -= item.Wis;
                    if ((player.MaxHealth -= item.Health) <= player.Health)
                    {
                        player.MaxHealth -= item.Health;
                        player.Health = player.MaxHealth;
                    }
                    if ((player.MaxArmor -= item.Armor) <= player.Armor)
                    {
                        player.MaxArmor -= item.Armor;
                        player.Armor = player.MaxArmor;
                    }
                    SerItem();
                }
                if (itemID != player.Legs)
                {
                    //replacing item and adding stats to char sheet
                    player.Legs = itemID;
                    DesItem(itemID);
                    player.Str += item.Str;
                    player.Spd += item.Spd;
                    player.Wisdom += item.Wis;
                    player.MaxHealth += item.Health;
                    player.MaxArmor += item.Armor;
                    SerItem();
                }
            }
            else if (slot == "Boots")
            {
                if (player.Boots != 0)
                {
                    //removing stats of old item
                    DesItem(player.Boots);
                    player.Str -= item.Str;
                    player.Spd -= item.Spd;
                    player.Wisdom -= item.Wis;
                    if ((player.MaxHealth -= item.Health) <= player.Health)
                    {
                        player.MaxHealth -= item.Health;
                        player.Health = player.MaxHealth;
                    }
                    if ((player.MaxArmor -= item.Armor) <= player.Armor)
                    {
                        player.MaxArmor -= item.Armor;
                        player.Armor = player.MaxArmor;
                    }
                    SerItem();
                }
                if (itemID != player.Boots)
                {

                    //replacing item and adding stats to char sheet
                    player.Boots = itemID;
                    DesItem(itemID);
                    player.Str += item.Str;
                    player.Spd += item.Spd;
                    player.Wisdom += item.Wis;
                    player.MaxHealth += item.Health;
                    player.MaxArmor += item.Armor;
                    SerItem();
                }
            }
           if (Is2h == true)
            {
                if (player.Offhand != 0)
                {
                    //removing stats of old item
                    DesItem(player.Offhand);
                    player.Str -= item.Str;
                    player.Spd -= item.Spd;
                    player.Wisdom -= item.Wis;
                    if ((player.MaxHealth -= item.Health) <= player.Health)
                    {
                        player.MaxHealth -= item.Health;
                        player.Health = player.MaxHealth;
                    }
                    if ((player.MaxArmor -= item.Armor) <= player.Armor)
                    {
                        player.MaxArmor -= item.Armor;
                        player.Armor = player.MaxArmor;
                    }
                    SerItem();
                }
                //replacing item and adding stats to char sheet
                player.Offhand = 0;
            }
                
        }
        //this gives the exp from the fight or exploration and checks if it is enought to lvl
        public void GiveExp(int xp)
        {
            if (player.Exp +xp >= CalcLvlReq(player.Lvl))
            {
                GivePlayerLvl();
                player.Exp = 0;
            }
            else
            {
                player.Exp += xp;
            }
           


        }
        //this calcs the current exp requirement for the next lvl
        private int CalcLvlReq(int lvl)
        {
           

            return 100*lvl;
        }
        //this lvls the player by 1 and gives stats according to class if player has not selected a class the player will get 1 to all stats instead
        private void GivePlayerLvl()
        {
            if (player.WeaponClass == "Warrior")
            {
                player.MaxHealth += 10;
                player.MaxArmor += 10;
                player.Str += 3;
                player.Spd += 1;
                player.Wisdom += 1;
                player.Health = player.MaxHealth;
            }
            else if (player.WeaponClass == "Assassin")
            {
                player.MaxHealth += 8;
                player.MaxArmor += 5;
                player.Str += 2;
                player.Spd += 2;
                player.Wisdom += 1;
                player.Health = player.MaxHealth;
            }
            else if (player.WeaponClass == "Mage")
            {
                player.MaxHealth += 5;
                player.MaxArmor += 5;
                player.Str += 1;
                player.Spd += 2;
                player.Wisdom += 4;
                player.Health = player.MaxHealth;
            }
            else
            {
                player.MaxHealth += 5;                
                player.MaxArmor += 5;
                player.Str += 1;
                player.Spd += 1;
                player.Wisdom += 1;
                player.Health = player.MaxHealth;
            }
        }
        // deserializes current raid boss
        public void DesRaidBoss()
        {

            string path = @"C:\\Users\\Alex\\source\repos\\DH Bot\\DH Bot\\Storage\\CurrentRaidData.Json";
            using (StreamReader file = File.OpenText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                currentraidboss = (RaidBosses)serializer.Deserialize(file, typeof(RaidBosses));

            }

        }
        //serializes current raid boss 
        public void SerRaidBoss()
        {
            string path = @"C:\\Users\\Alex\\source\repos\\DH Bot\\DH Bot\\Storage\\CurrentRaidData.Json";
            using (StreamWriter file = File.CreateText(path))
            {

                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, currentraidboss);

            }
        }
        //basic attack method is used to calc final dmg stat based on how many attacks the player has * amount of flat dmg done by str scaling
        public int BasicAttack()
        {
            
          return CalcAttackAmount() *CalcStrDmg();

        }
        //requires a spell to be selected (got during attack) and gets the raio from that spell to calc final dmg
        public int AbilityAttack(double ratio)
        {
          
           return CalcSpellDmg(ratio);

        }
        //this is a random check to see if a spell has cast or failed
        public bool CastCheck()
        {

            Random r = new Random();
            if (80 >= r.Next(0,100))
            {
                return true;
            }
            else
            {
                return false;
            }
          
        }
        //this calcs how many attacks the player has dependent on spd
        public int CalcAttackAmount()
        {
            if (player.Spd >300)
            {
              Random r = new Random();
                if (player.Spd >= r.Next(0, 1000))
                {
                    return 3;
                }
                else
                {
                    return 2;
                }
               
            }
            else
            {
                Random r = new Random();
                if (player.Spd >= r.Next(0,100))
                {
                    return 2;
                }
                else
                {
                    return 1;
                }
               
            }
            
        }
        //this calcs if the spell cast does ignore target armor
        public bool CalcSpellIgnore()
        {
            Random r = new Random();
            if (1+((2*player.Wisdom)/3 )  >= r.Next(0,100))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //this is checks the class of the user and attacks the raid boss accordingly
        public int AttackRB()
        {
            if (player.WeaponClass == "Warrior")
            {
                return ApplyDMGToRB(BasicAttack(), false);

            }
           else if (player.WeaponClass == "Assassin")
            {
                return ApplyDMGToRB(BasicAttack(), false);
            }
           else if (player.WeaponClass == "Mage")
            {

                if (CastCheck() == true)
                {
                    Random r = new Random();
                    if (33 >= r.Next(0, 100))
                    {
                        if (player.AbilityList[0] != 0)
                        {
                            return ApplyDMGToRB(AbilityAttack(player.AbilityList[0]), CalcSpellIgnore());
                        }
                        else
                        {
                            return ApplyDMGToRB(BasicAttack(),false);
                        }
                    }
                    else if (66 <= r.Next(0, 100))
                    {
                        if (player.AbilityList[1] != 0)
                        {

                            return ApplyDMGToRB(AbilityAttack(player.AbilityList[1]), CalcSpellIgnore());
                        }
                        else
                        {
                            return ApplyDMGToRB(BasicAttack(), false);
                        }
                    }
                    else
                    { 
                            if (player.AbilityList[2] != 0)
                            {
                            return ApplyDMGToRB(AbilityAttack(player.AbilityList[2]), CalcSpellIgnore());
                            }
                        else
                        {
                            return ApplyDMGToRB(BasicAttack(), false);
                        }
                    }

                }
                else
                {
                   return ApplyDMGToRB(BasicAttack(), false);
                }
            }
            else
            {
                return 0;
            }

        }
        //this simulates the raid boss attacking back
        public int RBAttackBack(int d, int attacks)
        {
            Random r = new Random();
            d = d * (r.Next(1,attacks));

            if (d > player.Armor)
            {
                d -= player.Armor;
                player.Armor = 0;
                player.Health -= d;
                CheckHealth();
                return d;
            }
            else
            {
                player.Armor -= d;
                return d;
            }

        }

        //this checks how the dmg is allocated dependant on armor or health ect
        public int ApplyDMGToRB(int d, bool ArmorIgn)
        {
            if (ArmorIgn == true)
            {
                currentraidboss.Health -= d;
                return d;
            }
            else
            {
                if (d > currentraidboss.Armor)
                {
                    int a = d;
                    d -= currentraidboss.Armor;
                    currentraidboss.Armor = 0;
                    currentraidboss.Health -= d;
                    return a;
                }
                else
                {
                    currentraidboss.Armor -= d;
                    return d;
                }
            }


        }
        //this displays the raid boss in a fancy layout
        public async Task DisplayRaidboss()
        {
           

            var eb = new EmbedBuilder();
            eb.WithTitle(currentraidboss.Name + "\n Stats:");
            eb.WithDescription(currentraidboss.Description + "\n" + DisplayCurrentRaidBossStats());
            eb.WithThumbnailUrl(currentraidboss.ImageURL);
            eb.Color = Color.Magenta;

            await Context.Channel.SendMessageAsync(Context.User.Mention, false, eb.Build());
        }
        //this displays the raid boss in a fancy layout but has parameters that are only used if a player has attacked the raid boss 
        public async Task DisplayRaidboss(int rdmg, int pdmg)
        {


            var eb = new EmbedBuilder();
            eb.WithTitle(currentraidboss.Name + "\n Stats:");
            eb.WithDescription(currentraidboss.Description + "\n" + DisplayCurrentRaidBossStats(rdmg,pdmg));
            eb.WithThumbnailUrl(currentraidboss.ImageURL);
            eb.Color = Color.Magenta;

            await Context.Channel.SendMessageAsync(Context.User.Mention, false, eb.Build());
        }
        //this basicly gets all the stats puts them into a string for layout for the displayraidboss method
        private string DisplayCurrentRaidBossStats(int rdmg,int pdmg)
        {
            return "Health: " + currentraidboss.Health + "|" + currentraidboss.MaxHealth + "\nArmor: " + currentraidboss.Armor + "|" + currentraidboss.MaxArmor + "\nDMG: " + currentraidboss.DMG + "\nAttacks: " + currentraidboss.MaxAttacks + "\nServer Debuff: " + currentraidboss.ServerWideDebuf+"\nYou Dealt: "+pdmg + "\nRaid Boss Dealt: " + rdmg;
        }
        //this basicly gets all the stats puts them into a string for layout for the displayraidboss method

        private string DisplayCurrentRaidBossStats()
        {
            return "Health: " + currentraidboss.Health + "|" + currentraidboss.MaxHealth + "\nArmor: " + currentraidboss.Armor + "|" + currentraidboss.MaxArmor + "\nDMG: " + currentraidboss.DMG + "\nAttacks: " + currentraidboss.MaxAttacks + "\nServer Debuff: " + currentraidboss.ServerWideDebuf;
        }
        //this checks if the player health is below 0 to not bring up errors later on when checking if player is dead
        public async Task CheckHealth()
        {
            if (player.Health <=0)
            {
                player.Health = 0;
                await Context.Channel.SendMessageAsync(Context.User.Mention + "You Have Been incapacitated");
            }
        }



        
    }
}


