
using System;
using Server.Spells;

namespace Server.Items
{
	public class EnergyHorn : FireHorn
	{
		public override Type ResourceType => typeof(GraveDust);
		public override string MissingResourceName => "grobowego pylu";
		public override int EffectSound => 0x20A;
		public override int EffectHue => 1092;
		public override int EffectId => 0x379F;

		public override int LabelNumber => 3070049;

		[Constructable]
		public EnergyHorn()
		{
			Hue = 1092;
		}

		public override void DoDamage(Mobile from, Mobile m, double damage)
		{
			SpellHelper.Damage(TimeSpan.Zero, m, from, damage, 0, 0, 0, 0, 100);
			Effects.SendBoltEffect(m, true, 0, false);
			m.PlaySound(0x29);
		}

		public EnergyHorn(Serial serial) : base(serial)
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
			var version = reader.ReadInt();
		}
	}
}
