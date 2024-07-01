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
					this.AddHtmlLocalized(73, 58, 252, 257, 3060048, true, true);
					break;
				}
				case SkillTooltipType.Smithy:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060049, true, true);
					break;
				}
				case SkillTooltipType.Carpenter:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060050, true, true);
					break;
				}
				case SkillTooltipType.Lumberjack:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060051, true, true);
					break;
				}
				case SkillTooltipType.Tinker:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060052, true, true);
					break;
				}
				case SkillTooltipType.Lockpick:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060053, true, true);
					break;
				}
				case SkillTooltipType.TrapRemove:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060054, true, true);
					break;
				}
				case SkillTooltipType.Peace:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060055, true, true);
					break;
				}
				case SkillTooltipType.Provocation:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060056, true, true);
					break;
				}
				case SkillTooltipType.Music:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060057, true, true);
					break;
				}
				case SkillTooltipType.Detect:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060058, true, true);
					break;
				}
				case SkillTooltipType.Hiding:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060059, true, true);
					break;
				}
				case SkillTooltipType.Peak:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060060, true, true);
					break;
				}
				case SkillTooltipType.Stealing:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060061, true, true);
					break;
				}
				case SkillTooltipType.Tracking:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060062, true, true);
					break;
				}
				case SkillTooltipType.Fencing:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060063, true, true);
					break;
				}
				case SkillTooltipType.Stealth:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060064, true, true);
					break;
				}
				case SkillTooltipType.Cooking:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060065, true, true);
					break;
				}
				case SkillTooltipType.Meditation:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060066, true, true);
					break;
				}
				case SkillTooltipType.Magic:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060067, true, true);
					break;
				}
				case SkillTooltipType.Scribe:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060068, true, true);
					break;
				}
				case SkillTooltipType.Eval:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060069, true, true);
					break;
				}
				case SkillTooltipType.Resist:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060070, true, true);
					break;
				}
				case SkillTooltipType.Wrestling:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060071, true, true);
					break;
				}
				case SkillTooltipType.Herbalism:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060072, true, true);
					break;
				}
				case SkillTooltipType.Alchemy:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060074, true, true);
					break;
				}
				case SkillTooltipType.Healing:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060075, true, true);
					break;
				}
				case SkillTooltipType.Anatomy:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060076, true, true);
					break;
				}
				case SkillTooltipType.SpiritSpeak:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060077, true, true);
					break;
				}
				case SkillTooltipType.ArmsLore:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060078, true, true);
					break;
				}
				case SkillTooltipType.MaceFightning:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060079, true, true);
					break;
				}
				case SkillTooltipType.Sword:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060080, true, true);
					break;
				}
				case SkillTooltipType.Focus:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060081, true, true);
					break;
				}
				case SkillTooltipType.Archery:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060082, true, true);
					break;
				}
				case SkillTooltipType.Fishing:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060083, true, true);
					break;
				}
				case SkillTooltipType.Cartography:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060084, true, true);
					break;
				}
				case SkillTooltipType.Necromancy:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060085, true, true);
					break;
				}
				case SkillTooltipType.Disco:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060086, true, true);
					break;
				}
				case SkillTooltipType.Chivalry:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060087, true, true);
					break;
				}
				case SkillTooltipType.Bushido:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060088, true, true);
					break;
				}
				case SkillTooltipType.Tactics:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060089, true, true);
					break;
				}
				case SkillTooltipType.Ninjitsu:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060090, true, true);
					break;
				}
				case SkillTooltipType.Poisoning:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060091, true, true);
					break;
				}
				case SkillTooltipType.Parry:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060092, true, true);
					break;
				}
				case SkillTooltipType.Fletching:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060093, true, true);
					break;
				}
				case SkillTooltipType.AnimalTaming:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060094, true, true);
					break;
				}
				case SkillTooltipType.AnimalLore:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060095, true, true);
					break;
				}
				case SkillTooltipType.Vet:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060096, true, true);
					break;
				}
				case SkillTooltipType.Camping:
				{
					this.AddHtmlLocalized(73, 58, 252, 257, 3060097, true, true);
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
