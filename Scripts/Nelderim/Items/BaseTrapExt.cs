#region References

using System.Collections.Generic;
using Nelderim;
using Server.Spells;

#endregion

namespace Server.Items
{
	public partial class BaseTrap
	{
		[CommandProperty(AccessLevel.GameMaster)]
		public TrapLevel Level
		{
			get { return BaseTrapExt.Get(this).Level; }
			set { BaseTrapExt.Get(this).Level = value; }
		}

		public enum TrapLevel
		{
			Small,
			Medium,
			Big
		}

		public bool Untrap(Mobile from, TrapRemovalKit kit, bool isSpell)
		{
			double reqDetect = 0;
			double detectHidden = from.Skills[SkillName.DetectHidden].Value;
			double magery = from.Skills[SkillName.Magery].Value;

			SpellHelper.Turn(from, this);

			switch (Level)
			{
				case TrapLevel.Small:
					reqDetect = 50;
					break;
				case TrapLevel.Medium:
					reqDetect = 80;
					break;
				case TrapLevel.Big:
					reqDetect = 100;
					break;
			}

			if (isSpell && magery >= 100 && Level == TrapLevel.Small)
				reqDetect = 0;

			if (detectHidden >= reqDetect)
			{
				Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
					0x376A, 9, 32, 5015);
				Effects.PlaySound(this.Location, this.Map, 0x1F0);

				if (kit != null)
					kit.ConsumeCharge(from);

				Delete();

				from.SendMessage("Z powodzeniem rozbroiles pulapke.");

				return true;
			}

			@from.SendMessage("Nie posiadasz wystarczajacych umiejetnosci na rozbrojenie tej pulapki.");

			return false;
		}
	}

	class BaseTrapExt() : NExtension<BaseTrapExtInfo>("BaseTrap")
	{
		public static void Configure()
		{
			Register(new BaseTrapExt());
		}
	}

	class BaseTrapExtInfo : NExtensionInfo
	{
		public BaseTrap.TrapLevel Level { get; set; }

		public override void Serialize(GenericWriter writer)
		{
			writer.Write( (int)0 );
			writer.Write((int)Level);
		}

		public override void Deserialize(GenericReader reader)
		{
			var version = reader.ReadInt();
			Level = (BaseTrap.TrapLevel)reader.ReadInt();
		}
	}
}
