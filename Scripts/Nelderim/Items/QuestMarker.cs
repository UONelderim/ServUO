using System;

namespace Server.Items
{
    class InternalSyncTimer : Timer
    {
        private BaseQuestMarker m_Marker;
        public InternalSyncTimer(BaseQuestMarker marker) : base(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1))
        {
            m_Marker = marker;
        }

        protected override void OnTick()
        {
            if(m_Marker != null)
                m_Marker.SyncLocation();
        }
    }
    
    public class BaseQuestMarker : Item
    {
        private Mobile m_LinkedMobile;
        
        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile LinkedMobile
        {
            get => m_LinkedMobile;
            set
            {
                m_LinkedMobile = value; 
                SyncLocation();
            }
        }

        public override bool HandlesOnMovement => true;

        public BaseQuestMarker( int itemId ) : base( itemId )
        {
            Movable = false;
            new InternalSyncTimer(this).Start();
        }
        
        public void SyncLocation()
        {
	        if (LinkedMobile == null || LinkedMobile.Location == Location)
	        {
		        return;
	        }

	        Point3D newLocation = LinkedMobile.Location;
	        newLocation.Z += 8;
	        MoveToWorld(newLocation);
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);
            
            if(LinkedMobile == m)
                SyncLocation();
        }
        

        public BaseQuestMarker( Serial serial ) : base( serial )
        {
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
        }
        
        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int) 0 ); // version
            
            writer.Write(LinkedMobile);
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();

            LinkedMobile = reader.ReadMobile();
            
            new InternalSyncTimer(this).Start();
        }
    }
    
    public class QuestMarker1 : BaseQuestMarker
    {
        [Constructable]
        public QuestMarker1( ) : base( 0x3FE5 )
        {
        }

        public QuestMarker1( Serial serial ) : base( serial )
        {
        }
        
        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int) 0 ); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
        }
    }

    public class QuestMarker2 : BaseQuestMarker
    {
        [Constructable]
        public QuestMarker2( ) : base( 0x3FE8 )
        {
        }

        public QuestMarker2( Serial serial ) : base( serial )
        {
        }
        
        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int) 0 ); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
        }
    }
}
