#region References

using Server.Targeting;

#endregion

namespace Server.Items
{
	public class LowerReqDeed : Item
	{
		[Constructable]
		public LowerReqDeed() : base(0x14F0)
		{
			Weight = 1.0;
			Name = "zwoj zmniejszajacy wymagania";
			Hue = 233;
			LootType = LootType.Blessed;
		}

		public override void OnDoubleClick(Mobile Gracz)
		{
			if (IsChildOf(Gracz.Backpack))
			{
				Gracz.BeginTarget(6, false, TargetFlags.None, OnTarget);
				Gracz.SendLocalizedMessage(502450);
			}
			else
			{
				Gracz.SendLocalizedMessage(1042001);
			}
		}

		public void OnTarget(Mobile Gracz, object targeted)
		{
			if (Deleted)
			{
				return;
			}

			if (!(targeted is Item)) return;

			Item item = (Item)targeted;

			if (item is BaseArmor)
			{
				BaseArmor armor = (BaseArmor)item;

				if (armor.StrReq > 10)
				{
					armor.ArmorAttributes.LowerStatReq = 100;
				}
			}

			if (item is BaseWeapon)
			{
				BaseWeapon weapon = (BaseWeapon)item;

				if (weapon.StrRequirement > 10)
				{
					weapon.WeaponAttributes.LowerStatReq = 100;
				}
			}
		}

		public LowerReqDeed(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int verison = reader.ReadInt();
		}
	}
}
