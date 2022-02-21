/* Copyright(c) 2016 UltimaLive
 * 
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the
 * "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so, subject to
 * the following conditions:
 *
 * The above copyright notice and this permission notice shall be included
 * in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
 * CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
 * TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/

#region References

using System;
using System.Collections.Generic;
using System.IO;
using Server;
using Server.Commands;
using Server.Misc;

#endregion

namespace UltimaLive
{
	public class ClientFileExport
	{
		public static void Initialize()
		{
			CommandSystem.Prefix = "[";
			Register("ExportClientFiles", AccessLevel.GameMaster, exportClientFiles_OnCommand);
			Register("PrintLandData", AccessLevel.GameMaster, printLandData_OnCommand);
			Register("PrintStaticsData", AccessLevel.GameMaster, printStaticsData_OnCommand);
			Register("PrintCrc", AccessLevel.GameMaster, printCrc_OnCommand);
		}


		[Usage("PrintCrc")]
		[Description("Increases the Z value of a static.")]
		private static void printCrc_OnCommand(CommandEventArgs e)
		{
			Mobile from = e.Mobile;
			Map playerMap = from.Map;
			TileMatrix tm = playerMap.Tiles;

			int blocknum = (((from.Location.X >> 3) * tm.BlockHeight) + (from.Location.Y >> 3));
			byte[] LandData = BlockUtility.GetLandData(blocknum, playerMap.MapID);
			byte[] StaticsData = BlockUtility.GetRawStaticsData(blocknum, playerMap.MapID);

			byte[] blockData = new byte[LandData.Length + StaticsData.Length];
			Array.Copy(LandData, blockData, LandData.Length);
			Array.Copy(StaticsData, 0, blockData, LandData.Length, StaticsData.Length);


			ushort crc = CRC.Fletcher16(blockData);
			Console.WriteLine("CRC is 0x" + crc.ToString("X4"));
		}

		[Usage("PrintLandData")]
		[Description("Increases the Z value of a static.")]
		private static void printLandData_OnCommand(CommandEventArgs e)
		{
			Mobile from = e.Mobile;
			Map playerMap = from.Map;
			TileMatrix tm = playerMap.Tiles;

			int blocknum = (((from.Location.X >> 3) * tm.BlockHeight) + (from.Location.Y >> 3));
			byte[] data = BlockUtility.GetLandData(blocknum, playerMap.MapID);
			BlockUtility.WriteLandDataToConsole(data);
		}

		[Usage("PrintStaticsData")]
		[Description("Increases the Z value of a static.")]
		private static void printStaticsData_OnCommand(CommandEventArgs e)
		{
			Mobile from = e.Mobile;
			Map playerMap = from.Map;
			TileMatrix tm = playerMap.Tiles;

			int blocknum = (((from.Location.X >> 3) * tm.BlockHeight) + (from.Location.Y >> 3));
			byte[] data = BlockUtility.GetRawStaticsData(blocknum, playerMap.MapID);
			BlockUtility.WriteDataToConsole(data);
		}


		[Usage("ExportClientFiles")]
		[Description("Increases the Z value of a static.")]
		private static void exportClientFiles_OnCommand(CommandEventArgs e)
		{
			ExportOnNextSave = true;
			AutoSave.Save();
		}


		public static void Register(string command, AccessLevel access, CommandEventHandler handler)
		{
			CommandSystem.Register(command, access, handler);
		}

		public static void Configure()
		{
			EventSink.WorldSave += OnSave;
		}

		public static bool ExportOnNextSave;

		//path used for hashes
		public static Map WorkMap { get; private set; }

		public static void OnSave(WorldSaveEventArgs e)
		{
			if (!ExportOnNextSave)
			{
				return;
			}

			ExportOnNextSave = false;

			if (!Directory.Exists(UltimaLiveSettings.UltimaLiveClientExportPath))
				Directory.CreateDirectory(UltimaLiveSettings.UltimaLiveClientExportPath);

			Console.Write("Exporting Client Files...");

			/* maps */
			foreach (KeyValuePair<int, MapRegistry.MapDefinition> kvp in MapRegistry.Definitions)
			{
				if (!MapRegistry.MapAssociations.ContainsKey(kvp.Key))
				{
					continue;
				}

				string filename = String.Format("map{0}.mul", kvp.Key);
				GenericWriter writer =
					new BinaryFileWriter(Path.Combine(UltimaLiveSettings.UltimaLiveClientExportPath, filename), true);
				WorkMap = Map.Maps[kvp.Key];
				TileMatrix CurrentMatrix = WorkMap.Tiles;
				int blocks = CurrentMatrix.BlockWidth * CurrentMatrix.BlockHeight;
				for (int xblock = 0; xblock < CurrentMatrix.BlockWidth; xblock++)
				for (int yblock = 0; yblock < CurrentMatrix.BlockHeight; yblock++)
				{
					writer.Write((uint)0);
					LandTile[] blocktiles = CurrentMatrix.GetLandBlock(xblock, yblock);
					if (blocktiles.Length == 196)
					{
						Console.WriteLine("Invalid landblock! Save failed!");
						return;
					}

					for (int j = 0; j < 64; j++)
					{
						writer.Write((short)blocktiles[j].ID);
						writer.Write((sbyte)blocktiles[j].Z);
					}
				}

				writer.Close();
			}

			/* Statics */
			foreach (KeyValuePair<int, MapRegistry.MapDefinition> kvp in MapRegistry.Definitions)
			{
				if (!MapRegistry.MapAssociations.ContainsKey(kvp.Key))
				{
					continue;
				}

				string filename = String.Format("statics{0}.mul", kvp.Key);
				GenericWriter staticWriter =
					new BinaryFileWriter(Path.Combine(UltimaLiveSettings.UltimaLiveClientExportPath, filename), true);
				filename = String.Format("staidx{0}.mul", kvp.Key);
				GenericWriter staticIndexWriter =
					new BinaryFileWriter(Path.Combine(UltimaLiveSettings.UltimaLiveClientExportPath, filename), true);

				WorkMap = Map.Maps[kvp.Key];
				TileMatrix CurrentMatrix = WorkMap.Tiles;

				int blocks = CurrentMatrix.BlockWidth * CurrentMatrix.BlockHeight;

				int startBlock = 0;
				int finishBlock = 0;

				for (int xblock = 0; xblock < CurrentMatrix.BlockWidth; xblock++)
				{
					for (int yblock = 0; yblock < CurrentMatrix.BlockHeight; yblock++)
					{
						StaticTile[][][] staticTiles = CurrentMatrix.GetStaticBlock(xblock, yblock);

						//Static File
						for (int i = 0; i < staticTiles.Length; i++)
						for (int j = 0; j < staticTiles[i].Length; j++)
						{
							StaticTile[] sortedTiles = staticTiles[i][j];
							Array.Sort(sortedTiles, BlockUtility.CompareStaticTiles);

							for (int k = 0; k < sortedTiles.Length; k++)
							{
								staticWriter.Write((ushort)sortedTiles[k].ID);
								staticWriter.Write((byte)i);
								staticWriter.Write((byte)j);
								staticWriter.Write((sbyte)sortedTiles[k].Z);
								staticWriter.Write((short)sortedTiles[k].Hue);
								finishBlock += 7;
							}
						}

						//Index File
						if (finishBlock != startBlock)
						{
							staticIndexWriter.Write(startBlock); //lookup
							staticIndexWriter.Write(finishBlock - startBlock); //length
							staticIndexWriter.Write(0); //extra
							startBlock = finishBlock;
						}
						else
						{
							staticIndexWriter.Write(UInt32.MaxValue); //lookup
							staticIndexWriter.Write(UInt32.MaxValue); //length
							staticIndexWriter.Write(UInt32.MaxValue); //extra
						}
					}
				}

				staticWriter.Close();
				staticIndexWriter.Close();
			}
		}
	}
}
