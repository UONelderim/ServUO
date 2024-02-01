using System;
using System.IO;
using Server;

namespace Nelderim
{
	public class BankLog
	{
		private static string GetTimeStamp()
		{
			DateTime now = DateTime.Now;
			return $"{now.Year}-{now.Month}-{now.Day}";
		}
		
		private static string LogPath = Path.Combine( "Logs/Bank", $"Bank_{GetTimeStamp()}.log");

		static BankLog()
		{
			if( !Directory.Exists( "Logs" ) )
				Directory.CreateDirectory( "Logs" );
				
			string directory = "Logs/Bank";
				
			if( !Directory.Exists( directory ) )
				Directory.CreateDirectory( directory );
		}
        
		public static void Log(Mobile from, long amount, string desc)
		{
			using (StreamWriter writer = new StreamWriter(LogPath, true))
			{
				writer.WriteLine($"{DateTime.Now} {from.Serial}({from.Name}) {amount} {desc}");
			}
		}
	}
}
