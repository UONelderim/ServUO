using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
    public class SC0Deed : Item
    {
        [Constructable]
        public SC0Deed() : base( 0x14F0 )
        {
            Weight = 1.0;
            Name = "FC 0/1 for SC0";
            Hue = 707;
            LootType = LootType.Blessed;
        } 
        
        public override void OnDoubleClick( Mobile Gracz )
        {
            if ( IsChildOf( Gracz.Backpack ) )
            {
                Gracz.BeginTarget( 6, false, TargetFlags.None, new TargetCallback( OnTarget ) );
                Gracz.SendLocalizedMessage( 502450 );
            }
            else
            {
                Gracz.SendLocalizedMessage( 1042001 );
            }
        }
        
        public void OnTarget( Mobile Gracz, object targeted )
        {
            if ( Deleted )
            {
                return;
            }
            
            if ( ! ( targeted is Item ) ) return;
            
            if ( ! ( IsChildOf( Gracz.Backpack ) ) )
            {
                Gracz.SendMessage( "Target needs to be in your backpack!" );
                return;
            }
            
            Item item = (Item) targeted;
     
            if ( ! ( item is BaseShield || item is BaseWeapon ) ) 
            {
               Gracz.SendMessage( "This item is not suitable!" );
               return;
            } 
            
            else if ( ! ( item.IsChildOf( Gracz.Backpack ) ) )
            {
                Gracz.SendMessage( "Target needs to be in your backpack!" );
                return;
            }
            
            
            else if ( item is BaseWeapon )
            {
                BaseWeapon weapon = (BaseWeapon) item;
                
                if ( weapon.Attributes.CastSpeed > -1 ) 
                {
                    weapon.Attributes.CastSpeed = 0;
                    weapon.Attributes.SpellChanneling = 1;
                    
                    Gracz.SendMessage( "SC0 added to item!" );
                }
                else 
                {
                     Gracz.SendMessage( "No -fc allowed!" );
                 
                }
            }
            
            else 
            {
                BaseShield shield = (BaseShield) item;
                
                if ( shield.Attributes.CastSpeed > -1 )
                {
                    shield.Attributes.CastSpeed = 0;
                    shield.Attributes.SpellChanneling = 1;
                    
                    Gracz.SendMessage( "SC0 added to item!" );
                }
                else 
                {
                     Gracz.SendMessage( "No -fc allowed!" );
                }              
            }
            
        }
        
        
        public SC0Deed( Serial serial ) : base ( serial )
        {
        }
        
        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            
            writer.Write( (int) 0 );
        }
        
        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );
            
            int verison = reader.ReadInt();
        }
        
    }
}