using Server.Targeting;
using Server.Mobiles;
using Server.Gumps;

namespace Server.Items
{
	public class RodOfResurrection : Scepter
	{
		private int _Charges;
		
		public override int InitMinHits => 255;

		public override int InitMaxHits => 255;

		[CommandProperty(AccessLevel.GameMaster)]
		public int Charges
		{
			get => _Charges;
			set
			{
				_Charges = value;
				InvalidateProperties();
			}
		}

		[Constructable]
		public RodOfResurrection()
		{
			Name = "Berlo Wskrzeszenia";
			Hue = 200;
			Charges = 100;
			Attributes.AttackChance = 12;
			Attributes.WeaponDamage = 35;
			WeaponAttributes.HitHarm = 30;
			WeaponAttributes.HitLeechHits = 25;
			WeaponAttributes.LowerStatReq = 70;
			WeaponAttributes.UseBestSkill = 1;

			Slayer = SlayerName.Exorcism;
		}

		public override void GetDamageTypes(Mobile wielder,
			out int phys,
			out int fire,
			out int cold,
			out int pois,
			out int nrgy,
			out int chaos,
			out int direct)
		{
			cold = 100;
			fire = pois = phys = nrgy = chaos = direct = 0;
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (Parent != from)
			{
				from.SendLocalizedMessage(502641); // You must equip this item to use it.
			}
			else if (Charges <= 0)
			{
				from.SendLocalizedMessage(502412); // There are no charges left on that item.
			}
			else
			{
				from.SendMessage("Kogo chcesz wskrzesic?");
				from.BeginTarget(1, false, TargetFlags.Beneficial, OnTarget);
			}
		}

		public void OnTarget(Mobile from, object targeted)
		{
			if (targeted is not Mobile m)
			{
				from.SendLocalizedMessage(500489); //Nie mozesz tego zrobic
			}
			else if (!from.CanSee(m))
			{
				from.SendLocalizedMessage(500237); // Target can not be seen.
			}
			else if (!from.Alive)
			{
				from.SendLocalizedMessage(501040); // The resurrecter must be alive.
			}
			else if (m is PlayerMobile && m.Alive)
			{
				from.SendLocalizedMessage(501041); // Target is not dead.
			}
			else if (!from.InRange(m, 2))
			{
				from.SendLocalizedMessage(501042); // Target is not close enough.
			}
			else if (m.Map == null || !m.Map.CanFit(m.Location, 16, false, false))
			{
				from.SendLocalizedMessage(501042); // Target can not be resurrected at that location.
				m.SendLocalizedMessage(502391); // Thou can not be resurrected there!
			}
			else if (targeted is PlayerMobile)
			{
				m.PlaySound(0x214);
				m.FixedEffect(0x376A, 10, 16);
				m.CloseGump<ResurrectGump>();
				m.SendGump(new ResurrectGump(m, 1));

				ConsumeCharge(from);
			}
			else if (targeted is BaseCreature pet)
			{
				var master = pet.GetMaster();

				m.PlaySound(0x214);
				m.FixedEffect(0x376A, 10, 16);

				master.CloseGump<PetResurrectGump>();
				master.SendGump(new PetResurrectGump(master, pet));

				ConsumeCharge(from);
			}
		}

		public override void AddNameProperties(ObjectPropertyList list)
		{
			base.AddNameProperties(list);
			list.Add("*dotkniecie symbolu na rekojesci powoduje wskrzeszenie czlowieka lub zwierzecia*");
		}

		public void ConsumeCharge(Mobile from)
		{
			Charges--;

			if (Charges == 0)
			{
				from.SendMessage("All of the magic has been drained from the rod.");
				Delete();
			}
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);
			list.Add(1060584, _Charges.ToString());
		}

		public RodOfResurrection(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
			writer.Write(_Charges);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			_Charges = reader.ReadInt();
		}
	}
}
