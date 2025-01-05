using System;
using System.Collections.Generic;
using Server.Network;
using Server.Spells.Eighth;

namespace Server.Items
{
	public class StaffofSnakes : GnarledStaff
	{
		private static readonly TimeSpan _UseDelay = TimeSpan.FromSeconds(4.0);

		private int m_Charges;

		[CommandProperty(AccessLevel.GameMaster)]
		public int Charges
		{
			get => m_Charges;
			set
			{
				m_Charges = value;
				InvalidateProperties();
			}
		}

		public override int InitMinHits => 60;

		public override int InitMaxHits => 60;

		[Constructable]
		public StaffofSnakes()
		{
			Weight = 5.0;
			Hue = 0x304;
			Name = "Kij Przywolywacza Demonow";

			AosElementDamages.Poison = 100;
			Attributes.SpellChanneling = 1;
			Attributes.WeaponDamage = 35;
			Attributes.WeaponSpeed = 20;
			Slayer = SlayerName.Fey;
			WeaponAttributes.HitPoisonArea = 50;
			WeaponAttributes.HitLeechMana = 50;
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);
			list.Add(1060741, m_Charges.ToString());
		}

		public override void AddNameProperties(ObjectPropertyList list)
		{
			base.AddNameProperties(list);
			list.Add(1049644, "Przyzywa Stworzenia");
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (Parent != from)
			{
				from.SendLocalizedMessage(502641); // You must equip this item to use it.
				return;
			}

			if (Charges <= 0)
			{
				from.SendLocalizedMessage(502412); // There are no charges left on that item.
				return;
			}

			if (from.BeginAction(typeof(StaffofSnakes)))
			{
				Charges--;
				new SummonDaemonSpell(from, this).Cast();
				Timer.DelayCall(_UseDelay, from.EndAction, typeof(StaffofSnakes));
			}
		}
		
		public StaffofSnakes(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
			writer.Write(m_Charges);
		}


		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			m_Charges = reader.ReadInt();
		}
	}
}
