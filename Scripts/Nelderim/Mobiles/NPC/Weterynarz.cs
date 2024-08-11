// 20.05.03 :: Maupushon :: zwiekszenie kasy z 3k na 20k za resa 

#region References

using System.Collections.Generic;
using Server.ContextMenus;
using Server.Items;
using Server.Targeting;

#endregion

namespace Server.Mobiles
{
	public class Weterynarz : BaseVendor
	{
		private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
		protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

		[Constructable]
		public Weterynarz() : base("- weterynarz")
		{
			AddItem(new WildStaff());

			SetSkill(SkillName.Herding, 80.0, 100.0);
			SetSkill(SkillName.AnimalLore, 85.0, 100.0);
			SetSkill(SkillName.AnimalTaming, 90.0, 100.0);
			SetSkill(SkillName.Veterinary, 90.0, 100.0);
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBHealer());
		}

		public override VendorShoeType ShoeType { get { return VendorShoeType.Sandals; } }

		public virtual int GetRobeColor()
		{
			return Utility.RandomGreenHue();
		}

		public override void InitOutfit()
		{
			base.InitOutfit();

			AddItem(new Robe(GetRobeColor()));
		}

		public override void AddCustomContextEntries(Mobile from, List<ContextMenuEntry> list)
		{
			if (from.Alive)
			{
				if (IsAssignedBuildingWorking())
				{
					list.Add(new ResEntry(this, from));
				}
			}

			base.AddCustomContextEntries(from, list);
		}

		private class ResEntry : ContextMenuEntry
		{
			private readonly Weterynarz m_Wet;
			private readonly Mobile m_From;

			public ResEntry(Weterynarz wet, Mobile from) : base(6072, 5)
			{
				m_Wet = wet;
				m_From = from;
			}

			public override void OnClick()
			{
				m_Wet.BeginRes(m_From);
				m_Wet.Say( "Wskrzeszenie tego zwierzecia bedzie Cie kosztowac 5 000 centarow." ); 
			}
		}

		public void BeginRes(Mobile from)
		{
			from.Target = new ResTarget(this);
		}

		private class ResTarget : Target
		{
			private readonly Weterynarz m_Wet;

			public ResTarget(Weterynarz wet) : base(12, false, TargetFlags.None)
			{
				m_Wet = wet;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (!m_Wet.IsAssignedBuildingWorking())
				{
					m_Wet.SayTo(from, 1063883); // Miasto nie oplacilo moich uslug. Nieczynne.
				}
				else if (targeted is Mobile && m_Wet.GetDistanceToSqrt((Mobile)targeted) > 7)
					from.SendLocalizedMessage(500446); // Za daleko.
				else if (targeted is BaseCreature)
				{
					m_Wet.EndRes(from, (BaseCreature)targeted);
				}
				else if (targeted == from)
					m_Wet.SayTo(from, 502672); // HA HA HA! Sorry, I am not an inn.
				else
					m_Wet.SayTo(from, "Nie mozna go wskrzesic"); // You can't stable that!
			}
		}

		public void EndRes( Mobile from, BaseCreature pet )
		{
			if ( Deleted )
				return;

			if (!IsAssignedBuildingWorking())
			{
				SayTo(from, 1063883); // Miasto nie oplacilo moich uslug. Nieczynne.
			}
			else if (!pet.IsDeadPet) 
			{
				SayTo(from, "Nie wskrzesze tego, co juz zyje.");
			}
			/*else if ( !pet.Controlled || pet.ControlMaster != from )
			{
			    SayTo( from, 1042562 ); // You do not own that pet!
			}*/
			else if ( pet.Body.IsHuman )
			{
				SayTo( from, 502672 ); // HA HA HA! Sorry, I am not an inn.
			}
			else if (BandageContext.AllowPetRessurection(from, pet, false))
			{
				Item gold = from.Backpack.FindItemByType(typeof(Gold));
				int amountToWithdraw = 5000;

				if (gold != null && gold.Amount >= amountToWithdraw)
				{
					gold.Consume(amountToWithdraw);
					pet.PlaySound( 0x214 );
					pet.FixedEffect( 0x376A, 10, 16 );
					pet.ResurrectPet();
					BandageContext.AllowPetRessurection(from, pet, true);
					Say("Powstan moj przyjacielu. Chcialbym ocalic kazde nieszczesliwe zwierze.");
				}
				else
				{
					if (Banker.Withdraw(from, amountToWithdraw - (gold != null ? gold.Amount : 0)))
					{
						pet.PlaySound( 0x214 );
						pet.FixedEffect( 0x376A, 10, 16 );
						pet.ResurrectPet();
						BandageContext.AllowPetRessurection(from, pet, true);
						Say("Powstan moj przyjacielu. Chcialbym ocalic kazde nieszczesliwe zwierze.");
					}
					else
					{
						SayTo(from, 502677); // But thou hast not the funds in thy bank account!
					}
				}
			}
			else
			{
				from.SendLocalizedMessage(1049670);
			}
		}

		public Weterynarz(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
		}
	}
}
