using Server.Mobiles;
using Server.SicknessSys.Gumps;

namespace Server.SicknessSys
{
    public class VirusCell : Item
    {
        public bool InDebug { get; set; }
        
        public PlayerMobile PM { get; set; }
        
        public PlayerMobile GM { get; set; }

        public int SerialNumber { get; set; }
        
        public IllnessType Illness { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public string Sickness { get; set; }
        
        public int SicknessChance { get; set; }

        public bool IsContagious { get; set; }

        public int Stage { get; set; }

        public int Level { get; set; }

        public int LevelMod { get; set; }

        public int Damage { get; set; }

        public int StatDrain { get; set; }

        public int Power { get; set; }

        public int MaxPower { get; set; }

        public int PowerDegenRate { get; set; }

        public int DefaultBody { get; set; }
        public int DefaultBodyHue { get; set; }
        
        public int DefaultHairHue { get; set; }
        public int DefaultFacialHue { get; set; }
        
        public int BaseHeart { get; set; }
        public int HeartBeat { get; set; }
        
        public bool IsMovingGump { get; set; }
        public int IsMovingRelease { get; set; }
        
        public int GumpX { get; set; }
        public int GumpY { get; set; }
        
        public int LastSick { get; set; }
        public int LastSkill { get; set; }

        public int WorldInfectionLevel { get; set; }

        [Constructable]
        public VirusCell(Mobile pm, IllnessType type, string sickness) : base(0xF13)
        {
            PM = pm as PlayerMobile;
            SerialNumber = Utility.RandomMinMax(100000, 999999);
            Illness = type;

            SetUpVirus(pm, type, sickness);

            Hue = 1175;

            IsMovingGump = false;
            GumpX = 25;
            GumpY = 25;

            InDebug = false;
            
            Stackable = false;
            Visible = false;
            Movable = false;

            IsMovingGump = false;
            IsMovingRelease = 0;

            LootType = LootType.Blessed;
        }

        public VirusCell(Serial serial) : base(serial)
        {
        }

        private void SetUpVirus(Mobile pm, IllnessType type, string sickness)
        {
            SetIllnessInfo(type, sickness);

            IsContagious = SicknessRO.GetContagious(type);

            Stage = 1;

            Level = 1;

            if (!SicknessHelper.IsSpecialVirus(this))
            {
                LevelMod = Utility.Random(((int)Illness * 2), 10 - ((int)Illness * 2));
                MaxPower = Utility.Random((100 - ((int)Illness * 20)), (int)Illness * 20);
            }
            else
            {
                LevelMod = 1;
                MaxPower = 100;
            }

            Power = MaxPower;

            DefaultBody = PM.BodyValue;
            DefaultBodyHue = PM.Hue;

            DefaultHairHue = PM.HairHue;
            DefaultFacialHue = PM.FacialHairHue;

            if (SicknessHelper.IsSpecialVirus(this))
            {
                if (Illness == IllnessType.Vampirism)
                    BaseHeart = 1174; //Vampire
                else
                    BaseHeart = 1176; //Werewolf
            }
            else
            {
                BaseHeart = 2749;
            }
            HeartBeat = 0;

            SicknessSpread.UpdateSickness(this);

            PM.SendMessage(0x35, "You have caught the " + Sickness + ", seek medical help!");

            LastSick = ((30 / Stage) - ((PowerDegenRate + Stage) / Stage));

            LastSkill = 10 + (5 * PowerDegenRate);

            WorldInfectionLevel = 0;
        }

        private void SetIllnessInfo(IllnessType type, string sickness)
        {
            switch (type)
            {
                case IllnessType.Cold:

                    if (sickness == null)
                        Sickness = Illnesses.Cold.Name;
                    else
                        Sickness = sickness;
                    
                    Name = Sickness + " cell";
                    Damage = Illnesses.Cold.BaseDamage;
                    StatDrain = Illnesses.Cold.StatDrain;
                    PowerDegenRate = Illnesses.Cold.PowerDegenRate;
                    break;

                case IllnessType.Flu:

                    if (sickness == null)
                        Sickness = Illnesses.Flu.Name;
                    else
                        Sickness = sickness;

                    Name = Sickness + " cell";
                    Damage = Illnesses.Flu.BaseDamage;
                    StatDrain = Illnesses.Flu.StatDrain;
                    PowerDegenRate = Illnesses.Flu.PowerDegenRate;
                    break;

                case IllnessType.Virus:

                    if (sickness == null)
                        Sickness = Illnesses.Virus.Name;
                    else
                        Sickness = sickness;

                    Name = Sickness + " cell";
                    Damage = Illnesses.Virus.BaseDamage;
                    StatDrain = Illnesses.Virus.StatDrain;
                    PowerDegenRate = Illnesses.Virus.PowerDegenRate;
                    break;

                case IllnessType.Vampirism:

                    if (sickness == null)
                        Sickness = Illnesses.Vampirism.Name;
                    else
                        Sickness = sickness;

                    Name = Sickness + " cell";
                    Damage = Illnesses.Vampirism.BaseDamage;
                    StatDrain = Illnesses.Vampirism.StatDrain;
                    PowerDegenRate = Illnesses.Vampirism.PowerDegenRate;
                    break;

                case IllnessType.Lycanthropia:

                    if (sickness == null)
                        Sickness = Illnesses.Lycanthropia.Name;
                    else
                        Sickness = sickness;

                    Name = Sickness + " cell";
                    Damage = Illnesses.Lycanthropia.BaseDamage;
                    StatDrain = Illnesses.Lycanthropia.StatDrain;
                    PowerDegenRate = Illnesses.Lycanthropia.PowerDegenRate;
                    break;

                default:
                    Sickness = "None";
                    break;
            }
        }

        public override double DefaultWeight
        {
            get
            {
                return 0.0;
            }
        }

        public void OnDeleteHelper()
        {
            if (PM != null)
            {
                if (SicknessCore.VirusCellList.Contains(this))
                    SicknessCore.VirusCellList.Remove(this);

                if (PM.HasGump(typeof(PowerGump)))
                {
                    PM.CloseGump(typeof(PowerGump));
                }

                PM.BodyValue = DefaultBody;
                PM.Hue = DefaultBodyHue;

                PM.HairHue = DefaultHairHue;
                PM.FacialHairHue = DefaultFacialHue;
            }
        }

        public override void OnDelete()
        {
            OnDeleteHelper();

            base.OnDelete();
        }

        public override void OnDoubleClick(Mobile from)
        {
            //start testing - trigger (Push Sickness Stage)
            //if (Level < 99)
            //{
            //    Power = 2;
            //    Level = 99;
            //}
            //end testing Remove this once done building system!

                
            //Sickness Current Information(Staff Eyes Only - Debug)
            if (InDebug)
            {
                GM.SendMessage(37, "Debugging Disabled");

                GM = null;

                InDebug = false;
                DebugStarted = false;
            }
            else
            {
                GM = from as PlayerMobile;

                GM.SendMessage(120, "---------------------");
                GM.SendMessage(68, "Debugging Enabled");
                GM.SendMessage(120, "---------------------");

                InDebug = true;
            }
        }
        
        private string LastTime;
        private bool DebugStarted;

        public void SendDebugInfo()
        {
            if (LastTime == SicknessTime.GetTimeLiteral(PM))
            {
                //do nothing
            }
            else if (!DebugStarted)
            {
                LastTime = SicknessTime.GetTimeLiteral(PM);
                DebugStarted = true;

                GM.SendMessage(120, "---------------------");
                GM.SendMessage(68, "Debug - Player Details");
                GM.SendMessage(120, "---------------------");

                GM.SendMessage(55, "Time : " + SicknessTime.GetTimeLiteral(PM));

                GM.SendLocalizedMessage(SicknessTime.GetTime(PM));

                GM.SendMessage(120, "---------------------");

                GM.SendMessage(55, PM.Name + " has " + Illness.ToString());

                GM.SendMessage(55, PM.Name + " is on stage " + Stage.ToString());

                if (!SicknessHelper.IsSpecialVirus(this))
                    GM.SendMessage(55, PM.Name + "'s level is " + Level + "/100");
                else
                    GM.SendMessage(55, PM.Name + "'s level is " + Level);

                GM.SendMessage(55, PM.Name + "'s level mod is " + LevelMod + "/9");

                GM.SendMessage(55, PM.Name + "'s power is " + Power + "/100");

                GM.SendMessage(55, PM.Name + "'s degen mod is " + PowerDegenRate + "/9");

                GM.SendMessage(55, PM.Name + "'s damage mod is " + Damage + "/9");

                GM.SendMessage(55, PM.Name + "'s stat mod is " + StatDrain + "/9");

                GM.SendMessage(120, "---------------------");
            }
            else
            {
                GM.SendMessage(120, "---------------------");
                GM.SendMessage(68, "Debug - Player Details");
                GM.SendMessage(120, "---------------------");

                GM.SendMessage(55, "Time : " + SicknessTime.GetTimeLiteral(PM));

                GM.SendLocalizedMessage(SicknessTime.GetTime(PM));

                GM.SendMessage(120, "---------------------");
                
                GM.SendMessage(55, PM.Name + "'s power is " + Power + "/100");

                GM.SendMessage(120, "---------------------");
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);

            writer.Write(PM as Mobile);
            writer.Write(SerialNumber);

            writer.Write((int)Illness);
            writer.Write(Sickness);

            writer.Write(SicknessChance);
            writer.Write(IsContagious);

            writer.Write(Stage);

            writer.Write(Level);
            writer.Write(LevelMod);

            writer.Write(Damage);
            writer.Write(StatDrain);

            writer.Write(Power);
            writer.Write(MaxPower);
            writer.Write(PowerDegenRate);

            writer.Write(DefaultBody);
            writer.Write(DefaultBodyHue);

            writer.Write(DefaultHairHue);
            writer.Write(DefaultFacialHue);

            writer.Write(BaseHeart);

            writer.Write(GumpX);
            writer.Write(GumpY);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            PM = reader.ReadMobile() as PlayerMobile;
            SerialNumber = reader.ReadInt();

            Illness = (IllnessType)reader.ReadInt();
            Sickness = reader.ReadString();

            SicknessChance = reader.ReadInt();
            IsContagious = reader.ReadBool();

            Stage = reader.ReadInt();

            Level = reader.ReadInt();
            LevelMod = reader.ReadInt();

            Damage = reader.ReadInt();
            StatDrain = reader.ReadInt();

            Power = reader.ReadInt();
            MaxPower = reader.ReadInt();
            PowerDegenRate = reader.ReadInt();

            DefaultBody = reader.ReadInt();
            DefaultBodyHue = reader.ReadInt();

            DefaultHairHue = reader.ReadInt();
            DefaultFacialHue = reader.ReadInt();

            BaseHeart = reader.ReadInt();
            HeartBeat = 0;

            IsMovingGump = false;
            IsMovingRelease = 0;

            GumpX = reader.ReadInt();
            GumpY = reader.ReadInt();

            LastSick = ((30 / Stage) - ((PowerDegenRate + Stage) / Stage));

            LastSkill = 10 + (5 * PowerDegenRate);
        }
    }
}
