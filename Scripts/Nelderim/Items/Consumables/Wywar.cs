using System;

namespace Server.Items
{
	public abstract class BaseWywar : Item
	{
		public override double DefaultWeight => 1.0;
		public virtual int Bonus => 10;
		public abstract StatType Type { get; }

		public BaseWywar() : base( 3854 )
		{
			Stackable = true;
		}

		public BaseWywar( Serial serial ) : base( serial )
		{
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			var modName = $"[Wywar] {Type}";
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042038 ); // You must have the object in your backpack to use it.
			}
			else if ( from.GetStatMod( modName ) != null )
			{
				from.SendLocalizedMessage( 1062927 ); // You have eaten one of these recently and eating another would provide no benefit.
			}
			else
			{
				from.PlaySound( 0x1EE );
				from.AddStatMod( new StatMod( Type, modName, Bonus, TimeSpan.FromMinutes( 5.0 ) ) );
				Consume();
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class WywarSily : BaseWywar
	{
		public override StatType Type => StatType.Str;
		
		[Constructable]
		public WywarSily()
		{
			Name = "Wywar Sily";
			Hue = 2377;
		}

		public WywarSily(Serial serial) : base(serial)
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
	
	public class WywarZrecznosci: BaseWywar
	{
		public override StatType Type => StatType.Dex;
		
		[Constructable]
		public WywarZrecznosci()
		{
			Name = "Wywar Zrecznosci";
			Hue = 2088;
		}

		public WywarZrecznosci(Serial serial) : base(serial)
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
	
	public class WywarInteligencji: BaseWywar
	{
		public override StatType Type => StatType.Int;
		
		[Constructable]
		public WywarInteligencji()
		{
			Name = "Wywar Inteligencji";
			Hue = 1569;
		}

		public WywarInteligencji(Serial serial) : base(serial)
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
