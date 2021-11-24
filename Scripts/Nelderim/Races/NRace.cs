using Server;
using Server.Misc;
using System;
using System.Collections.Generic;

namespace Nelderim.Races
{
    public abstract class NRace : Race
    {
	    new public static NRace[] Races { get; } = new NRace[0x100];

	    new public static List<NRace> AllRaces { get; } = new List<NRace>();

	    public static void Configure()
        {
            /* Here we configure all races. Some notes:
            * 
            * 1) The first 32 races are reserved for core use.
            * 2) Race 0x7F is reserved for core use.
            * 3) Race 0xFF is reserved for core use.
            * 4) Changing or removing any predefined races may cause server instability.
            */
            RegisterNRace( new None( 0, 0 ) );
            RegisterNRace( new Tamael( 1, 1 ) );
            RegisterNRace( new Jarling( 2, 2 ) );
            RegisterNRace( new Naur( 3, 3 ) );
            RegisterNRace( new Elf( 4, 4 ) );
            RegisterNRace( new Drow( 5, 5 ) );
            RegisterNRace( new Krasnolud( 6, 6 ) );
        }

	    private static void RegisterNRace(NRace race)
	    {
		    Races[race.RaceIndex] = race;
		    AllRaces.Add(race);
		    RaceDefinitions.RegisterRace(race);
	    }

        public NRace( int raceID, int raceIndex, string name, string pluralName) : base(raceID + NRaceOffset, raceIndex + NRaceOffset, name, pluralName, 400, 401, 402, 403 )
        {
        }
 
        public override FacialHairItemID[] FacialHairStyles => new FacialHairItemID[]
        {
            FacialHairItemID.None,
            FacialHairItemID.LongBeard,
            FacialHairItemID.ShortBeard,
            FacialHairItemID.Goatee,
            FacialHairItemID.Mustache,
            FacialHairItemID.MediumShortBeard,
            FacialHairItemID.MediumLongBeard,
            FacialHairItemID.Vandyke
        };
 
        public override HairItemID[] MaleHairStyles => new HairItemID[]
        {
            HairItemID.None,
            HairItemID.Short,
            HairItemID.Long,
            HairItemID.PonyTail,
            HairItemID.Mohawk,
            HairItemID.Pageboy,
            HairItemID.Buns,
            HairItemID.Afro,
            HairItemID.Receeding,
            HairItemID.TwoPigTails,
            HairItemID.Krisna
        };
 
        public override HairItemID[] FemaleHairStyles => new HairItemID[]
        {
            HairItemID.None,
            HairItemID.Short,
            HairItemID.Long,
            HairItemID.PonyTail,
            HairItemID.Mohawk,
            HairItemID.Pageboy,
            HairItemID.Buns,
            HairItemID.Afro,
            HairItemID.Receeding,
            HairItemID.TwoPigTails,
            HairItemID.Krisna
        };

        public override bool ValidateHair( bool female, int itemID )
        {
            HairItemID[] list = female ? FemaleHairStyles : MaleHairStyles;
            foreach(HairItemID hairItem in list )
            {
                if ( (int)hairItem == itemID ) return true;
            }
            return false;
        }

        public override int RandomHair( bool female )
        {
            if ( female ) return (int)Utility.RandomList( FemaleHairStyles );
            return (int)Utility.RandomList( MaleHairStyles );
        }

        public override bool ValidateFacialHair( bool female, int itemID )
        {
            if ( female ) return false;
            foreach(FacialHairItemID hairItem in FacialHairStyles )
            {
                if ( (int)hairItem == itemID ) return true;
            }
            return false;
        }

        public override int RandomFacialHair( bool female )
        {
            if ( female ) return 0;
            return (int)Utility.RandomList( FacialHairStyles );
        }

        public override bool ValidateFace( bool female, int itemID )
        {
            return Race.Human.ValidateFace( female, itemID );
        }

        public override int RandomFace( bool female )
        {
            return Race.Human.RandomFace( female );
        }

        public override bool ValidateEquipment( Item item )
        {
	        return true;
        }

        public override int ClipSkinHue( int hue )
        {
            return hue;
        }

        public override int RandomSkinHue()
        {
            return Utility.RandomList( SkinHues );
        }

        public override int ClipHairHue( int hue )
        {
            return hue;
        }

        public override int RandomHairHue()
        {
            return Utility.RandomList( HairHues );
        }

        public override int ClipFaceHue( int hue )
        {
            return hue;
        }

        public override int RandomFaceHue()
        {
            return Utility.RandomList( SkinHues );
        }
    }
}
