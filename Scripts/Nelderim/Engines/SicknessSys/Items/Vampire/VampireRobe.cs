#region References

using Server.Items;
using Server.Mobiles;

#endregion

namespace Server.SicknessSys.Items
{
	public class VampireRobe : Robe
	{
		public string pm { get; set; }

		[Constructable]
		public VampireRobe(PlayerMobile player, int stageID, int stageNum) :
			base(1175) //Stage  1 = 0x1F03, 2 = 0x2687, 3 = 0x7816, GM = 0x4B9D
		{
			Name = GetStageName(stageNum) + " Wampirza Szata";
			ItemID = stageID;
			pm = player.Name;
			Weight = 1.0;
			//LootType = LootType.Blessed;
		}

		public VampireRobe(Serial serial) : base(serial)
		{
		}

		private string GetStageName(int stage)
		{
			switch (stage)
			{
				case 1: return "Pomniejsza";
				case 2: return "Owiecajaca";
				case 3: return "Mistrzowska";
				default: return "Zacna";
			}
		}

		public override bool VerifyMove(Mobile from)
		{
			if (from.Name != pm)
				return false;

			return base.VerifyMove(from);
		}

		public override bool OnEquip(Mobile from)
		{
			if (from.Name != pm || pm == null)
				return false;

			if (!(from.Backpack.FindItemByType(typeof(VirusCell)) is VirusCell item))
				Delete();
			else
			{
				if (item.Illness != IllnessType.Vampirism)
					Delete();
			}

			return base.OnEquip(from);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version

			writer.Write(pm);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			pm = reader.ReadString();
		}
	}
}
