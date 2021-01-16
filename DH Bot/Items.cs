using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace DH_Bot
{
    class Items
    {
        private string[] Pre = {"Arcane","Quick","Defensive","Murderous", "Healthy", "Electric", "Resistant", "Vamp"};
        private string[] WeaponNamesA = {"Sword", "Shield", "Claymore", "Dagger", "Bow", "Staff", "Wand", "Grimoire"};
        private string[] ArmorNamesA = { "Helmet", "Chestplate", "Leggings", "Boots"};



        public static string path;
        public int ID { get; set; }
        public string Slot { get; set; }
        public string Teir { get; set; }
        public string Name { get; set; }
        public string Prefix { get; set; }
        public bool is2h { get; set; }
        public int Str { get; set; }
        public int Spd { get; set; }
        public int Wis { get; set; }

        public int Health { get; set; }
        public int Armor { get; set; }

        public string Effect { get; set; }

        
        public void GenerateItem()
        {
            ID = GenerateID();
            path = @"C:\\Users\\Alex\\source\repos\\DH Bot\\DH Bot\\Storage\\Items\\" + ID + ".Json";
            Console.WriteLine("generated ID");
            while (File.Exists(path))
            {
                ID = GenerateID();
                path = @"C:\\Users\\Alex\\source\repos\\DH Bot\\DH Bot\\Storage\\Items\\" + ID + ".Json";
                Console.WriteLine("generated ID");
            }
            
            
           
            
                if (!File.Exists(path))
                {

                   Teir = GenerateTeir();
                    Console.WriteLine("generated teir");
                    if (Teir == "Shoddy")
                    {
                      Prefix = GeneratePreFix(4);
                        Console.WriteLine("generated prefix");
                    }
                    else
                    {
                        Prefix = GeneratePreFix(7);
                        Console.WriteLine("generated prefix");
                    }
                    if (Prefix == Pre[5])
                    {
                        Effect = "Electric";
                    }
                    else if (Prefix == Pre[6])
                    {
                        Effect = "Resistant";
                    }
                    else if (Prefix == Pre[7])
                    {
                        Effect = "Vamp";
                    }
                    else
                    {
                        Effect = null;
                    }
                    
                    Name = GenerateName();
                    Console.WriteLine("generated Name");
                    GenerateStats(Prefix, Name, Teir);
                    GenerateSlot(Name);
                    Console.WriteLine("generated Stats");
                }
                



        }
        public int GenerateID()
        {
           
                Random r = new Random();
                return r.Next(1, 99999999);
          
            
        }
        public string GenerateTeir()
        {
            Random r = new Random();
            int rng = r.Next(0, 1000);
            if (rng <= 30 )
            {
               
                if (rng <= 1)
                {
                    return "Unreal";
                }
                else
                {
                    return "Legendary";
                }
            }
            else if (rng >= 200)
            {
                return "Shoddy";
            }
            else
            {
                return "Rare";
            }
           
        }
        public string GenerateName()
        {
            Random r = new Random();
           int choice = r.Next(0, 1);
            if (choice == 0)
            {
                return WeaponNamesA[r.Next(0, 7)];
            }
            else
            {
                return ArmorNamesA[r.Next(0, 3)];
            }
            
        }
        public string GeneratePreFix(int range)
        {
           
            Random r = new Random();
            return Pre[r.Next(0, range)]; 
        }

        public void GenerateStats(string pref, string n, string t)
        {
            if (pref == Pre[0])
            {
                Health += 10;
                Wis += 4;
            }
            else if (pref == Pre[1])
            {
                Health += 10;
                Spd += 2;
                Str += 1;
            }
            else if (pref == Pre[2])
            {
                Health += 10;
                Armor += 5;
            }
            else if (pref == Pre[3])
            {
                Health += 10;
                Str += 2;
            }
            else if (pref == Pre[4])
            {
                Health += 20;
                Armor += 2;
            }
            //       public string[] WeaponNamesA = { "Sword", "Shield", "Claymore", "Dagger", "Bow", "Staff", "Wand", "Grimoire" };

            if (n == WeaponNamesA[0])
            {
                Str += 3;
            }
            else if (n == WeaponNamesA[1])
            {

                if (t == "Rare")
                {
                    
                    Armor += 15;
                }
                else if (t == "Legendary")
                {
                    
                    Armor += 20;
                }
                else if (t == "Shoddy")
                {
                    
                    Armor += 10;
                }
                else if (t == "Unreal")
                {
                   
                    Armor += 50;
                }

            }
            else if (n == WeaponNamesA[2])
            {
                Str += 6;
                Spd -= 1;
            }
            else if (n == WeaponNamesA[3])
            {
                Str += 2;
                Spd += 4;
            }
            else if (n == WeaponNamesA[4])
            {
                Str += 3;
                Spd += 2;
                Wis += 1;
            }
            else if (n == WeaponNamesA[5])
            {
                Str += 1;
                Wis += 4;
            }
            else if (n == WeaponNamesA[6])
            {
                Spd += 1;
                Wis += 3;
            }
            else if (n == WeaponNamesA[7])
            {
                Wis += 4;
            }

            //+ to all stats 
            if (t == "Rare")
            {
                Random r = new Random();
                Str += r.Next(3, 6);
                Spd += r.Next(3, 6);
                Wis += r.Next(3, 6);
            }
            else if (t == "Legendary")
            {
                Random r = new Random();
                Str += r.Next(7, 11);
                Spd += r.Next(7, 11);
                Wis += r.Next(7, 11);
            }
            else if (t == "Shoddy")
            {
                Random r = new Random();
                Str += r.Next(1, 2);
                Spd += r.Next(1, 2);
                Wis += r.Next(1, 2);
            }
            else if (t == "Unreal")
            {
                Random r = new Random();
                Str += r.Next(12,19);
                Spd += r.Next(12, 19);
                Wis += r.Next(12, 19);

            }
        }




        public void GenerateSlot(string iname)
        {           
            if (iname == "Sword")
            {
                Slot = "MainHand";
                is2h = false;
            }
            else if (iname == "Shield")
            {
                Slot = "OffHand";
                is2h = false;
            }
            else if (iname == "Claymore")
            {
                Slot = "MainHand";
                is2h = true;
            }
            else if (iname == "Dagger")
            {
                Slot = "MainHand";
                is2h = false;
            }
            else if (iname == "Bow")
            {
                Slot = "MainHand";
                is2h = true;
            }
            else if (iname == "Staff")
            {
                Slot = "MainHand";
                is2h = true;
            }
            else if (iname == "Wand")
            {
                Slot = "MainHand";
                is2h = false;
            }
            else if (iname == "Grimoire")
            {
                Slot = "OffHand";
                is2h = false;
            }
            else if (iname == "Helmet")
            {
                Slot = "Helm";
                
            }
            else if (iname == "Chestplate")
            {
                Slot = "Chest";
              
            }
            else if (iname == "Leggings")
            {
                Slot = "Legs";
                
            }
            else if (iname == "Boots")
            {
                Slot = "Boots";
               
            }

        }
    }
}
