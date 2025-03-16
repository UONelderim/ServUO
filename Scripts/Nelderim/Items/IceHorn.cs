
using System;
using Server.Spells;

namespace Server.Items
{
	public class IceHorn : FireHorn
	{
		public override Type ResourceType => typeof(BlackPearl);
		public override string MissingResourceName => "czarnych perel";
		public override int EffectSound => 0x1F5;
		public override int EffectHue => 1151;
		public override int LabelNumber => 3070047;

		[Constructable]
		public IceHorn()
		{
			Hue = 2170;
		}

		public override void DoDamage(Mobile from, Mobile m, double damage)
		{
			SpellHelper.Damage(TimeSpan.Zero, m, from, damage, 0, 0, 0, 100, 0);
			m.FixedParticles(0x374A, 10, 15, 5038, 1181, 2, EffectLayer.Head);
			m.PlaySound(0x213);
		}

		public IceHorn(Serial serial) : base(serial)
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
