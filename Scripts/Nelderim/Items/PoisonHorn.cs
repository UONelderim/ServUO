
using System;
using Server.Spells;

namespace Server.Items
{
	public class PoisonHorn : FireHorn
	{
		public override Type ResourceType => typeof(NoxCrystal);
		public override string MissingResourceName => "krysztalu trucizny";
		public override int EffectSound => 0x229;
		public override int EffectHue => 0xA6;
		public override int LabelNumber => 3070048;

		[Constructable]
		public PoisonHorn()
		{
			Hue = 1557;
		}
		
		public override void DoDamage(Mobile from, Mobile m, double damage)
		{
			SpellHelper.Damage(TimeSpan.Zero, m, from, damage, 0, 0, 100, 0, 0);
			m.FixedParticles(0x36BD, 1, 10, 0x1F78, 0xA6, 0, (EffectLayer)255);
			m.ApplyPoison(from, Poison.Lesser);
		}

		public PoisonHorn(Serial serial) : base(serial)
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
