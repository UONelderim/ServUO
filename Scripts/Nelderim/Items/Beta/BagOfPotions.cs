// ID 0000129: dodac explosion potion do bag of potions 

using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class BagOfPotions : Bag
    {
        [Constructable]
        public BagOfPotions() : this ( 1 )
        {
        }
        
        [Constructable]
        public BagOfPotions( int Ilosc ) // Ilosc potion ktore zostana stworzone
        {
            Name = "worek z miksturami";

            for ( int i = 0; i < Ilosc; ++i )
            {
                DropItem( new GreaterStrengthPotion() );
                DropItem( new GreaterHealPotion() );
                DropItem( new GreaterCurePotion() );
                DropItem( new GreaterAgilityPotion() );
                DropItem( new TotalRefreshPotion() );
                // ID 0000129: dodac explosion potion do bag of potions 
                DropItem( new GreaterExplosionPotion() );
            }
            
            for ( int i = 0; i < Ilosc; ++i )
            {
                DropItem( new DeadlyPoisonPotion() );
            }
        } 
        
        public BagOfPotions( Serial serial ) : base ( serial )
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
            
            int version = reader.ReadInt();
        }
    }
}