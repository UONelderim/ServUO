using Server.Multis;
using Server.Targeting;

namespace Server.Items
{
	public class HouseMailBoxDeed : Item
	{
		[Constructable]
		public HouseMailBoxDeed() : base(0x14F0)
		{
			Name = "Przekaz skrzynki pocztowej";
			LootType = LootType.Blessed;
		}

		public HouseMailBoxDeed(Serial serial) : base(serial) { }

		public override void OnDoubleClick(Mobile from)
		{
			if (!from.InRange(GetWorldLocation(), 2))
			{
				from.SendMessage("Musisz być bliżej, aby użyć tego przedmiotu.");
				return;
			}

			from.SendMessage("Wskaż miejsce do postawienia skrzynki domowej.");
			from.Target = new PlaceMailBoxTarget(this);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}

		private class PlaceMailBoxTarget : Target
		{
			private readonly HouseMailBoxDeed _deed;

			public PlaceMailBoxTarget(HouseMailBoxDeed deed) : base(8, true, TargetFlags.None)
			{
				_deed = deed;
			}

			protected override void OnTarget(Mobile from, object target)
			{
				var loc = target as IPoint3D;
				if (loc == null)
				{
					from.SendMessage("Nieprawidłowa lokalizacja.");
					return;
				}

				var dummy = new Item(0x1);
				dummy.MoveToWorld(new Point3D(loc), from.Map);
				var house = BaseHouse.FindHouseAt(dummy);
				dummy.Delete();

				if (house == null || !house.IsOwner(from))
				{
					from.SendMessage("Możesz postawić skrzynkę tylko we własnym domu.");
					return;
				}

				var box = new HouseMailBox();
				box.MoveToWorld(new Point3D(loc), from.Map);
				from.SendMessage("Postawiłeś skrzynkę pocztową w swoim domu.");
				_deed.Delete();
			}
		}
	}
}
