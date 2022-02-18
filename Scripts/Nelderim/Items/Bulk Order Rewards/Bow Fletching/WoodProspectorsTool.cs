#region References

using System;
using Server.Engines.Harvest;
using Server.Targeting;

#endregion

namespace Server.Items
{
	public class WoodProspectorsTool : BaseBashing
	{
		public override int LabelNumber { get { return 1046493; } } // narzedzie poszukiwacza drewna

		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.CrushingBlow; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.ShadowStrike; } }

		public override int StrengthReq { get { return 40; } }
		public override int MinDamage { get { return 13; } }
		public override int MaxDamage { get { return 15; } }
		public override float Speed { get { return 3.25f; } }

		[Constructable]
		public WoodProspectorsTool() : base(0xFB4)
		{
			Hue = 0x973;
			Weight = 9.0;
			UsesRemaining = 50;
			ShowUsesRemaining = true;
		}

		public WoodProspectorsTool(Serial serial) : base(serial)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (IsChildOf(from.Backpack) || Parent == from)
				from.Target = new InternalTarget(this);
			else
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
		}

		public void Prospect(Mobile from, object toProspect)
		{
			if (!IsChildOf(from.Backpack) && Parent != from)
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
				return;
			}

			HarvestSystem system = Lumberjacking.System;

			int tileID;
			Map map;
			Point3D loc;

			if (!system.GetHarvestDetails(from, this, toProspect, out tileID, out map, out loc))
			{
				from.SendLocalizedMessage(1049048); // You cannot use your prospector tool on that.
				return;
			}

			HarvestDefinition def = system.GetDefinition(tileID);
			if (def == null)
			{
				from.SendLocalizedMessage(1049048); // You cannot use your prospector tool on that.
				return;
			}

			HarvestVein[] veinList;
			def.GetRegionVeins(out veinList, map, loc.X, loc.Y);

			if (veinList.Length <= 1)
			{
				from.SendLocalizedMessage(1049048); // You cannot use your prospector tool on that.
				return;
			}

			HarvestBank bank = def.GetBank(map, loc.X, loc.Y);

			if (bank == null)
			{
				from.SendLocalizedMessage(1049048); // You cannot use your prospector tool on that.
				return;
			}

			HarvestVein vein = bank.Vein, defaultVein = bank.DefaultVein;

			if (vein == null || defaultVein == null)
			{
				from.SendLocalizedMessage(1049048); // You cannot use your prospector tool on that.
				return;
			}

			if (vein != defaultVein)
			{
				@from.SendLocalizedMessage(1046484); // To drzewo wyglada na juz zbadane.
				return;
			}

			int veinIndex = Array.IndexOf(veinList, vein);

			if (veinIndex < 0)
			{
				from.SendLocalizedMessage(1049048); // You cannot use your prospector tool on that.
			}
			else if (veinIndex >= (veinList.Length - 1))
			{
				from.SendLocalizedMessage(1046485); // W tym miejscu nie znajdziesz juz lepszego drewna..
			}
			else
			{
				bank.Vein = veinList[veinIndex + 1];

				int indexInAllResources = Array.IndexOf(def.Resources, bank.Vein.PrimaryResource);
				if (indexInAllResources < 7 && indexInAllResources > 0)
					from.SendLocalizedMessage(1046486 + indexInAllResources);
				else
					from.SendMessage("Badasz drzewo, zauwazajac ze mozna tu pozyskac lepsze drewno.");

				--UsesRemaining;

				if (UsesRemaining <= 0)
				{
					from.SendLocalizedMessage(1049062); // You have used up your prospector's tool.
					Delete();
				}
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(2); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 2:
					break;
				case 1:
				{
					UsesRemaining = reader.ReadInt();
					break;
				}
				case 0:
				{
					UsesRemaining = 50;
					break;
				}
			}
		}

		private class InternalTarget : Target
		{
			private readonly WoodProspectorsTool m_Tool;

			public InternalTarget(WoodProspectorsTool tool) : base(2, true, TargetFlags.None)
			{
				m_Tool = tool;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				m_Tool.Prospect(from, targeted);
			}
		}
	}
}
