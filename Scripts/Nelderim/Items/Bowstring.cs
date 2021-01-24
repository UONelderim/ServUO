using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Items
{
    public enum BowstringType
    {
        Leather,
        Cannabis,
        Silk,
        Gut
    }

    public abstract class BaseBowstring : Item
    {
        public BaseBowstring(int amount) : base(0x1535) // 0x1535 0x379F 0x1420 
        {
            Stackable = true;
            Amount = amount;
            Weight = 0.1;
        }
        
        public BaseBowstring() : this(1)
        {
        }

        public BaseBowstring( Serial serial ) : base( serial )
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



    public class BowstringLeather : BaseBowstring
    {
        [Constructable]
        public BowstringLeather() : this(1)
        {
        }
            
        [Constructable]
        public BowstringLeather(int amount) : base(amount)
        {
            Hue = CraftResources.GetHue(CraftResource.BowstringLeather);
            Name = "Skorzana cieciwa"; // 1032733
        }   

        public BowstringLeather( Serial serial ) : base( serial )
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

    public class BowstringSpinedLeather : BaseBowstring
    {
        [Constructable]
        public BowstringSpinedLeather(int amount) : base(amount)
        {
            Hue = CraftResources.GetHue(CraftResource.BowstringSpined);
            Name = "Cieciwa z niebieskiej skory"; // 1032763
        }  
      
        [Constructable]
        public BowstringSpinedLeather() : this(1)
        {
        }

        public BowstringSpinedLeather( Serial serial ) : base( serial )
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

    public class BowstringHornedLeather : BaseBowstring
    {
        [Constructable]
        public BowstringHornedLeather(int amount)
            : base(amount)
        {
            Hue = CraftResources.GetHue(CraftResource.BowstringHorned);
            Name = "Cieciwa z czerwonej skory"; // 1032764
        }

        [Constructable]
        public BowstringHornedLeather()
            : this(1)
        {
        }

        public BowstringHornedLeather(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class BowstringBarbedLeather : BaseBowstring
    {
        [Constructable]
        public BowstringBarbedLeather(int amount)
            : base(amount)
        {
            Hue = CraftResources.GetHue(CraftResource.BowstringBarbed);
            Name = "Cieciwa z zielonej skory"; // 1032765
        }

        [Constructable]
        public BowstringBarbedLeather()
            : this(1)
        {
        }

        public BowstringBarbedLeather(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class BowstringCannabis : BaseBowstring
    {
        [Constructable]
        public BowstringCannabis(int amount)
            : base(amount)
        {
            Hue = 1164; //762
            Name = "Konopna cieciwa"; // 1032734
        }

        [Constructable]
        public BowstringCannabis()
            : this(1)
        {
        }

        public BowstringCannabis(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
    
    public class BowstringSilk : BaseBowstring
    {
        [Constructable]
        public BowstringSilk(int amount) : base(amount)
        {  
            Hue = 2886;
            Name = "Jedwabna cieciwa"; // 1032735
        }

        [Constructable]
        public BowstringSilk() : this(1)
        {
        }

        public BowstringSilk( Serial serial ) : base( serial )
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


    public class BowstringFiresteed : BaseBowstring
    {
        [Constructable]
        public BowstringFiresteed(int amount) : base(amount)
        {
            Hue = 1359;
            Name = "Cieciwa z wlosia ognistego rumaka";
        }

        [Constructable]
        public BowstringFiresteed() : this(1)
        {
        }

        public BowstringFiresteed( Serial serial ) : base( serial )
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


    public class BowstringUnicorn : BaseBowstring
    {
        [Constructable]
        public BowstringUnicorn(int amount) : base(amount)
        {
            Hue = 1363;
            Name = "Cieciwa z wlosia jednorozca";
        }

        [Constructable]
        public BowstringUnicorn() : this(1)
        {
        }

        public BowstringUnicorn( Serial serial ) : base( serial )
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


    public class BowstringNightmare : BaseBowstring
    {
        [Constructable]
        public BowstringNightmare(int amount) : base(amount)
        {
            Hue = 2065;
            Name = "Cieciwa z wlosia koszmara";
        }

        [Constructable]
        public BowstringNightmare() : this(1)
        {
        }

        public BowstringNightmare( Serial serial ) : base( serial )
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


    public class BowstringKirin : BaseBowstring
    {
        [Constructable]
        public BowstringKirin(int amount) : base(amount)
        {
            Hue = 2517;
            Name = "Cieciwa z wlosia ki-rina";
        }

        [Constructable]
        public BowstringKirin() : this(1)
        {
        }

        public BowstringKirin( Serial serial ) : base( serial )
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


    public class BowstringGut : BaseBowstring
    {
        [Constructable]
        public BowstringGut(int amount) : base(amount)
        {
            Hue = 1141;
            Name = "Jelitowa cieciwa"; // 1032736
        }

        [Constructable]
        public BowstringGut() : this(1)
        {
        }

        public BowstringGut( Serial serial ) : base( serial )
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


    public class Gut : Item
    {
        [Constructable]
        public Gut(int amount) : base(7407)
        {
            Stackable = true;
            Amount = amount;
            Name = "Jelito"; // 1032618
            Weight = 0.2;
        }

        [Constructable]
        public Gut() : this(1)
        {
        }

        public Gut( Serial serial ) : base( serial )
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
