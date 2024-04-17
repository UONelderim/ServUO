using System;
using Nelderim;

namespace Server.Mobiles
{
	class DifficultyLevelExt : NExtension<DifficultyLevelExtInfo>
	{
		public static string ModuleName = "DifficultyLevel";

		public static void Initialize()
		{
			EventSink.WorldSave += Save;
			Load(ModuleName);
		}

		public static void Save(WorldSaveEventArgs args)
		{
			Save(args, ModuleName);
		}

		private const double _DmgScalarExponent = 0.5;

		public static void Apply(BaseCreature bc)
		{
			double scalar = 0.25 * (int)bc.DifficultyLevel;

			bc.Name = $"{bc.DifficultyLevelPrefix} {bc.Name}";
			bc.Hue = bc.DifficultyLevelHue;
			bc.BodyValue = bc.DifficultyLevelBody;

			if (bc.HitsMaxSeed >= 0)
				bc.HitsMaxSeed = (int)(bc.HitsMaxSeed * scalar);

			bc.RawStr = (int)(bc.RawStr * scalar);
			bc.RawInt = (int)(bc.RawInt * scalar);
			bc.RawDex = (int)(bc.RawDex * scalar);

			bc.Hits = bc.HitsMax;
			bc.Mana = bc.ManaMax;
			bc.Stam = bc.StamMax;

			for (int i = 0; i < bc.Skills.Length; i++)
			{
				Skill skill = bc.Skills[i];

				if (skill.Base > 0.0)
					skill.Base *= scalar;
			}

			bc.PassiveSpeed /= scalar;
			bc.ActiveSpeed /= scalar;
			bc.CurrentSpeed = bc.PassiveSpeed;

			var dmgScalar = Math.Pow(scalar, _DmgScalarExponent);
			bc.DamageMin = (int)(bc.DamageMin * dmgScalar);
			bc.DamageMax = (int)(bc.DamageMax * dmgScalar);

			if (bc.Fame > 0)
				bc.Fame = (int)(bc.Fame * scalar);
			

			if (bc.Fame > 32000)
				bc.Fame = 32000;

			// TODO: Mana regeneration rate = Sqrt( buffedFame ) / 4

			if (bc.Karma != 0)
			{
				bc.Karma = (int)(bc.Karma * scalar);
			
				if (Math.Abs(bc.Karma) > 32000)
					bc.Karma = 32000 * Math.Sign(bc.Karma);
			}
		}

		public static void Restore(BaseCreature bc)
		{
			double scalar = 0.25 * (int)bc.DifficultyLevel;

			bc.Name = bc.Name.Replace($"{bc.DifficultyLevelPrefix} ", "");
			
			if (bc.HitsMaxSeed >= 0)
				bc.HitsMaxSeed = (int)(bc.HitsMaxSeed / scalar);

			bc.RawStr = (int)(bc.RawStr / scalar);
			bc.RawInt = (int)(bc.RawInt / scalar);
			bc.RawDex = (int)(bc.RawDex / scalar);

			bc.Hits = bc.HitsMax;
			bc.Mana = bc.ManaMax;
			bc.Stam = bc.StamMax;

			for (int i = 0; i < bc.Skills.Length; i++)
			{
				Skill skill = bc.Skills[i];

				if (skill.Base > 0.0)
					skill.Base /= scalar;
			}

			bc.PassiveSpeed *= scalar;
			bc.ActiveSpeed *= scalar;
			bc.CurrentSpeed = bc.PassiveSpeed;

			var dmgScalar = Math.Pow(scalar, _DmgScalarExponent);
			bc.DamageMin = (int)(bc.DamageMin / dmgScalar);
			bc.DamageMax = (int)(bc.DamageMax / dmgScalar);

			if (bc.Fame > 0)
				bc.Fame = (int)(bc.Fame / scalar);
			if (bc.Karma != 0)
				bc.Karma = (int)(bc.Karma / scalar);
		}
	}

	class DifficultyLevelExtInfo : NExtensionInfo
	{
		public DifficultyLevelValue DifficultyLevel { get; set; }

		public DifficultyLevelExtInfo()
		{
			DifficultyLevel = DifficultyLevelValue.Normal;
		}

		public override void Serialize(GenericWriter writer)
		{
			writer.Write( (int)0 ); //version
			writer.Write((byte)DifficultyLevel);
		}

		public override void Deserialize(GenericReader reader)
		{
			int version = 0;
			if (Fix)
				version = reader.ReadInt(); //version
			DifficultyLevel = (DifficultyLevelValue)reader.ReadByte();
		}
	}
}
