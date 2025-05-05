using Nelderim.Towns;

namespace Server.Items
{
	[Flipable]
	public abstract class MiastowaSzata : BaseOuterTorso
	{
		public abstract Towns AssignedTown { get; }
		public MiastowaSzata() : this(0)
		{
		}
		
		public MiastowaSzata(int hue) : this(0x1F03, hue)
		{
		}
		
		public MiastowaSzata(int itemId, int hue) : base(itemId, hue)
		{
			Weight = 3.0;
			LootType = LootType.Blessed;
		}
		
		public override int LabelNumber => 1063965;

		public override bool Dye(Mobile from, DyeTub sender)
		{
			from.SendLocalizedMessage(sender.FailMessage);
			return false;
		}
		
		public override bool CanEquip(Mobile m)
		{
			if (TownDatabase.IsCitizenOfGivenTown(m, AssignedTown))
			{
				return true;
			}

			m.SendLocalizedMessage(1063970);
			return false;
		}

		public override void AddNameProperty(ObjectPropertyList list)
		{
			list.Add(LabelNumber, AssignedTown.PrettyName()); // Szata miasta {1}
			if (Parent is Mobile m)
			{
				var status = TownDatabase.GetCitizenCurrentStatus(m);
				if (status <= TownStatus.Citizen)
				{
					list.Add(status.PrettyName());
				}
			}
		}
		

		public override void OnAdded(IEntity from)
		{
			InvalidateProperties();
			base.OnAdded(from);
		}

		public override void OnRemoved(IEntity parent)
		{
			InvalidateProperties();
			base.OnRemoved(parent);
		}


		public MiastowaSzata(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(1); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			var version = reader.ReadInt();
			if(version == 0)
				OldVersionFix = true;
		}

		protected bool OldVersionFix; //Remove me
	}
	
	
	[Flipable(0x2684, 0x2683)]
	public abstract class MiastowaSzataZKapturem : MiastowaSzata
	{
		public MiastowaSzataZKapturem(int hue) : base(0x2684, hue)
		{
			LootType = LootType.Blessed;
			Weight = 4.0;
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if (ItemID == 0x2684)
				ItemID = 0x1F03;
			else
				ItemID = 0x2684;
            
			base.OnDoubleClick(from);
		}

		public MiastowaSzataZKapturem(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			if(!OldVersionFix)
				reader.ReadInt();
		}
	}

	public class MiastowaSzataOrod : MiastowaSzata
	{
		public override Towns AssignedTown => Towns.Orod;
		public override int BaseColdResistance => 1;

		[Constructable]
		public MiastowaSzataOrod() : base(2081)
		{
		}

		public MiastowaSzataOrod(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	public class MiastowaSzataTwierdza : MiastowaSzata
	{
		public override Towns AssignedTown => Towns.Twierdza;
		public override int BaseEnergyResistance => 1;

		[Constructable]
		public MiastowaSzataTwierdza() : base(1333)
		{
		}

		public MiastowaSzataTwierdza(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	public class MiastowaSzataLDelmah : MiastowaSzata
	{
		public override Towns AssignedTown => Towns.LDelmah;
		public override int BaseFireResistance => 1;

		[Constructable]
		public MiastowaSzataLDelmah() : base(2882)
		{
		}

		public MiastowaSzataLDelmah(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	public class MiastowaSzataGarlan : MiastowaSzata
	{
		public override Towns AssignedTown => Towns.Garlan;
		public override int BasePoisonResistance => 1;

		[Constructable]
		public MiastowaSzataGarlan() : base(1253)
		{
		}

		public MiastowaSzataGarlan(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
	
	public class MiastowaSzataTirassa : MiastowaSzata
	{
		public override Towns AssignedTown => Towns.Tirassa;
		public override int BasePoisonResistance => 1;

		[Constructable]
		public MiastowaSzataTirassa() : base(1253)
		{
		}

		public MiastowaSzataTirassa(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
	
	public class MiastowaSzataLotharn : MiastowaSzata
	{
		public override Towns AssignedTown => Towns.Lotharn;
		public override int BaseFireResistance => 1;

		[Constructable]
		public MiastowaSzataLotharn() : base(1182)
		{
		}

		public MiastowaSzataLotharn(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
	
	public class MiastowaSzataZKapturemOrod : MiastowaSzataZKapturem
	{
		public override Towns AssignedTown => Towns.Orod;
		public override int BaseColdResistance => 2;

		[Constructable]
		public MiastowaSzataZKapturemOrod() : base(2081)
		{
		}

		public MiastowaSzataZKapturemOrod(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	public class MiastowaSzataZKapturemTwierdza : MiastowaSzataZKapturem
	{
		public override Towns AssignedTown => Towns.Twierdza;
		public override int BaseEnergyResistance => 2;

		[Constructable]
		public MiastowaSzataZKapturemTwierdza() : base(1333)
		{
		}

		public MiastowaSzataZKapturemTwierdza(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	public class MiastowaSzataZKapturemLDelmah : MiastowaSzataZKapturem
	{
		public override Towns AssignedTown => Towns.LDelmah;
		public override int BaseFireResistance => 2;

		[Constructable]
		public MiastowaSzataZKapturemLDelmah() : base(2882)
		{
		}

		public MiastowaSzataZKapturemLDelmah(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	public class MiastowaSzataZKapturemGarlan : MiastowaSzataZKapturem
	{
		public override Towns AssignedTown => Towns.Garlan;
		public override int BasePoisonResistance => 2;

		[Constructable]
		public MiastowaSzataZKapturemGarlan() : base(1253)
		{
		}

		public MiastowaSzataZKapturemGarlan(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
	
	public class MiastowaSzataZKapturemTirassa : MiastowaSzataZKapturem
	{
		public override Towns AssignedTown => Towns.Tirassa;
		public override int BasePoisonResistance => 2;

		[Constructable]
		public MiastowaSzataZKapturemTirassa() : base(1253) //TODO: zmienić hue
		{
		}

		public MiastowaSzataZKapturemTirassa(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
	
	
	public class MiastowaSzataZKapturemLotharn : MiastowaSzataZKapturem
	{
		public override Towns AssignedTown => Towns.Lotharn;
		public override int BaseFireResistance => 2;

		[Constructable]
		public MiastowaSzataZKapturemLotharn() : base(1182)
		{
		}

		public MiastowaSzataZKapturemLotharn(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
