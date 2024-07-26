using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("zwloki straznika")]
	public class StandardNelderimGuard : BaseNelderimGuard
	{
		[Constructable]
		public StandardNelderimGuard() : base(GuardType.StandardGuard)
		{
			PackGold(20, 80);
		}

		public StandardNelderimGuard(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	[CorpseName("zwloki straznika")]
	public class MageNelderimGuard : BaseNelderimGuard
	{
		public override double WeaponAbilityChance => 0.45;
		[Constructable]
		public MageNelderimGuard() : base(GuardType.MageGuard, AIType.AI_Mage)
		{
			SetWeaponAbility(WeaponAbility.ParalyzingBlow);
			SetWeaponAbility(WeaponAbility.Disarm);
			PackGold(40, 80);
		}

		public MageNelderimGuard(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	[CorpseName("zwloki straznika")]
	public class HeavyNelderimGuard : BaseNelderimGuard
	{
		public override double WeaponAbilityChance => 0.45;

		[Constructable]
		public HeavyNelderimGuard() : base(GuardType.HeavyGuard)
		{
			SetWeaponAbility(WeaponAbility.WhirlwindAttack);
			SetWeaponAbility(WeaponAbility.BleedAttack);
			PackGold(40, 80);
		}

		public HeavyNelderimGuard(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	[CorpseName("zwloki straznika")]
	public class MountedNelderimGuard : BaseNelderimGuard
	{
		public override double WeaponAbilityChance => 0.4;
		
		[Constructable]
		public MountedNelderimGuard() : base(GuardType.MountedGuard)
		{
			SetWeaponAbility(WeaponAbility.Disarm);
			SetWeaponAbility(WeaponAbility.BleedAttack);
			PackGold(40, 80);
		}

		public MountedNelderimGuard(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	[CorpseName("zwloki straznika")]
	public class ArcherNelderimGuard : BaseNelderimGuard
	{
		public override double WeaponAbilityChance => 0.4;
		
		[Constructable]
		public ArcherNelderimGuard() : base(GuardType.ArcherGuard, AIType.AI_Archer,  rangeFight: 6)
		{
			SetWeaponAbility(WeaponAbility.ParalyzingBlow);
			SetWeaponAbility(WeaponAbility.ArmorIgnore);
			PackGold(30, 90);
		}

		public ArcherNelderimGuard(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	[CorpseName("zwloki straznika")]
	public class EliteNelderimGuard : BaseNelderimGuard
	{
		public override double WeaponAbilityChance => 0.5;
		
		[Constructable]
		public EliteNelderimGuard() : base(GuardType.EliteGuard, rangePerception: 18)
		{
			SetWeaponAbility(WeaponAbility.WhirlwindAttack);
			SetWeaponAbility(WeaponAbility.BleedAttack);
			PackGold(50, 100);
		}

		public EliteNelderimGuard(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	[CorpseName("zwloki straznika")]
	public class SpecialNelderimGuard : BaseNelderimGuard
	{
		public override double WeaponAbilityChance => 1.0;

		[Constructable]
		public SpecialNelderimGuard() : base(GuardType.SpecialGuard, rangePerception: 20)
		{
			PackGold(60, 100);
			SetWeaponAbility(WeaponAbility.Disarm);
			SetWeaponAbility(WeaponAbility.BleedAttack);
		}

		public SpecialNelderimGuard(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}

