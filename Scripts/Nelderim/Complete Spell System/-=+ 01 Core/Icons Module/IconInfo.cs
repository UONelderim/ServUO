#region References

using System;

#endregion

namespace Server.ACC.CSS.Modules
{
	public class IconInfo
	{
		public Type SpellType { get; set; }

		public int Icon { get; set; }

		public Point3D Location { get; set; }

		public School School { get; set; }

		public IconInfo(Type type, int icon, int x, int y, int back, School school)
		{
			SpellType = type;
			Icon = icon;
			Location = new Point3D(x, y, back);
			School = school;
		}

		public IconInfo(GenericReader reader)
		{
			Deserialize(reader);
		}

		public void Serialize(GenericWriter writer)
		{
			writer.Write(1); //version

			writer.Write(SpellType.Name);
			writer.Write(Icon);
			writer.Write(Location);
			writer.Write((int)School);
		}

		public void Deserialize(GenericReader reader)
		{
			int version = reader.ReadInt();
			switch (version)
			{
				case 1:
				{
					SpellType = ScriptCompiler.FindTypeByName(reader.ReadString());
					Icon = reader.ReadInt();
					Location = reader.ReadPoint3D();
					School = (School)reader.ReadInt();

					break;
				}
				case 0:
				{
					int bad = reader.ReadInt();
					Icon = reader.ReadInt();
					Location = reader.ReadPoint3D();

					SpellType = null;
					School = School.Invalid;
					break;
				}
			}
		}
	}
}
