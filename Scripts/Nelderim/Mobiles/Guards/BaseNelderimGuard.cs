using System;
using Server.Items;
using Server.Nelderim;

namespace Server.Mobiles;

public enum GuardType
{
    StandardGuard,
    ArcherGuard,
    HeavyGuard,
    MageGuard,
    MountedGuard,
    EliteGuard,
    SpecialGuard
}

public enum WarFlag
{
    None,
    White,
    Black,
    Red,
    Green,
    Blue
}

public enum GuardMode
{
    Default,
    Spider,
    Harmless,
}

public class BaseNelderimGuard : BaseCreature
{
    private GuardType _Type;
    private GuardMode _GuardMode = GuardMode.Harmless;
    private string _RegionName;
    private WarFlag _Flag = WarFlag.None;
    private WarFlag _Enemy = WarFlag.None;

    [CommandProperty(AccessLevel.GameMaster)]
    public GuardType GuardType => _Type;
    
    [CommandProperty(AccessLevel.GameMaster)]
    public GuardMode GuardMode
    {
        get => _GuardMode;
        set => _GuardMode = value;
    }

    [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
    public string HomeRegionName
    {
        get => _RegionName;
        set
        {
            _RegionName = value;

            try
            {
                NelderimRegionSystem.GetRegion(Region.Name).MakeGuard(this);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                _RegionName = null;
            }
        }
    }
    
    public override bool IsEnemy(Mobile m)
    {
        return _GuardMode switch
        {
            GuardMode.Spider => IsEnemyOfSpider(m),
            GuardMode.Harmless => false,
            _ => DefaultIsEnemy(m)
        };
    }

    public override void CriminalAction(bool message)
    {
        // Straznik nigdy nie dostanie krima.
        // Byl problem, ze gdy straznik atakowal peta/summona gracza-krima to sam dostawal krima.s
        return;
    }

    public override bool AlwaysInnocent => true;

    [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
    public WarFlag WarSideFlag
    {
        get => _Flag;
        set
        {
            _Flag = value;

            if (_Flag != WarFlag.None && _Flag == _Enemy)
                _Enemy = WarFlag.None;
        }
    }

    [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
    public WarFlag WarOpponentFlag
    {
        get => _Enemy;
        set
        {
            _Enemy = value;

            if (_Enemy != WarFlag.None && _Flag == _Enemy)
                _Flag = WarFlag.None;
        }
    }

    public bool IsHuman => BodyValue == 400 || BodyValue == 401;


    public BaseNelderimGuard(GuardType type, AIType aiType = AIType.AI_Melee, int rangePerception = 16, int rangeFight = 1, double activeSpeed = 0.2, double passiveSpeed = 0.4) : 
        base(aiType, FightMode.Criminal, rangePerception, rangeFight, activeSpeed, passiveSpeed)
    {
        _Type = type;
        _RegionName = null;
        // Make it somewhat resistant, so it doesn't die by accident before receiving properties and behaviour from region profile:
        SetHits(500);
        SetDamage(1);
        SetSkill(SkillName.Wrestling, 90);
        SetResistance(ResistanceType.Physical, 45);
        SetResistance(ResistanceType.Fire, 45);
        SetResistance(ResistanceType.Cold, 45);
        SetResistance(ResistanceType.Poison, 45);
        SetResistance(ResistanceType.Energy, 45);
        
        SetWeaponAbility(WeaponAbility.Dismount);
        SetWeaponAbility(WeaponAbility.BleedAttack);

        new RaceTimer(this).Start();
    }

    public BaseNelderimGuard(Serial serial) : base(serial)
    {
    }

    public override bool AutoDispel => true;

    public override bool Unprovokable => true;

    public override bool Uncalmable => true;

    public override bool BardImmune => true;

    public override Poison PoisonImmune => Poison.Greater;

    public override bool HandlesOnSpeech(Mobile from)
    {
        return true;
    }

    public override bool ShowFameTitle => false;
    
    public override double WeaponAbilityChance => 0.4;

    public GuardType Type => _Type;

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write((int)4);

        writer.Write((int)_GuardMode);
        // writer.Write( (string) m_IsEnemyFunction );
        writer.Write((int)_Flag);
        writer.Write((int)_Enemy);
        writer.Write(_RegionName);
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);

        int version = reader.ReadInt();

        switch (version)
        {
            case 4:
                _GuardMode = (GuardMode)reader.ReadInt();
                goto case 2;
            case 3:
            {
                reader.ReadString(); //enemyFunction
                goto case 2;
            }
            case 2:
            {
                _Flag = (WarFlag)reader.ReadInt();
                _Enemy = (WarFlag)reader.ReadInt();
                goto case 1;
            }
            case 1:
            {
                if (version < 2)
                {
                    _Flag = WarFlag.None;
                    _Enemy = WarFlag.None;
                }

                _RegionName = reader.ReadString();
                break;
            }
            default:
            {
                if (version < 1)
                    _RegionName = null;
                break;
            }
        }
    }
    
    private class RaceTimer : Timer
    {
        private BaseNelderimGuard _Target;

        public RaceTimer(BaseNelderimGuard target) : base(TimeSpan.FromMilliseconds(250))
        {
            _Target = target;
            Priority = TimerPriority.FiftyMS;
        }

        protected override void OnTick()
        {
            try
            {
                if (!_Target.Deleted)
                {
                    NelderimRegionSystem.GetRegion(_Target.Region.Name).MakeGuard(_Target);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
    
    private bool IsEnemyOfSpider(Mobile m)
    {
	    if (m == null)
		    return false;
        
	    if (Faction != null && Faction.IsEnemy(m))
		    return true;
	    
        // Nie atakuj innych straznikow (obszarowka moze triggerowac walke miedzy nimi)
        if (m is BaseNelderimGuard)
            return false;

        // nie atakuj drowow i obywateli drowiego miasta (oraz ich zwierzat i przywolancow)
        if (BaseAI.IsSpidersFriend(m))
            return false;

        // atakuj wszystkich graczy
        if (m is PlayerMobile)
            return true;

        if (m is BaseCreature)
        {
            BaseCreature bc = m as BaseCreature;

            // atakuj pety i przywolance graczy
            if ((bc.Controlled && bc.ControlMaster != null && bc.ControlMaster.AccessLevel < AccessLevel.Counselor) ||
                (bc.Summoned && bc.SummonMaster != null && bc.SummonMaster.AccessLevel < AccessLevel.Counselor))
                return true;

            // nie atakuj dzikich pajakow
            if (SlayerGroup.GetEntryByName(SlayerName.SpidersDeath).Slays(m) && !bc.IsChampionSpawn && !(m is NSzeol) &&
                !(m is Mephitis))
                return false;

            // atakuj stworzenia red/krim
            if (bc.AlwaysMurderer || bc.Criminal || (!bc.Controlled && bc.FightMode == FightMode.Closest))
                return true;
        }

        return false;
    }

    private bool DefaultIsEnemy(Mobile m)
    {
        if (m == null)
            return false;
        
        if (Faction != null && Faction.IsEnemy(m))
	        return true;

        if (m is BaseNelderimGuard guard)
            return WarOpponentFlag != WarFlag.None && WarOpponentFlag == guard.WarSideFlag;

        if (m.Criminal || m.Kills >= 5)
            return true;

        if (m is BaseCreature bc)
        {
            if (bc.AlwaysMurderer || (!bc.Controlled && bc.FightMode == FightMode.Closest))
                return true;

            var master = bc switch
            {
	            {Controlled: true } => bc.ControlMaster,
				{Summoned: true} => bc.SummonMaster,
	            _ => null
            };
            return IsEnemy(master);
        }

        return false;
    }
}
