#region References

using Nelderim;
using Server.Items;
using System;

#endregion

namespace Server.Mobiles
{
	[CorpseName("resztki krwawego koscianego smoka")]
	public class NSkeletalDragon : BasePeerless
	{
		public override bool BardImmune => true;
		public override double AttackMasterChance => 0.15;
		public override double SwitchTargetChance => 0.15;
		public override bool BleedImmune => true;
		public override double DispelDifficulty => 135.0;
		public override double DispelFocus => 45.0;
		public override bool AutoDispel => true;
		public override Poison PoisonImmune => Poison.Lethal;
		public override bool Unprovokable => true;
		public override int Meat => 19; // where's it hiding these? :)
		public override int Hides => 20;
		public override HideType HideType => HideType.Barbed;

		[Constructable]
		public NSkeletalDragon() : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.25, 0.5)
		{
			Name = "krwawy kosciany smok";
			Body = 104;
			Hue = 1570;
			BaseSoundID = 0x488;

			SetStr(898, 1030);
			SetDex(130, 150);
			SetInt(488, 620);

			SetHits(15000);

			SetDamage(29, 35);

			SetDamageType(ResistanceType.Physical, 75);
			SetDamageType(ResistanceType.Fire, 25);

			SetResistance(ResistanceType.Physical, 75, 80);
			SetResistance(ResistanceType.Fire, 40, 60);
			SetResistance(ResistanceType.Cold, 40, 60);
			SetResistance(ResistanceType.Poison, 70, 80);
			SetResistance(ResistanceType.Energy, 40, 60);

			SetSkill(SkillName.EvalInt, 80.1, 100.0);
			SetSkill(SkillName.Magery, 80.1, 100.0);
			SetSkill(SkillName.MagicResist, 100.3, 130.0);
			SetSkill(SkillName.Tactics, 97.6, 100.0);
			SetSkill(SkillName.Wrestling, 97.6, 100.0);

			Fame = 22500;
			Karma = -22500;

			VirtualArmor = 80;
			AddItem(new LightSource());

			SetWeaponAbility(WeaponAbility.BleedAttack);
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				if (Utility.RandomDouble() < 0.40)
					corpse.DropItem(new Pumice());
			}

			base.OnCarve(from, corpse, with);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.SuperBoss);
			
			AddLoot(NelderimLoot.AvatarScrolls);
		}
		
		public override void OnDeath(Container c)
		{
			base.OnDeath(c); 
			
			Point3D moongateLocation = new Point3D(6044, 279, 11);
			Map targetMap = Map.Felucca; 
			
			Point3D destinationLocation = new Point3D(6036, 232, 8);
			Map destinationMap = Map.Felucca;

			Moongate portal = new Moongate(destinationLocation, destinationMap)
			{
				Name = "Portal do wyjścia",
				Hue = 32,
				Dispellable = false,
				ItemID = 0x1FD4,
			};
			
			portal.MoveToWorld(moongateLocation, targetMap);
			
			Timer.DelayCall(TimeSpan.FromMinutes(10), () =>
			{
				if (portal != null && !portal.Deleted)
					portal.Delete();
			});
		}

		public NSkeletalDragon(Serial serial) : base(serial)
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
