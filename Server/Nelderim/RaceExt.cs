namespace Server
{
    public partial class Race
    {
        public static readonly int NRaceOffset = 0x20;

        public static Race NTamael => m_Races[NRaceOffset + 1];
        public static Race NJarling => m_Races[NRaceOffset + 2];
        public static Race NNaur => m_Races[NRaceOffset + 3];
        public static Race NElf => m_Races[NRaceOffset + 4];
        public static Race NDrow => m_Races[NRaceOffset + 5];
        public static Race NKrasnolud => m_Races[NRaceOffset + 6];

        protected virtual string[] Names => new string[0];
        protected virtual string[] PluralNames => new string[0];
        public virtual int DescNumber => 1072202; // Description
        public virtual int[] SkinHues => new int[0];
        public virtual int[] HairHues => new int[0];
        public virtual HairItemID[] MaleHairStyles => new HairItemID[0];
        public virtual HairItemID[] FemaleHairStyles => new HairItemID[0];
        public virtual FacialHairItemID[] FacialHairStyles => new FacialHairItemID[0];

        public string GetName( Cases c )
        {
            return GetName( c, false );
        }

        public string GetName( Cases c, bool plural )
        {
            string[] list = plural ? PluralNames : Names;
            if ( list.Length == 0 ) return m_Name;

            int index = (int)c;
            if ( list[index] != null )
                return list[index];

            return "~ERROR~";
        }

        public virtual bool MakeRandomAppearance( Mobile m )
        {
            if ( !(m.BodyValue == 400 || m.BodyValue == 401) )
                return false;

            m.HairItemID = RandomHair( m.Female );
            m.FacialHairItemID = RandomFacialHair( m.Female );
            m.HairHue = ClipHairHue( RandomHairHue() );
            m.FacialHairHue = m.HairHue;
            m.Hue = ClipSkinHue( RandomSkinHue() );

            return true;
        }
    }
    public enum HairItemID
    {
        None = 0,
        Short = 0x203B,
        Long = 0x203C,
        PonyTail = 0x203D,
        Mohawk = 0x2044,
        Pageboy = 0x2045,
        Buns = 0x2046,
        Afro = 0x2047,
        Receeding = 0x2048,
        TwoPigTails = 0x2049,
        Krisna = 0x204A
    }

    public enum FacialHairItemID
    {
        None = 0,
        LongBeard = 0x203E,
        ShortBeard = 0x203F,
        Goatee = 0x2040,
        Mustache = 0x2041,
        MediumShortBeard = 0x204B,
        MediumLongBeard = 0x204C,
        Vandyke = 0x204D
    }

    public enum Cases
    {
        Mianownik,
        Dopelniacz,
        Celownik,
        Biernik,
        Narzednik,
        Miejscownik,
        Wolacz
    }
}
