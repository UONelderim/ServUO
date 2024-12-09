using System;
using System.Collections.Generic;
using System.Linq;
using Server;
using Server.Mobiles;

namespace Nelderim
{
    public static class AutomatedAFK
    {
        private static readonly TimeSpan InactivityThreshold = TimeSpan.FromMinutes(10);
        
        private static readonly Dictionary<PlayerMobile, DateTime> PlayerLastActivity = new();
        private static readonly HashSet<PlayerMobile> AfkPlayers = new();

        public static void Initialize()
        {
            EventSink.Speech += OnPlayerActivity;
            EventSink.Movement += OnPlayerMovement;
            
            foreach (Mobile m in World.Mobiles.Values)
            {
                if (m is PlayerMobile pm)
                {
                    UpdatePlayerActivity(pm);
                }
            }

            Timer.DelayCall(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1), CheckInactivity);
        }

        private static void OnPlayerActivity(SpeechEventArgs e)
        {
            if (e.Mobile is PlayerMobile pm)
            {
                UpdatePlayerActivity(pm);
            }
        }

        private static void OnPlayerMovement(MovementEventArgs e)
        {
            if (e.Mobile is PlayerMobile pm)
            {
                UpdatePlayerActivity(pm);
            }
        }

        private static void UpdatePlayerActivity(PlayerMobile pm)
        {
            if (pm?.Deleted != true)
            {
                PlayerLastActivity[pm] = DateTime.Now;
                if (AfkPlayers.Contains(pm))
                {
                    AfkPlayers.Remove(pm);
                }
            }
        }

        private static void CheckInactivity()
        {
            var now = DateTime.Now;
            foreach (var entry in PlayerLastActivity.ToList())
            {
                PlayerMobile pm = entry.Key;
                DateTime lastActivity = entry.Value;
                
                if (pm?.Deleted != true && pm.NetState != null && !pm.Hidden)
                {
                    if (now - lastActivity >= InactivityThreshold)
                    {
                        if (!AfkPlayers.Contains(pm))  
                        {
                            pm.Emote("*drzemie*");
                            AfkPlayers.Add(pm);
                        }
                    }
                }
                else
                {
                    PlayerLastActivity.Remove(pm);
                    AfkPlayers.Remove(pm);
                }
            }
        }
    }
}
