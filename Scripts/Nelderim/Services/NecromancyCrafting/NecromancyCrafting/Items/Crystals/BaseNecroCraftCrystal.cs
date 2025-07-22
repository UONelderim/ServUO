using System;
using System.Collections.Generic;
using System.Linq;
using Server.Mobiles;

namespace Server.Items
{
	public abstract class BaseNecroCraftCrystal : Item
	{
		
		// ——— Buff rates and caps ———
		private const double STAT_RATE       = 0.025;  // +2.5% Str/Dex/Int per point over
		private const double MAX_STAT_BUFF   = 0.50;   // cap at +50%
		private const double RES_RATE        = 0.005;  // +0.5% resist per point over
		private const double MAX_RES_BUFF    = 0.10;   // cap at +10%
		private const double SKILL_RATE      = 0.01;   // +1% skill per point over
		private const double MAX_SKILL_BUFF  = 0.30;   // cap at +30%
		private const double SLOT_PENALTY    = 0.10;   // −10% buff per extra control slot
		
		private static Dictionary<Type, string> BodyPartName = new()
		{
			{ typeof(RottingLegs), "gnijące nogi" },
			{ typeof(RottingTorso), "gnijący tułów " },
			{ typeof(SkeletonLegs), "nogi szkieleta" },
			{ typeof(SkeletonMageTorso), "tulow szkieleta maga" },
			{ typeof(SkeletonTorso), "tulow szkieleta" },
			{ typeof(WrappedLegs), "zmumifikowane nogi" },
			{ typeof(WrappedMageTorso), "zmumifikowany tułów oznaczony runami" },
			{ typeof(WrappedTorso), "zmumifikowany tułów" },
			{ typeof(Phylacery), "filakterium" },
			{ typeof(Brain), "mozg" },
		};

		public abstract double RequiredNecroSkill { get; }

		public abstract Type[] RequiredBodyParts { get; }
		
		// Timestamp of last summon for cooldown
		private DateTime m_LastUse;
		
		// The summoned creature instance
		protected BaseCreature m_Creature;

		public int[] RequiredBodyPartsAmounts => RequiredBodyParts.Select(x => 1).ToArray();

		public abstract Type SummonType { get; }

		public BaseNecroCraftCrystal() : base(0x1F19)
		{
			Weight = 1.0;
			Stackable = false;
		}

		public BaseNecroCraftCrystal(Serial serial) : base(serial)
		{
		}

 /// <summary>
    /// Handles the double-click on the crystal, enforcing a 5-second cooldown.
    /// </summary>
    public override void OnDoubleClick(Mobile from)
    {
        // 1) Cooldown check
        var nextAllowed = m_LastUse + TimeSpan.FromSeconds(5.0);
        if (DateTime.UtcNow < nextAllowed)
        {
            double secsLeft = (nextAllowed - DateTime.UtcNow).TotalSeconds;
            from.SendMessage($"Musisz odczekać jeszcze {secsLeft:F1} s, zanim użyjesz kryształu ponownie.");
            return;
        }

        // 2) Record this use
        m_LastUse = DateTime.UtcNow;

        // 3) Original usage logic (skill check, body-parts, summoning, scaling…) 
        if (!IsChildOf(from.Backpack))
        {
            from.SendLocalizedMessage(1042001);
            return;
        }

        double necroSkill = from.Skills[SkillName.Necromancy].Value;
        if (necroSkill < RequiredNecroSkill)
        {
            from.SendMessage($"Musisz mieć przynajmniej {RequiredNecroSkill:F1} w nekromancji.");
            return;
        }

        var pack = from.Backpack;
        if (pack == null) return;

        var bc = (BaseCreature)Activator.CreateInstance(SummonType);

        if (from.Followers + bc.ControlSlots > from.FollowersMax)
        {
            from.SendLocalizedMessage(1049607);
            bc.Delete();
            return;
        }

        int res = pack.ConsumeTotal(RequiredBodyParts, RequiredBodyPartsAmounts);
        if (res != -1)
        {
            string partName = BodyPartName.ContainsKey(RequiredBodyParts[res])
                ? BodyPartName[RequiredBodyParts[res]]
                : RequiredBodyParts[res].Name;
            from.SendMessage($"Musisz mieć: {partName}");
            if (from.AccessLevel > AccessLevel.Player)
                from.SendMessage("Boskie moce pomagają ci stworzyć przywołańca bez wszystkich części.");
            else
                return;
        }

        if (!bc.SetControlMaster(from))
        {
            bc.Delete();
            return;
        }

        bc.Allured = true;

        ScaleCreature(bc, necroSkill);

        bc.MoveToWorld(from.Location, from.Map);
        from.PlaySound(0x241);
        Consume();

        EventSink.InvokeNecromancySummonCrafted(new NecromancySummonCraftedEventArgs(from, bc));
        if (from is PlayerMobile pm)
            pm.Statistics.NecromancySummonsCrafted.Increment(SummonType);
    }
		
