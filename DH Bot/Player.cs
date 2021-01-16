using System;
using System.Collections.Generic;
using System.Text;

namespace DH_Bot
{
    public class Player
    {

        public int Lvl { get; set; }
        public int Exp { get; set; }
        public int Health { get; set; }

        public int MaxHealth { get; set; }
        public int Armor { get; set; }
        public int MaxArmor { get; set; }
        public bool Holding2h { get; set; }
        public int Str { get; set; }
        public int Stam { get; set; }
        public int MaxStam { get; set; }
        public int Dungeonstam  { get; set; }
        public int ExploreStam { get; set; }
        public int Spd { get; set; }
        public int Wisdom { get; set; }
        public int Etherium { get; set; }
        public DateTime DailyCheckin { get; set; }

        public int Helm { get; set; }
        public int Chest { get; set; }
        public int Legs { get; set; }
        public int Boots { get; set; }

        public int Mainhand { get; set; }
        public int Offhand { get; set; }
        public string WeaponClass { get; set; }
        public int ItemBuffer { get; set; }
       
        public bool InExplore { get; set; }
        public bool InDungeon { get; set; }
        public bool InRegion { get; set; }

        public int[] AbilityList = new int[3];
    }
}
