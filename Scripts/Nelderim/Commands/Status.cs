#region References

using Server.Mobiles;
using Server.Items;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Server.Commands
{
    public class StatusCommand
    {
       // Static dictionary to track murder decay times
       private static Dictionary<Serial, MurderInfo> _murderTracker = new Dictionary<Serial, MurderInfo>();

       public static void Initialize()
       {
          CommandSystem.Register("status", AccessLevel.Player, Status_OnCommand);
          
          // Register event handlers for murder tracking
          EventSink.PlayerDeath += OnPlayerDeath;
       }

       // Class to store murder information
       private class MurderInfo
       {
           public int ShortTermMurders { get; set; }
           public int LongTermMurders { get; set; }
           public DateTime[] ShortTermElapse { get; set; }
           public DateTime[] LongTermElapse { get; set; }

           public MurderInfo()
           {
               ShortTermMurders = 0;
               LongTermMurders = 0;
               ShortTermElapse = new DateTime[5]; // Track up to 5 short term murders
               LongTermElapse = new DateTime[15]; // Track up to 15 long term murders
           }
       }

       // Event handler for player deaths to track murders
       private static void OnPlayerDeath(PlayerDeathEventArgs e)
       {
           Mobile killer = e.Killer;
           Mobile victim = e.Mobile;

           // Ensure both are player mobiles and this is a murder scenario
           if (killer is PlayerMobile && victim is PlayerMobile && killer != victim)
           {
               // Check if killer initiated combat or victim was innocent
               bool isMurder = !victim.Criminal && !DidVictimAggress(victim, killer);
               
               if (isMurder)
               {
                   // Get or create murder info for the killer
                   Serial killerSerial = killer.Serial;
                   if (!_murderTracker.ContainsKey(killerSerial))
                       _murderTracker[killerSerial] = new MurderInfo();
                   
                   MurderInfo info = _murderTracker[killerSerial];
                   
                   // Add short term murder (8 hours decay)
                   if (info.ShortTermMurders < 5)
                   {
                       info.ShortTermElapse[info.ShortTermMurders] = DateTime.UtcNow.AddHours(8);
                       info.ShortTermMurders++;
                   }
                   
                   // Add long term murder (40 hours decay)
                   if (info.LongTermMurders < 15)
                   {
                       info.LongTermElapse[info.LongTermMurders] = DateTime.UtcNow.AddHours(40);
                       info.LongTermMurders++;
                   }
               }
           }
       }
       
       // Helper method to check if victim aggressed against killer
       private static bool DidVictimAggress(Mobile victim, Mobile killer)
       {
           // Check if victim has aggressed against killer
           foreach (AggressorInfo aggressor in victim.Aggressed)
           {
               if (aggressor.Defender == killer)
                   return true;
           }
           
           return false;
       }

       [Usage("Status")]
       [Description("Wyswietla informacje o postaci.")]
       public static void Status_OnCommand(CommandEventArgs e)
       {
          PlayerMobile pm = (PlayerMobile)e.Mobile;

          pm.SendMessage("Slawa: {0}", e.Mobile.Fame);
          pm.SendMessage("Karma: {0}", e.Mobile.Karma);
          pm.SendMessage("Morderstwa: {0}", e.Mobile.Kills);
          
          // Update and display murder count information
          UpdateMurderInfo(pm);
          
          // Display short and long term murders with time remaining
          if (_murderTracker.ContainsKey(pm.Serial))
          {
              MurderInfo info = _murderTracker[pm.Serial];
              
              // Calculate and display short term murders
              TimeSpan shortTermRemaining = TimeSpan.Zero;
              if (info.ShortTermMurders > 0)
              {
                  DateTime nextDecay = GetNextDecayTime(info.ShortTermElapse);
                  shortTermRemaining = nextDecay > DateTime.UtcNow 
                      ? nextDecay - DateTime.UtcNow 
                      : TimeSpan.Zero;
                      
                  pm.SendMessage("Krotkoterminowe morderstwa: {0} (Pozostalo: {1:hh\\:mm\\:ss})", 
                      info.ShortTermMurders, 
                      shortTermRemaining);
              }
              else
              {
                  pm.SendMessage("Krotkoterminowe morderstwa: 0");
              }
              
              // Calculate and display long term murders
              TimeSpan longTermRemaining = TimeSpan.Zero;
              if (info.LongTermMurders > 0)
              {
                  DateTime nextDecay = GetNextDecayTime(info.LongTermElapse);
                  longTermRemaining = nextDecay > DateTime.UtcNow 
                      ? nextDecay - DateTime.UtcNow 
                      : TimeSpan.Zero;
                      
                  pm.SendMessage("Dlugoterminowe morderstwa: {0} (Pozostalo: {1:hh\\:mm\\:ss})", 
                      info.LongTermMurders, 
                      longTermRemaining);
              }
              else
              {
                  pm.SendMessage("Dlugoterminowe morderstwa: 0");
              }
          }
          else
          {
              pm.SendMessage("Krotkoterminowe morderstwa: 0");
              pm.SendMessage("Dlugoterminowe morderstwa: 0");
          }
       }
       
       // Helper method to get the next decay time from an array of times
       private static DateTime GetNextDecayTime(DateTime[] times)
       {
           DateTime closest = DateTime.MaxValue;
           
           foreach (DateTime time in times)
           {
               if (time > DateTime.UtcNow && time < closest)
                   closest = time;
           }
           
           return closest == DateTime.MaxValue ? DateTime.UtcNow : closest;
       }
       
       // Update murder info to account for decayed murders
       private static void UpdateMurderInfo(Mobile m)
       {
           if (!_murderTracker.ContainsKey(m.Serial))
               return;
               
           MurderInfo info = _murderTracker[m.Serial];
           DateTime now = DateTime.UtcNow;
           bool updated = false;
           
           // Check for decayed short term murders
           for (int i = 0; i < info.ShortTermElapse.Length; i++)
           {
               if (info.ShortTermElapse[i] != DateTime.MinValue && info.ShortTermElapse[i] <= now)
               {
                   info.ShortTermElapse[i] = DateTime.MinValue;
                   if (info.ShortTermMurders > 0)
                   {
                       info.ShortTermMurders--;
                       updated = true;
                   }
               }
           }
           
           // Check for decayed long term murders
           for (int i = 0; i < info.LongTermElapse.Length; i++)
           {
               if (info.LongTermElapse[i] != DateTime.MinValue && info.LongTermElapse[i] <= now)
               {
                   info.LongTermElapse[i] = DateTime.MinValue;
                   if (info.LongTermMurders > 0)
                   {
                       info.LongTermMurders--;
                       updated = true;
                   }
               }
           }
           
           // If any counts were updated, sort the times to optimize finding the next decay
           if (updated)
           {
               Array.Sort(info.ShortTermElapse);
               Array.Sort(info.LongTermElapse);
           }
       }
    }
}
