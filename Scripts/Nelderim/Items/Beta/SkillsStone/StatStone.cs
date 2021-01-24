// ID 0000020
// ID 0000162
using System;
using Server.Prompts;


namespace Server.Items
{
    public class StatPrompt : Prompt
    {

        private StatStone pKamien;

        public StatPrompt( StatStone Kamien )
        {
            pKamien = Kamien;
        }

        public static bool IsNumeric( string anyString )
        {
            // ID 0000162
            try
            {
                if ( anyString == null )
                {
                    anyString = "";
                }
                if ( anyString.Length > 0 && anyString.Length < 5 )
                {
                    //double dummyOut = new double();
                    int dummyOut;
                    System.Globalization.CultureInfo cultureInfo = 
                        new System.Globalization.CultureInfo( "en-US", true );

                    return int.TryParse( anyString, System.Globalization.NumberStyles.Any,
                        cultureInfo.NumberFormat, out dummyOut );
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
                
        }

        public override void OnResponse( Mobile Gracz, string text )
        {
                if ( !IsNumeric( text ) )
                {
                    text = "10";
                }
      
            // ID 0000162
            try
            {
                pKamien.Stat = int.Parse( text );
            }
            catch
            {
                pKamien.Stat = 10;
            }
            //
            if ( pKamien.Stat < 10 )
                pKamien.Stat = 10;
            if ( pKamien.Stat > 125 )
                pKamien.Stat = 125;

            pKamien.UstawStat();
        }
    }


    public class StatStone : Item
    {
        public StatStone()
            : base( 0xEDD )
        {
            Movable = false;
            Name = "kamien statystyk";
        }

        public StatStone( Serial serial )
            : base( serial )
        {
        }

        public virtual void UstawStat()
        {
        }

        public int Stat;
        public Mobile Komu;

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );
        }
    }


    public class StrStone : StatStone
    {
        [Constructable]
        public StrStone()
        {
            Movable = false;
            Name = "kamien sily";
        }

        public StrStone( Serial serial )
            : base( serial )
        {
        }

        public override void OnDoubleClick( Mobile Gracz )
        {
            Gracz.SendMessage( "Ile pragniesz miec sily?" );
            Komu = Gracz;
            Gracz.Prompt = new StatPrompt( this );

        }

        public override void UstawStat()
        {
            if ( Komu.RawDex + Komu.RawInt + Stat > 225 )
            {
                Stat = 225 - Komu.RawDex - Komu.RawInt;
            }

            Komu.RawStr = Stat;
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

    public class DexStone : StatStone
    {
        [Constructable]
        public DexStone()
        {
            Movable = false;
            Name = "kamien zrecznosci";
        }

        public DexStone( Serial serial )
            : base( serial )
        {
        }

        public override void OnDoubleClick( Mobile Gracz )
        {
            Gracz.SendMessage( "Ile pragniesz miec zrecznosci?" );
            Komu = Gracz;
            Gracz.Prompt = new StatPrompt( this );

        }

        public override void UstawStat()
        {
            if ( Komu.RawStr + Komu.RawInt + Stat > 225 )
            {
                Stat = 225 - Komu.RawStr - Komu.RawInt;
            }
            Komu.RawDex = Stat;
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

    public class IntStone : StatStone
    {
        [Constructable]
        public IntStone()
        {
            Movable = false;
            Name = "kamien inteligencji";
        }

        public IntStone( Serial serial )
            : base( serial )
        {
        }

        public override void OnDoubleClick( Mobile Gracz )
        {
            Gracz.SendMessage( "Ile pragniesz miec inteligencji?" );
            Komu = Gracz;
            Gracz.Prompt = new StatPrompt( this );

        }

        public override void UstawStat()
        {
            if ( Komu.RawDex + Komu.RawStr + Stat > 225 )
            {
                Stat = 225 - Komu.RawDex - Komu.RawStr;
            }
            Komu.RawInt = Stat;
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