using System;
using System.Collections.Generic;
using System.Linq;
using Server;
using Server.Commands;
using Server.Mobiles;

namespace Nelderim
{
    public static class AutomatedAFK
    {
        // Inactivity threshold (10 minutes)
        private static readonly TimeSpan InactivityThreshold = TimeSpan.FromMinutes(10);

        // Tracks last activity for each player
        private static Dictionary<PlayerMobile, DateTime> PlayerLastActivity = new();

        public static void Initialize()
        {
            // Hook into ServUO event sinks to track player activity
            EventSink.Speech += OnPlayerActivity;
            EventSink.Movement += OnPlayerMovement;
    

            // Start the inactivity check timer
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

        private static void OnSkillUse(Mobile mobile, int skill)
        {
            if (mobile is PlayerMobile pm)
            {
                UpdatePlayerActivity(pm);
            }
        }

        private static void UpdatePlayerActivity(PlayerMobile pm)
        {
            PlayerLastActivity[pm] = DateTime.Now;
        }

        private static void CheckInactivity()
        {
            foreach (var entry in PlayerLastActivity.ToList())
            {
                PlayerMobile pm = entry.Key;
                DateTime lastActivity = entry.Value;

                // Check if player has been inactive
                if (DateTime.Now - lastActivity >= InactivityThreshold)
                {
                    // Auto-set AFK
                    pm.Emote("*drzemie*");
                }
            }
        }
    }
}