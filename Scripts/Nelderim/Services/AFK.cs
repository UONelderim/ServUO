using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Felladrin.Commands
{
    public static class AutoAFK
    {
        public static class Config
        {
            public static bool Enabled = true;           // Czy system jest włączony?
            public static int IdleMinutes = 30;           // Ilość minut bezczynności przed automatycznym AFK
            public static string DefaultMessage = "*drzemie*"; // Domyślna wiadomość AFK
            public static bool AnnouncePeriodically = true; // Okresowe ogłaszanie statusu AFK
            public static int AnnounceIntervalSeconds = 60;  // Jak często ogłaszać status AFK
        }

        public static void Initialize()
        {
            if (Config.Enabled)
            {
                // Rejestracja obsługi zdarzeń do śledzenia aktywności gracza
                EventSink.Speech += OnSpeech;
                EventSink.Movement += OnMovement;
                EventSink.Login += OnLogin;
                EventSink.Logout += OnLogout;
                
                // Uruchomienie timera sprawdzającego bezczynność
                Timer.DelayCall(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1), CheckIdlePlayers);
            }
        }

        class AfkInfo
        {
            public string Message = Config.DefaultMessage;
            public DateTime Time = DateTime.Now;
            public DateTime LastActivity = DateTime.Now;
            public Point3D Location;

            public AfkInfo(string message, Point3D location)
            {
                Message = message;
                Location = location;
            }
        }

        static Dictionary<int, AfkInfo> PlayersAfk = new Dictionary<int, AfkInfo>();
        static Dictionary<int, DateTime> PlayerActivity = new Dictionary<int, DateTime>();

        static void OnSpeech(SpeechEventArgs e)
        {
            var playerMobile = e.Mobile as PlayerMobile;
            if (playerMobile != null)
            {
                // Aktualizacja czasu ostatniej aktywności
                UpdateActivity(playerMobile);
                
                // Jeśli mówiąc podczas AFK, powrót ze stanu AFK
                if (isAFK(playerMobile))
                {
                    SetBack(playerMobile);
                }
            }
        }

        static void OnMovement(MovementEventArgs e)
        {
            var playerMobile = e.Mobile as PlayerMobile;
            if (playerMobile != null)
            {
                // Aktualizacja czasu ostatniej aktywności
                UpdateActivity(playerMobile);
                
                // Jeśli rusza się podczas AFK, powrót ze stanu AFK
                // W ServUO kierunek 0 oznacza brak ruchu
                if (isAFK(playerMobile) && e.Direction != Direction.North + 0)
                {
                    SetBack(playerMobile);
                }
            }
        }

        static void OnLogin(LoginEventArgs e)
        {
            var playerMobile = e.Mobile as PlayerMobile;
            if (playerMobile != null)
            {
                // Inicjalizacja śledzenia aktywności dla gracza
                UpdateActivity(playerMobile);
            }
        }

        static void OnLogout(LogoutEventArgs e)
        {
            var playerMobile = e.Mobile as PlayerMobile;
            if (playerMobile != null)
            {
                // Wyczyść status AFK i śledzenie aktywności, gdy gracz wyloguje się
                if (isAFK(playerMobile))
                {
                    PlayersAfk.Remove(playerMobile.Serial.Value);
                }
                
                if (PlayerActivity.ContainsKey(playerMobile.Serial.Value))
                {
                    PlayerActivity.Remove(playerMobile.Serial.Value);
                }
            }
        }

        static void UpdateActivity(Mobile m)
        {
            if (m == null)
                return;

            int serial = m.Serial.Value;
            DateTime now = DateTime.Now;
            
            // Aktualizacja czasu ostatniej aktywności gracza
            if (PlayerActivity.ContainsKey(serial))
            {
                PlayerActivity[serial] = now;
            }
            else
            {
                PlayerActivity.Add(serial, now);
            }
            
            // Również aktualizacja czasu ostatniej aktywności w AfkInfo, jeśli gracz jest AFK
            if (PlayersAfk.ContainsKey(serial))
            {
                AfkInfo info = PlayersAfk[serial];
                info.LastActivity = now;
            }
        }

        static void CheckIdlePlayers()
        {
            // Pobierz aktualny czas do porównania
            DateTime now = DateTime.Now;
            
            // Sprawdź wszystkich graczy online pod kątem bezczynności
            foreach (NetState ns in NetState.Instances)
            {
                Mobile m = ns.Mobile;
                
                if (m == null || !(m is PlayerMobile))
                    continue;
                
                PlayerMobile pm = (PlayerMobile)m;
                int serial = pm.Serial.Value;
                
                // Pomiń graczy, którzy już są w trybie AFK
                if (isAFK(pm))
                    continue;
                
                // Sprawdź, czy gracz ma zarejestrowany czas aktywności
                if (!PlayerActivity.ContainsKey(serial))
                {
                    // Inicjalizacja czasu aktywności dla gracza, jeśli brak
                    PlayerActivity.Add(serial, now);
                    continue;
                }
                
                // Obliczanie czasu bezczynności
                TimeSpan idleTime = now - PlayerActivity[serial];
                
                // Jeśli gracz jest bezczynny dłużej niż próg, ustaw automatycznie AFK
                if (idleTime.TotalMinutes >= Config.IdleMinutes)
                {
                    SetAFK(pm, "*glosno chrapie*" + Config.DefaultMessage);
                    AnnounceAFK(pm);
                }
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

            if (ts.Hours != 0)
            {
                string hourForm = GetPolishForm(ts.Hours, "dni", "dni", "dni");
                string minuteForm = GetPolishForm(ts.Minutes, "klepsydry", "klepsydra", "klepsydr");
                pm.Emote("*drzemie od {0} {1} i {2} {3}*", ts.Hours, hourForm, ts.Minutes, minuteForm);
            }
            else if (ts.Minutes != 0)
            {
                string minuteForm = GetPolishForm(ts.Minutes, "klepsydr", "klepsydra", "klepsydr");
                pm.Emote("*drzemie od {0} {1}*", ts.Minutes, minuteForm);
            }
            else if (ts.Seconds != 0)
            {
                string secondForm = GetPolishForm(ts.Seconds, "ziaren", "ziarna", "ziaren");
                pm.Emote("*drzemie od {0} {1}*", ts.Seconds, secondForm);
            }

            if (Config.AnnouncePeriodically)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(Config.AnnounceIntervalSeconds), delegate { AnnounceAFK(pm); });
            }
        }

        // Funkcja pomocnicza do prawidłowego odmienienia polskich form gramatycznych liczebników
        static string GetPolishForm(int number, string few, string one, string many)
        {
            if (number == 1)
                return one;
                
            if (number % 10 >= 2 && number % 10 <= 4 && (number % 100 < 10 || number % 100 >= 20))
                return few;
                
            return many;
        }

        static void SetAFK(Mobile m, string message)
        {
            int serial = m.Serial.Value;
            
            if (PlayersAfk.ContainsKey(serial))
                PlayersAfk.Remove(serial);
                
            PlayersAfk.Add(serial, new AfkInfo(message, m.Location));
            
            // Aktualizacja znacznika czasu aktywności
            if (!PlayerActivity.ContainsKey(serial))
                PlayerActivity.Add(serial, DateTime.Now);
                
            m.Emote("*smacznie pochrapuje*");
        }

        static void SetBack(Mobile m)
        {
            int serial = m.Serial.Value;
            
            if (PlayersAfk.ContainsKey(serial))
            {
                PlayersAfk.Remove(serial);
            }
            
            // Aktualizacja znacznika czasu aktywności
            if (PlayerActivity.ContainsKey(serial))
                PlayerActivity[serial] = DateTime.Now;
            else
                PlayerActivity.Add(serial, DateTime.Now);
                
            m.Emote("*przebudza sie*");
        }

        static bool isAFK(IEntity e)
        {
            return e != null && PlayersAfk.ContainsKey(e.Serial.Value);
        }

        static string GetAFKMessage(IEntity e)
        {
            if (e != null && PlayersAfk.ContainsKey(e.Serial.Value))
            {
                AfkInfo info;
                PlayersAfk.TryGetValue(e.Serial.Value, out info);
                if (info != null)
                {
                    return info.Message;
                }
            }

            return Config.DefaultMessage;
        }

        static Point3D GetAFKLocation(IEntity e)
        {
            if (e != null && PlayersAfk.ContainsKey(e.Serial.Value))
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

        static TimeSpan GetAFKTimeSpan(IEntity e)
        {
            if (e != null && PlayersAfk.ContainsKey(e.Serial.Value))
            {
                AfkInfo info;
                PlayersAfk.TryGetValue(e.Serial.Value, out info);
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
