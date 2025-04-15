using System;
using System.Reflection;

namespace Server.Commands
{
	public class GitInfo
	{
		private static string _commitId = "unknown";
		public static string CommitId => _commitId;
		
		public static void Configure()
		{
			try
			{
				var x = Assembly
					.GetExecutingAssembly()
					.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
					.InformationalVersion;
				_commitId = x.Split("+")[1];
				Console.WriteLine($"Commit ID: {_commitId}");
			}
			catch(Exception ex)
			{
				Console.WriteLine($"Error fetching commit id: {ex.Message}");
			}
		}
	}
}
