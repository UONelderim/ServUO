#region References

using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Targeting;

#endregion

namespace Nelderim.Engines.ChaosChest
{
	public class ChaosKey : BaseDecayingItem
	{
		private ChaosSigilType m_ChaosSigilType;

		private static readonly Dictionary<ChaosSigilType, int> hues = new Dictionary<ChaosSigilType, int>
		{
			{ ChaosSigilType.Natury, 2909 },
			{ ChaosSigilType.Morlokow, 2673 },
			{ ChaosSigilType.Smierci, 1109 },
			{ ChaosSigilType.Krysztalow, 2481 },
			{ ChaosSigilType.Ognia, 1161 },
			{ ChaosSigilType.Swiatla, 1153 },
			{ ChaosSigilType.Licza, 993 },
		};

		[CommandProperty(AccessLevel.GameMaster)]
		public ChaosSigilType ChaosSigilType
		{
			get { return m_ChaosSigilType; }
			set
			{
				m_ChaosSigilType = value;
				Name = "Klucz pieczeci " + value;
				if (hues.ContainsKey(value))
					Hue = hues[value];
			}
		}

		[Constructable]
		public ChaosKey() : this((ChaosSigilType)Math.Pow(2, Utility.Random(7)))
		{
		}

		[Constructable]
		public ChaosKey(ChaosSigilType chaosSigilType) : base(0x1f16)
		{
			ChaosSigilType = chaosSigilType;
			TimeLeft = (int)TimeSpan.FromHours(4).TotalSeconds;
		}

		public ChaosKey(Serial serial) : base(serial)
		{
		}

		public override bool Nontransferable { get { return false; } }

		public override void OnDoubleClick(Mobile from)
		{
			if (!this.IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(501661); // That key is unreachable.
				return;
			}

			Target t = new UnlockTarget(this);

			from.SendLocalizedMessage(501662); // What shall I use this key on?
			from.Target = t;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
			writer.Write((int)m_ChaosSigilType);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			m_ChaosSigilType = (ChaosSigilType)reader.ReadInt();
		}

		private class UnlockTarget : Target
		{
			private readonly ChaosKey m_ChaosKey;

			public UnlockTarget(ChaosKey key) : base(3, false, TargetFlags.None)
			{
				m_ChaosKey = key;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (m_ChaosKey.Deleted || !m_ChaosKey.IsChildOf(from.Backpack))
				{
					from.SendLocalizedMessage(501661); // That key is unreachable.
					return;
				}

				if (targeted is ChaosChest { IsLockedDown: false, IsSecure: false } chest)
				{
					if (chest.IsSealed(m_ChaosKey))
					{
						chest.Unseal(m_ChaosKey);
						m_ChaosKey.Delete();
					}
					else
						from.SendMessage("Ta pieczec jest juz otwarta");
				}
				else
				{
					from.SendLocalizedMessage(501666); // You can't unlock that!
				}
			}
		}
	}
}
