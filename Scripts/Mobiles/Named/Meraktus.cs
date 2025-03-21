using Server.Engines.CannedEvil;
using Server.Items;
using System;
using System.Collections;

namespace Server.Mobiles
{
    [CorpseName("zwloki Meraktusa")]
    public class Meraktus : BaseChampion
    {
        public override ChampionSkullType SkullType => ChampionSkullType.None;
        public override Type[] UniqueList => [];
        public override Type[] SharedList => [];
        public override Type[] DecorativeList => Type.EmptyTypes;
        public override MonsterStatuetteType[] StatueTypes => [];
        [Constructable]
        public Meraktus()
            : base(AIType.AI_Melee)
        {
            Name = "Meraktus";
            Title = "Umeczony";
            Body = 263;
            BaseSoundID = 680;
            Hue = 0x835;

            SetStr(1419, 1438);
            SetDex(309, 413);
            SetInt(129, 131);

            SetHits(4100, 4200);

            SetDamage(16, 30);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 65, 90);
            SetResistance(ResistanceType.Fire, 65, 70);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 40, 60);
            SetResistance(ResistanceType.Energy, 50, 55);

            SetSkill(SkillName.Anatomy, 0);
            SetSkill(SkillName.MagicResist, 107.0, 111.3);
            SetSkill(SkillName.Tactics, 107.0, 117.0);
            SetSkill(SkillName.Wrestling, 100.0, 105.0);

            Fame = 70000;
            Karma = -70000;

            NoKillAwards = true;

            Timer.DelayCall(TimeSpan.FromSeconds(1), SpawnTormented);

            SetWeaponAbility(WeaponAbility.Dismount);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.SuperBoss, 5);
            AddLoot(LootPack.PeerlessResource, 2);
            AddLoot(LootPack.LootItem<MalletAndChisel>()); 
            //AddLoot(LootPack.RandomLootItem(new Type[] { typeof(MinotaurHedge), typeof(BonePile), typeof(LightYarn) }, 100.0, 1, false, true));
            //AddLoot(LootPack.LootItem<TormentedChains>(50.0, 1));
            //AddLoot(LootPack.LootItem<CrimsonCincture>(2.25, 1));
            //AddLoot(LootPack.ArcanistScrolls, 1);
            //AddLoot(NelderimLoot.AncientScrolls);
            
            // 5% szansy na drop talizmanu
            if (Utility.RandomDouble() < 0.05)
            {
	            AddLoot(LootPack.Talisman, 1);
            }
        }

        public override int GetAngerSound()
        {
            return 0x597;
        }

        public override int GetIdleSound()
        {
            return 0x596;
        }

        public override int GetAttackSound()
        {
            return 0x599;
        }

        public override int GetHurtSound()
        {
            return 0x59a;
        }

        public override int GetDeathSound()
        {
            return 0x59c;
        }

        public override int Meat => 2;
        public override int Hides => 10;
        public override HideType HideType => HideType.Regular;
        public override Poison PoisonImmune => Poison.Regular;
        public override int TreasureMapLevel => 3;
        public override bool BardImmune => true;
        public override bool Unprovokable => true;
        public override bool Uncalmable => true;
        public override bool AllureImmune => true;

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);
            if (0.2 >= Utility.RandomDouble())
                Earthquake();
        }

        public void Earthquake()
        {
            Map map = Map;
            if (map == null)
                return;
            ArrayList targets = new ArrayList();
            IPooledEnumerable eable = GetMobilesInRange(8);
            foreach (Mobile m in eable)
            {
                if (m == this || !CanBeHarmful(m))
                    continue;
                if (m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != Team))
                    targets.Add(m);
                else if (m.Player)
                    targets.Add(m);
            }
            eable.Free();
            PlaySound(0x2F3);
            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                if (m == null || m.Deleted)
                    continue;

                if (m is PlayerMobile pm && pm.Mounted)
                {
                    pm.SetMountBlock(BlockMountType.DismountRecovery, TimeSpan.FromSeconds(10), true);
                }

                double damage = m.Hits * 0.6;//was .6

                if (damage < 10.0)
                    damage = 10.0;
                else if (damage > 75.0)
                    damage = 75.0;

                DoHarmful(m);

                AOS.Damage(m, this, (int)damage, 100, 0, 0, 0, 0);

                if (m.Alive && m.Body.IsHuman && !m.Mounted)
                    m.Animate(20, 7, 1, true, false, 0); // take hit
            }
        }

        public Meraktus(Serial serial)
            : base(serial)
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

        #region SpawnHelpers
        public void SpawnTormented()
        {
            BaseCreature spawna = new TormentedMinotaur();
            spawna.MoveToWorld(Location, Map);

            BaseCreature spawnb = new TormentedMinotaur();
            spawnb.MoveToWorld(Location, Map);

            BaseCreature spawnc = new TormentedMinotaur();
            spawnc.MoveToWorld(Location, Map);

            BaseCreature spawnd = new TormentedMinotaur();
            spawnd.MoveToWorld(Location, Map);
        }
        #endregion
    }
}
