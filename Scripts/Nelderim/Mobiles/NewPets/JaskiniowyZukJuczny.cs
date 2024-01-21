using System.Collections.Generic;
using Server.Items;
using Server.ContextMenus;

namespace Server.Mobiles
{
    [CorpseName("zwloki jaskiniowego zuka jucznego")]
    public class JaskiniowyZukJuczny : BaseCreature
    {
        [Constructable]
        public JaskiniowyZukJuczny() : base(AIType.AI_Animal, FightMode.Aggressor, 12, 1, 0.175, 0.35)
        {
            Name = "jaskiniowy zuk juczny";
            Body = 0x317;

            SetStr(44, 120);
            SetDex(36, 55);
            SetInt(6, 10);

            SetHits(61, 80);
            SetStam(81, 100);
            SetMana(0);

            SetDamage(5, 11);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 20, 25);
            SetResistance(ResistanceType.Fire, 10, 15);
            SetResistance(ResistanceType.Cold, 20, 25);
            SetResistance(ResistanceType.Poison, 10, 15);
            SetResistance(ResistanceType.Energy, 10, 15);

            SetSkill(SkillName.MagicResist, 25.1, 30.0);
            SetSkill(SkillName.Tactics, 29.3, 44.0);
            SetSkill(SkillName.Wrestling, 29.3, 44.0);

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 11.1;

            Container pack = Backpack;

            if (pack != null)
                pack.Delete();

            pack = new StrongBackpack();
            pack.Movable = false;

            AddItem(pack);
        }

        public override int GetAngerSound() => 0x21D;
        public override int GetIdleSound() => 0x21D;
        public override int GetAttackSound() => 0x162;
        public override int GetHurtSound() => 0x163;
        public override int GetDeathSound() => 0x21D;

        public override int Meat => 3;
        public override int Hides => 4;
        public override FoodType FavoriteFood => FoodType.Meat;

        public override void GetProperties(ObjectPropertyList list) // bedzie wyswietlac wage w zwierzach z paczka
        {
            base.GetProperties(list);

            if (Backpack != null)
            {
                list.Add(1072241, "{0}\t{1}\t{2}\t{3}", Backpack.TotalItems, Backpack.MaxItems, Backpack.TotalWeight, Backpack.MaxWeight); // Contents: ~1_COUNT~/~2_MAXCOUNT~ items, ~3_WEIGHT~/~4_MAXWEIGHT~ stones
            }
        }

        public override void OnWeightChange(int oldValue)
        {
            InvalidateProperties();
        }
        public JaskiniowyZukJuczny(Serial serial) : base(serial)
        {
        }

        public override DeathMoveResult GetInventoryMoveResultFor(Item item)
        {
            return DeathMoveResult.MoveToCorpse;
        }

        public override bool IsSnoop(Mobile from)
        {
            if (PackAnimal.CheckAccess(this, from))
                return false;

            return base.IsSnoop(from);
        }

        public override bool OnDragDrop(Mobile from, Item item)
        {
            if (CheckFeed(from, item))
                return true;

            if (PackAnimal.CheckAccess(this, from))
            {
                AddToBackpack(item);
                return true;
            }

            return base.OnDragDrop(from, item);
        }

        public override bool CheckNonlocalDrop(Mobile from, Item item, Item target)
        {
            return PackAnimal.CheckAccess(this, from);
        }

        public override bool CheckNonlocalLift(Mobile from, Item item)
        {
            return PackAnimal.CheckAccess(this, from);
        }

        public override void OnDoubleClick(Mobile from)
        {
            PackAnimal.TryPackOpen(this, from);
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            PackAnimal.GetContextMenuEntries(this, from, list);
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
