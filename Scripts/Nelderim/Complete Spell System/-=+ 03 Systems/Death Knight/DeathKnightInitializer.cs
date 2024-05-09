using Server.ACC.CSS;

namespace Server.Spells.DeathKnight
{
	public class DeathKnightInitializer : BaseInitializer
	{
		public static void Configure()
		{
			Register(typeof(BanishSpell),
				"Wygnanie",
				"Opis",
				"Regs",
				"Mana: ; Skill: ",
				2244,
				5054,
				School.DeathKnight);
			Register(typeof(DemonicTouchSpell),
				"Dotyk Demona",
				"Opis",
				"Regs",
				"Mana: ; Skill: ",
				20736,
				5054,
				School.DeathKnight);
			Register(typeof(DevilPactSpell),
				"Pakt Ze Smiercia",
				"Opis",
				"Regs",
				"Mana: ; Skill: ",
				20491,
				5054,
				School.DeathKnight);
			Register(typeof(GrimReaperSpell),
				"Ponury Zniwiarz",
				"Opis",
				"Regs",
				"Mana: ; Skill: ",
				2257,
				5054,
				School.DeathKnight);
			Register(typeof(HagHandSpell),
				"Reka Wiedzmy",
				"Opis",
				"Regs",
				"Mana: ; Skill: ",
				21001,
				5054,
				School.DeathKnight);
			Register(typeof(HellfireSpell),
				"Ogien Piekielny",
				"Opis",
				"Regs",
				"Mana: ; Skill: ",
				2281,
				5054,
				School.DeathKnight);
			Register(typeof(LucifersBoltSpell),
				"Promien Smierci",
				"Opis",
				"Regs",
				"Mana: ; Skill: ",
				20488,
				5054,
				School.DeathKnight);
			Register(typeof(OrbOfOrcusSpell),
				"Kula Smierci",
				"Opis",
				"Regs",
				"Mana: ; Skill: ",
				20745,
				5054,
				School.DeathKnight);
			Register(typeof(ShieldOfHateSpell),
				"Tarcza Nienawisci",
				"Opis",
				"Regs",
				"Mana: ; Skill: ",
				20745,
				5054,
				School.DeathKnight);
			Register(typeof(SoulReaperSpell),
				"Zniwarz Dusz",
				"Desc",
				"regs",
				"Mana: ; Skill: ",
				20491,
				5054,
				School.DeathKnight);
			Register(typeof(StrengthOfSteelSpell),
				"Wytrzymalosc Stali",
				"Desc",
				"regs",
				"Mana: ; Skill: ",
				20491,
				5054,
				School.DeathKnight);
			Register(typeof(StrikeSpell),
				"Uderzenie",
				"Desc",
				"regs",
				"Mana: ; Skill: ",
				20491,
				5054,
				School.DeathKnight);
			Register(typeof(SuccubusSkinSpell),
				"Skora Sukkuba",
				"Desc",
				"regs",
				"Mana: ; Skill: ",
				20491,
				5054,
				School.DeathKnight);
			Register(typeof(WrathSpell),
				"Gniew",
				"Desc",
				"regs",
				"Mana: ; Skill: ",
				20491,
				5054,
				School.DeathKnight);
		}
	}
}
