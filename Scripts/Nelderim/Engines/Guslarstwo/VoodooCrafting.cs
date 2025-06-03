using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Craft;

namespace Server.Engines.Craft
{
    /// <summary>
    /// System wytwarzania przedmiotów voodoo: przepisy na VoodooPin i AnimatedVDoll.
    /// Przeniesiony z kolizji z przestrzenią nazw Server.Skills.
    /// </summary>
    public class VoodooCrafting : CraftSystem
    {
        public override SkillName MainSkill => SkillName.TasteID;
        public override int GumpTitleNumber => 1044000; // przykładowy tytuł

        private static VoodooCrafting _instance;
        public static CraftSystem Instance => _instance ?? (_instance = new VoodooCrafting());

        private VoodooCrafting() : base(1, 1, 1.25)
        {
        }

        public override double GetChanceAtMin(CraftItem item) => 0.5;

        /// <summary>
        /// Sprawdza, czy można wytworzyć dany przedmiot.
        /// </summary>
        public override int CanCraft(Mobile from, ITool tool, Type craftItemType)
        {
            // Wywołanie bazowego warunku
            return 0;
        }

        /// <summary>
        /// Efekt przy rozpoczęciu craftingu.
        /// </summary>
        public override void PlayCraftEffect(Mobile from)
        {
            from.PlaySound(0x240);
        }

        /// <summary>
        /// Efekt przy zakończeniu craftingu.
        /// </summary>
        public override int PlayEndingEffect(
            Mobile from,
            bool failed,
            bool lostMaterial,
            bool toolBroken,
            int quality,
            bool makersMark,
            CraftItem item)
        {
            if (failed)
                return 0x42;
            return 0x5;
        }

        /// <summary>
        /// Definicja receptur voodoo.
        /// </summary>
        public override void InitCraftList()
        {
            //AddCraft(typeof(VoodooPin),      3922, 1044294, 30.0, 100.0, typeof(BlackPearl),   1, 1044253);
            //AddCraft(typeof(AnimatedVDoll),  3848, 1044295, 50.0, 100.0, typeof(DragonBlood),   2, 1042851);
        }
    }
}
