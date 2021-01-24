using Nelderim;

namespace Server.Items
{
	public partial class BaseQuiver : IHitPoints
	{
		[CommandProperty(AccessLevel.GameMaster)]
		public int MaxHitPoints
		{
			get { return ItemHitPoints.Get(this).MaxHitPoints; }
			set { ItemHitPoints.Get(this).MaxHitPoints = value; InvalidateProperties(); }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int HitPoints
		{
			get
			{
				return ItemHitPoints.Get(this).HitPoints;
			}
			set
			{
				int curval = ItemHitPoints.Get(this).HitPoints;
				if ( value != curval && MaxHitPoints > 0 )
				{
					curval = ItemHitPoints.Get(this).HitPoints = value;

					if ( curval < 0 )
						Delete();
					else if ( curval > MaxHitPoints )
						ItemHitPoints.Get(this).HitPoints = MaxHitPoints;

					InvalidateProperties();
				}
			}
		}

		public int InitMinHits { get { return 90; } }
		public int InitMaxHits { get { return 110; } }
	}
}
