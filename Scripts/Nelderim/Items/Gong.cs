using Nelderim.Towns;
using Server.Engines.CityLoyalty;
using Server.Mobiles;
using Server.Network;
using System;

namespace Server.Items
{
	[Flipable(0x1580, 0x1581)]
	public class Gong : Item
	{
		[PropertyObject]
		public class Races
		{
			[CommandProperty(AccessLevel.GameMaster)]
			public bool Tamael { get; set; } = true;

			[CommandProperty(AccessLevel.GameMaster)]
			public bool Jarling { get; set; } = true;

			[CommandProperty(AccessLevel.GameMaster)]
			public bool Krasnolud { get; set; } = true;

			[CommandProperty(AccessLevel.GameMaster)]
			public bool Elf { get; set; } = true;

			[CommandProperty(AccessLevel.GameMaster)]
			public bool Drow { get; set; } = true;

			[CommandProperty(AccessLevel.GameMaster)]
			public bool OtherOrNone { get; set; } = true;

			public void Serialize(GenericWriter writer)
			{
				writer.Write((int)0); // version
				writer.Write(Tamael);
				writer.Write(Jarling);
				writer.Write(Krasnolud);
				writer.Write(Elf);
				writer.Write(Drow);
				writer.Write(OtherOrNone);
			}

			public void Deserialize(GenericReader reader)
			{
				int version = reader.ReadInt();
				Tamael = reader.ReadBool();
				Jarling = reader.ReadBool();
				Krasnolud = reader.ReadBool();
				Elf = reader.ReadBool();
				Drow = reader.ReadBool();
				OtherOrNone = reader.ReadBool();
			}

			public bool Matches(PlayerMobile pm)
			{
				var playerRace = pm.Race;
				if (playerRace == Race.NTamael)
				{
					return Tamael;
				}
				if (playerRace == Race.NJarling)
				{
					return Jarling;
				}
				if (playerRace == Race.NKrasnolud)
				{
					return Krasnolud;
				}
				if (playerRace == Race.NElf)
				{
					return Elf;
				}
				if (playerRace == Race.NDrow)
				{
					return Drow;
				}
				return OtherOrNone;
			}
			
			public override string ToString() {
				return "...";
			}
		}

		[PropertyObject]
		public class Citizienships
		{
			[CommandProperty(AccessLevel.GameMaster)]
			public bool None { get; set; } = true;

			[CommandProperty(AccessLevel.GameMaster)]
			public bool Tasandora { get; set; } = true;

			[CommandProperty(AccessLevel.GameMaster)]
			public bool Garlan { get; set; } = true;

			[CommandProperty(AccessLevel.GameMaster)]
			public bool LDelmah { get; set; } = true;

			[CommandProperty(AccessLevel.GameMaster)]
			public bool Lotharn { get; set; } = true;

			[CommandProperty(AccessLevel.GameMaster)]
			public bool Twierdza { get; set; } = true;

			[CommandProperty(AccessLevel.GameMaster)]
			public bool Tirassa { get; set; } = true;

			[CommandProperty(AccessLevel.GameMaster)]
			public bool Orod { get; set; } = true;

			public void Serialize(GenericWriter writer)
			{
				writer.Write((int)1); // version
				writer.Write(None);
				writer.Write(Tasandora);
				writer.Write(Garlan);
				writer.Write(LDelmah);
				writer.Write(Lotharn);
				writer.Write(Twierdza);
				writer.Write(Tirassa);
				writer.Write(Orod);
			}

			public void Deserialize(GenericReader reader)
			{
				int version = reader.ReadInt();
				None = reader.ReadBool();
				Tasandora = reader.ReadBool();
				Garlan = reader.ReadBool();
				LDelmah = reader.ReadBool();
				Lotharn = reader.ReadBool();
				Twierdza = reader.ReadBool();
				Tirassa = (version >= 1) ? reader.ReadBool() : false;
				Orod = (version >= 1) ? reader.ReadBool() : false;
			}

			public bool Matches(PlayerMobile pm)
			{
				if (TownDatabase.IsCitizenOfAnyTown(pm))
				{
					Towns playerCity = TownDatabase.IsCitizenOfWhichTown(pm);
					switch (playerCity)
					{
						case Towns.None: return None;
						case Towns.Tasandora: return Tasandora;
						case Towns.Garlan: return Garlan;
						case Towns.Twierdza: return Twierdza;
						case Towns.LDelmah: return LDelmah;
						case Towns.Lotharn: return Lotharn;
						case Towns.Tirassa: return Tirassa;
						case Towns.Orod: return Orod;
						default:
							{
								Console.WriteLine("WARNING: w klasie Gong nie ma przypisanej reakcji na miasto " + playerCity);
								return false;
							}
					}
				}
				return None;
			}

			public override string ToString() {
				return "...";
			}
		}

		[PropertyObject]
		public class Areas
		{
			[CommandProperty(AccessLevel.GameMaster)]
			public bool GLOBAL { get; set; } = true;

