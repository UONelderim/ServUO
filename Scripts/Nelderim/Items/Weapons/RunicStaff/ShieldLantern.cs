using System;
using Server;

namespace Server.Items
{
    public class ShieldLantern : BaseShield
    {
        public override bool CanEquip(Mobile from)
        {
            if (!base.CanEquip(from))
                return false;

            if (from.Skills.Magery.Value < 70.0)
            {
                from.SendMessage("Twoja wiedza o magii jest zbyt mala, aby uzyc tego przedmiotu.");
                return false;
            }

            return true;
        }

        public override int BasePhysicalResistance { get { return 0; } }
        public override int BaseFireResistance { get { return 0; } }
        public override int BaseColdResistance { get { return 0; } }
        public override int BasePoisonResistance { get { return 0; } }
        public override int BaseEnergyResistance { get { return 0; } }
        public override int InitMinHits { get { return 40; } }
        public override int InitMaxHits { get { return 50; } }
        public override int StrReq => 20;
        
      ///  public override int ArmorBase => 7; // wykomentowalem bo nie wiem co to - TO CHECK

        private BaseShield m_ComponentShield;
        [CommandProperty(AccessLevel.GameMaster)]
        public BaseShield ComponentShield
        {
            get { return m_ComponentShield; }
            set
            {
                m_ComponentShield = value;
            }
        }

        [Constructable]
        public ShieldLantern() : base(0xA25)
        {
            Name = "Latarnia maga";
        }

        public override void OnDelete()
        {
            base.OnDelete();

            if (m_ComponentShield != null)
                m_ComponentShield.Delete();
        }

        public ShieldLantern(Serial serial) : base(serial)
        {
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_ComponentShield = (BaseShield) reader.ReadItem();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);//version

            writer.Write((BaseShield)m_ComponentShield);
        }
    }
}
