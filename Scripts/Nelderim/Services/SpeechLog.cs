using System;
using System.IO;
using Server;
using Server.Diagnostics;

namespace Nelderim
{
	public class SpeechLog
	{
		private static StreamWriter writer;

		private static string LogPath = Path.Combine( "Logs/Speech", $"Speech_{GetTimeStamp()}.log");

		public static void Initialize()
		{
			if( !Directory.Exists( "Logs" ) )
				Directory.CreateDirectory( "Logs" );
				
			var directory = "Logs/Speech";
				
			if( !Directory.Exists( directory ) )
				Directory.CreateDirectory( directory );
			
			writer = new StreamWriter( LogPath, true ) { AutoFlush = true };

			EventSink.Speech += OnSpeech;
			Console.WriteLine( "SpeechLog: Initialized." );
		}

		private static void OnSpeech(SpeechEventArgs e)
		{
			var m = e.Mobile;
			if(m.NetState == null || m.Deleted)
				return;
			try
			{
				var accountName = m?.Account.Username ?? "no account";
				writer.WriteLine($"{DateTime.Now} {m} ({accountName})[{m.LanguageSpeaking}]: {e.Speech}");
			}
			catch (Exception ex)
			{
				ExceptionLogging.LogException(ex);
			}
			
		}

		private static string GetTimeStamp()
		{
			var now = DateTime.Now;
			return $"{now.Year}-{now.Month}-{now.Day}";
		}
	}
}
