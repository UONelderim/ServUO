using System.IO;

namespace Server
{
	public static class FileOperations
	{
		public const int KB = 1024;
		public const int MB = 1024 * KB;

		public static int BufferSize { get; set; } = 1 * MB;
		public static int Concurrency { get; set; } = 1;

		public static bool Unbuffered { get; set; } = true;

		public static bool AreSynchronous => Concurrency < 1;
		public static bool AreAsynchronous => Concurrency > 0;

		public static FileStream OpenSequentialStream(string path, FileMode mode, FileAccess access, FileShare share)
		{
			var options = FileOptions.SequentialScan;

			if (Concurrency > 0)
			{
				options |= FileOptions.Asynchronous;
			}

			return new FileStream(path, mode, access, share, BufferSize, options);
		}
	}
}
