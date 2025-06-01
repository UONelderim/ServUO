using System;
using System.IO;
using Server;
using Server.Mobiles;

namespace Server.Logging
{
    public static class VoodooAuditLog
    {
        private static readonly string LogPath = Path.Combine(Core.BaseDirectory, "Logs", "VoodooAudit.log");

        public static void Log(Mobile from, object target, string action)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(LogPath));
                using (var writer = new StreamWriter(LogPath, true))
                {
                    writer.WriteLine($"{DateTime.Now:u} | {from} -> {target} : {action}");
                }
            }
            catch (Exception)
            {
                // Ignoruj błędy logowania
            }
        }
    }
}
