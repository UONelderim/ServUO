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
		
		private static string LogPath = Path.Combine( "Logs/Bank", String.Format( "Bank {0}.log", GetTimeStamp() ) );

		static BankLog()
		{
			if( !Directory.Exists( "Logs" ) )
				Directory.CreateDirectory( "Logs" );
				
			if( !Directory.Exists( "Logs/Bank" ) )
				Directory.CreateDirectory( "Logs/Bank" );
		}
        
		public static void Log(Mobile from, int amount, string desc)
		{
			using var writer = new StreamWriter(LogPath, true);
			writer.WriteLine("{0} {1}({2}) {3} {4}", DateTime.Now, from.Serial, from.Name, amount, desc);
		}
	}
}
