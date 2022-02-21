#region References

using Server.Mobiles;

#endregion

namespace Server.ACC.CSS.Systems.Rogue
{
	[CorpseName("niezidentyfikowane zwłoki")]
	public class CharmedMobile : BaseCreature
	{
		[CommandProperty(AccessLevel.GameMaster)]
		public BaseCreature Owner { get; set; }

		[Constructable]
		public CharmedMobile(BaseCreature owner) : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Owner = owner;
			Body = 777;
			Title = " mistyczny pasterz";
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public override bool ClickTitle { get { return false; } }

		public CharmedMobile(Serial serial) : base(serial)
		{
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			list.Add(1042971, this.Name);
			list.Add(1049644, "zaklęty");
		}

		public override bool OnBeforeDeath()
		{
			if (Owner != null)
			{
				Owner.MoveToWorld(this.Location, this.Map);
				Owner.Blessed = false;
				Owner.RevealingAction();
			}

			Delete();
			return false;
		}

		public override void OnAfterDelete()
		{
			if (Owner != null)
			{
				Owner.MoveToWorld(this.Location, this.Map);
				Owner.Blessed = false;
				Owner.RevealingAction();
			}

			base.OnAfterDelete();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
			writer.Write(Owner);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			Owner = reader.ReadMobile() as BaseCreature;

			Delete();
		}
	}
}
