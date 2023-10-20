// AFK Command v1.1.0
// Author: Felladrin
// Started: 2013-08-14
// Updated: 2016-01-03

using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.Mobiles;

namespace Felladrin.Commands
{
    public static class AFK
    {
        public static class Config
        {
            public static bool Enabled = true;    // Is this command enabled?
        }

        public static void Initialize()
        {
            if (Config.Enabled)
            {
                CommandSystem.Register("AFK", AccessLevel.Player, new CommandEventHandler(OnCommand));
                EventSink.Speech += OnSpeech;
            }
        }

        class AfkInfo
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

        static Dictionary<int, AfkInfo> PlayersAfk = new Dictionary<int, AfkInfo>();

        [Usage("AFK")]
        [Description("Puts your char in 'Away From Keyboard' mode.")]
        static void OnCommand(CommandEventArgs e)
        {
            PlayerMobile pm = e.Mobile as PlayerMobile;

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
            var playerMobile = e.Mobile as PlayerMobile;
            if (playerMobile != null && isAFK(playerMobile))
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

            Timer.DelayCall(TimeSpan.FromSeconds(10), delegate { AnnounceAFK(pm);  });
        }

        static void SetAFK(Mobile m, string message)
        {
            PlayersAfk.Add(m.Serial.Value, new AfkInfo(message, m.Location));
            m.Emote("*drzemie*");
        }

        static void SetBack(Mobile m)
        {
            PlayersAfk.Remove(m.Serial.Value);
            m.Emote("*wybudza sie z drzemski*");
        }

        static bool isAFK(IEntity e)
        {
            return PlayersAfk.ContainsKey(e.Serial.Value);
        }

        static string GetAFKMessage(IEntity e)
        {
            if (PlayersAfk.ContainsKey(e.Serial.Value))
            {
                AfkInfo info;
                PlayersAfk.TryGetValue(e.Serial.Value, out info);
                if (info != null)
                {
                    return info.Message;
                }
            }

            return "*drzemie*";
        }

        static Point3D GetAFKLocation(IEntity e)
        {
            if (PlayersAfk.ContainsKey(e.Serial.Value))
            {
                AfkInfo info;
                PlayersAfk.TryGetValue(e.Serial.Value, out info);
                if (info != null)
                {
                    return info.Location;
                }
            }

            return new Point3D();
        }

        static TimeSpan GetAFKTimeSpan(IEntity pm)
        {
            if (PlayersAfk.ContainsKey(pm.Serial.Value))
            {
                AfkInfo info;
                PlayersAfk.TryGetValue(pm.Serial.Value, out info);
                if (info != null)
                {
                    TimeSpan time;

                    try
                    {
                        time = DateTime.Now - info.Time;
                    }
                    catch
                    {
                        time = TimeSpan.Zero;
                    }

                    return time;
                }
            }

            return TimeSpan.Zero;
        }
    }
}
