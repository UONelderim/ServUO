using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.NecroRecovery
{
    public static class NecroResourceRecovery
    {
        // Map each summoned-creature type to its powder type
        private static readonly Dictionary<Type, Type> _powderMap = new()
        {
            { typeof(Skeleton),    typeof(SkeletonPowder)    },
            { typeof(Zombie),      typeof(ZombiePowder)      },
            { typeof(Ghoul),       typeof(GhoulPowder)       },
            { typeof(Lich),        typeof(LichPowder)        },
            { typeof(Mummy),       typeof(MummyPowder)       },
            { typeof(BoneKnight),  typeof(BoneKnightPowder)  },
            { typeof(BoneMagi),    typeof(BoneMagiPowder)    },
            { typeof(AncientLich), typeof(AncientLichPowder) },
            { typeof(Boner),       typeof(BonerPowder)       }
        };

        // Map each summoned-creature type to its crystal type
        private static readonly Dictionary<Type, Type> _crystalMap = new()
        {
            { typeof(Skeleton),    typeof(SkeletonCrystal)    },
            { typeof(Zombie),      typeof(ZombieCrystal)      },
            { typeof(Ghoul),       typeof(GhoulCrystal)       },
            { typeof(Lich),        typeof(LichCrystal)        },
            { typeof(Mummy),       typeof(MummyCrystal)       },
            { typeof(BoneKnight),  typeof(BoneKnightCrystal)  },
            { typeof(BoneMagi),    typeof(BoneMagiCrystal)    },
            { typeof(AncientLich), typeof(AncientLichCrystal) },
            { typeof(Boner),       typeof(BonerCrystal)       }
        };

        // Call this method from your shard's startup to enable resource recovery
        public static void Initialize()
        {
            EventSink.CreatureDeath += OnCreatureDeath;
        }

        private static void OnCreatureDeath(CreatureDeathEventArgs e)
        {
            // Only for necromancy summons
            var creature = e.Creature as BaseCreature;
            if (creature == null || !creature.Allured)
                return;

            // Only if controlled by a real player
            if (!(creature.ControlMaster is PlayerMobile))
                return;

            // Find the corpse container
            var corpse = creature.Corpse as Container;
            if (corpse == null)
                return;

            // Drop the corresponding powder into the corpse
            if (_powderMap.TryGetValue(creature.GetType(), out var powderType))
            {
                var powder = Activator.CreateInstance(powderType) as Item;
                if (powder != null)
                    corpse.DropItem(powder);
            }

            // Drop the corresponding crystal into the corpse
            if (_crystalMap.TryGetValue(creature.GetType(), out var crystalType))
            {
                var crystal = Activator.CreateInstance(crystalType) as Item;
                if (crystal != null)
                    corpse.DropItem(crystal);
            }
        }
    }
}
