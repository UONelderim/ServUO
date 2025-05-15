#region References

using System;
using System.Collections;
using Server.Items;
using Server.Mobiles;

#endregion

namespace Server.ACC.CSS.Systems.Ranger
{
	[CorpseName("zwłoki ognistego ogara")]
	public class HellHoundFamiliar : BaseFamiliar
	{
		public HellHoundFamiliar()
		{
			Name = "ognisty ogar";
			Body = 98;
			Hue = 1161;
			BaseSoundID = 229;

			SetStr(100);
			SetDex(90);
			SetInt(50);

			SetHits(80);
			SetStam(70);
			SetMana(0);

			SetDamage(15, 30);

			SetDamageType(ResistanceType.Fire, 100);

			SetResistance(ResistanceType.Physical, 10, 15);
			SetResistance(ResistanceType.Fire, 99);
			SetResistance(ResistanceType.Cold, 10, 15);
			SetResistance(ResistanceType.Poison, 10, 15);
			SetResistance(ResistanceType.Energy, 10, 15);

			SetSkill(SkillName.Wrestling, 60.0);
			SetSkill(SkillName.Tactics, 60.0);

			ControlSlots = 1;

			AddItem(new LightSource());

			SetSpecialAbility(SpecialAbility.DragonBreath);
		}

		private DateTime m_NextFlare;

		public override void OnThink()
		{
			base.OnThink();

			if (DateTime.Now < m_NextFlare)
				return;

			m_NextFlare = DateTime.Now + TimeSpan.FromSeconds(5.0 + (25.0 * Utility.RandomDouble()));

			this.FixedEffect(0x37C4, 1, 12, 1109, 6);
			this.PlaySound(230);

			Timer.DelayCall(TimeSpan.FromSeconds(0.5), Flare);
		}

		private void Flare()
		{
			Mobile caster = this.ControlMaster;

			if (caster == null)
				caster = this.SummonMaster;

			if (caster == null)
				return;

			ArrayList list = new ArrayList();

			var eable = this.GetMobilesInRange(5);
			foreach (Mobile m in eable)
			{
				if (m.Player && m.Alive && !m.IsDeadBondedPet && m.Karma <= 0)
					list.Add(m);
			}
			eable.Free();

			for (int i = 0; i < list.Count; ++i)
			{
				Mobile m = (Mobile)list[i];
				bool friendly = true;

				for (int j = 0; friendly && j < caster.Aggressors.Count; ++j)
					friendly = (caster.Aggressors[j].Attacker != m);

				for (int j = 0; friendly && j < caster.Aggressed.Count; ++j)
					friendly = (caster.Aggressed[j].Defender != m);

				if (friendly)
				{
					m.FixedEffect(0x37C4, 1, 12, 1109, 3); // At player
					m.Mana += 1 - (m.Karma / 1000);
				}
			}
		}

		public override bool AutoDispel => false;

		public HellHoundFamiliar(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
