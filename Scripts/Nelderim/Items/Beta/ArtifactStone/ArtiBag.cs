using System; 
using Server; 
using Server.Items;

namespace Server.Items 
{ 
	public class ArtiBag : Bag 
	{ 
		[Constructable] 
		public ArtiBag() : this( 1 ) 
		{            
                        Name = "worek ze zwojami artefaktow";
                        Hue = 2991;      
		} 

		[Constructable] 
		public ArtiBag( int amount ) 
		{ 
			DropItem( new ArtifactDeed() ); 
			DropItem( new MinorArtifactDeed() ); 
                        DropItem( new ArtifactDeed() ); 
			DropItem( new MinorArtifactDeed() );
                        DropItem( new ArtifactDeed() ); 
			DropItem( new MinorArtifactDeed() ); 
                        DropItem( new ArtifactDeed() ); 
			DropItem( new MinorArtifactDeed() );
                        DropItem( new MinorArtifactDeed() ); 
                        DropItem( new ArtifactDeed() ); 
			DropItem( new MinorArtifactDeed() );
                        DropItem( new ArtifactDeed() ); 
			DropItem( new MinorArtifactDeed() ); 
                        DropItem( new ArtifactDeed() ); 
			DropItem( new MinorArtifactDeed() );  
		} 
                 
		public ArtiBag( Serial serial ) : base( serial ) 
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
