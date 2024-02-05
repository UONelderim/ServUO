#region References

using System;
using System.Collections.Generic;
using System.Linq;
using Server.Items;

#endregion

namespace Server.Mobiles.Swiateczne
{
	public class KevinBoss : BaseCreature
	{
		private readonly List<BaseTrap> m_Traps;
		private const int MaxTraps = 20;
		private const int TrapRange = 8;

		[Constructable]
		public KevinBoss() : base(AIType.AI_Melee, FightMode.Closest, 10, 7, 0.2, 0.4)
		{
			Name = "Nivek";

			Body = 400;
			Hue = Race.RandomSkinHue();

			SetStr(800, 900);
			SetDex(200, 220);
			SetInt(600, 650);
			SetHits(15000);
			SetStam(205, 300);

			SetDamage(19, 25);

			SetDamageType(ResistanceType.Physical, 50);
			SetDamageType(ResistanceType.Cold, 50);

			SetResistance(ResistanceType.Physical, 55);
			SetResistance(ResistanceType.Fire, 55);
			SetResistance(ResistanceType.Cold, 100);
			SetResistance(ResistanceType.Poison, 75);
			SetResistance(ResistanceType.Energy, 75);

			SetSkill(SkillName.MagicResist, 120.0, 120.0);
			SetSkill(SkillName.Tactics, 120.0, 120.0);
			SetSkill(SkillName.Archery, 120.0, 120.0);
			SetSkill(SkillName.Anatomy, 120.0, 120.0);

			Fame = 22500;
			Karma = 22500;

			VirtualArmor = 80;
			AddItem(new FancyShirt(0x20));
			AddItem(new Surcoat(0x20));
			AddItem(new LongPants(0x20));
			AddItem(new FurCape(0x497));
			AddItem(new ElvenBoots());

			AddItem(new Crossbow());
			PackItem(new Bolt(Utility.Random(50, 60)));

			HairItemID = 0x203C;
			HairHue = 0x47F;

			FacialHairItemID = 0x204B;
			FacialHairHue = 0x47F;

			m_Traps = new List<BaseTrap>();
		}
		// public override Poison HitPoison { get { return Poison.Lethal; } }

		public override void OnDeath(Container c)
		{
			base.OnDeath(c);

			ArtifactHelper.ArtifactDistribution(this);

			foreach (var trap in m_Traps)
			{
				trap.Delete();
			}
		}

		public KevinBoss(Serial serial) : base(serial)
		{
		}

		public override bool BardImmune
		{
			get { return true; }
		}

		public override double DispelDifficulty
		{
			get { return 135.0; }
		}

		public override Poison PoisonImmune
		{
			get { return Poison.Lethal; }
		}

		public override void OnThink()
		{
			PlantTraps();

			base.OnThink();
		}

		private static readonly Type[] TrapTypes =
		{
			typeof(FireColumnTrap), typeof(FlameSpurtTrap), typeof(GasTrap), typeof(GiantSpikeTrap),
			typeof(MushroomTrap), typeof(SawTrap), typeof(SpikeTrap), typeof(StoneFaceTrap)
		};

		private readonly int[] TrapItemIds = { 0x1022, 0x1023, 0x1059, 0x105A, 0x119C, 0x11A2, 0x171D, 0x171E, };

		private BaseTrap GetRandomTrap()
		{
			Type type = TrapTypes[Utility.Random(TrapTypes.Length)];
			return (BaseTrap)Activator.CreateInstance(type);
		}

		private int GetTrapItemId()
		{
			return TrapItemIds[Utility.Random(TrapItemIds.Length)];
		}

		private void PlantTraps()
		{
			if (Combatant != null)
			{
				for (int i = 0; i < m_Traps.Count; i++)
				{
					BaseTrap trap = m_Traps[i];
					if (!trap.Deleted && !InRange(trap.Location, TrapRange))
					{
						trap.Delete();
					}

					if (trap.Deleted)
					{
						m_Traps.RemoveAt(i);
						--i;
					}
				}

				if (m_Traps.Count < MaxTraps)
				{
					BaseTrap newTrap = GetRandomTrap();
					Point3D newLocation;
					do
					{
						int newX = Location.X + Utility.RandomMinMax(-TrapRange, TrapRange);
						int newY = Location.Y + Utility.RandomMinMax(-TrapRange, TrapRange);
						int newZ = Map.GetAverageZ(newX, newY);
						newLocation = new Point3D(newX, newY, newZ);
					} while (!Map.CanFit(newLocation, 16) && m_Traps.All(trap => trap.Location != newLocation));

					newTrap.MoveToWorld(newLocation, Map);
					newTrap.ItemID = GetTrapItemId();
					newTrap.Visible = true;
					newTrap.Name = "Prowizoryczna pulapka";
					Effects.SendMovingEffect(this, newTrap, newTrap.ItemID, 7, 0, false, false, 0, 0);
					m_Traps.Add(newTrap);
				}
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			var version = reader.ReadInt();
		}
	}
}