			[CommandProperty(AccessLevel.GameMaster)]
			public bool Tasandora { get; set; }

			[CommandProperty(AccessLevel.GameMaster)]
			public bool Celendir { get; set; }

			[CommandProperty(AccessLevel.GameMaster)]
			public bool Talas { get; set; }

			[CommandProperty(AccessLevel.GameMaster)]
			public bool Tafroel { get; set; }

			[CommandProperty(AccessLevel.GameMaster)]
			public bool Ethrod { get; set; }

			[CommandProperty(AccessLevel.GameMaster)]
			public bool Ferion { get; set; }

			[CommandProperty(AccessLevel.GameMaster)]
			public bool Tingref { get; set; }

			[CommandProperty(AccessLevel.GameMaster)]
			public bool Uk { get; set; }

			[CommandProperty(AccessLevel.GameMaster)]
			public bool SnieznaPrzystan { get; set; }

			[CommandProperty(AccessLevel.GameMaster)]
			public bool SnieznaGarlan { get; set; }

			[CommandProperty(AccessLevel.GameMaster)]
			public bool Twierdza { get; set; }

			[CommandProperty(AccessLevel.GameMaster)]
			public bool Lotharn { get; set; }

			[CommandProperty(AccessLevel.GameMaster)]
			public bool LDelmah { get; set; }

			[CommandProperty(AccessLevel.GameMaster)]
			public bool NoamuthQuortek { get; set; }

			[CommandProperty(AccessLevel.GameMaster)]
			public bool Tirassa { get; set; }

			[CommandProperty(AccessLevel.GameMaster)]
			public bool Przemytnicy { get; set; }

			public void Serialize(GenericWriter writer)
			{
				writer.Write((int)0); // version
				writer.Write(GLOBAL);
				writer.Write(Tasandora);
				writer.Write(Celendir);
				writer.Write(Talas);
				writer.Write(Tafroel);
				writer.Write(Ethrod);
				writer.Write(Ferion);
				writer.Write(Tingref);
				writer.Write(Uk);
				writer.Write(SnieznaPrzystan);
				writer.Write(SnieznaGarlan);
				writer.Write(Twierdza);
				writer.Write(Lotharn);
				writer.Write(LDelmah);
				writer.Write(NoamuthQuortek);
				writer.Write(Tirassa);
				writer.Write(Przemytnicy);
			}

			public void Deserialize(GenericReader reader)
			{
				int version = reader.ReadInt();
				GLOBAL = reader.ReadBool();
				Tasandora = reader.ReadBool();
				Celendir = reader.ReadBool();
				Talas = reader.ReadBool();
				Tafroel = reader.ReadBool();
				Ethrod = reader.ReadBool();
				Ferion = reader.ReadBool();
				Tingref = reader.ReadBool();
				Uk = reader.ReadBool();
				SnieznaPrzystan = reader.ReadBool();
				SnieznaGarlan = reader.ReadBool();
				Twierdza = reader.ReadBool();
				Lotharn = reader.ReadBool();
				LDelmah = reader.ReadBool();
				NoamuthQuortek = reader.ReadBool();
				Tirassa = reader.ReadBool();
				Przemytnicy = reader.ReadBool();
			}

			public bool Matches(PlayerMobile pm)
			{
				if (GLOBAL)
					return true;

				foreach (var region in pm.Map.Regions.Values)
				{
					if (region.Contains(pm.Location))
					{
						var r = region.ToString();
						if (Tasandora && (r == "Tasandora" || r == "Tasandora_Kopalnia" || r == "Twierdza_Kopalnia" || r == "Tasandora_Housing" || r == "SwiatyniaKonca_Krypty" || r == "Tasandora_Kanaly"))
							return true;
						if (Celendir && (r == "Celendir"))
							return true;
						if (Talas && (r == "Talas"))
							return true;
						if (Tafroel && (r == "Tafroel"))
							return true;
						if (Ethrod && (r == "Ethrod" || r == "Ethrod_Kopalnia"))
							return true;
						if (Ferion && (r == "Ferion"))
							return true;
						if (Tingref && (r == "Tingref"))
							return true;
						if (Uk && (r == "Uk"))
							return true;
						if (SnieznaPrzystan && (r == "SnieznaPrzystan"))
							return true;
						if (SnieznaGarlan && (r == "Garlan" || r == "Garlan_Kopalnia" || r == "Garlan_Housing"))
							return true;
						if (Twierdza && (r == "Twierdza" || r == "Twierdza_Housing" || r == "Twierdza_Dungeon"))
							return true;
						if (Lotharn && (r == "Lotharn" || r == "Enedh_Kopalnia"))
							return true;
						if (LDelmah && (r == "L'Delmah" || r == "NoamuthQuortek_Housing"))
							return true;
						if (NoamuthQuortek && (r == "NoamuthQuortek" || r == "NoamuthQuortek_Kopalnia" || r == "SwiatyniaLoethe" || r == "NoamuthQuortek_Housing" || r == "Drowy_Dungeon"))
							return true;
						if (Tirassa && (r == "Tirassa" || r == "Tirassa_Kopalnia"))
							return true;
						if (Przemytnicy && (r == "KryjowkaPrzemytnikow" || r == "JaskiniaPrzemytnikow" || r == "ZagubionaKopalnia"))
							return true;
					}
				}
				return false;
			}
			
