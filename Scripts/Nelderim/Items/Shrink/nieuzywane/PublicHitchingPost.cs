//// 06.01.19 :: troyan :: lokalizacja
//// 07.10.29 :: emfor :: mozliwosc wylaczenia

//using System;
//using Server;
//using Server.Targeting;

//namespace Server.Items
//{
//    [Flipable( 0x14E8, 0x14E7 )]
//    public class HitchingPost : AddonComponent
//    {
//    private static bool ShrinkEnabled = true; // 07.10.29 :: emfor
	
//        #region Constructors
//        [Constructable]
//        public HitchingPost() : this( 0x14E7 )
//        {
//        }

//        [Constructable]
//        public HitchingPost( int itemID ) : base( itemID )
//        {
//        }
		
//        public HitchingPost( Serial serial ) : base( serial )
//        {
//        }
//        #endregion
    
//    // 07.10.29 :: emfor :: start
//        public override void OnDoubleClick( Mobile from )
//        {
//            if( ShrinkEnabled )
//            {
//        if( from.InRange( this.GetWorldLocation(), 2 ) == false )
//        {
//          from.SendLocalizedMessage( 500486 ); //That is too far away.
//        }
//        else
//        {
//          from.Target=new HitchingPostTarget( this );
//          from.SendLocalizedMessage( 505663 ); // Wybierz cel.
//        }
//       }
//       else
//       {
//        from.SendMessage(0x21, "Niestety nie ma w poblizu nikogo, kto moglby przypilnowac Twych zwierzat.");
//       }
//        }
//    // 07.10.29 :: emfor :: end

//        private class HitchingPostTarget : Target
//        {
//            private HitchingPost m_Post;

//            public HitchingPostTarget( Item i ) : base( 3, false, TargetFlags.None )
//            {
//                m_Post=(HitchingPost)i;
//            }
			
//            protected override void OnTarget( Mobile from, object targ )
//            {
//                if ( !(m_Post.Deleted) )
//                {
//                    ShrinkFunctions.Shrink( from, targ );
//                }

//                return;
//            }
//        }
        
//        #region Serialization
		
//        public override void Serialize( GenericWriter writer )
//        {
//            base.Serialize( writer );

//            writer.Write( (int) 0 ); // version
//        }

//        public override void Deserialize( GenericReader reader )
//        {
//            base.Deserialize( reader );

//            int version = reader.ReadInt();
//        } 
		
//        #endregion
//    }

//    public class HitchingPostEastAddon : BaseAddon
//    {
//        public override BaseAddonDeed Deed{ get{ return new HitchingPostEastDeed(); } }

//        [Constructable]
//        public HitchingPostEastAddon()
//        {
//            AddComponent( new HitchingPost( 0x14E7 ), 0, 0, 0);
//        }

//        public HitchingPostEastAddon( Serial serial ) : base( serial )
//        {
//        }

//        #region Serialization
//        public override void Serialize( GenericWriter writer )
//        {
//            base.Serialize( writer );

//            writer.Write( (int) 0 ); // version
//        }

//        public override void Deserialize( GenericReader reader )
//        {
//            base.Deserialize( reader );

//            int version = reader.ReadInt();
//        } 
//        #endregion
//    }

//    public class HitchingPostEastDeed : BaseAddonDeed
//    {
//        public override BaseAddon Addon{ get{ return new HitchingPostEastAddon(); } }

//        [Constructable]
//        public HitchingPostEastDeed()
//        {
//            Name="Pacholek (E)";
//        }

//        public HitchingPostEastDeed( Serial serial ) : base( serial )
//        {
//        }

//        #region Serialization
//        public override void Serialize( GenericWriter writer )
//        {
//            base.Serialize( writer );

//            writer.Write( (int) 0 ); // version
//        }

//        public override void Deserialize( GenericReader reader )
//        {
//            base.Deserialize( reader );

//            int version = reader.ReadInt();
//        } 
//        #endregion
//    }

//    public class HitchingPostSouthAddon : BaseAddon
//    {
//        public override BaseAddonDeed Deed{ get{ return new HitchingPostSouthDeed(); } }

//        [Constructable]
//        public HitchingPostSouthAddon()
//        {
//            AddComponent( new HitchingPost( 0x14E8 ), 0, 0, 0);
//        }

//        public HitchingPostSouthAddon( Serial serial ) : base( serial )
//        {
//        }

//        #region Serialization
//        public override void Serialize( GenericWriter writer )
//        {
//            base.Serialize( writer );

//            writer.Write( (int) 0 ); // version
//        }

//        public override void Deserialize( GenericReader reader )
//        {
//            base.Deserialize( reader );

//            int version = reader.ReadInt();
//        } 
//        #endregion
//    }

//    public class HitchingPostSouthDeed : BaseAddonDeed
//    {
//        public override BaseAddon Addon{ get{ return new HitchingPostSouthAddon(); } }

//        [Constructable]
//        public HitchingPostSouthDeed()
//        {
//            Name="Pacholek (S)";
//        }

//        public HitchingPostSouthDeed( Serial serial ) : base( serial )
//        {
//        }

//        #region Serialization
//        public override void Serialize( GenericWriter writer )
//        {
//            base.Serialize( writer );

//            writer.Write( (int) 0 ); // version
//        }

//        public override void Deserialize( GenericReader reader )
//        {
//            base.Deserialize( reader );

//            int version = reader.ReadInt();
//        } 
//        #endregion
//    }

//}
