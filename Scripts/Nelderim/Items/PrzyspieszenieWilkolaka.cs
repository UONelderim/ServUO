using System;
using Server;
using Server.Misc;
using Server.Items;
using Server.Targeting;
using Server.Commands;

namespace Server.Items 
{
    public class PrzyspieszenieWilkolaka : SilverRing
	{ 
		
		[Constructable] 
		public PrzyspieszenieWilkolaka()  
		{ 
			
			Weight = 1.0; 
			Attributes.RegenMana = -3;
            Attributes.RegenHits = -5;
			Hue = 1153; 
			Name = "Przyspieszenie Wilkolaka"; 
			LootType = LootType.Blessed;
            SkillBonuses.SetValues(0, SkillName.Fencing, 30.0);
			Label1 = "dotkniecie piersciena powoduje, iz Twe konczyny pracuja szybciej";
			
			
			
		}

		public override void OnDoubleClick( Mobile m ) 
		{
			string prefix = Server.Commands.CommandSystem.Prefix;

			if ( m.AccessLevel >= AccessLevel.Player)
			{
				CommandSystem.Handle( m, String.Format( "{0}speedboost", prefix ) );	
			}

			else
			{
				m.SendMessage( "You are unable to use that!" );
				this.Delete();
			}
		} 

		public PrzyspieszenieWilkolaka( Serial serial ) : base( serial ) 
		{ 
		} 
       
		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 1 ); // version 
		}

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt(); 
		} 
	}
}