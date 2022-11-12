using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Engines.Craft
{
    public class NecroRecycle : RecycleHelper
    {
        private static Dictionary<Type, Type> legacyMobMap = new Dictionary<Type, Type>
        {
            { typeof(BoneClaw), typeof(Boner) },
            { typeof(Ghast), typeof(Ghoul) },
            { typeof(MummyMagician), typeof(Lich) },
            { typeof(PesantMummy), typeof(Mummy) },
            { typeof(SkeletalFighter), typeof(Skeleton) },
            { typeof(SkeletalMagi), typeof(BoneMagi) },
            { typeof(SkeletalWorrior), typeof(BoneKnight) },
            { typeof(Vecna), typeof(AncientLich) },
            { typeof(ZombieMinion), typeof(Zombie) }
        };

        public NecroRecycle()
        {
        }

        public override string LabelString
        {
            get { return "<BASEFONT COLOR=#FFFFFF>ROZŁÓŻ</BASEFONT>"; }
        }

        public override void Do(Mobile from, CraftSystem craftSystem, BaseTool tool)
        {
            int num = craftSystem.CanCraft(from, tool, null);

            if (num > 0)
            {
                from.SendGump(new CraftGump(from, craftSystem, tool, num));
            }
            else
            {
                from.Target = new InternalTarget(craftSystem, tool);
                from.SendMessage("Wskaz przywolanca do rozłożenia");
            }
        }

        private class InternalTarget : Target
        {
            private CraftSystem m_CraftSystem;
            private BaseTool m_Tool;

            public InternalTarget(CraftSystem craftSystem, BaseTool tool) : base(2, false, TargetFlags.None)
            {
                m_CraftSystem = craftSystem;
                m_Tool = tool;
            }

            private bool Disassemble(Mobile from, BaseCreature bc)
            {
                try
                {
                    Type mobType = legacyMobMap.ContainsKey(bc.GetType()) ? legacyMobMap[bc.GetType()] : bc.GetType();
                    Type crystalType = ScriptCompiler.FindTypeByName(mobType.Name + "Crystal");
                    if (crystalType != null && crystalType.IsSubclassOf(typeof(BaseNecroCraftCrystal)))
                    {
                        List<Item> resources = new List<Item>();
                        BaseNecroCraftCrystal crystal = (BaseNecroCraftCrystal)Activator.CreateInstance(crystalType);
                        resources.Add(crystal);

                        foreach (Type bodyPartType in crystal.RequiredBodyParts)
                        {
                            Item bodyPart = (Item)Activator.CreateInstance(bodyPartType);
                            resources.Add(bodyPart);
                        }

                        foreach (Item resource in resources)
                        {
                            if (!from.AddToBackpack(resource))
                            {
                                from.SendMessage("Jeden z materiałów nie zmieścił się do plecaka");
                            }
                        }

                        bc.Delete();

                        from.PlaySound(0x5A9);
                        return true;
                    }
                }
                catch
                {
                }

                return false;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                int num = m_CraftSystem.CanCraft(from, m_Tool, null);

                if (num > 0)
                {
                    from.SendGump(new CraftGump(from, m_CraftSystem, m_Tool, num));
                }
                else
                {
                    bool success = false;

                    if (targeted is BaseCreature)
                    {
                        BaseCreature bc = (BaseCreature)targeted;
                        if (bc.Hits == bc.HitsMax)
                        {
                            success = Disassemble(from, (BaseCreature)targeted);
                        }
                    }

                    if (success)
                        from.SendGump(new CraftGump(from, m_CraftSystem, m_Tool, "Rozkladasz przywolanca na elementy"));
                    else
                        from.SendGump(new CraftGump(from, m_CraftSystem, m_Tool, "Nie mozesz tego rozlozyc"));
                }
            }
        }
    }
}