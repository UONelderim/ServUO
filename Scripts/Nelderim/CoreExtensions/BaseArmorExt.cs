#region References

using Nelderim;

#endregion

namespace Server.Items
{
	public partial class BaseArmor
	{
		[CommandProperty(AccessLevel.GameMaster)]
		public CraftResource Resource2
		{
			get { return ExtraCraftResource.Get(this).Resource2; }
			set
			{
				var old = ExtraCraftResource.Get(this).Resource2;
				if (old != value)
				{
					UnscaleDurability();
					
					ExtraCraftResource.Get(this).Resource2 = value;
					ApplyResourceResistances(old, value);
					
					Hue = CraftResources.GetHue(value);
					
					Invalidate();
					InvalidateProperties();
					
					if (Parent is Mobile)
						((Mobile)Parent).UpdateResistances();
					
					ScaleDurability();
				}
			}
		}
	}
}
