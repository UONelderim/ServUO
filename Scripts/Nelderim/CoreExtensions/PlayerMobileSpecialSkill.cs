using System;
using Nelderim;

namespace Server.Mobiles
{
	public partial class PlayerMobile
	{
		[CommandProperty(AccessLevel.GameMaster)]
		public SpecialSkills SpecialSkills
		{
			get => PlayerSpeciallSkills.Get(this).SpecialSkills;
			set => PlayerSpeciallSkills.Get(this).SpecialSkills = value;
		}
	}
	
	[Flags]
	public enum NSpecialSkill
	{
		Spellweaving = 0x00000001,
		Mysticism = 0x00000002,
		Ancient = 0x00000004,
		Avatar = 0x00000008,
		Bard = 0x00000010,
		Cleric = 0x00000020,
		Nature = 0x00000040, //Druid
		Ranger = 0x00000080,
		Rogue = 0x00000100,
		Undead = 0x00000200,
		DeathKnight = 0x00000400,
	}

	[PropertyObject]
	public class SpecialSkills
	{
		private NSpecialSkill _SpecialSkills;
		
		[CommandProperty(AccessLevel.GameMaster)]
		public bool Spellweaving
		{
			get => GetSpecialSkill(NSpecialSkill.Spellweaving);
			set => SetSpecialSkill(NSpecialSkill.Spellweaving, value);
		}
		
		[CommandProperty(AccessLevel.GameMaster)]
		public bool Mysticism
		{
			get => GetSpecialSkill(NSpecialSkill.Mysticism);
			set => SetSpecialSkill(NSpecialSkill.Mysticism, value);
		}
		
		[CommandProperty(AccessLevel.GameMaster)]
		public bool Ancient
		{
			get => GetSpecialSkill(NSpecialSkill.Ancient);
			set => SetSpecialSkill(NSpecialSkill.Ancient, value);
		}
		
		[CommandProperty(AccessLevel.GameMaster)]
		public bool Avatar
		{
			get => GetSpecialSkill(NSpecialSkill.Avatar);
			set => SetSpecialSkill(NSpecialSkill.Avatar, value);
		}
		
		[CommandProperty(AccessLevel.GameMaster)]
		public bool Bard
		{
			get => GetSpecialSkill(NSpecialSkill.Bard);
			set => SetSpecialSkill(NSpecialSkill.Bard, value);
		}
		
		[CommandProperty(AccessLevel.GameMaster)]
		public bool Cleric
		{
			get => GetSpecialSkill(NSpecialSkill.Cleric);
			set => SetSpecialSkill(NSpecialSkill.Cleric, value);
		}
		
		[CommandProperty(AccessLevel.GameMaster)]
		public bool Nature
		{
			get => GetSpecialSkill(NSpecialSkill.Nature);
			set => SetSpecialSkill(NSpecialSkill.Nature, value);
		}
		
		[CommandProperty(AccessLevel.GameMaster)]
		public bool Ranger
		{
			get => GetSpecialSkill(NSpecialSkill.Ranger);
			set => SetSpecialSkill(NSpecialSkill.Ranger, value);
		}
		
		[CommandProperty(AccessLevel.GameMaster)]
		public bool Rogue
		{
			get => GetSpecialSkill(NSpecialSkill.Rogue);
			set => SetSpecialSkill(NSpecialSkill.Rogue, value);
		}
		
		[CommandProperty(AccessLevel.GameMaster)]
		public bool Undead
		{
			get => GetSpecialSkill(NSpecialSkill.Undead);
			set => SetSpecialSkill(NSpecialSkill.Undead, value);
		}
		
		[CommandProperty(AccessLevel.GameMaster)]
		public bool DeathKnight
		{
			get => GetSpecialSkill(NSpecialSkill.DeathKnight);
			set => SetSpecialSkill(NSpecialSkill.DeathKnight, value);
		}
		
		private bool GetSpecialSkill(NSpecialSkill flag)
		{
			return (_SpecialSkills & flag) != 0;
		}

		private void SetSpecialSkill(NSpecialSkill flag, bool value)
		{
			if (value)
			{
				_SpecialSkills |= flag;
			}
			else
			{
				_SpecialSkills &= ~flag;
			}
		}

		public void Clear()
		{
			_SpecialSkills = 0;
		}

		public void Serialize(GenericWriter writer)
		{
			writer.Write(0); //version
			writer.Write((int)_SpecialSkills);
		}
		
		public void Deserialize(GenericReader reader)
		{
			int version = reader.ReadInt();
			_SpecialSkills = (NSpecialSkill)reader.ReadInt();
		}
		
		public override string ToString()
		{
			return "...";
		}
	}

	class PlayerSpeciallSkills() : NExtension<PlayerSpeciallSkillsInfo>("PlayerSpecialSkills")
	{
		public static void Configure()
		{
			Register(new PlayerSpeciallSkills());
		}
	}

	class PlayerSpeciallSkillsInfo : NExtensionInfo
	{
		public SpecialSkills SpecialSkills { get; set; } = new();

		public override void Serialize(GenericWriter writer)
		{
			writer.Write((int)0); //version
			SpecialSkills.Serialize(writer);
		}

		public override void Deserialize(GenericReader reader)
		{
			int version = reader.ReadInt();
			SpecialSkills = new SpecialSkills();
			SpecialSkills.Deserialize(reader);
		}
	}
}
