#region References

using System;
using Server.Items;
using Server.Misc;
using Server.Network;
using Server.Spells;
using Server.Targeting;

#endregion

namespace Server.ACC.CSS.Systems.Druid
{
	public class DruidMushroomGatewaySpell : DruidSpell
	{
		private static readonly SpellInfo m_Info = new SpellInfo(
			"Przejście Natury", "Vauk Sepa Ohm",
			//SpellCircle.Seventh,
			263,
			9032,
			Reagent.BlackPearl,
			Reagent.MandrakeRoot,
			CReagent.SpringWater
		);

		public override SpellCircle Circle => SpellCircle.Seventh;

		public override double CastDelay => 3.0;
		public override double RequiredSkill => 70.0;
		public override int RequiredMana => 40;

		private readonly RunebookEntry m_Entry;

		public DruidMushroomGatewaySpell(Mobile caster, Item scroll) : this(caster, scroll, null)
		{
			if (this.Scroll != null)
				Scroll.Consume();
		}

		public DruidMushroomGatewaySpell(Mobile caster, Item scroll, RunebookEntry entry) : base(caster, scroll, m_Info)
		{
			m_Entry = entry;
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

			return SpellHelper.CheckTravel(Caster, TravelCheckType.GateFrom);
		}

		public void Effect(Point3D loc, Map map, bool checkMulti)
		{
			if (map == null)
			{
				Caster.SendLocalizedMessage(1005570); // You can not gate to another facet.
			}
			else if (!SpellHelper.CheckTravel(Caster, TravelCheckType.GateFrom))
			{
			}
			else if (!SpellHelper.CheckTravel(Caster, map, loc, TravelCheckType.GateTo))
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
			else if (!map.CanSpawnMobile(loc.X, loc.Y, loc.Z))
			{
				Caster.SendLocalizedMessage(501942); // That location is blocked.
			}
			else if ((checkMulti && SpellHelper.CheckMulti(loc, map)))
			{
				Caster.SendLocalizedMessage(501942); // That location is blocked.
			}
			else if (CheckSequence())
			{
				Caster.SendMessage(
					"You open a mystical portal in a mushroom circle"); // You open a magical gate to another location

				Effects.PlaySound(Caster.Location, Caster.Map, 0x1);
				int mushx;
				int mushy;
				int mushz;


				InternalItem firstGatea = new InternalItem(loc, map);
				mushx = Caster.X;
				mushy = Caster.Y;
				mushz = Caster.Z;
				firstGatea.ItemID = 0xD10;
				Point3D mushxyz = new Point3D(mushx, mushy, mushz);
				firstGatea.MoveToWorld(mushxyz, Caster.Map);
				InternalItem firstGateb = new InternalItem(loc, map);
				mushx = Caster.X;
				mushy = Caster.Y;
				firstGateb.ItemID = 0x373A;
				mushz = Caster.Z + 1;
				Point3D mushxyza = new Point3D(mushx, mushy, mushz);
				firstGateb.MoveToWorld(mushxyza, Caster.Map);
				InternalItem firstGatec = new InternalItem(loc, map);
				mushx = Caster.X - 1;
				firstGatec.ItemID = 0xD11;
				mushy = Caster.Y + 1;
				mushz = Caster.Z;
				Point3D mushxyzb = new Point3D(mushx, mushy, mushz);
				firstGatec.MoveToWorld(mushxyzb, Caster.Map);
				InternalItem firstGated = new InternalItem(loc, map);
				firstGated.ItemID = 0xD0C;
				mushx = Caster.X;
				mushy = Caster.Y + 2;
				mushz = Caster.Z;
				Point3D mushxyzc = new Point3D(mushx, mushy, mushz);
				firstGated.MoveToWorld(mushxyzc, Caster.Map);
				InternalItem firstGatee = new InternalItem(loc, map);
				mushx = Caster.X + 1;
				firstGatee.ItemID = 0xD0D;
				mushy = Caster.Y + 1;
				mushz = Caster.Z;
				Point3D mushxyzd = new Point3D(mushx, mushy, mushz);
				firstGatee.MoveToWorld(mushxyzd, Caster.Map);
				InternalItem firstGatef = new InternalItem(loc, map);
				firstGatef.ItemID = 0xD0E;
				mushx = Caster.X + 2;
				mushy = Caster.Y;
				mushz = Caster.Z;
				Point3D mushxyze = new Point3D(mushx, mushy, mushz);
				firstGatef.MoveToWorld(mushxyze, Caster.Map);
				InternalItem firstGateg = new InternalItem(loc, map);
				mushx = Caster.X + 1;
				firstGateg.ItemID = 0xD0F;

				mushy = Caster.Y - 1;
				mushz = Caster.Z;
				Point3D mushxyzf = new Point3D(mushx, mushy, mushz);
				firstGateg.MoveToWorld(mushxyzf, Caster.Map);

				Effects.PlaySound(loc, map, 0x1);

				InternalItem secondGatea = new InternalItem(Caster.Location, Caster.Map);
				mushx = loc.X;
				mushy = loc.Y;
				mushz = loc.Z;
				secondGatea.ItemID = 0xD10;

				Point3D mushaxyz = new Point3D(mushx, mushy, mushz);
				secondGatea.MoveToWorld(mushaxyz, map);
				InternalItem secondGateb = new InternalItem(Caster.Location, Caster.Map);
				mushx = loc.X;
				mushy = loc.Y;
				secondGateb.ItemID = 0x373A;
				mushz = loc.Z + 1;
				Point3D mushaxyza = new Point3D(mushx, mushy, mushz);
				secondGateb.MoveToWorld(mushaxyza, map);
				InternalItem secondGatec = new InternalItem(Caster.Location, Caster.Map);
				mushx = loc.X - 1;
				secondGatec.ItemID = 0xD11;
				mushy = loc.Y + 1;
				mushz = loc.Z - 1;
				Point3D mushaxyzb = new Point3D(mushx, mushy, mushz);
				secondGatec.MoveToWorld(mushaxyzb, map);
				InternalItem secondGated = new InternalItem(Caster.Location, Caster.Map);
				mushx = loc.X;
				mushy = loc.Y + 2;
				secondGated.ItemID = 0xD0C;
				mushz = loc.Z;
				Point3D mushaxyzc = new Point3D(mushx, mushy, mushz);
				secondGated.MoveToWorld(mushaxyzc, map);
				InternalItem secondGatee = new InternalItem(Caster.Location, Caster.Map);
				mushx = loc.X + 1;
				mushy = loc.Y + 1;
				mushz = loc.Z;
				secondGatee.ItemID = 0xD0D;
				Point3D mushaxyzd = new Point3D(mushx, mushy, mushz);
				secondGatee.MoveToWorld(mushaxyzd, map);
				InternalItem secondGatef = new InternalItem(Caster.Location, Caster.Map);
				mushx = loc.X + 2;
				mushy = loc.Y;
				mushz = loc.Z;
				secondGatef.ItemID = 0xD0E;
				Point3D mushaxyze = new Point3D(mushx, mushy, mushz);
				secondGatef.MoveToWorld(mushaxyze, map);
				InternalItem secondGateg = new InternalItem(Caster.Location, Caster.Map);
				mushx = loc.X + 1;
				secondGateg.ItemID = 0xD0F;
				mushy = loc.Y - 1;
				mushz = loc.Z;
				Point3D mushaxyzf = new Point3D(mushx, mushy, mushz);
				secondGateg.MoveToWorld(mushaxyzf, map);
			}

			FinishSequence();
		}

