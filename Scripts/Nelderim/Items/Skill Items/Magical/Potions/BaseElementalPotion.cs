#region References

using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

#endregion

namespace Server.Items
{
	public abstract class BaseElementalPotion : BasePotion
	{
		public abstract Type CreatureType { get; }
		public abstract int[] LandTiles { get; }
		public abstract int[] ItemIDs { get; }

		public BaseElementalPotion(PotionEffect effect)
			: base(0xF0B, effect)
		{
		}

		public BaseElementalPotion(Serial serial)
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
			if (m.Paralyzed || m.Frozen || (m.Spell != null && m.Spell.IsCasting))
			{
				m.SendMessage("Nie mozesz rzucic mikstury bedac sparalizowanym!");
				return;
			}

			if (m.InRange(this.GetWorldLocation(), 1))
			{
				m.Target = new PotionThrowTarget(this, CreatureType, LandTiles, ItemIDs);
				m.RevealingAction();
				m.SendMessage("Gdzie chcesz rzucic miksture?");
			}
			else
			{
				m.LocalOverheadMessage(MessageType.Regular, 906, 1019045); // I can't reach that. 
			}
		}
	}

	public class PotionThrowTarget : Target
	{
		private readonly BaseElementalPotion m_Potion;
		private readonly Type m_CreatureType;
		private readonly int[] m_LandTiles;
		private readonly int[] m_ItemIDs;

		public PotionThrowTarget(BaseElementalPotion potion, Type creatureType, int[] landTiles, int[] itemIDs)
			: base(12, true, TargetFlags.None)
		{
			m_Potion = potion;
			m_CreatureType = creatureType;
			m_LandTiles = landTiles;
			m_ItemIDs = itemIDs;
		}

		private class ThrowTimer : Timer
		{
			private readonly Mobile m_From;
			private readonly Type m_CreatureType;

			public ThrowTimer(Mobile from, Type creatureType) : base(TimeSpan.FromSeconds(1.5), TimeSpan.FromSeconds(6),
				1)
			{
				m_From = from;
				m_CreatureType = creatureType;
			}

			protected override void OnTick()
			{
				TimeSpan duration = TimeSpan.FromMinutes(5);

				BaseCreature creature = Activator.CreateInstance(m_CreatureType) as BaseCreature;
				SpellHelper.Summon(creature, m_From, 0x217, duration, false, false);
			}
		}

		private void Result(Mobile from, object targeted)
		{
			IPoint3D p = targeted as IPoint3D;
			Map map = from.Map;
			IEntity to;
			if (p is Mobile)
				to = (Mobile)p;
			else
				to = new Entity(Serial.Zero, new Point3D(p), map);

			if (targeted is BaseCreature || targeted is PlayerMobile)
			{
				from.SendMessage("Lepiej nie rzucac tym w zywe istoty!");
				return;
			}

			Effects.SendMovingEffect(from, to, 0xF0B & 0x3FFF, 7, 0, false, false, m_Potion.Hue, 0);

			m_Potion.Consume();

			new ThrowTimer(from, m_CreatureType).Start();
		}

		protected override void OnTarget(Mobile from, object targeted)
		{
			if (targeted is StaticTarget)
			{
				StaticTarget obj = (StaticTarget)targeted;

				for (int i = 0; i < m_ItemIDs.Length - 1; i += 2)
				{
					if (obj.ItemID >= m_ItemIDs[i] && obj.ItemID <= m_ItemIDs[i + 1])
					{
						Result(from, targeted);
						return;
					}
				}
			}

			if (targeted is LandTarget)
			{
				LandTarget landTile = (LandTarget)targeted;

				for (int i = 0; i < m_LandTiles.Length - 1; i += 2)
				{
					if (landTile.TileID >= m_LandTiles[i] && landTile.TileID <= m_LandTiles[i + 1])
					{
						Result(from, targeted);
						return;
					}
				}
			}

			from.SendMessage("We wskazanym miejscu brakuje odpowiednich substancji...");
		}
	}
}
