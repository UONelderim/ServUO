using System;
using Server;

namespace Server.Items
{
	public class SnieznaOzdoba : GoldRing
	{

        public override int InitMinHits { get { return 45; } }
        public override int InitMaxHits { get { return 45; } }

		[Constructable]
		public SnieznaOzdoba()
		{
			Hue = 1153;
			Name = "Platek Sniegu";
			Attributes.CastRecovery = 3;
			Attributes.CastSpeed = 2;
			Attributes.RegenMana = 2;
			Attributes.LowerRegCost = 15;
			Attributes.SpellDamage = 10;

		}

		public SnieznaOzdoba( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}