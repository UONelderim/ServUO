using Server.Network;
using Server.Spells;
using Server.Targeting;
using System;
using System.Linq;

namespace Server.Items
{
    public class FireHorn : Item
    {
	    public const int InitMaxUses = 120;
	    public const int InitMinUses = 80;
	    
		[Constructable]
        public FireHorn()
            : base(0xFC7)
        {
            Hue = 0x466;
            Weight = 1.0;
            _UsesRemaining = Utility.RandomMinMax(InitMinUses, InitMaxUses);
        }

        public FireHorn(Serial serial)
            : base(serial)
        {
        }
        
        private int _UsesRemaining;
        
        [CommandProperty(AccessLevel.GameMaster)]
        public int UsesRemaining
        {
	        get => _UsesRemaining;
	        set
	        {
		        _UsesRemaining = value;
		        InvalidateProperties();
	        }
        }

        public virtual Type ResourceType => typeof(SulfurousAsh);
        public virtual string MissingResourceName => "siarki";
        public virtual int EffectSound => 0x15F;
        public virtual int EffectId => 0x36D4;
        public virtual int EffectHue => 0;
		public override int LabelNumber => 1060456;// fire horn
        public override void OnDoubleClick(Mobile from)
        {
            if (CheckUse(from))
            {
                from.SendLocalizedMessage(1049620); // Wskaz obszar do uzycia rogu
                from.Target = new InternalTarget(this);
            }
        }
        
        public override void GetProperties(ObjectPropertyList list)
        {
	        base.GetProperties(list);
	        list.Add(1060584, UsesRemaining.ToString()); // uses remaining: ~1_val~
        }

        public void Use(Mobile from, IPoint3D loc)
        {
            if (!CheckUse(from))
                return;

            from.BeginAction(typeof(FireHorn));
            Timer.DelayCall(TimeSpan.FromSeconds(6.0), EndAction, from);

            int music = from.Skills[SkillName.Musicianship].Fixed;

            int sucChance = 500 + (music - 775) * 2;
            double dSucChance = sucChance / 1000.0;

            if (!from.CheckSkill(SkillName.Musicianship, dSucChance))
            {
                from.SendLocalizedMessage(1049618); // The horn emits a pathetic squeak.
                from.PlaySound(0x18A);
                return;
            }

            from.Backpack.ConsumeUpTo(typeof(SulfurousAsh), 4);

            from.PlaySound(EffectSound);
            Effects.SendPacket(from, from.Map, new HuedEffect(EffectType.Moving, from.Serial, Serial.Zero, EffectId, from.Location, loc, 5, 0, false, true, EffectHue, 0));

            var targets = SpellHelper.AcquireIndirectTargets(from, loc, from.Map, 2).OfType<Mobile>().ToList();
            int count = targets.Count;
            bool playerVsPlayer = targets.Any(t => t.Player);

            if (count > 0)
            {
                int prov = from.Skills[SkillName.Provocation].Fixed;
                int disc = from.Skills[SkillName.Discordance].Fixed;
                int peace = from.Skills[SkillName.Peacemaking].Fixed;

                int minDamage, maxDamage;

                int musicScaled = music + Math.Max(0, music - 900) * 2;
                int provScaled = prov + Math.Max(0, prov - 900) * 2;
                int discScaled = disc + Math.Max(0, disc - 900) * 2;
                int peaceScaled = peace + Math.Max(0, peace - 900) * 2;

                int weightAvg = (musicScaled + provScaled * 3 + discScaled * 3 + peaceScaled) / 80;

                int avgDamage;
                if (playerVsPlayer)
                    avgDamage = weightAvg / 3;
                else
                    avgDamage = weightAvg / 2;

                minDamage = (avgDamage * 9) / 10;
                maxDamage = (avgDamage * 10) / 9;

                double damage = Utility.RandomMinMax(minDamage, maxDamage);

                if (count > 1)
                    damage = (damage * 2) / count;

                foreach (Mobile m in targets)
                {
	                from.DoHarmful(m);
	                DoDamage(from, m, damage);
                }
            }

            ColUtility.Free(targets);

            if ( --UsesRemaining <= 0 )
            {
                from.SendLocalizedMessage(1049619); // The fire horn crumbles in your hands.
                Delete();
            }
        }

        public virtual void DoDamage(Mobile from, Mobile m, double damage)
        {
	        SpellHelper.Damage(TimeSpan.Zero, m, from, damage, 0, 100, 0, 0, 0);
	        Effects.SendTargetEffect(m, 0x3709, 10, 30);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(1); // version
            writer.Write(_UsesRemaining); //NELDERIM
        }

        public override void Deserialize(GenericReader reader)
        {
	        base.Deserialize(reader);

	        int version = reader.ReadEncodedInt();
	        if (version > 1)
	        {
		        _UsesRemaining = reader.ReadInt();
	        }
	        else
	        {
		        Timer.DelayCall(TimeSpan.Zero,
			        () =>
			        {
				        _UsesRemaining = FireHornExt.Get(this).UsesRemaining;
				        FireHornExt.Delete(this);
			        }
		        );
	        }
        }

        private static void EndAction(Mobile m)
        {
            m.EndAction(typeof(FireHorn));
            m.SendLocalizedMessage(1049621); // You catch your breath.
        }

        private bool CheckUse(Mobile from)
        {
            if (!IsAccessibleTo(from))
                return false;

            if (from.Map != Map || !from.InRange(GetWorldLocation(), 2))
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
                return false;
            }

            if (!from.CanBeginAction(typeof(FireHorn)))
            {
                from.SendLocalizedMessage(1049615); // You must take a moment to catch your breath.
                return false;
            }

            if (from.Backpack == null || from.Backpack.GetAmount(ResourceType) < 4)
            {
                from.SendLocalizedMessage(1049617, MissingResourceName); // Nie masz wystarczajacej ilosci ~1_resource~.
                return false;
            }

            return true;
        }

        private class InternalTarget : Target
        {
            private readonly FireHorn m_Horn;
            public InternalTarget(FireHorn horn)
                : base(3, true, TargetFlags.Harmful)
            {
                m_Horn = horn;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Horn.Deleted)
                    return;

                IPoint3D loc;
                if (targeted is Item)
                    loc = ((Item)targeted).GetWorldLocation();
                else
                    loc = targeted as IPoint3D;

                m_Horn.Use(from, loc);
            }
        }
    }
}
