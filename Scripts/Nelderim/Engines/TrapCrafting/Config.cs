//
// ** Basic Trap Framework (BTF)
// ** Author: Lichbane
//
//  Trapcrafting System Variables.  Tweak the bits you want here.
//
using System;

namespace Server.Items.Trapcrafting
{
    public class Config
    {
        // Does setting a trap attract a loss of Karma? 
        // Note: If you use this option, Karma Loss can be adjusted in the script for
        // each trap type.
        public static bool KarmaLossOnArming = false;

        // Limits the number of active traps a player may have out at any one time.
        public static bool TrapsLimit = true;
        public static int TrapLimitNumber = 10;
    }
}