#region References

using System;
using System.Collections.Generic;
using Server.Mobiles;

#endregion

namespace Server.ACC.CSS.Systems.Ancient
{
	[CorpseName("ciało")]
	public class Souless : BaseCreature
	{
		[CommandProperty(AccessLevel.GameMaster)]
		public Mobile Owner { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public int OldBody { get; set; }

		private readonly AncientPeerSpell spell;

		[Constructable]
		public Souless(AncientPeerSpell m_spell)
			: base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
		{
			Body = 777;
			Title = " ";
			CantWalk = true;
			spell = m_spell;
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (Owner != null && Owner == from)
			{
				Owner.Map = this.Map;
				Owner.Location = this.Location;
				Owner.BodyValue = OldBody;
				Owner.Blessed = this.Blessed;
				Owner.Direction = this.Direction;
				this.Delete();
				Owner.SendMessage("Wracasz do swego ciała.");
				if (spell != null)
				{
					spell.RemovePeerMod();
				}

				if (!Owner.CanBeginAction(typeof(AncientPeerSpell)))
					Owner.EndAction(typeof(AncientPeerSpell));
			}
		}

		public override bool OnBeforeDeath()
		{
			if (Owner != null)
				Owner.Map = this.Map;
			Owner.Location = this.Location;
			Owner.Blessed = this.Blessed;
			Owner.Direction = this.Direction;
			AFKKiller();
			Owner.Kill();
			Owner.BodyValue = 402;
			Delete();
			return false;
		}

		public void AFKKiller()
		{
			List<Mobile> toGive = new List<Mobile>();

			List<AggressorInfo> list = Aggressors;

			for (int i = 0; i < list.Count; ++i)
			{
				AggressorInfo info = list[i];

				if (info.Attacker.Player && (DateTime.Now - info.LastCombatTime) < TimeSpan.FromSeconds(30.0) &&
				    !toGive.Contains(info.Attacker))
					toGive.Add(info.Attacker);
			}

			list = Aggressed;
			for (int i = 0; i < list.Count; ++i)
			{
				AggressorInfo info = list[i];

				if (info.Defender.Player && (DateTime.Now - info.LastCombatTime) < TimeSpan.FromSeconds(30.0) &&
				    !toGive.Contains(info.Defender))
					toGive.Add(info.Defender);
			}

			if (toGive.Count == 0)
				return;

			for (int i = 0; i < toGive.Count; ++i)
			{
				Mobile m = toGive[i % toGive.Count];

				if (m != null)
				{
					m.DoHarmful(Owner);
				}
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public override bool ClickTitle { get { return false; } }

		public Souless(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
			writer.Write(Owner);
			writer.Write(OldBody);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			Owner = reader.ReadMobile();
			OldBody = reader.ReadInt();
		}
	}
}
