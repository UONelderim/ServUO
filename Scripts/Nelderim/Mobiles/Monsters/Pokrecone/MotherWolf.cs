// Wolf Family
// a RunUO ver 2.0 Script
// Written by David 
// last edited 6/17/06

using System;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("zwloki wilka")]
    public class MotherWolf : BaseCreature
    {
        private List<WolfPup> _pups;
        private int _maxPups = Utility.RandomMinMax(2, 5);

        public override int Meat => 1;
        public override int Hides => 3;
        public override FoodType FavoriteFood => FoodType.Meat;
        public override PackInstinct PackInstinct => PackInstinct.Canine;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool RespawnPups
        {
            get => false;
            set
            {
                if (value) SpawnBabies();
            }
        }

        [Constructable]
        public MotherWolf() : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.1, 0.3)
        {
            Name = "wilcza matka";
            Body = 25;
            BaseSoundID = 0xE5;

            SetStr(91, 110);
            SetDex(76, 95);
            SetInt(31, 50);

            SetHits(42, 68);
            SetMana(0);

            SetDamage(11, 21);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Fire, 1, 10);
            SetResistance(ResistanceType.Cold, 20, 25);
            SetResistance(ResistanceType.Poison, 10, 15);
            SetResistance(ResistanceType.Energy, 10, 15);

            SetSkill(SkillName.MagicResist, 30.6, 45.0);
            SetSkill(SkillName.Tactics, 50.1, 70.0);
            SetSkill(SkillName.Wrestling, 60.1, 75.0);

            Tamable = false;

            _pups = new List<WolfPup>();
            new WolfFamilyTimer(this).Start();
        }

        public override bool OnBeforeDeath()
        {
            foreach (WolfPup pup in _pups)
            {
                if (pup.Alive && pup.ControlMaster != null)
                    pup.Kill();
            }

            return base.OnBeforeDeath();
        }

        public void SpawnBabies()
        {
            ClearPups();
            if (Map == null)
                return;
            
            if (_pups.Count >= _maxPups)
                return;
            

            while (_pups.Count < _maxPups)
            {
                WolfPup pup = new WolfPup();

                Point3D loc = Location;

                for (int j = 0; j < 10; ++j)
                {
                    int x = X + Utility.Random(5) - 1;
                    int y = Y + Utility.Random(5) - 1;
                    int z = Map.GetAverageZ(x, y);

                    if (Map.CanFit(x, y, Z, 16, false, false))
                    {
                        loc = new Point3D(x, y, Z);
                        break;
                    }
                    if (Map.CanFit(x, y, z, 16, false, false))
                    {
                        loc = new Point3D(x, y, z);
                        break;
                    }
                }

                pup.Mother = this;
                pup.Team = Team;
                pup.Home = Location;
                pup.RangeHome = Math.Max((RangeHome + 1) / 2, 4);

                pup.MoveToWorld(loc, Map);
                _pups.Add(pup);
            }
        }

        protected override void OnLocationChange(Point3D oldLocation)
        {
            foreach (WolfPup pup in _pups)
            {
                if (pup.Alive && pup.ControlMaster == null)
                {
                    pup.Home = Location;
                }
            }
            
            base.OnLocationChange(oldLocation);
        }

        public void ClearPups()
        {
            List<WolfPup> toRemove = new List<WolfPup>();
            foreach (var pup in _pups)
            {
                if (pup == null || !pup.Alive || pup.Deleted)
                {
                    toRemove.Add(pup);
                }
                else if (pup.Controlled || pup.IsStabled)
                {
                    pup.Mother = null;
                    toRemove.Add(pup);
                }
            }
            foreach (var wolfPup in toRemove)
            {
                _pups.Remove(wolfPup);
            }
        }

        public override void OnDelete()
        {
            ClearPups();

            foreach (WolfPup m in _pups)
            {
                if (m.Alive && m.ControlMaster == null)
                    m.Delete();
            }

            base.OnDelete();
        }

        public MotherWolf(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.WriteMobileList(_pups, true);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            List<Mobile> mobiles = reader.ReadStrongMobileList();
            _pups = new List<WolfPup>(mobiles.Count);
            foreach (var m in mobiles)
            {
                WolfPup pup = m as WolfPup;
                if (pup != null)
                {
                    _pups.Add(pup);
                }
            }
            
            new WolfFamilyTimer(this).Start();
        }
    }

    [CorpseName("a young wolf corpse")]
    public class WolfPup : BaseCreature
    {
        public override int Meat => 1;

        public override FoodType FavoriteFood => FoodType.Meat;

        public override PackInstinct PackInstinct => PackInstinct.Canine;

        private MotherWolf m_mommy;

        [CommandProperty(AccessLevel.GameMaster)]
        public MotherWolf Mother
        {
            get => m_mommy;
            set => m_mommy = value;
        }

        [Constructable]
        public WolfPup() : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "mlody wilk";
            Body = 0xD9;
            BaseSoundID = 0xE5;

            SetStr(37, 47);
            SetDex(38, 53);
            SetInt(39, 47);

            SetHits(17, 42);
            SetMana(0);

            SetDamage(4, 7);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 15);

            SetSkill(SkillName.MagicResist, 22.1, 47.0);
            SetSkill(SkillName.Tactics, 19.2, 31.0);
            SetSkill(SkillName.Wrestling, 19.2, 46.0);

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 53.1;
        }

        public override void OnCombatantChange()
        {
            if (Combatant != null && Combatant.Alive && m_mommy != null && m_mommy.Combatant == null)
                m_mommy.Combatant = Combatant;
        }

        public WolfPup(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_mommy);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_mommy = (MotherWolf)reader.ReadMobile();
        }
    }

    public class WolfFamilyTimer : Timer
    {
        private MotherWolf m_from;

        public WolfFamilyTimer(MotherWolf from) : base(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(20))
        {
            Priority = TimerPriority.OneMinute;
            m_from = from;
        }

        protected override void OnTick()
        {
            if (m_from != null && m_from.Alive)
                m_from.SpawnBabies();
            else
                Stop();
        }
    }
}
