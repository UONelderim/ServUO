using System;
using Server;
using Server.Targeting;
using Server.Network;
using System.Collections;
using Server.ContextMenus;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
	public abstract class SiegeCannonball : BaseSiegeProjectile
	{
		public SiegeCannonball()
			: this(1)
		{
		}

		public SiegeCannonball(int amount)
			: base(amount, 0xE74)
		{
		}

		public SiegeCannonball(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

//	public class LightCannonball : SiegeCannonball
//	{
//		[Constructable]
//		public LightCannonball()
//			: this(1)
//		{
//		}

//		[Constructable]
//		public LightCannonball(int amount)
//			: base(amount)
//		{
//			Range = 17;
//			Area = 0;
//			AccuracyBonus = 0;
//			PhysicalDamage = 280;
//			FireDamage = 0;
//			FiringSpeed = 35;
//			Name = "lekkie kule armatnie";
//		}

//		public LightCannonball(Serial serial)
//			: base(serial)
//		{
//		}

//		public override void Serialize(GenericWriter writer)
//		{
//			base.Serialize(writer);

//			writer.Write((int)0); // version
//		}

//		public override void Deserialize(GenericReader reader)
//		{
//			base.Deserialize(reader);

//			int version = reader.ReadInt();
//		}
///*
//		public override Item Dupe(int amount)
//		{
//			LightCannonball s = new LightCannonball(amount);

//			return this.Dupe(s, amount);
//		}
// * */
//	}

	public class IronCannonball : SiegeCannonball
	{
		[Constructable]
		public IronCannonball()
			: this(1)
		{
		}

		[Constructable]
		public IronCannonball(int amount)
			: base(amount)
		{
			Range = 15;
			Area = 0;
			AccuracyBonus = 0;
			PhysicalDamage = 300;
			FireDamage = 0;
			FiringSpeed = 25;
			Name = "żelazne kule armatnie";
		}

		public IronCannonball(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
/*
		public override Item Dupe(int amount)
		{
			IronCannonball s = new IronCannonball(amount);

			return this.Dupe(s, amount);
		}
 * */
	}

	public class ExplodingCannonball : SiegeCannonball
	{
		[Constructable]
		public ExplodingCannonball()
			: this(1)
		{
		}

		[Constructable]
		public ExplodingCannonball(int amount)
			: base(amount)
		{
			Range = 11;
			Area = 1;
			AccuracyBonus = -10;
			PhysicalDamage = 100;
			FireDamage = 400;
			FiringSpeed = 20;
			Hue = 46;
			Name = "eksplodujace kule armatnie";
		}

		public ExplodingCannonball(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
/*
		public override Item Dupe(int amount)
		{
			ExplodingCannonball s = new ExplodingCannonball(amount);

			return this.Dupe(s, amount);
		}
 * */
	}

	public class FieryCannonball : SiegeCannonball
	{
		// use a fireball animation when fired
		public override int AnimationID { get { return 0x36D4; } }
		public override int AnimationHue { get { return 0; } }

		[Constructable]
		public FieryCannonball()
			: this(1)
		{
		}

		[Constructable]
		public FieryCannonball(int amount)
			: base(amount)
		{
			Range = 8;
			Area = 2;
			AccuracyBonus = -20;
			PhysicalDamage = 0;
			FireDamage = 600;
			FiringSpeed = 10;
			Hue = 33;
			Name = "Ogniste kule armatnie";
		}

		public FieryCannonball(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
/*
		public override Item Dupe(int amount)
		{
			FieryCannonball s = new FieryCannonball(amount);

			return this.Dupe(s, amount);
		}
 * */
	}

	public class GrapeShot : SiegeCannonball
	{
		// only does damage to mobiles
		public override double StructureDamageMultiplier { get { return 0.0; } } //  damage multiplier for structures

		[Constructable]
		public GrapeShot()
			: this(1)
		{
		}

		[Constructable]
		public GrapeShot(int amount)
			: base(amount)
		{
			Range = 17;
			Area = 1;
			AccuracyBonus = 0;
			PhysicalDamage = 200;
			FireDamage = 0;
			FiringSpeed = 35;
			Name = "kula z łańcuchem";
		}

		public GrapeShot(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
