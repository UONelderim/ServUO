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
			var x = Assembly
				.GetExecutingAssembly()
				.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
				.InformationalVersion;
			_commitId = x.Split("+")[1];
			Console.WriteLine($"Commit ID: {_commitId}");
		}
	}
}