		[DispellableField]
		private class InternalItem : Moongate
		{
			public override bool ShowFeluccaWarning => true;

			public InternalItem(Point3D target, Map map) : base(target, map)
			{
				Map = map;

				if (ShowFeluccaWarning && map == Map.Felucca)
					ItemID = 0xDDA;

				Dispellable = true;

				InternalTimer t = new InternalTimer(this);
				t.Start();
			}

			public InternalItem(Serial serial) : base(serial)
			{
			}

			public override void Serialize(GenericWriter writer)
			{
				base.Serialize(writer);
			}

			public override void Deserialize(GenericReader reader)
			{
				base.Deserialize(reader);
				Delete();
			}

			private class InternalTimer : Timer
			{
				private readonly Item m_Item;

				public InternalTimer(Item item) : base(TimeSpan.FromSeconds(30.0))
				{
					m_Item = item;
				}

				protected override void OnTick()
				{
					m_Item.Delete();
				}
			}
		}

		private class InternalTarget : Target
		{
			private readonly DruidMushroomGatewaySpell m_Owner;

			public InternalTarget(DruidMushroomGatewaySpell owner) : base(12, false, TargetFlags.None)
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
						from.SendLocalizedMessage(501803); // That rune is not yet marked.
				}
				else if (o is Runebook)
				{
					RunebookEntry e = ((Runebook)o).Default;

					if (e != null)
						m_Owner.Effect(e.Location, e.Map, true);
					else
						from.SendLocalizedMessage(502354); // Target is not marked.
				}
				else
				{
					from.Send(new MessageLocalized(from.Serial, from.Body, MessageType.Regular, 0x3B2, 3, 501030,
						from.Name, "")); // I can not gate travel from that object.
				}
			}

			protected override void OnTargetFinish(Mobile from)
			{
				m_Owner.FinishSequence();
			}
		}
	}
}
