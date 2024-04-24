#region References
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
#endregion

namespace Server
{
	public static class HueData
	{
		private static readonly int[] m_Header = new int[0];

		public static Hue[] List { get; } = new Hue[3000];

		public static bool CheckFile => Core.FindDataFile("hues.mul") != null;

		static HueData()
		{
			var index = 0;

			if (CheckFile)
			{
				var path = Core.FindDataFile("hues.mul");

				if (path != null)
				{
					using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
					{
						var blockCount = (int)fs.Length / 708;

						if (blockCount > 375)
							blockCount = 375;

						m_Header = new int[blockCount];

						var structsize = Marshal.SizeOf(typeof(HueEntry));
						var buffer = new byte[blockCount * (4 + 8 * structsize)];

						var gc = GCHandle.Alloc(buffer, GCHandleType.Pinned);

						try
						{
							fs.Read(buffer, 0, buffer.Length);

							long currpos = 0;

							for (var i = 0; i < blockCount; ++i)
							{
								var ptrheader = new IntPtr((long)gc.AddrOfPinnedObject() + currpos);

								currpos += 4;

								m_Header[i] = (int)Marshal.PtrToStructure(ptrheader, typeof(int));

								for (var j = 0; j < 8; ++j, ++index)
								{
									var ptr = new IntPtr((long)gc.AddrOfPinnedObject() + currpos);

									currpos += structsize;

									var cur = (HueEntry)Marshal.PtrToStructure(ptr, typeof(HueEntry));

									List[index] = new Hue(index, cur);
								}
							}
						}
						finally
						{
							gc.Free();
						}
					}
				}
			}

			while (index < List.Length)
			{
				List[index] = new Hue(index);

				++index;
			}
		}

		public static Hue GetHue(int index)
		{
			index &= 0x3FFF;

			if (index >= 0 && index < List.Length)
				return List[index];

			return List[0];
		}

		private static int Convert32(ushort value)
		{
			int argb;

			argb = ((value & 0x7C00) << 9) | ((value & 0x03E0) << 6) | ((value & 0x1F) << 3);
			argb = ((value & 0x8000) * 0x1FE00) | argb | ((argb >> 5) & 0x070707);

			return argb;
		}

		private static ushort Convert16(int value)
		{
			return (ushort)(((value >> 16) & 0x8000) | ((value >> 9) & 0x7C00) | ((value >> 6) & 0x03E0) | ((value >> 3) & 0x1F));
		}

		public sealed class Hue
		{
			public int Index { get; }

			public Color[] Colors { get; } = new Color[32];

			public string Name { get; }

			public Color TableStart { get; }
			public Color TableEnd { get; }

			internal Hue(int index)
			{
				Name = "Null";
				Index = index;
			}

			internal Hue(int index, HueEntry entry)
			{
				Index = index;

				for (var i = 0; i < 32; ++i)
					Colors[i] = Color.FromArgb(Convert32((ushort)(entry.Colors[i] | 0x8000)));

				TableStart = Color.FromArgb(Convert32((ushort)(entry.TableStart | 0x8000)));
				TableEnd = Color.FromArgb(Convert32((ushort)(entry.TableEnd | 0x8000)));

				var count = 0;

				while (count < 20 && count < entry.Name.Length && entry.Name[count] != 0)
					++count;

				Name = Encoding.Default.GetString(entry.Name, 0, count);
				Name = Name.Replace("\n", " ");
			}
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct HueEntry
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
			public ushort[] Colors;

			public ushort TableStart;
			public ushort TableEnd;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
			public byte[] Name;
		}
	}
}
