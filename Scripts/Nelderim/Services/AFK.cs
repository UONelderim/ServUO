// AFK Command v1.1.0
// Author: Felladrin

using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.Mobiles;

namespace Nelderim
{
    public static class AFK
    {
        public static void Initialize()
        {
            CommandSystem.Register("AFK", AccessLevel.Player, OnCommand);
            EventSink.Speech += OnSpeech;
        }

        private class AfkInfo
        {
            public string Message = "*drzemie*";
            public DateTime Time = DateTime.Now;
            public Point3D Location;

            public AfkInfo(string message, Point3D location)
            {
                Message = message;
                Location = location;
            }
        }

        static Dictionary<PlayerMobile, AfkInfo> PlayersAfk = new();

        [Usage("AFK")]
        [Description("Puts your char in 'Away From Keyboard' mode.")]
        static void OnCommand(CommandEventArgs e)
        {
	        if (e.Mobile is not PlayerMobile pm)
	            return;

            if (isAFK(pm))
            {
                SetBack(pm);
            }
            else
            {
                if (e.Length == 0)
                {
                    SetAFK(pm, "*drzemie*");
                }
                else
                {
                    SetAFK(pm, e.ArgString);
                }

                AnnounceAFK(pm);
            }
        }

        static void OnSpeech(SpeechEventArgs e)
        {
	        if (e.Mobile is PlayerMobile playerMobile && isAFK(playerMobile))
            {
                SetBack(playerMobile);
            }
        }

        static void AnnounceAFK(PlayerMobile pm)
        {
            if (!isAFK(pm))
                return;

            if (pm.Location != GetAFKLocation(pm) || pm.NetState == null || pm.Deleted)
            {
                SetBack(pm);
                return;
            }

            TimeSpan ts = GetAFKTimeSpan(pm);

            pm.Emote("*{0}*", GetAFKMessage(pm));

            if (ts.Seconds != 0)
            {
                pm.Emote("chrapie", ts.Hours);
            }

            Timer.DelayCall(TimeSpan.FromSeconds(10), () => AnnounceAFK(pm));
        }

        static void SetAFK(PlayerMobile pm, string message)
        {
            PlayersAfk.Add(pm, new AfkInfo(message, pm.Location));
            pm.Emote("*drzemie*");
        }

        static void SetBack(PlayerMobile pm)
        {
            PlayersAfk.Remove(pm);
            pm.Emote("*wybudza sie z drzemki*");
        }

        static bool isAFK(PlayerMobile pm)
        {
            return PlayersAfk.ContainsKey(pm);
        }

        static string GetAFKMessage(PlayerMobile pm)
        {
            if (PlayersAfk.ContainsKey(pm))
            {
                PlayersAfk.TryGetValue(pm, out var info);
                if (info != null)
                {
                    return info.Message;
                }
            }

            return "*drzemie*";
        }

        static Point3D GetAFKLocation(PlayerMobile pm)
        {
            if (PlayersAfk.ContainsKey(pm))
            {
                PlayersAfk.TryGetValue(pm, out var info);
                if (info != null)
                {
                    return info.Location;
                }
            }

            return new Point3D();
        }

        static TimeSpan GetAFKTimeSpan(PlayerMobile pm)
        {
	        if (!PlayersAfk.ContainsKey(pm)) return TimeSpan.Zero;
	        
	        PlayersAfk.TryGetValue(pm, out var info);
	        if (info == null) return TimeSpan.Zero;
	        
            try
            {
	            return DateTime.Now - info.Time;
            }
            catch
            {
	            return TimeSpan.Zero;
            }
        }
    }
}
