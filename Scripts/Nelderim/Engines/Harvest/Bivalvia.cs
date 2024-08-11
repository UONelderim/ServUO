#region References

using System;
using Server.Items;

#endregion

namespace Server.Engines.Harvest
{
	public class Bivalvia : HarvestSystem
	{
		private static Bivalvia m_System;

		public static Bivalvia System
		{
			get
			{
				if (m_System == null)
					m_System = new Bivalvia();

				return m_System;
			}
		}

		public HarvestDefinition Definition { get; }

		private Bivalvia()
		{
			HarvestResource[] res;
			HarvestVein[] veins;

			#region Bivalvia

			HarvestDefinition shell = new HarvestDefinition();

			// Ilosc wydobywanej jednorazowo perly (zaleznosc liniowa od skilla)
			shell.ConsumedPerHarvest = 5;

			// Resource banks are every 6x6 tiles
			shell.BankWidth = 6;
			shell.BankHeight = 6;

			// Calkowita ilosc perly w jednym spawnie:
			shell.MinTotal = 2 * shell.ConsumedPerHarvest;
			shell.MaxTotal = 2 * shell.ConsumedPerHarvest;

			// A resource bank will respawn its content every 10 to 20 minutes
			shell.MinRespawn = TimeSpan.FromMinutes(7.0);
			shell.MaxRespawn = TimeSpan.FromMinutes(14.0);

			// Skill checking is done on the Zielarstwo skill
			shell.Skill = SkillName.Herbalism;

			// Set the list of harvestable tiles
			shell.Tiles = m_WaterTiles;
			shell.RangedTiles = true;

			// Players must be within 4 tiles to harvest
			shell.MaxRange = 4;

			// The Bivalvia
			shell.EffectActions = new[] { 12 };
			shell.EffectSounds = new int[0];
			shell.EffectCounts = new[] { 1 };
			shell.EffectDelay = TimeSpan.Zero;
			shell.EffectSoundDelay = TimeSpan.FromSeconds(10.0); // Czas pojedynczego polowu perel

			shell.NoResourcesMessage = 1031002; // Wyglada na to, ze w tym miejscu nie ma juz malz.
			shell.FailMessage = 1031003; // Lowiles przez chwile, ale nic nie wylowiles.
			shell.TimedOutOfRangeMessage = 1031004; // Jestes za daleko.
			shell.OutOfRangeMessage = 1031005; // Jestes za daleko.
			shell.PackFullMessage = 1031006; // Twoj plecak jest przepelniony, nie zmiesci wiecej muszli.
			shell.ToolBrokeMessage = 1031007; // Twoja siec do polowu malz rozpadla sie.

			res = new[] { new HarvestResource(0.0, 0.0, 100.0, 1031008, typeof(BlackPearl)) };

			veins = new[] { new HarvestVein(100.0, 0.0, res[0], null) };

			shell.BonusResources = new[]
			{
				new BonusHarvestResource(200, 100, null, null), // Nothing
			};

			shell.Resources = res;
			shell.Veins = veins;

			Definition = shell;
			Definitions.Add(shell);

			#endregion
		}

		public override void OnHarvestStarted(Mobile from, Item tool, HarvestDefinition def, object toHarvest)
		{
			base.OnHarvestStarted(from, tool, def, toHarvest);

			int tileID;
			Map map;
			Point3D loc;

			if (GetHarvestDetails(from, tool, toHarvest, out tileID, out map, out loc))
				Timer.DelayCall(TimeSpan.FromSeconds(1.5),
					delegate
					{
						from.RevealingAction();

						Effects.SendLocationEffect(loc, map, 0x352D, 16, 4);
						Effects.PlaySound(loc, map, 0x364);
					});
		}

		public override void OnHarvestFinished(Mobile from, Item tool, HarvestDefinition def, HarvestVein vein,
			HarvestBank bank, HarvestResource resource, object harvested)
		{
			base.OnHarvestFinished(from, tool, def, vein, bank, resource, harvested);

			from.RevealingAction();
		}

		public override bool BeginHarvesting(Mobile from, Item tool)
		{
			if (!base.BeginHarvesting(from, tool))
				return false;

			from.SendLocalizedMessage(1031009); // Wskaz miejsce, gdzie chcesz lowic malze:
			return true;
		}

		public override bool CheckHarvest(Mobile from, Item tool)
		{
			if (!base.CheckHarvest(from, tool))
				return false;

			if (from.Mounted)
			{
				from.SendLocalizedMessage(1072018); // Nie mozesz wykonywac tej czynnosci bedac konno!
				return false;
			}

			if (from.IsBodyMod && !from.Body.IsHuman)
			{
				from.SendLocalizedMessage(1072019); // Musisz przybrac ludzka forme, aby moc wykonac ta czynnosc!
				return false;
			}

			return true;
		}

		public override bool CheckHarvest(Mobile from, Item tool, HarvestDefinition def, object toHarvest)
		{
			if (!base.CheckHarvest(from, tool, def, toHarvest))
				return false;

			if (from.Mounted)
			{
				from.SendLocalizedMessage(1031010); // Nie mozesz lowic z konia!
				return false;
			}

			return true;
		}

		private static readonly int[] m_WaterTiles =
		{
			0x00A8, 0x00AB, 0x0136, 0x0137, 0x5797, 0x579C, 0x746E, 0x7485, 0x7490, 0x74AB, 0x74B5, 0x75D5
		};
	}
}
