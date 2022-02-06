using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	public class TrapCraftingKit : BaseTool
	{
		public override CraftSystem CraftSystem{ get{ return DefTrapCrafting.CraftSystem; } }

		[Constructable]
		public TrapCraftingKit() : base( 0x1EB8 )
		{
			Weight = 2.0;
			Name = "zestaw do tworzenia pułapek";
			Hue = 2774;
		}

		[Constructable]
		public TrapCraftingKit( int uses ) : base( uses, 0x1EBA )
		{
			Weight = 2.0;
		}

		public TrapCraftingKit( Serial serial ) : base( serial )
		{
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
}