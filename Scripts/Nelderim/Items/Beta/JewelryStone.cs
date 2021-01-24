
namespace Server.Items
{
    public class JewelryStone : Item
    {
        private int minProperties = 4;
        private int maxProperties = 5;

        private int minIntensity = 80;
        private int maxIntensity = 100;

        private int cost = 0;
        private bool ring = true;

        public int NumberOfProperties = 25; // Ilosc propsow ktore sa mozliwe do wylosowania 

        #region [props
        [CommandProperty( AccessLevel.GameMaster )]
        public int MinProperties
        {
            get
            {
                return minProperties;
            }
            set
            {
                minProperties=(value <= NumberOfProperties ? value : NumberOfProperties);
                if ( minProperties > maxProperties )
                    MaxProperties = minProperties;
            }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int MaxProperties
        {
            get
            {
                return maxProperties;
            }
            set
            {
                maxProperties=(value <= NumberOfProperties ? value : NumberOfProperties);
                if ( maxProperties < minProperties )
                    MinProperties = maxProperties;
            }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int MinIntensity
        {
            get
            {
                return minIntensity;
            }
            set
            {
                minIntensity=value;
            }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int MaxIntensity
        {
            get
            {
                return maxIntensity;
            }
            set
            {
                maxIntensity=value;
            }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int Cost
        {
            get
            {
                return cost;
            }
            set
            {
                cost=value;
            }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public bool Ring
        {
            get
            {
                return ring;
            }
            set
            {
                ring=value;
            }
        }
        #endregion

        [Constructable]
        public JewelryStone()
            : base( 3796 )
        {
            Name = "kamien bizuterii";
            Movable = false;
        }

        public JewelryStone( Serial serial )
            : base( serial )
        {
        }

        public override void OnDoubleClick( Mobile from )
        {
            Container b = from.Backpack;
            if ( b == null )
                return;

            BaseJewel jewel = this.CreateJewel();

            b.DropItem( jewel );
            from.SendMessage( "Bizuteria znalazla sie w Twoim plecaku" );
        }

        public BaseJewel CreateJewel()
        {
            BaseJewel jewel= ( ring ? (BaseJewel)( new GoldRing()) : (BaseJewel)(new GoldBracelet()) );
            int props = Utility.Random( this.maxProperties - this.minProperties ) + this.minProperties;

            BaseRunicTool.ApplyAttributesTo( jewel, props, this.minIntensity, this.maxIntensity );

            return jewel;
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int) 3 );

            // 3
            writer.Write( ring );
            // 2
            writer.Write( cost );
            // 1
            writer.Write( minIntensity );
            writer.Write( maxIntensity );
            writer.Write( minProperties );
            writer.Write( maxProperties );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();

            switch ( version )
            {
                case 3:
                    {
                        ring = reader.ReadBool();
                        goto case 2;
                    }
                case 2:
                    {
                        cost = reader.ReadInt();
                        goto case 1;
                    }
                case 1:
                    {
                        minIntensity = reader.ReadInt();
                        maxIntensity = reader.ReadInt();
                        minProperties = reader.ReadInt();
                        maxProperties = reader.ReadInt();
                        goto case 0;
                    }
                case 0:
                    {
                        break;
                    }
            }
        }

    }
}
