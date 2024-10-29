using Nelderim.Races;
using Server.Items;

namespace Server.Nelderim.Misc
{
	public class RaceRoomMoongate : ConfirmationMoongate
	{
	
		[Constructable]
		public RaceRoomMoongate()
		{
			GumpWidth = 300;
			GumpHeight = 200;
			TitleColor = 0xFFFFFF;
			MessageColor = 0xFFFFFF;
			TitleNumber = 1124185;
			MessageNumber = 1124186;
		}
		
		public override void OnGateUsed(Mobile m)
		{
			var target = m.Race switch
			{
				NElf => new Point3D(1874, 505, 2), //Lotharn
				NJarling => new Point3D(1013, 526, 39), //Garlan
				NDrow => new Point3D(5374, 1932, 25), //L'Delmah
				NKrasnolud => new Point3D(5522, 1162, 1), //Twierdza
				NTamael => m.Faction switch
				{
					West => new Point3D(695, 2055, 5), //Orod
					East => new Point3D(2067, 2697, 0), //Tirassa
					_ => Point3D.Zero
				},
				_ => Point3D.Zero
			};
			
			if(Point3D.Zero == target)
			{
				m.SendLocalizedMessage(1124187); // Musisz wybrać rasę, aby wejść do świata Nelderim.
				return;
			}

			m.MoveToWorld(target, Map.Felucca);
			m.PlaySound(0x66C);
		}

		public RaceRoomMoongate(Serial serial) : base(serial)
		{
		}
	}
}
