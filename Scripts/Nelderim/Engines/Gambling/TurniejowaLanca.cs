using System;
using Server;
using Server.Engines.Craft;
using Server.Network;

namespace Server.Items
{
    [Flipable(0x26C0, 0x26CA)]
    public class TournamentLance : Lance
    {
        private const double BreakChance = 0.40; // 40% szansy na rozpad przy trafieniu

        public override int InitMinHits => 1;
        public override int InitMaxHits => 1;
        public override string DefaultName => "Turniejowa Lanca";

        [Constructable]
        public TournamentLance()
        {
            Hue    = 0x489; // wyróżniający kolor
            Weight = 12.0;

            // +120 do Fencing, gdy przedmiot jest w ekwipunku
            SkillBonuses.SetValues(0, SkillName.Fencing, 120);
        }

        public TournamentLance(Serial serial) : base(serial)
        {
        }

        // zachowujemy zdolności z klasy Lance
        public override WeaponAbility PrimaryAbility   => WeaponAbility.Dismount;
        public override WeaponAbility SecondaryAbility => WeaponAbility.ConcussionBlow;

        public override int StrengthReq => 95;
        public override int MinDamage   => 18;
        public override int MaxDamage   => 22;
        public override float Speed     => 4.25f;

        public override int DefHitSound         => 0x23C;
        public override int DefMissSound        => 0x238;
        public override SkillName DefSkill      => SkillName.Fencing;
        public override WeaponType DefType      => WeaponType.Piercing;
        public override WeaponAnimation DefAnimation => WeaponAnimation.Pierce1H;

        // tutaj dodajemy szansę na rozpad przy trafieniu
        public void OnHit(Mobile attacker, Mobile defender)
        {
            base.OnHit(attacker, defender);

            if (Utility.RandomDouble() < BreakChance)
            {
                attacker.SendMessage("Twoja turniejowa lanca się złamała!");
                Delete();
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // przy deserializacji przywracamy bonus do skilla
            SkillBonuses.SetValues(0, SkillName.Fencing, 120);
        }
    }
}
