using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DH_Bot
{
    class Raid 
    {
        public static bool raidRefresh = false;
        private static DateTime oldTime;
        private static Timer aTimer;
        public void Start()
        {


            oldTime = DateTime.Now;
            RaidTimer();
        }


        //timed methods
        public void RaidTimer()
        {

            aTimer = new System.Timers.Timer();
            aTimer.Interval = 600000;

            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += RaidTimerCheck;

            // Have the timer fire repeated events (true is the default)
            aTimer.AutoReset = true;

            // Start the timer
            aTimer.Enabled = true;
        }
        private void RaidTimerCheck(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);
            if (DateTime.Compare(DateTime.Now, oldTime.AddHours(168)) == 1)
            {
                Console.WriteLine("it has been a week, resetting raid"); 
                raidRefresh = true;

              
              oldTime  = DateTime.Now;
            }
            else if (DateTime.Compare(DateTime.Now, oldTime.AddMinutes(10)) == 1)
            {
                raidRefresh = false;
            }
        }        

        public bool Raidenabledreturn() 
        {
            return raidRefresh;
        }
    }
}
