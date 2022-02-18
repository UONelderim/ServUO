#region References

using System;

#endregion

namespace Server.ACC.CSS.Modules
{
	public class CastInfo
	{
		public Type SpellType { get; set; }

		public School School { get; set; }

		public CastInfo(Type type, School school)
		{
			SpellType = type;
			School = school;
		}

		public CastInfo(GenericReader reader)
		{
			Deserialize(reader);
		}

		public void Serialize(GenericWriter writer)
		{
			writer.Write(0); //version

			writer.Write(SpellType.Name);
			writer.Write((int)School);
		}

		public void Deserialize(GenericReader reader)
		{
			int version = reader.ReadInt();
			switch (version)
			{
				case 0:
				{
					SpellType = ScriptCompiler.FindTypeByName(reader.ReadString());
					School = (School)reader.ReadInt();

					break;
				}
			}
		}
	}
}
