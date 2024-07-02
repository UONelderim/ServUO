using Server.Regions;

namespace Server.Gumps
{
	public class SkillTooltipGump : Gump
	{
		public SkillTooltipGump(Mobile from, SkillTooltipType tooltip) : base(0, 0)
		{
			this.Closable = true;
			this.Disposable = true;
			this.Dragable = true;
			this.Resizable = false;
			this.AddPage(0);
			this.AddBackground(50, 35, 300, 300, 9200);
			this.AddAlphaRegion(60, 45, 279, 280);

			switch (tooltip)
			{
				case SkillTooltipType.Tailor:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060108, true, true);
					break;
				}
				case SkillTooltipType.Smithy:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060109, true, true);
					break;
				}
				case SkillTooltipType.Carpenter:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060110, true, true);
					break;
				}
				case SkillTooltipType.Lumberjack:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060111, true, true);
					break;
				}
				case SkillTooltipType.Tinker:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060112, true, true);
					break;
				}
				case SkillTooltipType.Lockpick:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060113, true, true);
					break;
				}
				case SkillTooltipType.TrapRemove:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060114, true, true);
					break;
				}
				case SkillTooltipType.Peace:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060115, true, true);
					break;
				}
				case SkillTooltipType.Provocation:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060116, true, true);
					break;
				}
				case SkillTooltipType.Music:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060117, true, true);
					break;
				}
				case SkillTooltipType.Detect:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060118, true, true);
					break;
				}
				case SkillTooltipType.Hiding:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060119, true, true);
					break;
				}
				case SkillTooltipType.Peak:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060120, true, true);
					break;
				}
				case SkillTooltipType.Stealing:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060121, true, true);
					break;
				}
				case SkillTooltipType.Tracking:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060122, true, true);
					break;
				}
				case SkillTooltipType.Fencing:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060123, true, true);
					break;
				}
				case SkillTooltipType.Stealth:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060124, true, true);
					break;
				}
				case SkillTooltipType.Cooking:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060125, true, true);
					break;
				}
				case SkillTooltipType.Meditation:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060126, true, true);
					break;
				}
				case SkillTooltipType.Magic:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060127, true, true);
					break;
				}
				case SkillTooltipType.Scribe:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060128, true, true);
					break;
				}
				case SkillTooltipType.Eval:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060129, true, true);
					break;
				}
				case SkillTooltipType.Resist:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060130, true, true);
					break;
				}
				case SkillTooltipType.Wrestling:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060131, true, true);
					break;
				}
				case SkillTooltipType.Herbalism:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060132, true, true);
					break;
				}
				case SkillTooltipType.Alchemy:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060134, true, true);
					break;
				}
				case SkillTooltipType.Healing:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060135, true, true);
					break;
				}
				case SkillTooltipType.Anatomy:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060136, true, true);
					break;
				}
				case SkillTooltipType.SpiritSpeak:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060137, true, true);
					break;
				}
				case SkillTooltipType.ArmsLore:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060138, true, true);
					break;
				}
				case SkillTooltipType.MaceFightning:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060139, true, true);
					break;
				}
				case SkillTooltipType.Sword:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060140, true, true);
					break;
				}
				case SkillTooltipType.Focus:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060141, true, true);
					break;
				}
				case SkillTooltipType.Archery:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060142, true, true);
					break;
				}
				case SkillTooltipType.Fishing:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060143, true, true);
					break;
				}
				case SkillTooltipType.Cartography:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060144, true, true);
					break;
				}
				case SkillTooltipType.Necromancy:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060145, true, true);
					break;
				}
				case SkillTooltipType.Disco:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060146, true, true);
					break;
				}
				case SkillTooltipType.Chivalry:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060147, true, true);
					break;
				}
				case SkillTooltipType.Bushido:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060148, true, true);
					break;
				}
				case SkillTooltipType.Tactics:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060149, true, true);
					break;
				}
				case SkillTooltipType.Ninjitsu:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060150, true, true);
					break;
				}
				case SkillTooltipType.Poisoning:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060151, true, true);
					break;
				}
				case SkillTooltipType.Parry:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060152, true, true);
					break;
				}
				case SkillTooltipType.Fletching:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060153, true, true);
					break;
				}
				case SkillTooltipType.AnimalTaming:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060154, true, true);
					break;
				}
				case SkillTooltipType.AnimalLore:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060155, true, true);
					break;
				}
				case SkillTooltipType.Vet:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060156, true, true);
					break;
				}
				case SkillTooltipType.Camping:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060157, true, true);
					break;
				}
				case SkillTooltipType.None:
				{
					this.AddHtml(73, 58, 252, 257, "", true, true);
					break;
				}
			}
		}
	}
}