			public override string ToString() {
				return "...";
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Disabled { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public bool AnnounceOnlyGM { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public Races AnnounceRace { get; set; } = new();

		[CommandProperty(AccessLevel.GameMaster)]
		public Citizienships AnnounceCitizienship { get; set; } = new();

		[CommandProperty(AccessLevel.GameMaster)]
		public Races HearRace { get; set; } = new();

		[CommandProperty(AccessLevel.GameMaster)]
		public Citizienships HearCitizienship { get; set; } = new();

		[CommandProperty(AccessLevel.GameMaster)]
		public Areas HearRange { get; set; } = new();

		[CommandProperty(AccessLevel.GameMaster)]
		public string AnnouncedMessage { get; set; }

		private static string DefaultAnnounceMessage(PlayerMobile triggerPlayer) => "Z okolicy " + triggerPlayer.Map + " " + triggerPlayer.Location + " roznosi sie dzwiek czyjejs obecnosci.";

		[CommandProperty(AccessLevel.GameMaster)]
		public int AnnouncedMessageHue { get; set; } = 53;

		[CommandProperty(AccessLevel.GameMaster)]
		public string TriggerMessage { get; set; } = "Uzyles gongu rozglaszajac swiatu swoja obecnosc w tym miejscu.";

		[CommandProperty(AccessLevel.GameMaster)]
		public int LocalSound { get; set; } = 0x65c;

		[CommandProperty(AccessLevel.GameMaster)]
		public int CooldownMinutes { get; set; } = 5;

		private DateTime _LastUsage;

		[Constructable]
		public Gong() : base(0x1580)
		{
			Name = "Gong";
			Label1 = "(Jego uzycie rozglosi swiatu twoja obecnosc w tym miejscu)";
			Movable = false;
		}

		public Gong(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version

			writer.Write(Disabled);
			writer.Write(AnnounceOnlyGM);
			writer.Write(LocalSound);
			writer.Write(AnnouncedMessage);
			writer.Write(AnnouncedMessageHue);
			writer.Write(TriggerMessage);
			AnnounceRace.Serialize(writer);
			AnnounceCitizienship.Serialize(writer);
			HearRace.Serialize(writer);
			HearCitizienship.Serialize(writer);
			HearRange.Serialize(writer);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();

			Disabled = reader.ReadBool();
			AnnounceOnlyGM = reader.ReadBool();
			LocalSound = reader.ReadInt();
			AnnouncedMessage = reader.ReadString();
			AnnouncedMessageHue = reader.ReadInt();
			TriggerMessage = reader.ReadString();
			AnnounceRace.Deserialize(reader);
			AnnounceCitizienship.Deserialize(reader);
			HearRace.Deserialize(reader);
			HearCitizienship.Deserialize(reader);
			HearRange.Deserialize(reader);
		}

		public override void OnDoubleClick(Mobile from)
		{
			var pm = from as PlayerMobile;
			if (pm == null || Disabled)
				return;

			if (!pm.InRange(GetWorldLocation(), 2) || !pm.InLOS(this))
			{
				pm.SendLocalizedMessage(500446); // That is too far away.
				return;
			}

			if (AnnounceOnlyGM && pm.AccessLevel < AccessLevel.Counselor)
			{
				pm.SendMessage("Brak dostepu.");
				return;
			}

			if (!AnnounceRace.Matches(pm) || !AnnounceCitizienship.Matches(pm))
			{
				pm.SendMessage("To nie jest przeznaczone dla takich, jak ty!");
				return;
			}

			var cooldown = TimeSpan.FromMinutes(CooldownMinutes);
			if(pm.AccessLevel >= AccessLevel.Counselor)
				cooldown = TimeSpan.Zero;
			
			if ( DateTime.UtcNow - _LastUsage  < cooldown)
			{
				pm.SendMessage("Trzeba chwile odczekac przed ponownym uzyciem tego przedmiotu.");
				return;
			}

			var announceText = string.IsNullOrEmpty(AnnouncedMessage) ? DefaultAnnounceMessage(pm) : AnnouncedMessage;

			if (TriggerMessage != null)
				pm.SendMessage(TriggerMessage);
			else
				pm.SendMessage("Uzyles gongu rozglaszajac swiatu swoja obecnosc w tym miejscu.");

			foreach (var ns in NetState.Instances)
			{
				if (ns.Mobile is PlayerMobile listener && HearRace.Matches(listener) && HearCitizienship.Matches(listener) && HearRange.Matches(pm))
				{
					listener.SendMessage(AnnouncedMessageHue, announceText);
				}
			}
			if(LocalSound != -1)
				from.PlaySound(LocalSound);

			_LastUsage = DateTime.UtcNow;
		}
	}
}
