#region References

using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Multis;
using Server.Network;
using Server.Spells;
using Server.Targeting;

#endregion

namespace Server.ACC.CSS.Systems.Druid
{
	public class DruidNaturesPassageSpell : DruidSpell
	{
		private static readonly SpellInfo m_Info = new SpellInfo(
			"Naznaczenie", "Kes Sec Vauk",
			//SpellCircle.Fourth,
			239,
			9031,
			Reagent.BlackPearl,
			Reagent.Bloodmoss,
			Reagent.MandrakeRoot
		);

		public override SpellCircle Circle => SpellCircle.Fourth;

		public override double CastDelay => 0.5;
		public override double RequiredSkill => 15.0;
		public override int RequiredMana => 10;
		private readonly RunebookEntry m_Entry;
		private readonly Runebook m_Book;

		public DruidNaturesPassageSpell(Mobile caster, Item scroll) : this(caster, scroll, null, null)
		{
			if (this.Scroll != null)
				Scroll.Consume();
		}

		public DruidNaturesPassageSpell(Mobile caster, Item scroll, RunebookEntry entry, Runebook book) : base(caster,
			scroll, m_Info)
		{
			m_Entry = entry;
			m_Book = book;
		}

		public override void OnCast()
		{
			if (m_Entry == null)
				Caster.Target = new InternalTarget(this);
			else
				Effect(m_Entry.Location, m_Entry.Map, true);
		}

		public override bool CheckCast()
		{
			if (Caster.Criminal)
			{
				Caster.SendLocalizedMessage(1005561, "", 0x22); // Thou'rt a criminal and cannot escape so easily.
				return false;
			}

			if (SpellHelper.CheckCombat(Caster))
			{
				Caster.SendLocalizedMessage(1005564, "", 0x22); // Wouldst thou flee during the heat of battle??
				return false;
			}

			if (WeightOverloading.IsOverloaded(Caster))
			{
				Caster.SendLocalizedMessage(502359, "", 0x22); // Thou art too encumbered to move.
				return false;
			}

			return SpellHelper.CheckTravel(Caster, TravelCheckType.RecallFrom);
		}

		public void Effect(Point3D loc, Map map, bool checkMulti)
		{
			if (map == null)
			{
				Caster.SendLocalizedMessage(1005569); // You can not recall to another facet.
			}
			else if (!SpellHelper.CheckTravel(Caster, TravelCheckType.RecallFrom))
			{
			}
			else if (!SpellHelper.CheckTravel(Caster, map, loc, TravelCheckType.RecallTo))
			{
			}
			else if (Caster.Kills >= 5 && map != Map.Felucca)
			{
				Caster.SendLocalizedMessage(1019004); // You are not allowed to travel there.
			}
			else if (Caster.Criminal)
			{
				Caster.SendLocalizedMessage(1005561, "", 0x22); // Thou'rt a criminal and cannot escape so easily.
			}
			else if (SpellHelper.CheckCombat(Caster))
			{
				Caster.SendLocalizedMessage(1005564, "", 0x22); // Wouldst thou flee during the heat of battle??
			}
			else if (WeightOverloading.IsOverloaded(Caster))
			{
				Caster.SendLocalizedMessage(502359, "", 0x22); // Thou art too encumbered to move.
			}
			else if (!map.CanSpawnMobile(loc.X, loc.Y, loc.Z))
			{
				Caster.SendLocalizedMessage(501942); // That location is blocked.
			}
			else if ((checkMulti && SpellHelper.CheckMulti(loc, map)))
			{
				Caster.SendLocalizedMessage(501942); // That location is blocked.
			}
			else if (m_Book != null && m_Book.CurCharges <= 0)
			{
				Caster.SendLocalizedMessage(502412); // There are no charges left on that item.
			}
			else if (CheckSequence())
			{
				BaseCreature.TeleportPets(Caster, loc, map, true);
				if (m_Book != null)
					--m_Book.CurCharges;

				Caster.PlaySound(0x19);
				Effects.SendLocationParticles(Caster, 0xC87, 9, 10, 5025);
				Caster.Map = map;
				Caster.Location = loc;
				Caster.PlaySound(0x19);
				Effects.SendLocationParticles(Caster, 0xC87, 9, 10, 5025);
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private readonly DruidNaturesPassageSpell m_Owner;

			public InternalTarget(DruidNaturesPassageSpell owner) : base(12, false, TargetFlags.None)
			{
				m_Owner = owner;

				owner.Caster.LocalOverheadMessage(MessageType.Regular, 0x3B2, 501029); // Select Marked item.
			}

			protected override void OnTarget(Mobile from, object o)
			{
				if (o is RecallRune)
				{
					RecallRune rune = (RecallRune)o;

					if (rune.Marked)
						m_Owner.Effect(rune.Target, rune.TargetMap, true);
					else
						from.SendLocalizedMessage(501805); // That rune is not yet marked.
				}
				else if (o is Runebook)
				{
					RunebookEntry e = ((Runebook)o).Default;

					if (e != null)
						m_Owner.Effect(e.Location, e.Map, true);
					else
						from.SendLocalizedMessage(502354); // Target is not marked.
				}
				else if (o is Key && ((Key)o).KeyValue != 0 && ((Key)o).Link is BaseBoat)
				{
					BaseBoat boat = ((Key)o).Link as BaseBoat;

					if (!boat.Deleted && boat.CheckKey(((Key)o).KeyValue))
						m_Owner.Effect(boat.GetMarkedLocation(), boat.Map, false);
					else
						from.Send(new MessageLocalized(from.Serial, from.Body, MessageType.Regular, 0x3B2, 3, 502357,
							from.Name, "")); // I can not recall from that object.
				}
				else
				{
					from.Send(new MessageLocalized(from.Serial, from.Body, MessageType.Regular, 0x3B2, 3, 502357,
						from.Name, "")); // I can not recall from that object.
				}
			}

			protected override void OnTargetFinish(Mobile from)
			{
				m_Owner.FinishSequence();
			}
		}
	}
}