	/// <summary>
    /// Applies all “over‐skill” scaling to the summoned creature.
    /// </summary>
    /// <param name="bc">The creature being summoned.</param>
    /// <param name="skillValue">Caster’s Necromancy skill.</param>
    
protected void ScaleCreature(BaseCreature bc, double skillValue)
{
    // 1) Over‐skill amount
    double over = Math.Max(0, skillValue - RequiredNecroSkill);

    // 2) Raw buff percentages
    double statBuff  = Math.Min(over * STAT_RATE,  MAX_STAT_BUFF);
    double resBuff   = Math.Min(over * RES_RATE,   MAX_RES_BUFF);
    double skillBuff = Math.Min(over * SKILL_RATE, MAX_SKILL_BUFF);

    // 3) Slot penalty
    double slotFactor = Math.Max(0.0, 1.0 - SLOT_PENALTY * (bc.ControlSlots - 1));
    statBuff  *= slotFactor;
    resBuff   *= slotFactor;
    skillBuff *= slotFactor;

    // 4) Stats & Seeds (with caps)
    bc.RawStr      = Math.Min((int)(bc.RawStr      * (1 + statBuff)), 350);
    bc.RawDex      = Math.Min((int)(bc.RawDex      * (1 + statBuff)), 350);
    bc.RawInt      = Math.Min((int)(bc.RawInt      * (1 + statBuff)), 350);

    bc.HitsMaxSeed = Math.Min((int)(bc.HitsMaxSeed * (1 + statBuff)), 350);
    bc.StamMaxSeed = (int)(bc.StamMaxSeed * (1 + statBuff));          // no cap
    bc.ManaMaxSeed = Math.Min((int)(bc.ManaMaxSeed * (1 + statBuff)), 350);

    // Refresh current HP/Stam/Mana
    bc.Hits = bc.HitsMax;
    bc.Stam = bc.StamMax;
    bc.Mana = bc.ManaMax;

    // 5) Damage ranges (with caps)
    bc.DamageMin = Math.Min((int)(bc.DamageMin * (1 + statBuff)), 10);
    bc.DamageMax = Math.Min((int)(bc.DamageMax * (1 + statBuff)), 15);

    // 6) Skills (buff + cap at 100 for those originally ≤ 100)
    int skillPct = (int)(skillBuff * 100);
    foreach (var sk in bc.Skills)
    {
	    if (sk.Base <= 0)
		    continue;

	    // Remember original fixed‐point to see if it was ≤ 100
	    int origFixed = sk.BaseFixedPoint;

	    // Apply the buff
	    sk.BaseFixedPoint += AOS.Scale(sk.BaseFixedPoint, skillPct);

	    // If it started at 0–100, cap at 100 (i.e. 1000 fixed‐point)
	    if (origFixed <= 1000)
		    sk.BaseFixedPoint = Math.Min(sk.BaseFixedPoint, 1000);
    }

    // 7) Resists
    bc.SetResistance(ResistanceType.Physical, (int)(bc.PhysicalResistance * (1 + resBuff)));
    bc.SetResistance(ResistanceType.Fire,     (int)(bc.FireResistance     * (1 + resBuff)));
    bc.SetResistance(ResistanceType.Cold,     (int)(bc.ColdResistance     * (1 + resBuff)));
    bc.SetResistance(ResistanceType.Poison,   (int)(bc.PoisonResistance   * (1 + resBuff)));
    bc.SetResistance(ResistanceType.Energy,   (int)(bc.EnergyResistance   * (1 + resBuff)));
}



		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
