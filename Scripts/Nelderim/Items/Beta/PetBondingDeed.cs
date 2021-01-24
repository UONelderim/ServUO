using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
    public class PetBondingDeed : Item // Create the item class which is derived from the base item class 
    {
        [Constructable]
        public PetBondingDeed() : base( 0x14F0 )
        {
            Weight = 1.0;
            Name = "zwoj oswajacza";
            LootType = LootType.Blessed;
            Hue = 572;
        }

        public PetBondingDeed( Serial serial ) : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int)0 ); // version 
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );
            LootType = LootType.Blessed;

            int version = reader.ReadInt();
        }

        public override bool DisplayLootType { get { return false; } }

        public override void OnDoubleClick( Mobile from ) // Override double click of the deed to call our target 
        {
            if ( !IsChildOf( from.Backpack ) ) // Make sure its in their pack 
            {
                from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it. 
            }
            else
            {
                from.SendMessage( "Choose the pet you wish to bond with." );
                from.Target = new BondingDeedTarget( this ); // Call our target 
            }
        }
    }

    public class BondingDeedTarget : Target
    {
        private readonly PetBondingDeed m_Deed;

        public BondingDeedTarget( PetBondingDeed deed ) : base( 1, false, TargetFlags.None )
        {
            m_Deed = deed;
        }

        protected override void OnTarget( Mobile from, object target )
        {
            if ( m_Deed == null || m_Deed.Deleted || !m_Deed.IsChildOf( from.Backpack ) )
                return;

            if ( target is BaseCreature )
            {
                BaseCreature t = (BaseCreature)target;

                if ( t.IsBonded == true )
                {
                    from.SendLocalizedMessage( 1152925 ); // That pet is already bonded to you.
                }
                else if ( t.ControlMaster != from )
                {
                    from.SendLocalizedMessage( 1114368 ); // This is not your pet!
                }
                else if ( t.Allured || t.Summoned )
                {
                    from.SendLocalizedMessage( 1152924 ); // That is not a valid pet.
                }
                else if ( target is BaseTalismanSummon )
                {
                    from.SendLocalizedMessage( 1152924 ); // That is not a valid pet.
                }
                else
                {
                    t.IsBonded = !t.IsBonded;
                    from.SendLocalizedMessage( 1049666 ); // Your pet has bonded with you!
                    m_Deed.Delete();
                }
            }
            else
            {
                from.SendLocalizedMessage( 1152924 );  // That is not a valid pet.
            }
        }
    }
}
