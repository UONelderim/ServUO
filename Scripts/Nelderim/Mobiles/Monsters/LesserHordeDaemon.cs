#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki mniejszego demona hordy")]
	public class LesserHordeDaemon : BaseCreature
	{
		[Constructable]
		public LesserHordeDaemon() : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
		{
			Name = "mniejszy demon hordy";
			Body = 776;
			BaseSoundID = 357;

			SetStr(16, 40);
			SetDex(31, 60);
			SetInt(11, 25);

			SetHits(10, 24);

			SetDamage(5, 10);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 15, 20);
			SetResistance(ResistanceType.Fire, 5, 10);

			SetSkill(SkillName.MagicResist, 10.0);
			SetSkill(SkillName.Tactics, 0.1, 15.0);
			SetSkill(SkillName.Wrestling, 25.1, 40.0);

			Fame = 500;
			Karma = -500;

			VirtualArmor = 18;

			AddItem(new LightSource());


			// TODO: Body parts
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				if (Utility.RandomDouble() < 0.15)
					corpse.DropItem(new DaemonBone());
				if (Utility.RandomDouble() < 0.07)
					corpse.DropItem(new Bloodspawn());
			}

			base.OnCarve(from, corpse, with);
		}

		//public override int Bones { get { return 3; } } //TODO

		public override int GetIdleSound()
		{
			return 338;
		}

		public override int GetAngerSound()
		{
			return 338;
		}

		public override int GetDeathSound()
		{
			return 338;
		}

		public override int GetAttackSound()
		{
			return 406;
		}

		public override int GetHurtSound()
		{
			return 194;
		}

		public LesserHordeDaemon(Serial serial) : base(serial)
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
