#region References

using Server.Mobiles;
using Server.Network;
using Server.Targeting;

#endregion

namespace Server.Items
{
	public class PetResurrectPotion : BasePotion
	{
		[Constructable]
		public PetResurrectPotion()
			: base(0xF0B, PotionEffect.PetResurrect)
		{
			Weight = 0.5;
			Movable = true;
			Hue = 871;
			Name = "Mikstura Wskrzeszenia Zwierzecia";
		}

		public PetResurrectPotion(Serial serial)
			: base(serial)
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

		public override void Drink(Mobile m)
		{
			if (m.InRange(this.GetWorldLocation(), 1))
			{
				m.Target = new PetResTarget(this);
				m.SendMessage("Ktore zwierze chcesz ozywic?");
			}
			else
			{
				m.LocalOverheadMessage(MessageType.Regular, 906, 1019045); // I can't reach that.
			}
		}
	}

	public class PetResTarget : Target
	{
		private readonly PetResurrectPotion m_Potion;

		public PetResTarget(PetResurrectPotion pot)
			: base(12, false, TargetFlags.None)
		{
			m_Potion = pot;
		}

		protected override void OnTarget(Mobile from, object targeted)
		{
			if (targeted is Item || targeted is PlayerMobile || targeted is StaticTarget)
			{
				from.SendMessage("Tego nie da sie wskrzesic!");
				return;
			}

			BaseCreature pet = targeted as BaseCreature;

			if (pet == null || pet.Deleted || !pet.IsBonded || !pet.IsDeadPet)
			{
				from.SendMessage("Mozesz wskrzesic jedynie wierne stworzenie, ktore poleglo!");
			}
			else if (!pet.InRange(from, 2))
			{
				from.SendMessage("Podejdz blizej, aby uzyc mikstury na zwierzeciu...");
			}
			else if (BandageContext.AllowPetRessurection(from, pet, false))
			{
				BandageContext.AllowPetRessurection(from, pet, true);

				m_Potion.Consume();
				from.AddToBackpack(new Bottle());
			}
			else
			{
				from.SendLocalizedMessage(1049670);
			}
		}
	}
}
