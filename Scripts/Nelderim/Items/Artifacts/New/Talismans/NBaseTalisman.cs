using System;
using Server.Engines.Craft;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Spells.Necromancy;
using Server.Spells.Fourth;

namespace Server.Items
{
	public class NBaseTalisman : BaseClothing
	{
        public NBaseTalisman(int itemID, int rarity): base(itemID, Layer.Talisman, 0)
		{
            Generate(rarity);
		}

		public NBaseTalisman( Serial serial ) : base( serial )
		{
		}

        public override int InitMinHits { get { return 25; } }
        public override int InitMaxHits { get { return 25; } }

		public override bool OnEquip( Mobile from )
		{
			return base.OnEquip(from);
		}

		public override void OnDoubleClick( Mobile m ) 
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{	
			base.GetProperties( list );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version		= reader.ReadInt();
		}

		public void Generate(int rarity)
		{
            if (rarity >= 2)
            {
                int whichstat = Utility.RandomMinMax(1, 3);
                if (whichstat == 1) Attributes.BonusInt  = 6;
                if (whichstat == 2) Attributes.BonusDex  = 6;
                if (whichstat == 3) Attributes.BonusStr  = 6;
               // if (whichstat == 4) Attributes.RegenHits = 2;
               // if (whichstat == 5) Attributes.RegenMana = 2;
               // if (whichstat == 6) Attributes.RegenStam = 3;
            }

            if (rarity >= 3)
            {
                int whichone = Utility.RandomMinMax(1, 13);
                if (whichone == 1) Attributes.LowerManaCost   = Utility.RandomMinMax(4, 6);
                if (whichone == 2) Attributes.LowerRegCost    = Utility.RandomMinMax(8, 15);
                if (whichone == 3) Attributes.AttackChance    = Utility.RandomMinMax(4, 6);
                if (whichone == 4) Attributes.DefendChance    = Utility.RandomMinMax(4, 6);
                if (whichone == 5) Attributes.WeaponDamage    = Utility.RandomMinMax(7, 15);
                if (whichone == 6) Attributes.BonusMana       = Utility.RandomMinMax(4, 7);
                if (whichone == 7) Attributes.BonusStam       = Utility.RandomMinMax(4, 7);
                if (whichone == 8) Attributes.BonusHits       = Utility.RandomMinMax(4, 7);
                if (whichone == 9) Attributes.SpellDamage     = Utility.RandomMinMax(4, 6);
                if (whichone == 10) Attributes.CastRecovery   = 1;
                if (whichone == 11) Attributes.Luck           = Utility.RandomMinMax(3, 5) * 10;
				if (whichone == 12) Attributes.ReflectPhysical     = Utility.RandomMinMax(10, 20);
				if (whichone == 13) Attributes.EnhancePotions     = Utility.RandomMinMax(10, 15);
            }
		}
	}
}
