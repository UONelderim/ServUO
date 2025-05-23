//Edits by Raist/Tass23 2/2/2017

using System;
using Server;
using Server.Gumps;
using Server.Misc;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Mobiles;
using Server.Spells;

namespace Server.Items
{
	public enum PotionUse
	{
		Create
	}

	public class AnimatedVDoll : Item
	{
		private PotionUse m_Use;

		[CommandProperty(AccessLevel.GameMaster)]
		public PotionUse Use
		{
			get { return m_Use; }
			set { m_Use = value; }
		}

		[Constructable]
		public AnimatedVDoll() : base(3848)
		{
			Name = "mikstura animowania";
			Weight = 1;
			Hue = 2992;
			Stackable = true;
			Label1 = "*mikstura wymagajaca wiedzy o Guslarstwie";
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
				return;
			}

			if (!Movable)
				return;
			from.Target = new InternalTarget(this);
		}

		public AnimatedVDoll(Serial serial) : base(serial)
		{
		}

		public void Create(Mobile from, Mobile target)
		{
			target.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
			target.PlaySound(0x202);
			target.SendMessage("Tajemnicze siły, kierowane przez {0}, ożywiły twoją laleczkę!",
				from.Name); //The player creating the animated voodoo doll is {0} and this message goes to the target player.
		}

		private class InternalTarget : Target
		{
			private AnimatedVDoll Doll;

			public InternalTarget(AnimatedVDoll doll) : base(1, false, TargetFlags.None)
			{
				Doll = doll;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (Doll.Deleted)
					return;

				if (targeted is VoodooDoll v && v.CursedPerson != null)
				{
					Mobile m = v.CursedPerson;

					// Tylko skill-check, niezależnie od typu ofiary i stanu sieci
					if (from.CheckSkill(SkillName.TasteID, 50, 100))
					{
						m.RevealingAction();

						if (m.Body.IsHuman && !m.Mounted)
							m.Animate(20, 5, 1, true, false, 0);

						if (Doll.Use == PotionUse.Create)
						{
							from.Karma -= 25;
							from.SendLocalizedMessage(1019063); // utrata karmy
							Doll.Create(from, m);
							v.Animated++;

							// Zamiast Delete(), zmniejszamy stos o 1
							// Item.Consume() obsłuży stackowalność automatycznie
							Doll.Consume();
							// Automatycznie otwieramy gump z zaklęciami tuż po animacji:
							from.SendGump(new VoodooSpellGump(from, v));
						}
					}
					else
					{
						from.SendMessage(
							"Siły duchowe uniemożliwiają ci skoncentrowanie ich energii. Spróbuj ponownie później.");
					}
				}
				else
				{
					from.SendMessage("Tego nie ozywisz!");
				}
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
			writer.Write((int)m_Use);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			m_Use = (PotionUse)reader.ReadInt();
		}
	}
}
