#region References

using System;
using Nelderim;

#endregion

namespace Server.Items
{
	public partial class BaseArmor : IResource2
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

		//TODO: RemoveMe
		private void FixDragonArmorResource()
		{
			if (this is DragonArms or DragonChest or DragonGloves or DragonLegs or DragonHelm)
			{
				Timer.DelayCall(() =>
				{
					if ((Resource == DefaultResource || Resource >= CraftResource.RedScales || Resource <= CraftResource.BlueScales) &&
					    Resource2 is >= CraftResource.Iron and <= CraftResource.Valorite)
					{
						Console.WriteLine($"Swapping resources of {GetType().Name}:{Serial}");
						var origRes = Resource;
						m_Resource = Resource2;
						ExtraCraftResource.Get(this).Resource2 = origRes <= CraftResource.RedScales ? CraftResource.RedScales : origRes;
					}

					if (Resource2 == CraftResource.None)
					{
						Console.WriteLine($"Fixing resource2 of {GetType().Name}:{Serial}");
						Resource2 = CraftResource.RedScales;
					}
				});
			}
		}
	}
}
