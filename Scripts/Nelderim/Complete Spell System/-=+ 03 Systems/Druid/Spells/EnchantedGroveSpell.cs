using System;
using Server.Targeting;
using Server.Misc;
using System.Collections.Generic;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Druid
{
	public class DruidEnchantedGroveSpell : DruidSpell
	{
		private static SpellInfo m_Info = new(
			"Zaklęty Gaj",
			"En Ante Ohm Sepa",
			266,
			9040,
			false,
			Reagent.MandrakeRoot,
			CReagent.PetrafiedWood,
			CReagent.SpringWater
		);

		public override SpellCircle Circle => SpellCircle.Eighth;
		public override double CastDelay => 3.5;
		public override double RequiredSkill => 78.0;
		public override int RequiredMana => 70;

		public DruidEnchantedGroveSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
		{
			Scroll?.Consume();
		}

		public override void OnCast()
		{
			Caster.BeginTarget(12, true, TargetFlags.None, (from, targeted) =>
			{
				if (targeted is IPoint3D)
					Target((IPoint3D)targeted);
			});
		}

		public void Target(IPoint3D p)
		{
			if (!Caster.CanSee(p))
			{
				Caster.SendLocalizedMessage(500237); // Target can not be seen.
			}
			else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
			{
				SpellHelper.Turn(Caster, p);

				SpellHelper.GetSurfaceTop(ref p);

				Effects.PlaySound(p, Caster.Map, 0x2);

				//x,y,id
				var groves = new[]
				{
					(0, 0, 0x08e3), (-2, -2, 3290), (0, -3, 3293), (2, -2, 3290), (3, 0, 3290), (2, 2, 3292),
					(0, 3, 3290), (-2, 2, 3293), (-3, 0, 3293), (-2, -2, 3291), (0, -3, 3294), (2, -2, 3291),
					(3, 0, 3291), (2, 2, 3294), (0, 3, 3291), (-2, 2, 3294), (-3, 0, 3294)
				};
				
				for (var i = 0; i < groves.Length; i++)
				{
					var grove = groves[i];
					var loc = new Point3D(p.X + grove.Item1, p.Y + grove.Item2, p.Z);
					var item = new GroveItem(loc, Caster.Map, Caster);
					item.ItemID = grove.Item3;
					if (i == 0)
					{
						item.Name = "swiety kamien";
					}
				}
			}

			FinishSequence();
		}

		[DispellableField]
		private class GroveItem : Item
		{
			public override bool BlocksFit => true;

			public GroveItem(Point3D loc, Map map, Mobile caster) : base(0x3274)
			{
				Movable = false;
				MoveToWorld(loc, map);
				Timer.DelayCall(TimeSpan.FromSeconds(30), Delete);
				new BlessTimer(this, caster).Start();
			}

			public GroveItem(Serial serial) : base(serial)
			{
			}

			public override void Serialize(GenericWriter writer)
			{
				base.Serialize(writer);
				writer.Write((int)0); // version
			}

			public override void Deserialize(GenericReader reader)
			{
				base.Deserialize(reader);
				int version = reader.ReadInt();
				Delete();
			}
		}

		private class BlessTimer : Timer
		{
			private Item m_DruidEnchantedGrove;
			private Mobile m_Caster;
			private DateTime m_Duration;

			public BlessTimer(Item item, Mobile caster) : base(TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(1.0))
			{
				m_DruidEnchantedGrove = item;
				m_Caster = caster;
				m_Duration = DateTime.Now + TimeSpan.FromSeconds(15.0 + (Utility.RandomDouble() * 15.0));
			}

			protected override void OnTick()
			{
				if (m_DruidEnchantedGrove.Deleted)
				{
					Stop();
					return;
				}

				if (DateTime.Now > m_Duration)
				{
					Stop();
				}
				else
				{
					var list = new List<Mobile>();

					IPooledEnumerable eable = m_DruidEnchantedGrove.GetMobilesInRange(5);
					foreach (Mobile m in eable)
					{
						if (m.Player && m.Karma >= 0 && m.Alive)
							list.Add(m);
					}

					eable.Free();

					foreach (var m in list)
					{
						bool friendly = true;

						for (int j = 0; friendly && j < m_Caster.Aggressors.Count; ++j)
							friendly = ((m_Caster.Aggressors[j]).Attacker != m);

						for (int j = 0; friendly && j < m_Caster.Aggressed.Count; ++j)
							friendly = ((m_Caster.Aggressed[j]).Defender != m);

						if (friendly)
						{
							m.FixedEffect(0x37C4, 1, 12, 1109, 3); // At player
							m.Mana += (1 + (m_Caster.Karma / 100000));
							m.Hits += (1 + (m_Caster.Karma / 100000));
						}
					}
				}
			}
		}
	}
}
