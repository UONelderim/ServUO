using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Misc;
using Server;

namespace Server.Mobiles
{
    [CorpseName("zainfekowany trup")]
    public class WidowSpawn : GiantBlackWidow
    {
        public override bool CanRegenHits { get { return true; } }
        public override bool IsScaredOfScaryThings { get { return false; } }
        public override bool IsScaryToPets { get { return true; } }

        private Mobile boss;

        [Constructable]
        public WidowSpawn() : base()
        {
            Name = "pomiot wodwy";
            Body = 728;
            BaseSoundID = 471;
            Hue = 1931;

            SetStr(200);
            SetDex(25);
            SetInt(10);

            SetHits(300);
            SetStam(150);
            SetMana(0);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Cold, 25);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 50, 80);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 99, 100);
            SetResistance(ResistanceType.Poison, 99, 100);
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.Poisoning, 120.0);
            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            PassiveSpeed = .4;
            ActiveSpeed = .8;

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 30;
        }

        public Item CloneItem(Item item)
        {
            if ((item.Layer != Layer.Bank) && (item.Layer != Layer.Backpack))
            {
                Item newItem = new Item(item.ItemID);
                newItem.Hue = item.Hue;
                newItem.Layer = item.Layer;

                return newItem;
            }

            return null;
        }

        public virtual void NewSpawn(Mobile from, Mobile widow)
        {
            if ((from is BaseCreature) || (from is BaseCreature && ((BaseCreature)from).Summoned))
            {
                return;
            }
            else
            {
                if (from is PlayerMobile)
                {
                    double dhits, dstr;
                    dhits = from.HitsMax * 1.2;
                    dstr = from.Str * 0.80;

                    Female = from.Female;
                    Body = from.Body;
                    Hue = 1931;
                    Name = from.Name;
                    Title = "- pomiot";

                    if (widow != null)
                        boss = widow;

                    HairItemID = from.HairItemID;
                    HairHue = from.HairHue;
                    FacialHairItemID = from.FacialHairItemID;
                    FacialHairHue = from.FacialHairHue;

                    SetResistance(ResistanceType.Physical, 40, 70);
                    SetResistance(ResistanceType.Fire, 20, 40);
                    SetResistance(ResistanceType.Cold, 99, 100);
                    SetResistance(ResistanceType.Poison, 99, 100);
                    SetResistance(ResistanceType.Energy, 30, 60);

                    for (int i = 0; i < from.Skills.Length; ++i)
                    {
                        Skills[i].Base = from.Skills[i].Base * 0.80;
                        Skills[i].Cap = from.Skills[i].Cap;
                    }

                    HitsMaxSeed = from.HitsMax + (int)dhits;
                    Hits = from.HitsMax + (int)dhits;
                    Str = from.Str + (int)dstr;

                    if (from.Items != null && from.Backpack != null)
                    {
                        List<Item> mobitems = new List<Item>(from.Items);

                        foreach (Item item in mobitems)
                        {
                            if (item.Layer == Layer.Backpack || item.Layer == Layer.Bank || item.Insured || item.LootType == LootType.Blessed || item.LootType == LootType.Newbied)
                                continue;
                            EquipItem(item);
                        }

                        mobitems = new List<Item>(from.Backpack.Items);
                        if (Backpack == null)
                        {
                            Backpack backpack = new Backpack();
                            backpack.Movable = false;
                            AddItem(backpack);
                        }
                        foreach (Item item in mobitems)
                        {
                            if (item.Insured || item.LootType == LootType.Blessed || item.LootType == LootType.Newbied) continue;
                            Backpack.AddItem(item);
                        }

                        if (from.Mounted)
                        {
                            if (from.Mount is EtherealMount)
                            {
                                EtherealMount pet = from.Mount as EtherealMount;
                                pet.Internalize();
                                pet.Rider = null;
                            }
                            else if (from.Mount is BaseMount)
                            {
                                BaseMount petm = from.Mount as BaseMount;
                                petm.Rider = null;
                                BaseCreature pet = petm as BaseCreature;
                                pet.Internalize();
                                pet.ControlTarget = null;
                                pet.SetControlMaster(null);
                                pet.SummonMaster = null;
                                pet.MoveToWorld(Location, Map);
                            }
                        }
                    }

                    MoveToWorld(from.Location, from.Map);
                    if (from is BaseCreature)
                    {
                        if (!((BaseCreature)from).IsBonded)
                        {
                            from.Delete();
                        }
                    }
                }
            }
            return;
        }

        public override bool IsEnemy(Mobile m)
        {
            if (m is PlayerMobile)
            {
                if (m.AccessLevel >= AccessLevel.GameMaster)
                    return false;
                else if (this.Combatant == m)
                    return true;
                else if (m == this.boss)
                    return false;
                else if (m is BaseCreature && ((BaseCreature)m).Controlled && ((BaseCreature)m).ControlMaster == m)
                    return false;
                else if (m is BaseCreature && ((BaseCreature)m).Summoned && ((BaseCreature)m).SummonMaster == m)
                    return false;
                else
                    return true;
            }
            else if (m is BaseCreature && ((((BaseCreature)m).ControlMaster) is PlayerMobile && !IsEnemy(((BaseCreature)m).ControlMaster)))
                return false;
            return true;
        }

        public override void OnThink()
        {
            base.OnThink();

            // Check if the boss is a PlayerMobile and has WidowMorphItems
            if (boss != null && boss is PlayerMobile && boss.FindItemOnLayer(Layer.Arms) is WidowMorphArms && boss.FindItemOnLayer(Layer.InnerTorso) is WidowMorphChest && boss.FindItemOnLayer(Layer.Gloves) is WidowMorphGloves && boss.FindItemOnLayer(Layer.Neck) is WidowMorphGorget && boss.FindItemOnLayer(Layer.Helm) is WidowMorphHelm && boss.FindItemOnLayer(Layer.Pants) is WidowMorphLegs && boss.FindItemOnLayer(Layer.Cloak) is WidowCloak)
            {
                // 3% chance to spawn WidowSpawn
                if (Utility.RandomDouble() < 1.00)
                {
                    WidowSpawn newWidowSpawn = new WidowSpawn();
                    newWidowSpawn.NewSpawn(boss, this); // Set boss and widow references
                    newWidowSpawn.MoveToWorld(boss.Location, boss.Map); // Spawn next to the player
                }
            }

            // Continue with the rest of the OnThink logic
            if (boss != null && boss is PlayerMobile && boss.BodyMod == 84 && boss.Combatant != null && boss.InRange(this, 14) && this.CanSee(boss) && this.InLOS(boss))
                this.Combatant = boss.Combatant;
            else if (boss != null && boss is PlayerMobile && (boss.FindItemOnLayer(Layer.Arms) == null || boss.BodyMod != 84))
            {
                boss = null;
                this.Kill();
            }
        }

        public WidowSpawn(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(boss);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            boss = reader.ReadMobile();

            // Remove timer here to stop decay but don't forget the one Constructable section!
            // m_Timer = new RotTimer(this);
            // m_Timer.Start();
        }
    }
}
