using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("resztki zardzewialego konstruktu")]
    public class RustMonster : BaseCreature
    {
        [Constructable]
        public RustMonster() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Zardzewialy Konstrukt";
            Body = 752;
            BaseSoundID = 755;

            SetStr(200, 300);
            SetDex(100, 160);
            SetInt(50, 80);

            SetHits(230, 290);

            SetDamage(3, 6);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 45, 55);
            SetResistance(ResistanceType.Cold, 45, 55);
            SetResistance(ResistanceType.Poison, 45, 55);
            SetResistance(ResistanceType.Energy, 45, 55);

            SetSkill(SkillName.MagicResist, 50.3, 80.0);
            SetSkill(SkillName.Tactics, 120.1, 160.0);
            SetSkill(SkillName.Wrestling, 125.1, 160.0);
        }
        public override int Meat => 1;
        public override int Hides => 8;

        public RustMonster(Serial serial) : base(serial)
        {
        }

        public void DoSpecialAbility(Mobile target)
        {
            PlayerMobile pm = target as PlayerMobile;
            if (pm == null) return;
            
            IDurability randomItem = Utility.RandomList(pm.Items) as IDurability;
            if (randomItem == null) return;
            
            CraftResource itemMaterial;
            if (randomItem is BaseArmor)
                itemMaterial = ((BaseArmor)randomItem).Resource;
            else if (randomItem is BaseWeapon)
                itemMaterial = ((BaseWeapon)randomItem).Resource;
            else
                return;

            if (itemMaterial >= CraftResource.Iron && itemMaterial <= CraftResource.Platinum &&
                randomItem.HitPoints >= 21)
            {
                randomItem.HitPoints -= 20;
                target.SendMessage("Jeden z twoich przedmiotow mocno pordzewial");
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            DoSpecialAbility(defender);
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
